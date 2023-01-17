using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            //Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            //Process the InteractionCreated payloads to execute Interactions commands
            _client.InteractionCreated += HandleInteraction;

            //Handle the command
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            //_commands.ContextCommandExecuted += ContextCommandExecuted;
            //_commands.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        // private Task ComponentCommandExecuted(ComponentCommandInfo commandInfo, Discord.IInteractionContext context, IResult result)
        // {
        //     if (!result.IsSuccess)
        //     {
        //         switch (result.Error)
        //         {
        //             case InteractionCommandError.UnmetPrecondition:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.UnknownCommand:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.BadArgs:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.Exception:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.Unsuccessful:
        //                 //TO DO: implement
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }    
        //     return Task.CompletedTask;
        // }

        // private Task ContextCommandExecuted(ContextCommandInfo commandInfo, Discord.IInteractionContext context, IResult result)
        // {
        //     if (!result.IsSuccess)
        //     {
        //         switch (result.Error)
        //         {
        //             case InteractionCommandError.UnmetPrecondition:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.UnknownCommand:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.BadArgs:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.Exception:
        //                 //TO DO: implement
        //                 break;
        //             case InteractionCommandError.Unsuccessful:
        //                 //TO DO: implement
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        //     return Task.CompletedTask;
        // }

        private Task SlashCommandExecuted(SlashCommandInfo commandInfo, Discord.IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        //TO DO: implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        //TO DO: implement
                        break;
                    case InteractionCommandError.BadArgs:
                        //TO DO: implement
                        break;
                    case InteractionCommandError.Exception:
                        //TO DO: implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        //TO DO: implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction (SocketInteraction arg)
        {
            try
            {
                //Create an execution context that matches the generic type parameter of InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                //response, or at least let the user know that something went wrong during the command execution.
                if(arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }