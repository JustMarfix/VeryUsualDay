using System;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ClassD : ICommand
    {
        public string Command => "classd";
        public string[] Aliases => new string[] { };
        public string Description => "Позволяет встать в очередь на спавн за Испытуемого. Работает только на Слишком Обычном Дне.";
        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (!VeryUsualDay.Instance.IsDboysSpawnAllowed)
            {
                response = "Самостоятельный спавн за D-класс закрыт до начала ивента и во время мини-ивентов.";
                return false;
            }
            var playerSender = Player.Get(sender);
            if (VeryUsualDay.Instance.DBoysQueue.Contains(playerSender.Id) || !(playerSender.CustomInfo == "Человек" || playerSender.CustomInfo is null) || playerSender.Role.Type != RoleTypeId.Tutorial)
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
