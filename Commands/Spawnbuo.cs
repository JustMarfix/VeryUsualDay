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
    public class Spawnbuo : ICommand
    {
        public string Command { get; set; } = "spawnbuo";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Позволяет заспавнить бойцов БУО. Использование: spawnbuo <id через пробел>. Для СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён";
                return false;
            }
            if (arguments.Count < 2)
            {
                response = "Формат команды: spawnbuo <id через пробел>.";
                return false;
            }
            VeryUsualDay.Instance.BuoCounter += 1;
            int peopleCounter = 1;
            foreach (string id in arguments.ToArray().Skip(1))
            {
                if (Player.TryGet(int.Parse(id), out Player player))
                {
                    player.Role.Set(RoleTypeId.ChaosMarauder, RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        player.MaxHealth = 300f;
                        player.Health = 300f;
                        player.ResetInventory(VeryUsualDay.Instance.Config.BUOInventory);
                        player.AddAmmo(AmmoType.Ammo44Cal, 16);
                        player.AddAmmo(AmmoType.Ammo12Gauge, 28);
                        player.CustomName = $"БУО #{VeryUsualDay.Instance.BuoCounter} - ##-{peopleCounter}";
                        player.CustomInfo = "[Боец БУО]";
                        player.Broadcast(15, "<color=#708090><b>Вы стали бойцом <color=#138808>Боевого Ударного Отряда<color=#708090>. Спасите <color=#ffd800>сотрудников фонда<color=#708090>, устраните <color=red>угрозу<color=#708090> в комплексе и <color=#120a8f>выполните миссию<color=#708090>!");
                        peopleCounter += 1;
                    });
                }
            }
            response = "Бойцы БУО заспавнены!";
            return true;
        }
    }
}
