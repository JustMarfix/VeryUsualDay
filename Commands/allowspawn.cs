using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class allowspawn : ICommand
    {
        public string Command { get; set; } = "allowspawn";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Включает/выключает самостоятельный спавн ClassD. Доступно только на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (VeryUsualDay.Instance.IsDboysSpawnAllowed)
            {
                response = "Самоспавн за ClassD теперь запрещён!";
                VeryUsualDay.Instance.IsDboysSpawnAllowed = false;
            }
            else
            {
                response = "Самоспавн за ClassD теперь разрешён!";
                VeryUsualDay.Instance.IsDboysSpawnAllowed = true;
            }
            return true;
        }
    }
}
