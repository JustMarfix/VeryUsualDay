using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawn035_2 : ICommand
    {
        public string Command { get; set; } = "spawn035-2";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит SCP-035-2.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player scp0352))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp0352.MaxHealth = 100f;
                    scp0352.CustomInfo = "Человек";
                    scp0352.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp0352.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }
                else
                {
                    Timing.CallDelayed(2f, () =>
                    {
                        scp0352.CustomInfo = "<b><color=#960018>SCP-035-2</color></b>";
                        scp0352.MaxHealth = 250f;
                        scp0352.Health = 250f;
                        scp0352.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                        scp0352.IsGodModeEnabled = false;
                        scp0352.Broadcast(10, "Вы теперь подчиняетесь SCP-035.");
                        VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp0352);
                    });
                    response = "SCP-035-2 создан!";
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
