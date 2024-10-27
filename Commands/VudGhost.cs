using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class VudGhost : ICommand
    {
        public string Command => "vudghost";
        public string[] Aliases => null;
        public string Description => "Использование: vudghost ghostId targetIds. Делает/отменяет действие игрока невидимым для всех, кроме targets.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён.";
                return false;
            }
            
            var args = arguments.ToArray();
            var targets = args.Skip(1).ToArray();
            if (!Player.TryGet(args[0], out var ghost))
            {
                response = "Не удалось получить человека по ID.";
                return false;
            }
            if (!ghost.Role.Is<FpcRole>(out var fpcRole))
            {
                response = "Призрак не имеет роль, которая может быть видна другим.";
                return false;
            }
            
            foreach (var player in Player.List.ToList().Where(p => !targets.Contains(p.Id.ToString())))
            {
                if (!fpcRole.IsInvisibleFor.Add(player))
                {
                    fpcRole.IsInvisibleFor.Remove(player);
                }
            }

            response = "Успешно!";
            return true;
        }
    }
}