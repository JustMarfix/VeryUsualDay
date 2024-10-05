using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Utils
{
    public class Scientist
    {
        private Player User { get; set; }

        public Scientist(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.Scientist, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(2f, () =>
            {
                User.ClearInventory();
                User.MaxHealth = 100f;
                User.Health = 100f;
                User.AddItem(ItemType.KeycardJanitor);
                User.AddItem(ItemType.Painkillers);
                User.AddItem(ItemType.Flashlight);
                User.AddItem(ItemType.Radio);
                User.CustomName = $"Сотрудник - ##-{VeryUsualDay.Instance.SpawnedScientistCounter}";
                User.CustomInfo = "Человек";
                User.Broadcast(10, "<b>Вы вступили в <color=#ffd800>Научный</color> отдел! Исследуйте и сдерживайте <color=red>аномалии</color>, помогайте работе <color=#120a8f>фонда</color>.");
                VeryUsualDay.Instance.SpawnedScientistCounter += 1;
            });

        }
    }
}