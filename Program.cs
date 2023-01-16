namespace discord_music_bot {
    public class Program
    {
        public static void Main(string[] args)
        {
            Player p = new Player();

            AudioFile track1 = new AudioFile("C:\\_work\\_repo\\discord-music-bot\\tracks\\track1.mp3");
            p.AddToQueue(track1);

            AudioFile track2 = new AudioFile("C:\\_work\\_repo\\discord-music-bot\\tracks\\track2.mp3");
            p.AddToQueue(track2);

            while(true) {
                var command = Console.ReadLine();

                switch(command) {
                    case "play": {
                        p.Play();
                        break;
                    }
                    case "pause": {
                        p.Pause();
                        break;
                    }
                    case "stop": {
                        p.Stop();
                        break;
                    }
                    case "next": {
                        p.Next();
                        break;
                    }
                    case "prev": {
                        p.Prev();
                        break;
                    }
                }
            }
        }
    }
}