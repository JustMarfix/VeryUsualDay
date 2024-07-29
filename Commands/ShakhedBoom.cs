using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ShakhedBoom : ICommand
    {
        public string Command => "boom";
        public string[] Aliases => new string[] { };
        public string Description => "Для Foundation-X. Позволяет взорваться, если на вас пояс шахида.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }

            var playerSender = Player.Get(sender);
            if (!VeryUsualDay.Instance.Shakheds.Contains(playerSender.Id))
            {
                response = "Вы не носите пояс шахида.";
                return false;
            }
            playerSender.Explode(ProjectileType.FragGrenade, playerSender);
            VeryUsualDay.Instance.Shakheds.Remove(playerSender.Id);
            response = "Бабах.";
            return true;
        }
    }
}