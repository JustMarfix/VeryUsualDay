using System;
using CommandSystem;
using Exiled.API.Features.Items;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Recontain019 : ICommand
    {
        public string Command => "recontain019";
        public string[] Aliases => null;
        public string Description => "Восстанавливает ОУС SCP-019.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            VeryUsualDay.Instance.Vase.Destroy();
            var vase = Item.Create(ItemType.SCP244a);
            vase.Scale = new Vector3(8f, 8f, 8f);
            VeryUsualDay.Instance.Vase = vase.CreatePickup(VeryUsualDay.Instance.VaseCoords);
            response = "SCP-019 закрыт.";
            return true;
        }
    }
}