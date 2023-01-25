using System.Diagnostics;

namespace discord_music_bot
{
    public interface IFilePlayer {
        public Process CreateStream(string path);
        public void TerminateStream();
        public AudioFile GetTrackById(int id);
        public int GetCurrentTrackId();

        public void Play();
        public void Stop();
        public void Pause();
        public void Next();
        public void Prev();

        public IDictionary<int, AudioFile> GetQueue();
    }

    public class FilePlayer : IFilePlayer
    {
        private Process? _ffmpeg; //Process of audio translating
        private IDictionary<int, AudioFile> _queue = new Dictionary<int, AudioFile>();
        private int _currentTrackId;

        public FilePlayer() { 
            _currentTrackId = 1;

            AudioFile track1 = new AudioFile("track1.mp3", "C:\\_work\\_repo\\discord-music-bot\\tracks\\track1.mp3");
            AddToQueue(track1);

            AudioFile track2 = new AudioFile("track2.mp3", "C:\\_work\\_repo\\discord-music-bot\\tracks\\track2.mp3");
            AddToQueue(track2);
        }
        
        public void Play(){}
        
        public void Stop(){}
        
        public void Pause(){}
        
        public void Next() {
            if(_currentTrackId < _queue.Count) {
                _currentTrackId++;
            }
        }
        
        public void Prev() {
            if(_currentTrackId > 0) {
                _currentTrackId--;
            }
        }

        public void SetCurrentTrack(int index) {
            _currentTrackId = index;
        }

        public AudioFile GetCurrentTrack() {
            return _queue[_currentTrackId];
        }

        public int GetCurrentTrackId() => _currentTrackId;

        public AudioFile GetTrackById(int id) {
            return _queue[id];
        }
        
        public IDictionary<int, AudioFile> GetQueue() => _queue;

        public void AddToQueue(AudioFile track) => _queue.Add(_queue.Count+1, track);
        
        public void RemoveFromQueue(int id) => _queue.Remove(id);

        public Process CreateStream(string path)
        {
            //Starts process with ffpeg player to stream audio from file.
            //Returns the reference of the process.

            _ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
            return _ffmpeg;
        }

        public void TerminateStream()
        {
            if(_ffmpeg != null) {
                _ffmpeg.Kill();
                _ffmpeg = null;
            }
        }

        public Process GetProcess() => _ffmpeg;
    }
}