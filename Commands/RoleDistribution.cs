using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RoleDistribution : ICommand
    {
        public string Command { get; set; } = "roledistr";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Распределяет роли для людей из БД. Не использовать без СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            VeryUsualDay.Instance.RoleDistribution();
            response = "Роли распределены!";
            return true;
        }
    }
}
