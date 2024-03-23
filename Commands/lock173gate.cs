using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class lock173gate : ICommand
    {
        public string Command { get; set; } = "lock173gate";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Заблокировать новый гейт SCP-173.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Door door = Door.Get(DoorType.Scp173NewGate);
            if (door.IsLocked)
            {
                door.Unlock();
                response = "Гейт разблокирован.";
            }
            else
            {
                door.Lock(float.PositiveInfinity, DoorLockType.AdminCommand);
                response = "Гейт заблокирован.";
            }
            return true;
        }
    }
}
