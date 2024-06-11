using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Usables.Scp244;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class recontain244 : ICommand
    {
        public string Command => "recontain244";
        public string[] Aliases => new string[] { };
        public string Description => "Вызывает ВОУС объекта SCP-244-A/B. Только для FX.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            
            var counter = 0;
            var pickups = Pickup.List.Where(p => (p.Type == ItemType.SCP244a || p.Type == ItemType.SCP244b) && !p.InUse).ToList();
            foreach (var pickup in pickups)
            {
                var type = pickup.Type;
                var pos = pickup.Position;
                var rot = pickup.Rotation;
                pickup.Destroy();
                Pickup.CreateAndSpawn(type, pos, rot).As<Scp244Pickup>().State = Scp244State.Idle;
                counter += 1;
            }
            
            response = $"ВОУС успешно вызвано. Деактивировано {counter} объекта(ов).";
            return true;
        }
    }
}