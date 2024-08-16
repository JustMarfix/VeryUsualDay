using System;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CustomHelp : ICommand
    {
        public string Command => "help";
        public string[] Aliases => new[] { "hhelp", "help" };
        public string Description => "Описание команд.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            player.SendConsoleMessage(VeryUsualDay.Instance.Config.CustomHelp, "aqua");
            response = "";
            return false;
        }
    }
}