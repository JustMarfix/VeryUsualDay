using CommandSystem;
using System;

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
            if (VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Green)
            {
                response = "Код - Зелёный";
            }
            else if (VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Emerald)
            {
                response = "Код - Изумрудный";
            }
            else if (VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Blue)
            {
                response = "Код - Синий";
            }
            else if (VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Yellow)
            {
                response = "Код - Жёлтый";
            }
            else
            {
                response = "Код - Красный";
            }
            return true;
        }
    }
}
