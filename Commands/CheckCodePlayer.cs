using System;
using CommandSystem;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CheckCodePlayer : ICommand
    {
        public string Command => "code";
        public string[] Aliases => new string[] { };
        public string Description => "Показывает текущий код FX.";

        public bool Execute(
    ArraySegment<string> arguments,
    ICommandSender sender,
    out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }

            string lunchStatus = "";

            if ((int)VeryUsualDay.Instance.CurrentCode < 2)
            {
                lunchStatus = VeryUsualDay.Instance.IsLunchtimeActive
                    ? " Статус обеда: активен."
                    : " Статус обеда: неактивен.";
            }

            string teslaStatus = VeryUsualDay.Instance.IsTeslaEnabled
                ? " Тесла-ворота: включены."
                : " Тесла-ворота: выключены.";

            response =
                $"Текущий код - {VeryUsualDay.Instance.CurrentCode.Description()}." +
                lunchStatus +
                teslaStatus;

            return true;
        }
    }
}
