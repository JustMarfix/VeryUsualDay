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
    public class spawn008_2 : ICommand
    {
        public string Command => "spawn008-2";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при СОД. Спавнит SCP-008-2.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp0082))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp0082.MaxHealth = 100f;
                    scp0082.CustomInfo = "Человек";
                    scp0082.DisableEffect(EffectType.Stained);
                    scp0082.DisableEffect(EffectType.Poisoned);
                    scp0082.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp0082.Scale = new Vector3(1f, 1f, 1f);
                    scp0082.UnMute();
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                scp0082.Role.Set(RoleTypeId.Scp0492, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scp0082.CustomInfo = "<b><color=#960018>SCP-008-2</color></b>";
                    scp0082.MaxHealth = 2075f;
                    scp0082.Health = 2075f;
                    scp0082.Scale = new Vector3(1f, 1f, 1f);
                    scp0082.EnableEffect(EffectType.Stained);
                    scp0082.EnableEffect(EffectType.Poisoned);
                    scp0082.IsGodModeEnabled = false;
                    scp0082.Broadcast(10, "<b>Вы стали <color=#DC143C>SCP-008-2</color>!\n<color=#960018>Атакуйте людей</color> до конца жизни!</b>");
                    scp0082.Mute();
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
