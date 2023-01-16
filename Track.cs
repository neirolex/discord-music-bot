namespace discord_music_bot
{
    public class AudioFile
    {
        private string _source;
        public AudioFile(string address) {
            _source = address;
        }

        public FileInfo GetFileInfo() => new FileInfo(_source);
    }
}