using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Linq;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudjanitor : ICommand
    {
        public string Command { get; set; } = "vudjanitor";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Спавнит стажёра-уборщика на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (arguments.Array.Length < 2)
            {
                response = "Формат команды: vudjanitor <id через пробел>.";
                return false;
            }
            foreach (string id in arguments.Array.Skip(1).ToList())
            {
                if (Player.TryGet(id, out Player janitor))
                {
                    janitor.Role.Set(PlayerRoles.RoleTypeId.ClassD, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        janitor.ClearInventory();
                        janitor.MaxHealth = 100f;
                        janitor.Health = 100f;
                        janitor.AddItem(ItemType.KeycardJanitor);
                        janitor.AddItem(ItemType.Flashlight);
                        janitor.CustomName = $"Уборщик - ##-{VeryUsualDay.Instance.SpawnedJanitorsCounter}";
                        janitor.CustomInfo = "Человек";
                        janitor.Broadcast(10, "<b>Вы вступили в отдел <color=#FF9966>Уборщиков</color>! Работайте в <color=#ffa8af>столовой комплекса</color> и следите за порядком в <color=#98FB98>коридорах</color>.");
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
