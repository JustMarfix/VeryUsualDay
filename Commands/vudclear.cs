using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features.Pickups;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudclear : ICommand
    {
        public string Command => "vudclear";
        public string[] Aliases => new string[] { };
        public string Description => "Очищает с пола все предметы, за исключением тех, что указаны в конфиге.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            var counter = 0;
            var pickups = Pickup.List.Where(p => !VeryUsualDay.Instance.Config.ClearImmunityItems.Contains(p.Type)).ToList();
            foreach (var pickup in pickups)
            {
                pickup.Destroy();
                counter += 1;
            }
            response = $"Готово. Очищено {counter} предметов.";
            return true;
        }
    }
}