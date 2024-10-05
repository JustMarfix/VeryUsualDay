using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Utils
{
    public class Worker
    {
        private Player User { get; set; }

        public Worker(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.ClassD, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(2f, () =>
            {
                User.ClearInventory();
                User.MaxHealth = 110f;
                User.Health = 110f;
                User.AddItem(ItemType.KeycardJanitor);
                User.AddItem(ItemType.Flashlight);
                User.CustomName = $"Рабочий - ##-{VeryUsualDay.Instance.SpawnedWorkersCounter}";
                User.CustomInfo = "Человек";
                User.Broadcast(10, "<b>Вы вступили в отдел <color=#FF9966>Рабочих</color>! Работайте в <color=#ffa8af>столовой комплекса</color> и следите за порядком в <color=#98FB98>коридорах</color>.");
                VeryUsualDay.Instance.SpawnedWorkersCounter += 1;
            });

        }
    }
}