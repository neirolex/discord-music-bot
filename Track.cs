namespace discord_music_bot
{
    public class AudioFile
    {
        public string Name;
        private string _source;
        public AudioFile(string name, string address) {
            _source = address;

            Name = name;
        }

        public FileInfo GetFileInfo() => new FileInfo(_source);
    }
}