﻿using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class VudMedic : ICommand
    {
        public string Command => "vudmedic";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит Медика Реагирования. Для Foundation-X.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudmedic <id через пробел>.";
                return false;
            }
            
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var medic))
                {
                    var mtf = new MtfMedic(medic);
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