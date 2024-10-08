﻿using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn0762 : ICommand
    {
        public string Command => "spawn076-2";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит Авеля.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }

            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var avel))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    var human = new TutorialHuman(avel);
                    response = "SCP удалён!";
                    return true;
                }

                var scp = new Scp0762(avel);
                response = "Авель создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}