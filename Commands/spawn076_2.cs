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
    public class spawn076_2 : ICommand
    {
        public string Command => "spawn076-2";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при СОД. Спавнит Авеля.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var avel))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    avel.MaxHealth = 100f;
                    avel.CustomInfo = "Человек";
                    avel.DisableEffect(EffectType.MovementBoost);
                    avel.DisableEffect(EffectType.BodyshotReduction);
                    avel.DisableEffect(EffectType.DamageReduction);
                    avel.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    avel.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                avel.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    avel.CustomInfo = "<b><color=#960018>SCP-076-2</color></b>";
                    avel.MaxHealth = 15000f;
                    avel.Health = 15000f;
                    avel.EnableEffect(EffectType.MovementBoost);
                    avel.EnableEffect(EffectType.BodyshotReduction);
                    avel.EnableEffect(EffectType.DamageReduction);
                    avel.ChangeEffectIntensity(EffectType.MovementBoost, 25);
                    avel.ChangeEffectIntensity(EffectType.BodyshotReduction, 65);
                    avel.ChangeEffectIntensity(EffectType.DamageReduction, 65);
                    avel.CurrentItem = avel.AddItem(ItemType.Jailbird);
                    avel.Scale = new Vector3(1.15f, 1.15f, 1.15f);
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp0762);
                });
                response = "Авель создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
