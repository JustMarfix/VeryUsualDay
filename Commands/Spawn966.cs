using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn966 : ICommand
    {
        public string Command => "spawn966";

        public string[] Aliases => new string[] { };

        public string Description => "Работает при FX. Спавнит SCP-966.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp966))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    var human = new TutorialHuman(scp966);
                    response = "SCP удалён!";
                    return true;
                }

                var scp = new Scp966(scp966);
                response = "SCP-966 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
