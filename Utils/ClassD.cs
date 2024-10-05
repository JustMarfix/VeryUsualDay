using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Utils
{
    public class ClassD
    {
        private Player User { get; set; }

        public ClassD(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.ClassD, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.All);
            Timing.CallDelayed(1f, () =>
            {
                User.ClearInventory();
                User.Handcuff();
                User.MaxHealth = 115f;
                User.Health = 115f;
                User.CustomName = $"Испытуемый - ##-{VeryUsualDay.Instance.SpawnedDboysCounter}";
                User.CustomInfo = "Человек";
                User.Broadcast(10, "<b>Вы стали <color=#EE7600>Испытуемым</color>! Можете сотрудничать с <color=#120a8f>фондом</color> или принимать попытки <color=#ff2b2b>побега</color> при первой возможности. </b>");
                VeryUsualDay.Instance.SpawnedDboysCounter += 1;
            });
        }
    }
}