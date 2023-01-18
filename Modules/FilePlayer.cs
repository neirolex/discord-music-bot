using Vlc.DotNet.Core;

namespace discord_music_bot
{
    public class FilePlayer
    {
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
            if(_currentTrackId+1 < _queue.Count) {
                _currentTrackId++;
            }
        }
        
        public void Prev() {
            if(_currentTrackId > 0) {
                _currentTrackId--;
            }
        }
        
        public IDictionary<int, AudioFile> GetQueue() => _queue;

        public void SetCurrentTrack(int index) {
            _currentTrackId = index;
        }

        public AudioFile GetCurrentTrack() {
            return _queue[_currentTrackId];
        }

        public AudioFile GetTrackById(int id) {
            return _queue[id];
        }
        
        public void AddToQueue(AudioFile track) => _queue.Add(_queue.Count+1, track);
        
        public void RemoveFromQueue(int id) => _queue.Remove(id);
    }
}