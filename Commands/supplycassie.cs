﻿using System;
using CommandSystem;
using Exiled.API.Features;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class supplycassie : ICommand
    {
        public string Command => "supplycassie";
        public string[] Aliases => new string[] { };
        public string Description => "Вызывает CASSIE о доставке SCP. Только для СОД.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён";
                return false;
            }
            Cassie.Message("<b><color=#EE7600>[Заказ прибыл]</color>: аномальные объекты </color></b> <size=0> pitch_0.4 .G1 . . .G1 .G1 pitch_1.00 . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
            response = "CASSIE успешно вызвано.";
            return true;
        }
    }
}