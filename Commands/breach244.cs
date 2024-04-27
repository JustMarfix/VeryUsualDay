using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Usables.Scp244;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class breach244 : ICommand
    {
        public string Command => "breach244";
        public string[] Aliases => new string[] { };
        public string Description => "Вызывает НУС объекта SCP-244-A/B. Только для СОД.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            
            var counter = 0;
            var pickups = Pickup.List.Where(p => (p.Type == ItemType.SCP244a || p.Type == ItemType.SCP244b) && !p.InUse).ToList();
            foreach (var pickup in pickups)
            {
                pickup.As<Scp244Pickup>().State = Scp244State.Active;
                counter += 1;
            }
            
            response = $"НУС успешно вызван. Активировано {counter} объекта(ов).";
            return true;
        }
    }
}