using MEC;

namespace VeryUsualDay.Handlers
{
    public class Server
    {
        public void OnWaitingForPlayers()
        {
            VeryUsualDay.Instance.IsEnabledInRound = false;
            VeryUsualDay.Instance.Is008Leaked = false;
            VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
            VeryUsualDay.Instance.BUOCounter = 0;
            VeryUsualDay.Instance.IsLunchtimeActive = false;
            VeryUsualDay.Instance.LockerPlayers.Clear();
            VeryUsualDay.Instance.Avels.Clear();
            VeryUsualDay.Instance.Zombies.Clear();
            Timing.KillCoroutines("_avel");
            Timing.KillCoroutines("_008_poisoning");
        }
    }
}
