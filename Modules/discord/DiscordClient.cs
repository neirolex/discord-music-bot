using System.Diagnostics;

using Discord.Audio;

namespace discord_music_bot
{
    public class DiscordClient
    {
        public Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        public async Task SendAsync(IAudioClient client, string path)
        {
            using (var ffmpeg = CreateStream(path)) //Create stream based on ffmpeg binary
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed)) //Create stream to transmit audio to discord AudioClient
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }
    }
}