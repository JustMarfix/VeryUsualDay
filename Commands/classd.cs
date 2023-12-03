using CommandSystem;
using Exiled.API.Features;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class classd : ICommand
    {
        public string Command { get; set; } = "classd";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Позволяет встать в очередь на спавн за Испытуемого. Работает только на Слишком Обычном Дне.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (!VeryUsualDay.Instance.IsDboysSpawnAllowed)
            {
                response = "Самостоятельный спавн за D-класс закрыт до начала ивента и во время мини-ивентов.";
                return false;
            }
            Player playerSender = Player.Get(sender);
            if (VeryUsualDay.Instance.DBoysQueue.Contains(playerSender.Id) || !(playerSender.CustomInfo == "Человек" || playerSender.CustomInfo is null) || playerSender.Role.Type != PlayerRoles.RoleTypeId.Tutorial)
            {
                response = "Вы уже стоите в очереди, либо уже играете за кого-то.";
                return false;
            }
            VeryUsualDay.Instance.DBoysQueue.Add(playerSender.Id);
            response = "Вы встали в очередь на появление за Испытуемого. Испытуемые появляются раз в 5 минут по 3 человека или меньше. Всего может быть не более 5 Испытуемых.";
            return true;
        }
    }
}
