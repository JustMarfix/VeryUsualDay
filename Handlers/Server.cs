using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace VeryUsualDay.Handlers
{
    public static class Server
    {
        public static void OnWaitingForPlayers()
        {
            VeryUsualDay.Instance.IsEnabledInRound = false;
            VeryUsualDay.Instance.IsLunchtimeActive = false;
            VeryUsualDay.Instance.IsDboysSpawnAllowed = false;
            VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
            VeryUsualDay.Instance.BuoCounter = 0;
            VeryUsualDay.Instance.SpawnedDboysCounter = 1;
            VeryUsualDay.Instance.SpawnedWorkersCounter = 1;
            VeryUsualDay.Instance.SpawnedScientistCounter = 1;
            VeryUsualDay.Instance.SpawnedSecurityCounter = 1;
            VeryUsualDay.Instance.ScpPlayers.Clear();
            VeryUsualDay.Instance.JoinedDboys.Clear();
            VeryUsualDay.Instance.DBoysQueue.Clear();
            // Timing.KillCoroutines("_avel");
            Timing.KillCoroutines("_joining");
        }

        public static void OnRoundStarted()
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            Timing.CallDelayed(5f, () =>
            {
                VeryUsualDay.Instance.SupplyBoxCoords = Room.Get(RoomType.EzGateB).Position + new Vector3(-6.193f, 2.243f, -5.901f);
            });
        }
    }
}
