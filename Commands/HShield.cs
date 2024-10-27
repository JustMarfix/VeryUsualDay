using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class HShield : ICommand
    {
        public string Command => "hshield";
        public string[] Aliases => null;
        public string Description => "Использование: hshield id float. Добавляет human shield игроку.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }

            var args = arguments.ToArray();
            if (!Player.TryGet(args[0], out var player))
            {
                response = "Не удалось найти игрока.";
                return false;
            }
            
            player.AddAhp(float.Parse(args[1]), float.Parse(args[1]), 0f, 1f);
            response = "Успешно добавлено.";
            return true;
        }
    }
}