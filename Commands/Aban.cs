using System;
using System.Linq;
using System.Text.RegularExpressions;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Aban : ICommand
    {
        public string Command => "aban";
        public string[] Aliases => new string[] {};
        public string Description => "Admin-ban для ивентов. Срок идёт только во время ивентов. Работает только на Foundation-X.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён";
                return false;
            }
            if (arguments.Count < 3)
            {
                response = "aban <id игрока> <срок в формате 1s/2m/3h/4d> <причина>";
                return false;
            }

            var args = arguments.ToArray();
            if (Player.TryGet(args[0], out var target))
            {
                if (!Regex.IsMatch(args[1], "^\\d+[smhd]$"))
                {
                    response = "Время введено в неправильном формате.";
                    return false;
                }
                var timeToPrison = VeryUsualDay.ConvertToTimeSpan(args[1]);
                if (VeryUsualDay.SendToPrison(target, Convert.ToInt32(timeToPrison.TotalSeconds), string.Join(" ", args.Skip(2))))
                {
                    response = $"Игрок {target.Nickname} был отправлен в тюрьму.";
                    return true;
                }

                response = "Не удалось отправить игрока в тюрьму - ошибка со стороны сервера.";
                return false;
            }
            
            response = "Не удалось найти игрока.";
            return false;
        }
    }
}