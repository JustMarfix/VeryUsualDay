using CommandSystem;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DoorLocker : ICommand
    {
        public string Command { get; set; } = "doorlocker";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Позволяет людям блокировать дверь при закрытии при режиме СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (VeryUsualDay.Instance.LockerPlayers.Contains(id))
            {
                VeryUsualDay.Instance.LockerPlayers.Remove(id);
                response = "Этот человек больше не обладает DoorLock способностью.";
            }
            else
            {
                VeryUsualDay.Instance.LockerPlayers.Add(id);
                response = "Этот человек теперь обладает DoorLock способностью.";
            }
            return true;
        }
    }
}
