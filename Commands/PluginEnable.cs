using System;
using CommandSystem;
using MEC;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PluginEnable : ICommand
    {
        public string Command => "veryusualday";
        public string[] Aliases => new [] { "vudmode" };
        public string Description => "Не использовать, если не проводите Слишком Обычный День!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound)
            {
                VeryUsualDay.Instance.IsEnabledInRound = false;
                VeryUsualDay.Instance.Is008Leaked = false;
                VeryUsualDay.Instance.IsLunchtimeActive = false;
                VeryUsualDay.Instance.IsDboysSpawnAllowed = false;
                VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
                VeryUsualDay.Instance.BuoCounter = 0;
                VeryUsualDay.Instance.SpawnedDboysCounter = 0;
                VeryUsualDay.Instance.SpawnedJanitorsCounter = 0;
                VeryUsualDay.Instance.SpawnedScientistCounter = 0;
                VeryUsualDay.Instance.SpawnedSecurityCounter = 0;
                VeryUsualDay.Instance.ScpPlayers.Clear();
                VeryUsualDay.Instance.Zombies.Clear();
                VeryUsualDay.Instance.JoinedDboys.Clear();
                VeryUsualDay.Instance.DBoysQueue.Clear();
                Timing.KillCoroutines("_avel");
                Timing.KillCoroutines("_008_poisoning");
                Timing.KillCoroutines("_joining");
                response = "Режим Очень Обычного Дня выключен.";
            }
            else
            {
                VeryUsualDay.Instance.IsEnabledInRound = true;
                Timing.RunCoroutine(VeryUsualDay.Instance._avel(), "_avel");
                Timing.RunCoroutine(VeryUsualDay.Instance._joining(), "_joining");
                response = "Режим Очень Обычного Дня включён.";
            }
            return true;
        }
    }
}
