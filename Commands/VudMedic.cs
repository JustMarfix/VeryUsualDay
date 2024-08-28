using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class VudMedic : ICommand
    {
        public string Command => "vudmedic";
        public string[] Aliases => new string[] { };
        public string Description => "Спавнит Медика Реагирования. Для Foundation-X.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound)
            {
                response = "Режим FX не включён!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Формат команды: vudmedic <id через пробел>.";
                return false;
            }
            
            foreach (var id in arguments.ToArray())
            {
                if (Player.TryGet(id, out var medic))
                {
                    medic.Role.Set(RoleTypeId.FacilityGuard, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        medic.MaxHealth = 200f;
                        medic.Health = 200f;
                        medic.Scale = new Vector3(1.03f, 1.03f, 1.03f);
                        medic.ClearInventory();
                        medic.AddItem(ItemType.GunCrossvec);
                        medic.AddItem(ItemType.KeycardGuard);
                        medic.AddItem(ItemType.ArmorCombat);
                        medic.AddItem(ItemType.Radio);
                        medic.AddItem(ItemType.Painkillers);
                        medic.AddItem(ItemType.Painkillers);
                        medic.AddAmmo(AmmoType.Nato9, 60);
                        CustomItem.TryGive(medic, "MG-119");
                        medic.EnableEffect(EffectType.BodyshotReduction, 10);
                        medic.EnableEffect(EffectType.DamageReduction, 10);
                        medic.EnableEffect(EffectType.MovementBoost, 2);
                        medic.CustomInfo = "<b><color=#4DFFB8>Медик Реагирования</color></b>";
                        medic.Broadcast(10, "<b>Вы стали <color=#727472>медиком СБ</color>! Вы прошли обучение в мед. центре <color=#120a8f>Фонда</color>, и теперь готовы защищать сотрудников от <color=#ffa000>аномалий</color>.");
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