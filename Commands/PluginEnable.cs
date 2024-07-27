﻿using System;
using System.Runtime.CompilerServices;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PluginEnable : ICommand
    {
        public string Command => "fxmode";
        public string[] Aliases => new string[] {};
        public string Description => "Не использовать, если не проводите FX!";
        public bool SanitizeResponse => false;

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
                VeryUsualDay.Instance.ChaosRooms.Clear();
                // Timing.KillCoroutines("_avel");
                Timing.KillCoroutines("_joining");
                Timing.KillCoroutines("_prisonTimer");
                Timing.KillCoroutines("_chaos");
                foreach (var player in Player.List)
                {
                    if (player.TryGetSessionVariable("isInPrison", out bool prisonState) && prisonState)
                    {
                        player.TryGetSessionVariable("prisonReason", out string reason);
                        player.TryGetSessionVariable("prisonTime", out Int32 time);
                        VeryUsualDay.SendToPrison(player, time, reason);
                        // Log.Info($"Игроку {player.UserId} осталось в тюрьме {time} секунд. СОД закончен.");
                        Timing.CallDelayed(3f, () =>
                        {
                            player.UnMute();
                            player.DisableEffect(EffectType.SilentWalk);
                            player.Role.Set(RoleTypeId.Tutorial);
                            player.SessionVariables.Remove("isInPrison");
                            player.SessionVariables.Remove("prisonTime");
                            player.SessionVariables.Remove("prisonReason");
                        });
                    }
                }
                response = "Режим FX выключен.";
            }
            else
            {
                VeryUsualDay.Instance.IsEnabledInRound = true;
                // Timing.RunCoroutine(VeryUsualDay.Instance._avel(), "_avel");
                Timing.RunCoroutine(VeryUsualDay.Instance._joining(), "_joining");
                Timing.RunCoroutine(VeryUsualDay.Instance._prisonTimer(), "_prisonTimer");
                Timing.RunCoroutine(VeryUsualDay.Instance._chaos(), "_chaos");
                foreach (var room in Room.List)
                {
                    if (room.Zone != ZoneType.Unspecified && room.Color == Color.red)
                    {
                        room.ResetColor();
                    }
                }
                Timing.CallDelayed(5f, () =>
                {
                    VeryUsualDay.Instance.SupplyBoxCoords = Room.Get(RoomType.EzGateB).Position + new Vector3(-6.193f, 2.243f, -5.901f);
                });
                foreach (var player in Player.List)
                {
                    var userData = (ITuple)VeryUsualDay.CheckIfPlayerInPrison(player);
                    if ((bool)userData[0])
                    {
                        player.Mute();
                        player.EnableEffect(EffectType.SilentWalk, 255);
                        player.Teleport(VeryUsualDay.PrisonPosition);
                        player.SessionVariables.Add("isInPrison", true);
                        player.SessionVariables.Add("prisonTime", (Int32)userData[1]);
                        player.SessionVariables.Add("prisonReason", (string)userData[2]);
                        // Log.Info($"Игрок {player.UserId} будет находиться в тюрьме {(Int32)userData[1]} секунд.");
                    }
                }
                response = "Режим FX включён.";
            }
            return true;
        }
    }
}
