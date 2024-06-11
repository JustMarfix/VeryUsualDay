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
    public class spawn035 : ICommand
    {
        public string Command => "spawn035";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит SCP-035.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp035))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp035.MaxHealth = 100f;
                    scp035.CustomInfo = "Человек";
                    scp035.DisableEffect(EffectType.BodyshotReduction);
                    scp035.DisableEffect(EffectType.DamageReduction);
                    scp035.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp035.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp035.Role.Set(RoleTypeId.ClassD, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp035.CustomInfo = "<b><color=#960018>SCP-035</color></b>";
                    scp035.AddItem(ItemType.GunRevolver);
                    scp035.AddAmmo(AmmoType.Ammo44Cal, 32);
                    scp035.MaxHealth = 7500f;
                    scp035.Health = 7500f;
                    scp035.Scale = new Vector3(0.87f, 0.87f, 1f);
                    scp035.EnableEffect(EffectType.BodyshotReduction);
                    scp035.ChangeEffectIntensity(EffectType.BodyshotReduction, 15);
                    scp035.EnableEffect(EffectType.DamageReduction);
                    scp035.ChangeEffectIntensity(EffectType.DamageReduction, 15);
                    scp035.EnableEffect(EffectType.Poisoned);
                    scp035.IsGodModeEnabled = true;
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp035);
                });
                response = "SCP-035 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
