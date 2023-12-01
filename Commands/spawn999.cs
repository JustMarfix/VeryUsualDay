using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawn999 : ICommand
    {
        public string Command { get; set; } = "spawn999";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит SCP-999.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player scp999))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp999.MaxHealth = 100f;
                    scp999.CustomInfo = "Человек";
                    scp999.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: Exiled.API.Enums.SpawnReason.ForceClass);
                    scp999.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }
                else
                {
                    scp999.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        scp999.CustomInfo = "<b><color=#960018>SCP-999</color></b>";
                        scp999.MaxHealth = 10000f;
                        scp999.Health = 10000f;
                        scp999.Scale = new UnityEngine.Vector3(1f, 0.1f, 1f);
                        scp999.IsGodModeEnabled = false;
                        VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp999);
                    });
                    response = "SCP-999 создан!";
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
