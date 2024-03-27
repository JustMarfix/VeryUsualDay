using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class lock049gate : ICommand
    {
        public string Command => "lock049gate";
        public string[] Aliases => new string[] { };
        public string Description => "Заблокировать/разблокировать гейт К.С. 049.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var door = Door.Get(DoorType.Scp049Gate);
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