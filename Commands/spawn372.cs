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
    public class spawn372 : ICommand
    {
        public string Command => "spawn372";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит SCP-372.";
        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp372))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp372.MaxHealth = 100f;
                    scp372.CustomInfo = "Человек";
                    scp372.DisableEffect(EffectType.MovementBoost);
                    scp372.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp372.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp372.Role.Set(RoleTypeId.Scp939, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp372.CustomInfo = "<b><color=#960018>SCP-372</color></b>";
                    scp372.MaxHealth = 650f;
                    scp372.Health = 650f;
                    scp372.Scale = new Vector3(0.1f, 0.8f, 1f);
                    scp372.EnableEffect(EffectType.MovementBoost);
                    scp372.ChangeEffectIntensity(EffectType.MovementBoost, 255);
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp372);
                });
                response = "SCP-372 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
