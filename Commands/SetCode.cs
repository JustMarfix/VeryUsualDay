﻿using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetCode : ICommand
    {
        public string Command => "setcode";
        public string[] Aliases => new [] { "code" };
        public string Description => "Установить код в комплексе. Используется для СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (arguments.Count != 1)
            {
                response = "Формат команды: setcode <название>. Допустимые названия: green, emerald, blue, orange, yellow, red.";
                return false;
            }
            switch (arguments.ToArray()[0])
            {
                case "green":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#32CD32>Зелёный Код</color>. Сотрудникам работать в штатном режиме. <size=0> pitch_0.1 .G2 . pitch_1.0 . . . . . . . . . . . . . .", isSubtitles: true, isNoisy: false);
                    foreach (var ragdoll in Ragdoll.List.ToList())
                    {
                        ragdoll.Destroy();
                    }
                    response = "Установлен код \"Зелёный\"!";
                    return true;
                case "emerald":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Emerald;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#50C878>Изумрудный Код</color>. Замечены сбои в системе. Возможны поломки или нарушения в зонах содержания. Службе Безопасности быть на готове. <size=0> pitch_0.35 .G3 .G3 .G1 .G2 . pitch_1.0 . . . . . . . . . . . . . .\r\n", isSubtitles: true, isNoisy: false);
                    foreach (var ragdoll in Ragdoll.List.ToList())
                    {
                        ragdoll.Destroy();
                    }
                    response = "Установлен код \"Изумрудный\"!";
                    return true;
                case "blue":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Blue;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#005EBC>Синий Код</color>. Зафиксированы малые нарушения. Персоналу следует принимать меры предосторожности. <size=0> pitch_0.1 .G1 .G2 . pitch_1.0 . . . . . . . . . . . . . .", isSubtitles: true, isNoisy: false);
                    response = "Установлен код \"Синий\"!";
                    return true;
                case "orange":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Orange;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#EE7600>Оранжевый Код</color>. В комплексе зафиксированы нарушения, превышающие слабый уровень опасности. Всем боевым единицам приступить к ликвидации угрозы или принять меры для восстановления безопасной обстановки. <b><color=#002DB3>ЭВС</color></b> Разрешено войти в подземную часть. <size=0> pitch_0.15 .G6 pitch_0.08 .G1 .G3 . pitch_1.0 . . . . . . . . . . . . . .", isSubtitles: true, isNoisy: false);
                    response = "Установлен код \"Оранжевый\"!";
                    return true;
                case "yellow":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Yellow;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#EFC01A>Жёлтый Код</color>. Возможно включение <b><color=#FD8272>Тесла-Ворот</b></color>. Службе безопасности приступить к ликвидации угрозы или принять меры для восстановления безопасной обстановки. <b><color=#002DB3>ЭВС</color></b> Разрешено войти в подземную часть. <size=0> pitch_0.1 .G3 .G1 . pitch_1.0 . . . . . . . . . . . . . .", isSubtitles: true, isNoisy: false);
                    response = "Установлен код \"Жёлтый\"!";
                    return true;
                case "red":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Red;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#C50000>Красный Код</color>. Всем мирным сотрудникам пройти на поверхность до устранения основных угроз. Всем боевым единицам принять действия устранения опасности. <size=0> pitch_0.1 .G5 . .G5 . .G5 . .G1 . pitch_1.0 . . . . . . . . . . . . . .", isSubtitles: true, isNoisy: false);
                    response = "Установлен код \"Красный\"!";
                    return true;
                default:
                    response = "Формат команды: setcode <название>. Допустимые названия: green, emerald, blue, yellow, red.";
                    return false;
            }
        }
    }
}
