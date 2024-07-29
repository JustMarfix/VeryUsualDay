using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Shakhed : ICommand
    {
        public string Command => "shakhed";
        public string[] Aliases => new string[] { };
        public string Description => "Для FX. Надевает/снимает с игрока пояс шахида. Использование: shakhed [id]";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count != 1)
            {
                response = "Использование: shakhed [id]";
                return false;
            }
            var args = arguments.ToArray();
            if (!Player.TryGet(args[0], out var player))
            {
                response = "Не удалось найти игрока.";
                return false;
            }

            if (VeryUsualDay.Instance.Shakheds.Contains(player.Id))
            {
                VeryUsualDay.Instance.Shakheds.Remove(player.Id);
                response = "Игрок больше не носит шахид-пояс!";
                return true;
            }
            VeryUsualDay.Instance.Shakheds.Add(player.Id);
            response = "Игрок теперь носит шахид-пояс.";
            return true;
        }
    }
}