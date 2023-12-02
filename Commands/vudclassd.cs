using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class vudclassd : ICommand
    {
        public string Command { get; set; } = "vudclassd";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Спавнит испытуемого на СОД.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player dboy))
            {
                dboy.Role.Set(PlayerRoles.RoleTypeId.ClassD, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.All);
                Timing.CallDelayed(1f, () =>
                {
                    dboy.ClearInventory();
                    dboy.Handcuff();
                    dboy.MaxHealth = 115f;
                    dboy.Health = 115f;
                    dboy.CustomName = $"Испытуемый - ##-{VeryUsualDay.Instance.SpawnedDboysCounter}";
                    dboy.Broadcast(10, "<b>Вы стали <color=#EE7600>Испытуемым</color>! Можете сотрудничать с <color=#120a8f>фондом</color> или принимать попытки <color=#ff2b2b>побега</color> при первой возможности. </b>");
                    VeryUsualDay.Instance.SpawnedDboysCounter += 1;
                });
                Cassie.Message("<b>Вы стали <color=#EE7600>Испытуемым</color>! Можете сотрудничать с <color=#120a8f>фондом</color> или принимать попытки <color=#ff2b2b>побега</color> при первой возможности. </b>", isNoisy: false, isSubtitles: true);
                response = "Испытуемый заспавнен успешно!";
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
