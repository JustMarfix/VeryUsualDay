﻿using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawn008_2 : ICommand
    {
        public string Command { get; set; } = "spawn008-2";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит SCP-008-2.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.ToArray()[1]);
            if (Player.TryGet(id, out Player scp0082))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp0082.MaxHealth = 100f;
                    scp0082.CustomInfo = "Человек";
                    scp0082.DisableEffect(EffectType.Stained);
                    scp0082.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp0082.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp0082.Role.Set(RoleTypeId.Scp0492, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp0082.CustomInfo = "<b><color=#960018>SCP-008-2</color></b>";
                    scp0082.MaxHealth = 1850f;
                    scp0082.Health = 1850f;
                    scp0082.Scale = new Vector3(1f, 1f, 1f);
                    scp0082.EnableEffect(EffectType.Stained);
                    scp0082.IsGodModeEnabled = false;
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp0082);
                });
                response = "SCP-008-2 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
