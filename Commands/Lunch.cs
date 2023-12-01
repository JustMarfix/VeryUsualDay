using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Lunch : ICommand
    {
        public string Command { get; set; } = "lunch";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Начинает или принудительно заканчивает обед. Сделано для СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (VeryUsualDay.Instance.IsLunchtimeActive)
            {
                Cassie.Message("<b><color=#EE7600>Перерыв окончен!</color></b> <size=0> pitch_0.4 .G3 pitch_1.0 . . . .", isNoisy: false, isSubtitles: true);
                VeryUsualDay.Instance.IsLunchtimeActive = false;
                response = "Обед отменён досрочно!";
                return true;
            }
            else
            {
                VeryUsualDay.Instance.IsLunchtimeActive = true;
                Cassie.Message("<b><color=#EE7600>[Обеденный перерыв]: пять минут.</color></b> <size=0> pitch_0.4 .G1 . . .G1 .G1", isNoisy: false, isSubtitles: true);
                Timing.CallDelayed(300f, () =>
                {
                    if (VeryUsualDay.Instance.IsLunchtimeActive)
                    {
                        Cassie.Message("<b><color=#EE7600>Перерыв окончен!</color></b> <size=0> pitch_0.4 .G3 pitch_1.0 . . . .", isNoisy: false, isSubtitles: true);
                        VeryUsualDay.Instance.IsLunchtimeActive = false;
                    }
                });
                response = "Обед объявлен!";
                return true;
            }
        }
    }
}
