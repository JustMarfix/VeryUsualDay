using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Utils
{
    public class Scp966
    {
        private Player User { get; set; }

        public Scp966(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.Scp0492, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(2f, () =>
            {
                User.CustomInfo = "<b><color=#960018>SCP-966</color></b>";
                User.MaxHealth = 1000f;
                User.Health = 1000f;
                User.Scale = new Vector3(0f, 1f, 0f);
                User.IsGodModeEnabled = false;
                VeryUsualDay.Instance.ScpPlayers.Add(User.Id, VeryUsualDay.Scps.Scp966);
            });

        } 
    }
}