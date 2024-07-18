using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RoleDistribution : ICommand
    {
        public string Command => "roledistr";
        public string[] Aliases => new string[] { };
        public string Description => "Распределяет роли для людей из БД. Не использовать без FX.";
        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            VeryUsualDay.Instance.RoleDistribution();
            response = "Роли распределены!";
            return true;
        }
    }
}
