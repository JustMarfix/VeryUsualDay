using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ChaosMode : ICommand
    {
        public string Command => "chaosmode";
        public string[] Aliases => new string[] { };
        public string Description => "Для FX. Включает режим хаоса для конкретной комнаты / для всего комплекса.";
        public bool SanitizeResponse => false;
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Использование: chaosmode <list(l)/set(s)/unset(u)> [комнаты через пробел / all]";
                return false;
            }
            var args = arguments.ToArray();
            if (args[0].In("list", "l"))
            {
                response = "Список комнат:\n";
                foreach (var door in Room.List)
                {
                    if (door.Zone != ZoneType.Unspecified)
                    {
                        response += "\n" + door.Name;
                    }
                }
                return true;
            }

            if (args[0].In("set", "s"))
            {
                if (arguments.Count < 2)
                {
                    response = "Использование: chaosmode set <комнаты через пробел / all>";
                    return false;
                }

                foreach (var roomName in args.Skip(1))
                {
                    if (roomName == "all" && arguments.Count == 2)
                    {
                        foreach (var room in Room.List.Where(p => p.Zone != ZoneType.Unspecified))
                        {
                            room.Color = Color.red;
                            if (!room.Type.In(VeryUsualDay.Instance.ChaosRooms.ToArray()))
                            {
                                VeryUsualDay.Instance.ChaosRooms.Add(room.Type);
                            }
                        }
                    }
                    else
                    {
                        var rooms = Room.Get(p => p.Name == roomName).ToArray();
                        if (!rooms.Any())
                        {
                            response = "Использование: chaosmode set <комнаты через пробел / all>. Список комнат - chaosmode list.";
                            return false;
                        }
                        rooms[0].Color = Color.red;
                        if (!rooms[0].Type.In(VeryUsualDay.Instance.ChaosRooms.ToArray()))
                        {
                            VeryUsualDay.Instance.ChaosRooms.Add(rooms[0].Type);
                        }
                    }
                }

                response = "Комнаты успешно назначены на ChaosMode.";
                return true;
            }

            if (args[0].In("unset", "u"))
            {
                if (arguments.Count < 2)
                {
                    response = "Использование: chaosmode unset <комнаты через пробел / all>";
                    return false;
                }

                foreach (var roomName in args.Skip(1))
                {
                    if (roomName == "all" && arguments.Count == 2)
                    {
                        foreach (var room in Room.List.Where(p => p.Zone != ZoneType.Unspecified))
                        {
                            room.ResetColor();
                            VeryUsualDay.Instance.ChaosRooms.Clear();
                        }
                    }
                    else
                    {
                        var rooms = Room.Get(p => p.Name == roomName).ToArray();
                        if (!rooms.Any())
                        {
                            response = "Использование: chaosmode unset <комнаты через пробел / all>. Список комнат - chaosmode list.";
                            return false;
                        }
                        rooms[0].ResetColor();
                        VeryUsualDay.Instance.ChaosRooms.Remove(rooms[0].Type);
                    }
                }

                response = "Комнаты успешно убраны из ChaosMode.";
                return true;
            }

            response = "Использование: chaosmode <list(l)/set(s)/unset(u)> [комнаты через пробел / all]";
            return false;
        }
    }
}