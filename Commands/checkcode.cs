using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class checkcode : ICommand
    {
        public string Command => "checkcode";
        public string[] Aliases => new string[] { };
        public string Description => "Для СОД. Вывести действующий код.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            response = $"Код - {VeryUsualDay.Instance.CurrentCode.Description()}";
            return true;
        }
    }
}
