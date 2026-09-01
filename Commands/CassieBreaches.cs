using System;
using System.Linq;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CassieBreaches : ICommand
    {
        public string Command => "breach";

        public string[] Aliases => new[] { "br" };

        public string Description =>
            "Отправляет CASSIE-оповещение об аномальной угрозе. Использование: breach euclid/keter/apollyon/unknown";

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

            if (args.Length < 1)
            {
                response = "Использование: breach euclid/keter/apollyon/unknown";
                return false;
            }

            string cassieMessage;
            string translation;

            switch (args[0].ToLowerInvariant())
            {
                case "euclid":
                    cassieMessage =
                        "$PITCH_0.2 .G4. .G4 . .G6 .";

                    translation =
                        @"[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>

<b><color=#727472>[Система предупреждения]</color></b>

<size=35><color=#C50000><b>|⚠️👽⚠️|</b></color></size><size=25> Опасность! На территории учреждения зафиксирована аномальная угроза
|⚠️<color=#FF9966>🔘</color>| Класс объекта - <color=#FF9966><u>Евклид</u></color></size>";
                    break;


                case "keter":
                    cassieMessage =
                        "$PITCH_0.2 .G4. .G4 . $pitch_0.1 .G3 .";

                    translation =
@"[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>

<b><color=#727472>[Система предупреждения]</color></b>

<size=35><color=#C50000><b>|⚠️👽⚠️|</b></color></size><size=25> Опасность! На территории учреждения зафиксирована аномальная угроза
|⚠️<color=#960018>🔘</color>| Класс объекта - <color=#960018><b><u>Кетер</u></b></color></size>";
                    break;


                case "apollyon":
                    cassieMessage =
                        "$PITCH_0.1 .G4 . .G4 . $PITCH_0.05 .G6 .";

                        translation =
    @"[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>

<b><color=#727472>[Система предупреждения]</color></b>

<size=35><color=#C50000><b>|⚠️👽⚠️|</b></color></size><size=25> Опасность! На территории учреждения зафиксирована аномальная угроза
|⚠️<color=#8137CE>🔘</color>| Класс объекта - <color=#8137CE><b><u>Аполлион</u></b></color></size>";
                    break;


                case "unknown":
                    cassieMessage =
"$PITCH_0.2 .G4. .G4 . $PITCH_3.00 .G5  .G5 .G5 .G5 .G5 .G5 .G5 .G5 .G5 .G5 $PITCH_0.6 . . . . . $PITCH_3.00 .G5 .G5 .G5 .G5 .G5 .G5 .G5 .G5 .G5 $PITCH_0.5 .G6";

                        translation =
    @"[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>

<b><color=#727472>[Система предупреждения]</color></b>

<size=35><color=#C50000><b>|⚠️👽⚠️|</b></color></size><size=25> Опасность! На территории учреждения зафиксирована аномальная угроза
|⚠️<color=#98FB98>❔</color>| Класс объекта - <color=#98FB98><b>[не установлено]</b></color></size>";
                    break;

                default:
                    response =
                        "Неизвестный класс объекта. Использование: breach euclid/keter/apollyon/unknown";
                    return false;
            }

            global::Exiled.API.Features.Cassie.MessageTranslated(
                message: cassieMessage,
                translation: translation,
                isHeld: false,
                isNoisy: false,
                isSubtitles: true);

            response =
                $"CASSIE-оповещение о бриче класса {args[0]} отправлено.";

            return true;
        }
    }
}