using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudguard : ICommand
    {
        public string Command { get; set; } = "vudguard";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Спавнит СБ-стажёра на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player guard))
            {
                guard.Role.Set(PlayerRoles.RoleTypeId.FacilityGuard, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    guard.MaxHealth = 100f;
                    guard.Health = 100f;
                    guard.ClearInventory();
                    guard.AddItem(ItemType.GunCOM15);
                    guard.AddItem(ItemType.KeycardScientist);
                    guard.AddItem(ItemType.Painkillers);
                    guard.CustomName = $"Охранник - ##-{VeryUsualDay.Instance.SpawnedSecurityCounter}";
                    guard.CustomInfo = "Человек";
                    guard.Broadcast(10, "<b>Вы вступили в <color=#727472>Службу Безопасности</color>! Патрулируйте <color=#120a8f>комплекс</color>, сохраняйте безопасную обстановку в <color=#ffa000>подземной части</color>.");
                    VeryUsualDay.Instance.SpawnedSecurityCounter += 1;
                });
                response = "СБ заспавнен успешно!";
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
