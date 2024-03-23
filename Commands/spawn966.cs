using System;
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
    public class spawn966 : ICommand
    {
        public string Command { get; set; } = "spawn966";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит SCP-966.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.ToArray()[1]);
            if (Player.TryGet(id, out Player scp966))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp966.MaxHealth = 100f;
                    scp966.CustomInfo = "Человек";
                    scp966.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp966.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp966.Role.Set(RoleTypeId.Scp0492, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp966.CustomInfo = "<b><color=#960018>SCP-966</color></b>";
                    scp966.MaxHealth = 1000f;
                    scp966.Health = 1000f;
                    scp966.Scale = new Vector3(0f, 1f, 0f);
                    scp966.IsGodModeEnabled = false;
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp966);
                });
                response = "SCP-966 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
