﻿using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PluginEnable : ICommand
    {
        public string Command => "fxmode";
        public string[] Aliases => new string[] {};
        public string Description => "Не использовать, если не проводите FX!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound)
            {
                VeryUsualDay.Instance.IsEnabledInRound = false;
                VeryUsualDay.Instance.IsLunchtimeActive = false;
                VeryUsualDay.Instance.IsDboysSpawnAllowed = false;
                VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Green;
                VeryUsualDay.Instance.BuoCounter = 0;
                VeryUsualDay.Instance.SpawnedDboysCounter = 1;
                VeryUsualDay.Instance.SpawnedWorkersCounter = 1;
                VeryUsualDay.Instance.SpawnedScientistCounter = 1;
                VeryUsualDay.Instance.SpawnedSecurityCounter = 1;
                VeryUsualDay.Instance.ScpPlayers.Clear();
                VeryUsualDay.Instance.JoinedDboys.Clear();
                VeryUsualDay.Instance.DBoysQueue.Clear();
                // Timing.KillCoroutines("_avel");
                Timing.KillCoroutines("_joining");
                response = "Режим FX выключен.";
            }
            else
            {
                VeryUsualDay.Instance.IsEnabledInRound = true;
                // Timing.RunCoroutine(VeryUsualDay.Instance._avel(), "_avel");
                Timing.RunCoroutine(VeryUsualDay.Instance._joining(), "_joining");
                Timing.CallDelayed(5f, () =>
                {
                    VeryUsualDay.Instance.SupplyBoxCoords = Room.Get(RoomType.EzGateB).Position + new Vector3(-6.193f, 2.243f, -5.901f);
                });
                response = "Режим FX включён.";
            }
            return true;
        }
    }
}
