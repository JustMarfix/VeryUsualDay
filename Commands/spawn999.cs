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
    public class spawn999 : ICommand
    {
        public string Command => "spawn999";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при СОД. Спавнит SCP-999.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp999))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp999.MaxHealth = 100f;
                    scp999.CustomInfo = "Человек";
                    scp999.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp999.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp999.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp999.CustomInfo = "<b><color=#960018>SCP-999</color></b>";
                    scp999.MaxHealth = 10000f;
                    scp999.Health = 10000f;
                    scp999.Scale = new Vector3(1f, 0.1f, 1f);
                    scp999.IsGodModeEnabled = false;
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp999);
                });
                response = "SCP-999 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
