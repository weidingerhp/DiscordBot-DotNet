using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DiscordBot.Domain.CoderDojoInfoModule.ServicesImpl
{
    public class LibOpusLoader
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static IntPtr _opusHandle = IntPtr.Zero;
        private static IntPtr _libSodiumHandle = IntPtr.Zero;

        public static void Init()
        {
            // the dll-loading is now available only for Windows-Plattform. 
            // For Linux/MAC you have to make sure that libsodium and opus codecs are installed on the system.
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return;
            }

            string bitness = "win32";
            if (Environment.Is64BitProcess)
            {
                bitness = "win64";
            }
            string opusDirPAth = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "nativelibs", bitness);
            if (_opusHandle == IntPtr.Zero)
            {
                _opusHandle = LoadLibrary(Path.Combine(opusDirPAth, "opus.dll"));
            }

            if (_libSodiumHandle == IntPtr.Zero)
            {
                _libSodiumHandle = LoadLibrary(Path.Combine(opusDirPAth, "libsodium.dll"));
            }
        }

        public static void Dispose()
        {
            if (_opusHandle != IntPtr.Zero)
            {
                FreeLibrary(_opusHandle);
            }
            if (_libSodiumHandle != IntPtr.Zero)
            {
                FreeLibrary(_libSodiumHandle);
            }
        }

    }
}