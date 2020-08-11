using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.WebSocket;
using Google.Cloud.TextToSpeech.V1;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace DiscordBot.Domain.CoderDojoInfoModule.ServicesImpl
{
    public class CoderDojoSpeechOutputService : IHostedService
    {
        private readonly ILogger<CoderDojoSpeechOutputService> _logger;
        private readonly DiscordSocketClient _discordSocketClient;


        public CoderDojoSpeechOutputService(DiscordSocketClient discordSocketClient,
            ILogger<CoderDojoSpeechOutputService> logger)
        {
            _logger = logger;
            _discordSocketClient = discordSocketClient;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            LibOpusLoader.Init();
            SayHelloOneSpeechChannel();
            return Task.Factory.StartNew(() => { _logger.LogInformation("StartAsync"); }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LibOpusLoader.Dispose();
            return Task.Factory.StartNew(() => { _logger.LogInformation("StopAsync"); }, cancellationToken);
        }

        public async Task SayHelloOneSpeechChannel()
        {
            try
            {
                await Task.Delay(1000);
                var server =
                    _discordSocketClient.Guilds.FirstOrDefault(x => x.Name?.Contains("Rafi und Elias") ?? false);
                if (server != null)
                {
                    var speechChannel =
                        server.Channels.FirstOrDefault(x => (x is IVoiceChannel && x.Name == "Allgemein"));
                    if (speechChannel is IVoiceChannel voiceChannel)
                    {
                        _logger.LogInformation($"Sending msg to {server.Name} - {speechChannel.Name}");

                        TextToSpeechClient client = await TextToSpeechClient.CreateAsync();
                        SynthesisInput input = new SynthesisInput
                        {
                            Text = "Du wolltest eine Dumme Antwort hören? Hier kommt sie: 1 + 1 = 3"
                        };
                        VoiceSelectionParams voiceSelection = new VoiceSelectionParams
                        {
                            LanguageCode = "de-DE",
                            SsmlGender = SsmlVoiceGender.Female,
                            Name = "de-DE-Wavenet-A"
                        };
                        AudioConfig audioConfig = new AudioConfig
                        {
                            AudioEncoding = AudioEncoding.Linear16,
                            SpeakingRate = 1.1
                            
                        };
                        SynthesizeSpeechResponse response = await client.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

                        using (IAudioClient audioClient = await voiceChannel.ConnectAsync())
                        {
                            await audioClient.SetSpeakingAsync(true);

                            audioClient.StreamCreated += AudioClientOnStreamCreated;
                            audioClient.Connected += AudioClientOnConnected;
                            var content = response.AudioContent.ToByteArray();
                            var format = new WaveFormat(24000, 16, 1);

                            using (var source = new RawSourceWaveStream(new MemoryStream(content), format))
                            {
                                int channels = 2;
                                int sampleRate = 48000;

                                var outFormat = new WaveFormat(sampleRate, 16, channels);   
                                var naudio = new WaveFormatConversionStream(outFormat, source);
                                await audioClient.SetSpeakingAsync(true);

                                using (var audioStream = audioClient.CreatePCMStream(AudioApplication.Music, voiceChannel.Bitrate)) 
                                {
                                    _logger.LogInformation("Start Talking");

                                    await naudio.CopyToAsync(audioStream, 1920);

                                    await audioStream.FlushAsync();
                                    _logger.LogInformation("Finished Talking");
                                }
                                await audioClient.SetSpeakingAsync(false);
                            }

                        }
                        // using (var oStream = File.Create("audio.mp3"))
                        // {
                        //     response.AudioContent.WriteTo(oStream);
                        // }

                        await Task.Delay(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }
        }

        private async Task AudioClientOnConnected()
        {
            await Task.Factory.StartNew(() =>  _logger.LogInformation("Connected"));
        }

        private async Task AudioClientOnStreamCreated(ulong arg1, AudioInStream audioIn)
        {
            await Task.Factory.StartNew(() =>  _logger.LogInformation("Stream created"));
        }
    }
}