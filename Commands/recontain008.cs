using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class recontain008 : ICommand
    {
        public string Command { get; set; } = "recontain008";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Восстанавливает ОУС SCP-008. Использовать только в К.С. SCP-008.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.Is008Leaked)
            {
                response = "ОУС SCP-008 не нарушены!";
                return false;
            }
            Player playerSender = Player.Get(sender);
            if (playerSender.CurrentRoom.Type != RoomType.Hcz106)
            {
                response = "Вы не находитесь в К.С. SCP-008.";
                return false;
            }
            if (Door.Get(DoorType.Scp106Primary).IsLocked)
            {
                response = "Для ВОУС необходимо, чтобы дверь в К.С. SCP-008 была разблокирована.";
                return false;
            }
            if (playerSender.Role.Team != Team.FoundationForces && playerSender.Role.Team != Team.ChaosInsurgency)
            {
                response = "Восстановить ОУС SCP-008 может только человек с ролью Охраны/МОГ/ПХ (рп-отыгровка: вам не хватило силы/знаний)";
                return false;
            }
            if (Math.Abs(Room.Get(RoomType.Hcz106).Position.z - playerSender.Position.z) < 19 && Math.Abs(Room.Get(RoomType.Hcz106).Position.x - playerSender.Position.x) < 18)
            {
                response = "Вы не находитесь в К.С. SCP-008.";
                return false;
            }
            Cassie.DelayedMessage("<b><color=#727472>[ВОУС]</color></b>: Объект-008 был перекрыт, распространение патогена прекращено. <size=0> pitch_0.1 .G2 . pitch_1.0 . . . . . . ", 1f, isSubtitles: true, isNoisy: false);
            Timing.KillCoroutines("_008_poisoning");
            VeryUsualDay.Instance.Is008Leaked = false;
            response = "ВОУС SCP-008 прошло успешно.";
            return true;
        }
    }
}
