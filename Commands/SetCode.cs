using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using PlayerRoles;
namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetCode : ICommand
    {
        public string Command => "setcode";
        public string[] Aliases => new [] { "code" };
        public string Description => "Установить код в комплексе. Используется для FX.";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (VeryUsualDay.Instance.IsCleanCountdownActive)
            {
                response = "Невозможно изменить рабочий режим без подготовки к штатному режиму.";
                return false;
            }
            if (arguments.Count != 1)
            {
                response = "Формат команды: setcode <название>. Допустимые названия: green, emerald, blue, orange, yellow, red, clean.";
                return false;
            }
            switch (arguments.ToArray()[0])
            {
                case "green":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.2 .G2 . $PITCH_0.2 .G2 . $PITCH_0.1 .G2 . . . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b> <color=#32CD32>🔲</color>\r\n\r\n<b>|<color=#727472>🏰</color><color=#32CD32>🔲</color>| Территория учреждения безопасно функционирует, объявлен <color=#32CD32>Зелёный Код</color> \r\n|<color=#BC8F8F>👤</color><color=#32CD32>✅</color>| Персонал может возвращаться к своей штатной работе</b>", isSubtitles: true, isNoisy: false, isHeld: false);
                    foreach (var ragdoll in Ragdoll.List.ToList())
                    {
                        ragdoll.Destroy();
                    }
                    response = "Установлен код \"Зелёный\"!";
                    return true;
                case "emerald":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Emerald;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.2 .G1 . $PITCH_0.15 .G1 .G1 . $PITCH_1.0 . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b> <color=#50C878>🔲</color>\r\n\r\n<b>|<color=#727472>🏰</color><color=#50C878>🔲</color>| Фиксация некритичных нарушений, объявлен <color=#50C878><u>Изумрудный Код</color></u>\r\n|<color=#BC8F8F>👤</color><color=#32CD32>✅</color>| Персонал действует согласно указаниям начальства", isSubtitles: true, isNoisy: false, isHeld: false);
                    foreach (var ragdoll in Ragdoll.List.ToList())
                    {
                        ragdoll.Destroy();
                    }
                    response = "Установлен код \"Изумрудный\"!";
                    return true;
                case "blue":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Blue;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.15 .G1 .G1 . . .G2 . $PITCH_1.0 . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b> <color=#005EBC>🔲</color>\r\n\r\n<b>|<color=#727472>🏰</color><color=#005EBC>🔲</color>| Зафиксированы малые нарушения, объявлен <color=#005EBC><u>Синий Код</color></u>\r\n|<color=#696969>🔫</color><color=#696969>👤</color>| Службе Безопасности вести активный патруль зон для подавления нарушений\r\n|<color=#BC8F8F>👤</color><color=#FFD700>❕</color>| Мирному персоналу покинуть Зону Тяжёлого Содержания, если нет причин оставаться в ней", isSubtitles: true, isNoisy: false, isHeld: false);
                    response = "Установлен код \"Синий\"!";
                    return true;
                case "orange":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Orange;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.1 .G3 . .G3 $PITCH_1.0 . . . . . . . . . . . . . . . . . . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b> <color=#EE7600>🔲</color>\r\n\r\n<b>|<color=#727472>🏰</color><color=#EE7600>🔲</color>| В комплексе зафиксированы нарушения, превышающие слабый уровень опасности. Объявлен <color=#EE7600><u>Оранжевый Код</color></u>\r\n|<color=#696969>🔫</color><color=#696969>👤</color>| Боевым единицам приступить к ликвидации угрозы или принять меры для восстановления безопасной обстановки \r\n|<color=#0000CD>🔫</color><color=#0000CD>👤</color>| <b><color=#002DB3>Группе Особого Реагирования</color></b> разрешено войти в подземную часть", isSubtitles: true, isNoisy: false, isHeld: false);
                    response = "Установлен код \"Оранжевый\"!";
                    return true;
                case "yellow":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Yellow;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.50 .G4 . $PITCH_0.10 .G4 $PITCH_0.50 . $PITCH_0.10 .G4 $PITCH_0.50 . $PITCH_0.10 .G4 . .G3 . $PITCH_1.0 . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b> <color=#EFC01A>🔲</color>\r\n\r\n<b>|<color=#727472>🏰</color><color=#EFC01A>🔲</color>| На территории учреждения зафиксирован повышенный уровень угрозы. Объявлен <color=#EFC01A><u>Жёлтый Код</color></u>\r\n|<color=#696969>🔫</color><color=#696969>👤</color>| Боевым единицам немедленно приступить к ликвидации угрозы и защите мирного персонала\r\n|<color=#0000CD>🔫</color><color=#0000CD>👤</color>| <b><color=#002DB3>Группе Особого Реагирования</color></b> разрешено войти в подземную часть", isSubtitles: true, isNoisy: false, isHeld: false);
                    response = "Установлен код \"Жёлтый\"!";
                    return true;
                case "red":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Red;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.07 .G3 . $PITCH_0.05 .G3 . $PITCH_0.03 .G1 . $PITCH_1.0 . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b> <color=#C50000>🔲</color>\r\n\r\n<b>|<color=#727472>🏰</color><color=#C50000>🔲</color>| Ситуация критическая. Объявлен <color=#C50000><u>Красный Код</color></u>\r\n|<color=#BC8F8F>👤</color><color=#C50000>❕</color>| Персоналу немедленно эвакуироваться на поверхность до устранения основных угроз\r\n|<color=#696969>🔫</color><color=#696969>👤</color>| Службе безопасности разрешено эвакуироваться на поверхность; эвакуируйте мирных сотрудников\r\n|<color=#0000CD>🔫</color><color=#0000CD>👤</color>| <b><color=#002DB3>Группе Особого Реагирования</color></b> немедленно нейтрализовать опасность", isSubtitles: true, isNoisy: false, isHeld: false);
                    response = "Установлен код \"Красный\"!";
                    return true;
                case "clean":
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Clean;
                    VeryUsualDay.Instance.IsCleanCountdownActive = true;
                    Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.25 .G3 . $PITCH_0.15 .G4 . $PITCH_0.08 .G5 . $PITCH_0.03 .G6 . $PITCH_1.0 . . .", translation: "[<color=#eb8f34>🔊</color>] <b><color=#eb8f34>А.С.К.К.</color></b>\r\n\r\n<b><color=#727472>[Система Предупреждения]</color></b><color=#00FFFF>⚠️🔲⚠️</color> \r\n\r\n<b>|<color=#727472>🏰</color><color=#00FFFF>🔲</color>| Объявлен <color=#00FFFF><u>Код Очистки</color></u>\r\n|<color=#BC8F8F>👤</color><color=#C50000>❕❕❕</color>| Всем оставшимся сотрудникам укрыться в бункерах немедленно!\r\n|<color=#EE7600>⌛</color><color=#EE7600>5</color>| Минимальное время для эвакуации в убежища - <color=#EE7600>5 минут</color>\r\n|<color=#C50000>⚠️</color></color><color=#00FFFF>💀</color>| Все биологические сущности за пределами бункерных зон будут уничтожены", isSubtitles: true, isNoisy: false, isHeld: false);
                    response = "Установлен код \"Очистка\"!";
                    Timing.CallDelayed(300f, () =>
                    {

                           foreach (var door in Door.List)
                            {
                                door.IsOpen = false;
                                door.Lock(DoorLockType.AdminCommand);
                            }

                        Map.TurnOffAllLights(float.MaxValue);

                        foreach (var player in Player.List)
                        {
                            if (player.Role.Type == RoleTypeId.Tutorial &&
                                player.CustomInfo == "Человек")
                                continue;

                            if (player.CurrentRoom == null)
                                continue;

                            if (player.CurrentRoom.Type == RoomType.Hcz079 ||
                                player.CurrentRoom.Type == RoomType.LczGlassBox)
                                continue;

                            player.EnableEffect(EffectType.Decontaminating);
                        }

                        Exiled.API.Features.Cassie.MessageTranslated(message: "$PITCH_0.01 .G6 .", translation: "<b><color=#960018>[ЛОКДАУН]</b></color>", isSubtitles: true, isNoisy: false, isHeld: false);
                    });
                    return true;
                default:
                    response = "Формат команды: setcode <название>. Допустимые названия: green, emerald, blue, yellow, red, clean.";
                    return false;
            }
        }
    }
}