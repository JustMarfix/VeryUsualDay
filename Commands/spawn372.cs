using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawn372 : ICommand
    {
        public string Command { get; set; } = "spawn372";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит SCP-372.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player scp372))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp372.MaxHealth = 100f;
                    scp372.CustomInfo = "Человек";
                    scp372.DisableEffect(Exiled.API.Enums.EffectType.MovementBoost);
                    scp372.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: Exiled.API.Enums.SpawnReason.ForceClass);
                    scp372.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }
                else
                {
                    scp372.Role.Set(PlayerRoles.RoleTypeId.Scp939, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        scp372.CustomInfo = "<b><color=#960018>SCP-372</color></b>";
                        scp372.MaxHealth = 650f;
                        scp372.Health = 650f;
                        scp372.Scale = new UnityEngine.Vector3(0.1f, 0.8f, 1f);
                        scp372.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost);
                        scp372.ChangeEffectIntensity(Exiled.API.Enums.EffectType.MovementBoost, 255);
                        VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp372);
                    });
                    response = "SCP-372 создан!";
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
