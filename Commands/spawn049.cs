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
    public class spawn049 : ICommand
    {
        public string Command => "spawn049";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при СОД. Спавнит SCP-049.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp049))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp049.MaxHealth = 100f;
                    scp049.CustomInfo = "Человек";
                    scp049.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp049.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp049.Role.Set(RoleTypeId.Scp049, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp049.CustomInfo = "<b><color=#960018>SCP-049</color></b>";
                    scp049.MaxHealth = 13000f;
                    scp049.Health = 13000f;
                    scp049.Scale = new Vector3(1f, 1f, 1f);
                    scp049.IsGodModeEnabled = false;
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp049);
                });
                response = "SCP-049 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
