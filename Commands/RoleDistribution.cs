using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RoleDistribution : ICommand
    {
        public string Command => "roledistr";
        public string[] Aliases => new string[] { };
        public string Description => "Распределяет роли для людей из БД. Не использовать без СОД.";

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
