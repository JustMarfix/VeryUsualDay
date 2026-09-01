using System;
using System.Linq;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Scp682EventMode : ICommand
    {
        public string Command => "682event";

        public string[] Aliases => new string[] { };

        public string Description =>
            "Управление режимом SCP-682. Использование: 682event on/off. Работает при FX.";

        public bool Execute(
            ArraySegment<string> arguments,
            ICommandSender sender,
            out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }

            var args = arguments.ToArray();

            if (args.Length < 1 ||
                (!string.Equals(
                    args[0],
                    "on",
                    StringComparison.OrdinalIgnoreCase) &&
                 !string.Equals(
                    args[0],
                    "off",
                    StringComparison.OrdinalIgnoreCase)))
            {
                response = "Использование: 682event on/off";
                return false;
            }

            bool enabled = string.Equals(
                args[0],
                "on",
                StringComparison.OrdinalIgnoreCase);

            VeryUsualDay.Instance.Set682EventMode(enabled);

            response = enabled
                ? "Режим SCP-682 активирован."
                : "Режим SCP-682 деактивирован.";

            return true;
        }
    }
}
