using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using VeryUsualDay.Utils;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn0082 : ICommand
    {
        public string Command => "spawn008-2";
        public string[] Aliases => new string[] { };
        public string Description => "Работает при СОД. Спавнит SCP-008-2.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            var id = int.Parse(arguments.ToArray()[0]);
            if (Player.TryGet(id, out var scp0082))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    var human = new TutorialHuman(scp0082);
                    response = "SCP удалён!";
                    return true;
                }
                var scp = new Scp0082(scp0082, isPatogenZombie: false);
                response = "SCP-008-2 создан!";
                return true;
            }
            response = "Не удалось найти игрока с таким ID!";
            return false;
        }
    }
}