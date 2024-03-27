using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class checkcode_player : ICommand
    {
        public string Command => "code";
        public string[] Aliases => new string[] { };
        public string Description => "Показывает текущий код СОД.";
        
        private const string Str1 = "Статус обеда: активен";
        private const string Str2 = "Статус обеда: неактивен";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            response = $"Текущий код - {VeryUsualDay.Instance.CurrentCode.Description()}. {((int)VeryUsualDay.Instance.CurrentCode < 2 ? (VeryUsualDay.Instance.IsLunchtimeActive ? Str1 : Str2) : "")}";
            return true;
        }
    }
}
