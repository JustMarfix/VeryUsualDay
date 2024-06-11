using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudscience : ICommand
    {
        public string Command => "vudscience";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит стажёра-научника на FX.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudscience <id через пробел>.";
                return false;
            }
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var scientist))
                {
                    scientist.Role.Set(RoleTypeId.Scientist, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        scientist.ClearInventory();
                        scientist.MaxHealth = 100f;
                        scientist.Health = 100f;
                        scientist.AddItem(ItemType.KeycardJanitor);
                        scientist.AddItem(ItemType.Painkillers);
                        scientist.AddItem(ItemType.Flashlight);
                        scientist.CustomName = $"Сотрудник - ##-{VeryUsualDay.Instance.SpawnedScientistCounter}";
                        scientist.CustomInfo = "Человек";
                        scientist.Broadcast(10, "<b>Вы вступили в <color=#ffd800>Научный</color> отдел! Исследуйте и сдерживайте <color=red>аномалии</color>, помогайте работе <color=#120a8f>фонда</color>.");
                        VeryUsualDay.Instance.SpawnedScientistCounter += 1;
                    });
                }
                else
                {
                    response = "Не удалось найти игрока с таким ID!";
                    return false;
                }
            }
            response = "Игроки заспавнены.";
            return true;
        }
    }
}
