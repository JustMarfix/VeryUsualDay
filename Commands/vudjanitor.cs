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
    public class vudjanitor : ICommand
    {
        public string Command => "vudworker";

        public string[] Aliases => new string[] { };

        public string Description => "Спавнит стажёра-рабочего на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudworker <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray().Skip(1).ToList())
            {
                if (Player.TryGet(id, out var janitor))
                {
                    janitor.Role.Set(RoleTypeId.ClassD, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        janitor.ClearInventory();
                        janitor.MaxHealth = 100f;
                        janitor.Health = 100f;
                        janitor.AddItem(ItemType.KeycardJanitor);
                        janitor.AddItem(ItemType.Flashlight);
                        janitor.CustomName = $"Рабочий - ##-{VeryUsualDay.Instance.SpawnedJanitorsCounter}";
                        janitor.CustomInfo = "Человек";
                        janitor.Broadcast(10, "<b>Вы вступили в отдел <color=#FF9966>Рабочих</color>! Работайте в <color=#ffa8af>столовой комплекса</color> и следите за порядком в <color=#98FB98>коридорах</color>.");
                        VeryUsualDay.Instance.SpawnedJanitorsCounter += 1;
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
