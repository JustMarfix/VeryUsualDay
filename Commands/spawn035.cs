using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawn035 : ICommand
    {
        public string Command { get; set; } = "spawn035";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит SCP-035.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player scp035))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp035.MaxHealth = 100f;
                    scp035.CustomInfo = "Человек";
                    scp035.DisableEffect(Exiled.API.Enums.EffectType.BodyshotReduction);
                    scp035.DisableEffect(Exiled.API.Enums.EffectType.DamageReduction);
                    scp035.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: Exiled.API.Enums.SpawnReason.ForceClass);
                    scp035.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }
                else
                {
                    scp035.Role.Set(PlayerRoles.RoleTypeId.ClassD, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        scp035.CustomInfo = "<b><color=#960018>SCP-035</color></b>";
                        scp035.AddItem(ItemType.GunRevolver);
                        scp035.AddAmmo(Exiled.API.Enums.AmmoType.Ammo44Cal, 32);
                        scp035.MaxHealth = 7500f;
                        scp035.Health = 7500f;
                        scp035.Scale = new UnityEngine.Vector3(0.87f, 0.87f, 1f);
                        scp035.EnableEffect(Exiled.API.Enums.EffectType.BodyshotReduction);
                        scp035.ChangeEffectIntensity(Exiled.API.Enums.EffectType.BodyshotReduction, 15);
                        scp035.EnableEffect(Exiled.API.Enums.EffectType.DamageReduction);
                        scp035.ChangeEffectIntensity(Exiled.API.Enums.EffectType.DamageReduction, 15);
                        scp035.EnableEffect(Exiled.API.Enums.EffectType.Poisoned);
                        scp035.IsGodModeEnabled = true;
                        VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp035);
                    });
                    response = "SCP-035 создан!";
                    return true;
                }
            }
            else
            {
                response = "Не удалось найти игрока с таким ID!";
                return false;
            }
        }
    }
}
