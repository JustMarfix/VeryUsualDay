using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Utils
{
    public class TutorialHuman
    {
        private Player User { get; set; }

        public TutorialHuman(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
            Timing.CallDelayed(1.5f, () =>
            {
                User.MaxHealth = 100f;
                User.Health = 100f;
                User.CustomName = null;
                User.CustomInfo = "Человек";
                User.Scale = new Vector3(1f, 1f, 1f);
                User.IsGodModeEnabled = false;
                if (VeryUsualDay.Instance.Zombies.Contains(User.Id))
                {
                    User.UnMute();
                    VeryUsualDay.Instance.Zombies.Remove(User.Id);
                }
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(User.Id))
                {
                    VeryUsualDay.Instance.ScpPlayers.Remove(User.Id);
                }
                if (VeryUsualDay.Instance.DBoysQueue.Contains(User.Id))
                {
                    VeryUsualDay.Instance.DBoysQueue.Remove(User.Id);
                }
                if (VeryUsualDay.Instance.JoinedDboys.Contains(User.Id))
                {
                    VeryUsualDay.Instance.JoinedDboys.Remove(User.Id);
                }
                if (VeryUsualDay.Instance.Shakheds.Contains(User.Id))
                {
                    VeryUsualDay.Instance.Shakheds.Remove(User.Id);
                }

            });
        }
    }
}