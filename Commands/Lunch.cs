using System;
using CommandSystem;
using MEC;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Lunch : ICommand
    {
        public string Command => "lunch";
        public string[] Aliases => new string[] { };
        public string Description => "Начинает или принудительно заканчивает обед. Сделано для FX.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (VeryUsualDay.Instance.IsLunchtimeActive)
            {
                Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.5 .G1 $PITCH_0.5 . . . . . . . . . . . . . . . .G1 .G1", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>Руководство Комплекса</color></b>\r\n\r\n<b><color=#727472>[Время персонала]</color></b> \r\n\r\n|🍜<color=#EE7600>⌛</color>| Обеденный <u>перерыв окончен</u>\r\n|<color=#BC8F8F>👤</color><color=#32CD32>✅</color>| Персонал продолжает работу в штатном режиме </color></b>", isNoisy: false, isSubtitles: true, isHeld: false);
                VeryUsualDay.Instance.IsLunchtimeActive = false;
                response = "Обед отменён досрочно!";
                return true;
            }

            VeryUsualDay.Instance.IsLunchtimeActive = true;
            Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.4 .G3 .G3 . $PITCH_0.8 . . . . . . . .G1 . . . . $PITCH_0.4 . . . . . .G3", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>Руководство Комплекса</color></b>\r\n\r\n<b><color=#727472>[Время персонала]</color></b> \r\n\r\n|👤<color=#EE7600>🍜</color>| Объявлен <b><color=#EE7600><u>Обеденный перерыв</u></color></b>! \r\n|<color=#EE7600>⌛</color>5| Длительность - <size=0><size=25>5 минут\r\n|<color=#BC8F8F>👤</color>⌛| После принятия пищи персонал имеет свободное время до окончания перерыва</b></color>", isNoisy: false, isSubtitles: true, isHeld: false);
            Timing.CallDelayed(300f, () =>
            {
                if (!VeryUsualDay.Instance.IsLunchtimeActive) return;
                Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.5 .G1 $PITCH_0.5 . . . . . . . . . . . . . . . .G1 .G1", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>Руководство Комплекса</color></b>\r\n\r\n<b><color=#727472>[Время персонала]</color></b> \r\n\r\n|🍜<color=#EE7600>⌛</color>| Обеденный <u>перерыв окончен</u>\r\n|<color=#BC8F8F>👤</color><color=#32CD32>✅</color>| Персонал продолжает работу в штатном режиме </color></b>", isNoisy: false, isSubtitles: true, isHeld: false);
                VeryUsualDay.Instance.IsLunchtimeActive = false;
            });
            response = "Обед объявлен!";
            return true;
        }
    }
}
