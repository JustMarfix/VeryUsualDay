using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Utils
{
    public class Guard
    {
        private Player User { get; set; }
        
        public Guard(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.FacilityGuard, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(2f, () =>
            {
                User.MaxHealth = 100f;
                User.Health = 100f;
                User.ClearInventory();
                User.AddItem(ItemType.GunCOM15);
                User.AddItem(ItemType.KeycardJanitor);
                User.AddItem(ItemType.Painkillers);
                User.AddItem(ItemType.Radio);
                User.CustomName = $"Охранник - ##-{VeryUsualDay.Instance.SpawnedSecurityCounter}";
                User.CustomInfo = "Человек";
                User.Broadcast(10, "<b>Вы вступили в <color=#727472>Службу Безопасности</color>! Патрулируйте <color=#120a8f>комплекс</color>, сохраняйте безопасную обстановку в <color=#ffa000>подземной части</color>.");
                VeryUsualDay.Instance.SpawnedSecurityCounter += 1;
            });
        }
    }
}