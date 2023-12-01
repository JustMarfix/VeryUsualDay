using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawn076_2 : ICommand
    {
        public string Command { get; set; } = "spawn076-2";

        public string[] Aliases { get; set; } = { };

        public string Description { get; set; } = "Работает при СОД. Спавнит Авеля.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим СОД не включён!";
                return false;
            }
            int id = int.Parse(arguments.Array[1]);
            if (Player.TryGet(id, out Player avel))
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(id))
                {
                    avel.MaxHealth = 100f;
                    avel.CustomInfo = "Человек";
                    avel.DisableEffect(Exiled.API.Enums.EffectType.MovementBoost);
                    avel.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: Exiled.API.Enums.SpawnReason.ForceClass);
                    avel.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                    VeryUsualDay.Instance.ScpPlayers.Remove(id);
                    response = "SCP удалён!";
                    return true;
                }
                else
                {
                    avel.Role.Set(PlayerRoles.RoleTypeId.Tutorial, reason: Exiled.API.Enums.SpawnReason.ForceClass, spawnFlags: PlayerRoles.RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        avel.CustomInfo = "<b><color=#960018>SCP-076-2</color></b>";
                        avel.MaxHealth = 5000f;
                        avel.Health = 5000f;
                        avel.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost);
                        avel.ChangeEffectIntensity(Exiled.API.Enums.EffectType.MovementBoost, 25);
                        avel.CurrentItem = avel.AddItem(ItemType.Jailbird);
                        avel.Scale = new UnityEngine.Vector3(1.15f, 1.15f, 1.15f);
                        VeryUsualDay.Instance.ScpPlayers.Add(id, VeryUsualDay.Scps.Scp0762);
                    });
                    response = "Авель создан!";
                    return true;
                }
            }
            else
            {
                response = "Не удалось найти игрока с таким ID!";
                return false;
            }
        }
    }
}
