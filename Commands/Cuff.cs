using System;
using CommandSystem;
using Exiled.API.Features;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Cuff : ICommand
    {
        public string Command => "vcuff";
        public string[] Aliases => new string[] { };
        public string Description => "Связывает человека. Для Foundation-X.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }

            var playerSender = Player.Get(sender);
            if (playerSender.CustomName == null || (!playerSender.CustomName.Contains("ОВБ") &&
                                                    !playerSender.CustomName.Contains("ГОР") &&
                                                    !playerSender.CustomName.Contains("Глава Охраны")))
            {
                response = "У вас нет прав использовать эту команду!";
                return false;
            }

            if (!playerSender.CurrentItem.IsWeapon)
            {
                response = "Вы не держите в руках оружие!";
                return false;
            }
            
            var layerMask = 1 << 8;
            if (!Physics.Raycast(new Ray(playerSender.CameraTransform.position + playerSender.CameraTransform.forward, playerSender.CameraTransform.forward), out RaycastHit raycastHit, maxDistance: 3f, layerMask: ~layerMask))
            {
                response = "Вы не смотрите ни на кого, либо вы недостаточно близко.";
                return false;
            }
            
            Player cuffed = null;
            var hub = raycastHit.transform?.GetComponentInParent<ReferenceHub>();
            if (hub == null || !Player.TryGet(hub, out cuffed) || cuffed == playerSender)
            {
                response = "Не получилось связать никого, попробуйте подойти ближе / отойти дальше!";
                return false;
            }

            var cuffer = "";
            if (playerSender.CustomName.Contains("ОВБ"))
            {
                cuffer = "<color=#e34234>Агентом ОВБ</color>";
            }
            else if (playerSender.CustomName.Contains("ГОР"))
            {
                cuffer = "<color=#42aaff>Бойцом ГОР</color>";
            }
            else if (playerSender.CustomName.Contains("Глава Охраны"))
            {
                cuffer = "<color=#979aaa>Главой Охраны</color>";
            }

            if (cuffed.IsCuffed)
            {
                response = "Игрок уже связан!";
                return false;
            }
            cuffed.Handcuff();
            cuffed.Broadcast(7, $"<b>Вы были связаны {cuffer}</b>");
            response = "Игрок успешно связан!";
            return true;
        }
    }
}