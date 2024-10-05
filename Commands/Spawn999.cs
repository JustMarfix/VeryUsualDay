using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn999 : ICommand
    {
        public string Command => "spawn999";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит SCP-999.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp999))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    var human = new TutorialHuman(scp999);
                    response = "SCP удалён!";
                    return true;
                }

                var scp = new Scp999(scp999);
                response = "SCP-999 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
