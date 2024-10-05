using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CheckCode : ICommand
    {
        public string Command => "checkcode";
        public string[] Aliases => new string[] { };
        public string Description => "Для FX. Вывести действующий код.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            response = $"Код - {VeryUsualDay.Instance.CurrentCode.Description()}";
            return true;
        }
    }
}
