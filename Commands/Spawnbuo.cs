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
        public string Command => "spawnbuo";

        public string[] Aliases => new string[] { };

        public string Description => "Позволяет заспавнить бойцов БУО. Использование: spawnbuo <id через пробел>. Для FX.";
        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: spawnbuo <id через пробел>";
                return false;
            }
            VeryUsualDay.Instance.BuoCounter += 1;
            var peopleCounter = 1;
            foreach (var id in arguments.ToArray())
            {
                if (!Player.TryGet(int.Parse(id), out var player)) continue;
                player.Role.Set(RoleTypeId.ChaosMarauder, RoleSpawnFlags.AssignInventory);
                Timing.CallDelayed(2f, () =>
                {
                    player.MaxHealth = 300f;
                    player.Health = 300f;
                    player.ResetInventory(VeryUsualDay.Instance.Config.BuoInventory);
                    player.AddAmmo(AmmoType.Ammo44Cal, 16);
                    player.AddAmmo(AmmoType.Ammo12Gauge, 28);
                    player.CustomName = $"БУО #{VeryUsualDay.Instance.BuoCounter} - ##-{peopleCounter}";
                    player.CustomInfo = "[Боец БУО]";
                    player.Broadcast(15, "<color=#708090><b>Вы стали бойцом <color=#138808>Боевого Ударного Отряда<color=#708090>. Спасите <color=#ffd800>сотрудников фонда<color=#708090>, устраните <color=red>угрозу<color=#708090> в комплексе и <color=#120a8f>выполните миссию<color=#708090>!");
                    peopleCounter += 1;
                });
            }
            response = "Бойцы БУО заспавнены!";
            return true;
        }
    }
}
