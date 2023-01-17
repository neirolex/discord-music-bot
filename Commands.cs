using Discord;
using Discord.Interactions;

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
        if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

        //For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
        var audioClient = await channel.ConnectAsync();
    }
}