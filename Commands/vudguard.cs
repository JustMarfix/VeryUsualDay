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
    public class vudguard : ICommand
    {
        public string Command => "vudguard";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит СБ-стажёра на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudguard <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var guard))
                {
                    guard.Role.Set(RoleTypeId.FacilityGuard, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        guard.MaxHealth = 100f;
                        guard.Health = 100f;
                        guard.ClearInventory();
                        guard.AddItem(ItemType.GunCOM15);
                        guard.AddItem(ItemType.KeycardJanitor);
                        guard.AddItem(ItemType.Painkillers);
                        guard.CustomName = $"Охранник - ##-{VeryUsualDay.Instance.SpawnedSecurityCounter}";
                        guard.CustomInfo = "Человек";
                        guard.Broadcast(10, "<b>Вы вступили в <color=#727472>Службу Безопасности</color>! Патрулируйте <color=#120a8f>комплекс</color>, сохраняйте безопасную обстановку в <color=#ffa000>подземной части</color>.");
                        VeryUsualDay.Instance.SpawnedSecurityCounter += 1;
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
