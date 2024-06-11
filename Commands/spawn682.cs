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
    public class spawn682 : ICommand
    {
        public string Command => "spawn682";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при FX. Спавнит SCP-682-MT.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp682))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp682.MaxHealth = 100f;
                    scp682.CustomInfo = "Человек";
                    scp682.DisableEffect(EffectType.Invigorated);
                    scp682.DisableEffect(EffectType.RainbowTaste);
                    scp682.DisableEffect(EffectType.Disabled);
                    scp682.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp682.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp682.Role.Set(RoleTypeId.Scp939, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp682.CustomInfo = "<b><color=#960018>SCP-682-MT</color></b>";
                    scp682.MaxHealth = 15000f;
                    scp682.Health = 15000f;
                    scp682.HumeShield = 5000f;
                    scp682.Scale = new Vector3(1.2f, 1.25f, 1.2f);
                    scp682.IsGodModeEnabled = false;
                    scp682.EnableEffect(EffectType.Invigorated);
                    scp682.EnableEffect(EffectType.RainbowTaste);
                    scp682.EnableEffect(EffectType.Disabled);
                    scp682.EnableEffect(EffectType.DamageReduction);
                    scp682.EnableEffect(EffectType.BodyshotReduction);
                    scp682.ChangeEffectIntensity(EffectType.DamageReduction, 30);
                    scp682.ChangeEffectIntensity(EffectType.BodyshotReduction, 30);
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp682);
                });
                response = "SCP-682 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
