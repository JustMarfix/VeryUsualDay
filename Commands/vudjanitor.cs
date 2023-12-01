using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

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
            int id = int.Parse(arguments.Array[1]);
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
                    janitor.Broadcast(10, "<b>Вы вступили в отдел <color=#FF9966>Уборщиков</color>! Работайте в <color=#ffa8af>столовой комплекса</color> и следите за порядком в <color=#98FB98>коридорах</color>.");
                    VeryUsualDay.Instance.SpawnedJanitorsCounter += 1;
                });
                response = "Уборщик заспавнен успешно!";
                return true;
            }
            else
            {
                response = "Не удалось найти игрока с таким ID!";
                return false;
            }
        }
    }
}
