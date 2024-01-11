using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Linq;

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
            if (arguments.Array.Length < 2)
            {
                response = "Формат команды: vudclassd <id через пробел>.";
                return false;
            }
            int cnt = 0;
            foreach (string id in arguments.Array.Skip(1))
            {
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
                        dboy.CustomInfo = "Человек";
                        dboy.Broadcast(10, "<b>Вы стали <color=#EE7600>Испытуемым</color>! Можете сотрудничать с <color=#120a8f>фондом</color> или принимать попытки <color=#ff2b2b>побега</color> при первой возможности. </b>");
                        VeryUsualDay.Instance.SpawnedDboysCounter += 1;
                    });
                    cnt += 1;
                }
                else
                {
                    response = "Не удалось найти игрока с таким ID!";
                    return false;
                }
            }
            if (cnt == 1)
            {
                Cassie.Message("<b><color=#FF0090>O5</color> >>> <color=#008080>Комплекс</color></b>: один испытуемый прибыл в блок D. <size=0> . . . . . . . . . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
                response = "Испытуемый заспавнен успешно!";
            }
            else
            {
                Cassie.Message($"<b><color=#FF0090>O5</color> >>> <color=#008080>Комплекс</color></b>: <b><color=#50C878>{cnt}</b></color> испытуемых прибыло в блок D. <size=0> . . . . . . . . . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
                response = "Испытуемые заспавнены успешно!";
            }
            return true;
        }
    }
}
