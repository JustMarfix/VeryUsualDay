using CommandSystem;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PluginEnable : ICommand
    {
        public string Command { get; set; } = "veryusualday";

        public string[] Aliases { get; set; } = { "vudmode" };

        public string Description { get; set; } = "Не использовать, если не проводите Слишком Обычный День!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound)
            {
                VeryUsualDay.Instance.IsEnabledInRound = false;
                VeryUsualDay.Instance.Is008Leaked = false;
                VeryUsualDay.Instance.IsLunchtimeActive = false;
                VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
                VeryUsualDay.Instance.BUOCounter = 0;
                VeryUsualDay.Instance.LockerPlayers.Clear();
                VeryUsualDay.Instance.Avels.Clear();
                VeryUsualDay.Instance.Zombies.Clear();
                Timing.KillCoroutines("_avel");
                Timing.KillCoroutines("_008_poisoning");
                response = "Режим Очень Обычного Дня выключен.";
            }
            else
            {
                VeryUsualDay.Instance.IsEnabledInRound = true;
                Timing.RunCoroutine(VeryUsualDay.Instance._avel(), "_avel");
                response = "Режим Очень Обычного Дня включён.";
            }
            return true;
        }
    }
}
