using System;
using System.Linq;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ManageTesla : ICommand
    {
        public string Command => "managetesla";
        public string[] Aliases => new string[] { "mtesla" };
        public string Description => "Управление тесла-гейтами. Использование: mtesla on/off";
        public bool SanitizeResponse => false;
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var args = arguments.ToArray();
            if (args.Length < 1 || (args[0] != "on" && args[0] != "off"))
            {
                response = "Использование: mtesla on/off";
                return false;
            }
            VeryUsualDay.Instance.IsTeslaEnabled = args[0] == "on";
            response = "Статус тесла-ворот успешно изменён.";
            return true;
        }
    }
}