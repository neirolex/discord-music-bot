using System.Diagnostics;

using Discord.Audio;

namespace discord_music_bot
{
    public class DiscordClient
    {
        public DiscordAudioService AudioService; //TODO: Incapslate, make private
        public DiscordClient(DiscordAudioService audioService) {
            AudioService = audioService;
        }
    }
}