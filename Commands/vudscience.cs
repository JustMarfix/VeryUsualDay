using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

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
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player scientist))
            {
                scientist.Role.Set(PlayerRoles.RoleTypeId.Scientist, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    scientist.ClearInventory();
                    scientist.MaxHealth = 115f;
                    scientist.Health = 115f;
                    scientist.AddItem(ItemType.KeycardScientist);
                    scientist.AddItem(ItemType.Painkillers);
                    scientist.AddItem(ItemType.Flashlight);
                    scientist.CustomName = $"Сотрудник - ##-{VeryUsualDay.Instance.SpawnedScientistCounter}";
                    scientist.Broadcast(10, "<b>Вы вступили в <color=#ffd800>Научный</color> отдел! Исследуйте и сдерживайте <color=red>аномалии</color>, помогайте работе <color=#120a8f>фонда</color>.");
                    VeryUsualDay.Instance.SpawnedScientistCounter += 1;
                });
                response = "НС заспавнен успешно!";
                return true;
            }
            else
            {
                response = "Не удалось найти игрока с таким ID!";
                return false;
            }
        }
    }
}
