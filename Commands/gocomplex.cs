using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;
using System.Linq;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class gocomplex : ICommand
    {
        public string Command { get; set; } = "gocomplex";

        public string[] Aliases { get; set; } = { "gocm" };

        public string Description { get; set; } = "Для СОД. Отправляет людей на поверхность и кидает CASSIE.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён";
                return false;
            }
            if (arguments.Array.Length < 2)
            {
                response = "Формат команды: gocomplex <id через пробел>.";
                return false;
            }
            foreach (string id in arguments.Array.Skip(1))
            {
                Player player = Player.Get(int.Parse(id));
                Vector3 pos = Door.Get(DoorType.SurfaceGate).Position;
                pos.x -= 2f;
                pos.y += 1f;
                player.Teleport(pos);
            }
            Cassie.Message("<b><color=#EE7600>[ПЕРСОНАЛ]</color></b> сотрудники прибыли на поверхность комплекса. <size=0> .G1 . . . . . pitch_0.2 .G5 . . . . . ", isNoisy: false, isSubtitles: true);
            response = "Персонал заспавнен";
            return true;
        }
    }
}
