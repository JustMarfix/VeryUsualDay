using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class VudGuard : ICommand
    {
        public string Command => "vudguard";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит СБ-стажёра на FX.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudguard <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var guard))
                {
                    var security = new Guard(guard);
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
