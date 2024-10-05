using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class VudMood : ICommand
    {
        public string Command => "vudmood";
        public string[] Aliases => new string[] { };
        public string Description => "Для FX. Изменяет состояние игрока. Использование: vudmood <id> <новое состояние>";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Использование: vudmood <id> <новое состояние>";
                return false;
            }
            var args = arguments.ToArray();
            if (!Player.TryGet(args[0], out var player))
            {
                response = $"Не удалось найти игрока.";
                return false;
            }
            var mood = string.Join(" ", args.Skip(1));
            player.SessionVariables.Remove("vudmood");
            player.SessionVariables.Add("vudmood", mood);
            response = "Игроку назначено новое состояние.";
            return true;
        }
    }
}