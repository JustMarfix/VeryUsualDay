using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudworker : ICommand
    {
        public string Command => "vudworker";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит стажёра-рабочего на FX.";
        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudworker <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var worker))
                {
                    worker.Role.Set(RoleTypeId.ClassD, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        worker.ClearInventory();
                        worker.MaxHealth = 110f;
                        worker.Health = 110f;
                        worker.AddItem(ItemType.KeycardJanitor);
                        worker.AddItem(ItemType.Flashlight);
                        worker.CustomName = $"Рабочий - ##-{VeryUsualDay.Instance.SpawnedWorkersCounter}";
                        worker.CustomInfo = "Человек";
                        worker.Broadcast(10, "<b>Вы вступили в отдел <color=#FF9966>Рабочих</color>! Работайте в <color=#ffa8af>столовой комплекса</color> и следите за порядком в <color=#98FB98>коридорах</color>.");
                        VeryUsualDay.Instance.SpawnedWorkersCounter += 1;
                    });
                }
                else
                {
                    response = "Не удалось найти игрока с таким ID!";
                    return false;
                }
            }
            response = "Игроки заспавнены.";
            return true;
        }
    }
}
