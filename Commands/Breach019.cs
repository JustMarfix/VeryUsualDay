using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Usables.Scp244;
using MEC;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Breach019 : ICommand
    {
        public string Command => "breach019";
        public string[] Aliases => null;
        public string Description => "Открывает SCP-019 без дыма.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Door.Get(DoorType.Scp173Armory).Lock(float.PositiveInfinity, DoorLockType.AdminCommand);
            VeryUsualDay.Instance.Vase.As<Scp244Pickup>().State = Scp244State.Active;
            Timing.CallDelayed(1f, () =>
            {
                VeryUsualDay.Instance.Vase.As<Scp244Pickup>().State = Scp244State.Idle;
            });
            response = "SCP-019 открыт.";
            return true;
        }
    }
}