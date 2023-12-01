using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class redoors : ICommand
    {
        public string Command { get; set; } = "redoors";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Позволяет рестартнуть систему дверей (СОД).";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            foreach (Door door in Door.List)
            {
                if (!door.IsElevator)
                {
                    door.IsOpen = false;
                }
            }
            Cassie.Message("<b><color=#727472>[Обновление]</b></color> система дверей была перезапущена. <size=0> pitch_0.6 .g1 pitch_1.0 . . . . . . . . . . . . . ", isNoisy: false, isSubtitles: true);
            response = "Система дверей перезапущена!";
            return true;
        }
    }
}
