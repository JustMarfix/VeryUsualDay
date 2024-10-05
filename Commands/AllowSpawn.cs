using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AllowSpawn : ICommand
    {
        public string Command => "allowspawn";
        public string[] Aliases => new string[] { };
        public string Description => "Включает/выключает самостоятельный спавн ClassD. Доступно только на FX.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
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
