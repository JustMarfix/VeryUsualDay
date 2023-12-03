using MEC;

namespace VeryUsualDay.Handlers
{
    public class Server
    {
        public void OnWaitingForPlayers()
        {
            VeryUsualDay.Instance.IsEnabledInRound = false;
            VeryUsualDay.Instance.Is008Leaked = false;
            VeryUsualDay.Instance.IsLunchtimeActive = false;
            VeryUsualDay.Instance.IsDboysSpawnAllowed = false;
            VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
            VeryUsualDay.Instance.BUOCounter = 0;
            VeryUsualDay.Instance.SpawnedDboysCounter = 0;
            VeryUsualDay.Instance.SpawnedJanitorsCounter = 0;
            VeryUsualDay.Instance.SpawnedScientistCounter = 0;
            VeryUsualDay.Instance.SpawnedSecurityCounter = 0;
            VeryUsualDay.Instance.LockerPlayers.Clear();
            VeryUsualDay.Instance.ScpPlayers.Clear();
            VeryUsualDay.Instance.Zombies.Clear();
            VeryUsualDay.Instance.JoinedDboys.Clear();
            VeryUsualDay.Instance.DBoysQueue.Clear();
            Timing.KillCoroutines("_avel");
            Timing.KillCoroutines("_008_poisoning");
            Timing.KillCoroutines("_joining");
        }
    }
}
