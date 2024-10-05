using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Utils
{
    public class Scp01921
    {
        private Player User { get; set; }

        public Scp01921(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.Scp0492, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(2f, () =>
            {
                var hp = Random.Range(500, 1700);
                User.CustomInfo = "<b><color=#960018>SCP-019-2</color></b>";
                User.CustomName = "Объект";
                User.MaxHealth = hp;
                User.Health = hp;
                User.Scale = new Vector3(0.6f, 0.6f, 0.6f);
                User.IsGodModeEnabled = false;
                User.Teleport(Room.Get(RoomType.Lcz173).Position + new Vector3(20.193f, 13.7f, 7.638f));
                VeryUsualDay.Instance.ScpPlayers.Add(User.Id, VeryUsualDay.Scps.Scp01921);
            });

        }
    }
}