using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Linq;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudscience : ICommand
    {
        public string Command { get; set; } = "vudscience";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Спавнит стажёра-научника на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            if (arguments.Array.Length < 2)
            {
                response = "Формат команды: vudscience <id через пробел>.";
                return false;
            }
            foreach (string id in arguments.Array.Skip(1).ToList())
            {
                if (Player.TryGet(id, out Player scientist))
                {
                    scientist.Role.Set(PlayerRoles.RoleTypeId.Scientist, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        scientist.ClearInventory();
                        scientist.MaxHealth = 100f;
                        scientist.Health = 100f;
                        scientist.AddItem(ItemType.KeycardScientist);
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
