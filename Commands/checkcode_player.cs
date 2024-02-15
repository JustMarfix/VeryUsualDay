using CommandSystem;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class checkcode_player : ICommand
    {
        public string Command { get; set; } = "code";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Показывает текущий код СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            string str1 = "Статус обеда: активен";
            string str2 = "Статус обеда: неактивен";
            response = $"Текущий код - {VeryUsualDay.Instance.CurrentCode.Description()}. {((int)VeryUsualDay.Instance.CurrentCode < 2 ? (VeryUsualDay.Instance.IsLunchtimeActive ? str1 : str2) : "")}";
            return true;
        }
    }
}
