using Vlc.DotNet.Core;

namespace discord_music_bot
{
    public class FilePlayer
    {
        private IList<AudioFile> _queue = new List<AudioFile>();
        private int _currentTrackId;

        public FilePlayer() { 
            _currentTrackId = 0;

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
        
        public IDictionary<int, AudioFile> ListQueue() {
            var result = new Dictionary<int, AudioFile>();
            for(int i = 0; i < _queue.Count; i++) {
                result.Add(i+1, _queue[i]);
            }
            return result;
        }

        public void SetCurrentTrack(int index) {
            _currentTrackId = index;
        }

        public AudioFile GetCurrentTrack() {
            return _queue[_currentTrackId];
        }
        
        public void AddToQueue(AudioFile track) => _queue.Add(track);
        
        public void RemoveFromQueue(int id) => _queue.RemoveAt(id);
    }
}