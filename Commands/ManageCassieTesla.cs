using System;
using System.Linq;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ManageCassieTesla : ICommand
    {
        public string Command => "managecassietesla";

        public string[] Aliases => new[] { "mctesla" };

        public string Description =>
            "Управление тесла-гейтами с CASSIE. Использование: mctesla on/off. Работает только с FX.";

        public bool Execute(
            ArraySegment<string> arguments,
            ICommandSender sender,
            out string response)
        {
            if (VeryUsualDay.Instance == null ||
                !VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён.";
                return false;
            }

            var args = arguments.ToArray();

            if (args.Length < 1 ||
                (!string.Equals(args[0], "on", StringComparison.OrdinalIgnoreCase) &&
                 !string.Equals(args[0], "off", StringComparison.OrdinalIgnoreCase)))
            {
                response = "Использование: mctesla on/off";
                return false;
            }

            bool isEnabled =
                string.Equals(args[0], "on", StringComparison.OrdinalIgnoreCase);

            VeryUsualDay.Instance.IsTeslaEnabled = isEnabled;

            string cassieMessage = isEnabled
                ? "$PITCH_0.6 .G4 .G2 .G2 . . $PITCH_1.99 .G3 .G3 .G3 .G3 $PITCH_0.20 . . . . $PITCH_1.99 .G3 .G3 .G3 .G3"
                : "$PITCH_0.6 .G4 $PITCH_0.55 .G4 $PITCH_0.45 .G4 . . $PITCH_1.99 .G3 .G3 .G3 .G3 $PITCH_0.20 . . . . $PITCH_1.99 .G3 .G3 .G3 .G3";

            string translation = isEnabled
                ? @"[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>

<b>|<color=#EFC01A>⚡</color><color=#32CD32>ON</color>| <color=#FD8272>Тесла-Ворота</color></b> были активированы <b><color=#7a7a7a>|📟</color><color=#eb8f34>💻</color><color=#7a7a7a>|</color></b> Инициатор: <b><color=#eb8f34>А.С.К.К.</color></b>"
                : @"[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>

<b>|<color=#EFC01A>⚡</color><color=#8B0000>OFF</color>| <color=#FD8272>Тесла-Ворота</color></b> были деактивированы <b><color=#7a7a7a>|📟</color><color=#eb8f34>💻</color><color=#7a7a7a>|</color></b> Инициатор: <b><color=#eb8f34>А.С.К.К.</color></b>";

            global::Exiled.API.Features.Cassie.MessageTranslated(
                message: cassieMessage,
                translation: translation,
                isHeld: false,
                isNoisy: false,
                isSubtitles: true);

            response = isEnabled
                ? "Тесла-ворота активированы с CASSIE-оповещением."
                : "Тесла-ворота деактивированы с CASSIE-оповещением.";

            return true;
        }
    }
}
