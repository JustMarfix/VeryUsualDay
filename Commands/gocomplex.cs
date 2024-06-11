﻿using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class gocomplex : ICommand
    {
        public string Command => "gocomplex";
        public string[] Aliases => new [] { "gocm" };
        public string Description => "Для FX. Отправляет людей на поверхность и кидает CASSIE.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим КМ не включён";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: gocomplex <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray())
            {
                if (!Player.TryGet(int.Parse(id), out var player))
                {
                    response = $"Человека с ID {id} нету на сервере.";
                    return false;
                }
                var pos = VeryUsualDay.Instance.SpawnPosition;
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
