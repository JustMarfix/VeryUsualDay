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
    public class spawn035_2 : ICommand
    {
        public string Command => "spawn035-2";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при СОД. Спавнит SCP-035-2.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp0352))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    scp0352.MaxHealth = 100f;
                    scp0352.CustomInfo = "Человек";
                    scp0352.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    scp0352.Scale = new Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }

                Timing.CallDelayed(2f, () =>
                {
                    scp0352.CustomInfo = "<b><color=#960018>SCP-035-2</color></b>";
                    scp0352.MaxHealth = 350f;
                    scp0352.Health = 350f;
                    scp0352.Scale = new Vector3(1f, 1f, 1f);
                    scp0352.IsGodModeEnabled = false;
                    scp0352.Broadcast(10, "Вы теперь подчиняетесь SCP-035.");
                    VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp0352);
                });
                response = "SCP-035-2 создан!";
                return true;
            }

            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}
