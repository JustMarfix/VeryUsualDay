using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AShield : ICommand
    {
        public string Command => "ashield";
        public string[] Aliases => null;
        public string Description => "Использование: ashield id float. Добавляет scp shield игроку.";
        
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
            
            player.HumeShield = float.Parse(args[1]);
            response = "Успешно установлено.";
            return true;
        }
    }
}