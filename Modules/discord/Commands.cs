using Discord;
using Discord.Interactions;
using Discord.Audio;

public class Commands : InteractionModuleBase<SocketInteractionContext>
{
    //Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
    public InteractionService InteractionService { get; set; }
    private CommandHandler _handler;

    //Constructor injection is also a valid way to access the dependecies
    public Commands (CommandHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("join", "join", runMode: Discord.Interactions.RunMode.Async)]
    public async Task JoinChannel(IVoiceChannel channel = null)
    {
        //Get the audio channel
        channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
        if (channel == null) { await Context.Channel.SendMessageAsync("You must be in a voice channel."); return; }

        //For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
        var audioClient = await channel.ConnectAsync();
        await RespondAsync($"Bot joined to the channel {channel.Name}");
    }

    [SlashCommand("leave", "leave", runMode: Discord.Interactions.RunMode.Async)]
    public async Task LeaveChannel(IVoiceChannel channel = null)
    {
        //Get the audio channel
        channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
        //For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
        await channel.DisconnectAsync();
        await RespondAsync($"Bot left the channel");
    }
}