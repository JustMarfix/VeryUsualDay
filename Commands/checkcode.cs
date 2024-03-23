using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class checkcode : ICommand
    {
        public string Command { get; set; } = "checkcode";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Для СОД. Вывести действующий код.";

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
