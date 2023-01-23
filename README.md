* For transmitting audio, following libs are required;
    Sodium and Opus (Both libraries must be placed in the runtime directory of the bot (bin/debug or where "dotnet run" start from))
        Libs for Windows can be downloaded here: https://github.com/discord-net/Discord.Net/tree/dev/voice-natives;
        Libs for Linux: 
            Sodium https://download.libsodium.org/libsodium/releases/; 
            Opus https://ftp.osuosl.org/pub/xiph/releases/opus/;
    
    Ffmpeg 
        Libs for Windows and Linux can be download here: https://ffmpeg.org/download.html

* Resources used
    Discord.NET - general API for comunacting with Discord API
    ffmpeg - libraries for working with audio
    Microsoft.AspNetCore - general hosting and processing framework