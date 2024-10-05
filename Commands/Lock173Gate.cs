using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Lock173Gate : ICommand
    {
        public string Command => "lock173gate";
        public string[] Aliases => new string[] { };
        public string Description => "Заблокировать/разблокировать новый гейт SCP-173.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var door = Door.Get(DoorType.Scp173NewGate);
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
