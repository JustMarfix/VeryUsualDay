using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn035 : ICommand
    {
        public string Command => "spawn035";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит SCP-035.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp035))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    var human = new TutorialHuman(scp035);
                    response = "SCP удалён!";
                    return true;
                }

                var scp = new Scp035(scp035);
                response = "SCP-035 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
