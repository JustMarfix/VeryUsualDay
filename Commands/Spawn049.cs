using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn049 : ICommand
    {
        public string Command => "spawn049";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит SCP-049.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp049))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    var human = new TutorialHuman(scp049);
                    response = "SCP удалён!";
                    return true;
                }

                var scp = new Scp049(scp049);
                response = "SCP-049 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
