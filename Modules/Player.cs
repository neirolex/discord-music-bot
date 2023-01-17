using Vlc.DotNet.Core;

namespace discord_music_bot
{
    public class Player
    {
        private static DirectoryInfo libDirectory = new DirectoryInfo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        VlcMediaPlayer mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(libDirectory);
        private IList<AudioFile> _queue = new List<AudioFile>();
        private int _currentTrackId;

        public Player() { 
            _currentTrackId = 0;

            AudioFile track1 = new AudioFile("C:\\_work\\_repo\\discord-music-bot\\tracks\\track1.mp3");
            AddToQueue(track1);

            AudioFile track2 = new AudioFile("C:\\_work\\_repo\\discord-music-bot\\tracks\\track2.mp3");
            AddToQueue(track2);

            // while(true) {
            //     var command = Console.ReadLine();
            //     switch(command) {
            //         case "play": {
            //             player.Play();
            //             break;
            //         }
            //         case "pause": {
            //             player.Pause();
            //             break;
            //         }
            //         case "stop": {
            //             player.Stop();
            //             break;
            //         }
            //         case "next": {
            //             player.Next();
            //             break;
            //         }
            //         case "prev": {
            //             player.Prev();
            //             break;
            //         }
            //     }
            // }

        }
        public void Play() {
            mediaPlayer.SetMedia(_queue[_currentTrackId].GetFileInfo());
            mediaPlayer.Play();
        }
        public void Stop() => mediaPlayer.Stop();
        public void Pause() => mediaPlayer.Pause();
        public void Next() {
            if(_currentTrackId+1 < _queue.Count) {
                _currentTrackId++;
                mediaPlayer.SetMedia(_queue[_currentTrackId].GetFileInfo());
                mediaPlayer.Play();
            }
        }
        public void Prev() {
            if(_currentTrackId > 0) {
                _currentTrackId--;
                mediaPlayer.SetMedia(_queue[_currentTrackId].GetFileInfo());
                mediaPlayer.Play();
            }
        }
        public void AddToQueue(AudioFile track) => _queue.Add(track);
        public void RemoveFromQueue(int id) => _queue.RemoveAt(id);
    }
}