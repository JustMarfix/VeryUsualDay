using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class VudWorker : ICommand
    {
        public string Command => "vudworker";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит стажёра-рабочего на FX.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudworker <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var worker))
                {
                    var player = new Worker(worker);
                }
                else
                {
                    response = "Не удалось найти игрока с таким ID!";
                    return false;
                }
            }
            response = "Игроки заспавнены.";
            return true;
        }
    }
}
