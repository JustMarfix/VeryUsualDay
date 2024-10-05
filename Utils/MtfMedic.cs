using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Utils
{
    public class MtfMedic
    {
        private Player User { get; set; }

        public MtfMedic(Player player)
        {
            User = player;
            _spawn();
        }

        private void _spawn()
        {
            User.Role.Set(RoleTypeId.FacilityGuard, reason: SpawnReason.ForceClass, spawnFlags: RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(2f, () =>
            {
                User.MaxHealth = 200f;
                User.Health = 200f;
                User.Scale = new Vector3(1.03f, 1.03f, 1.03f);
                User.ClearInventory();
                User.AddItem(ItemType.GunCrossvec);
                User.AddItem(ItemType.KeycardGuard);
                User.AddItem(ItemType.ArmorCombat);
                User.AddItem(ItemType.Radio);
                User.AddItem(ItemType.Painkillers);
                User.AddItem(ItemType.Painkillers);
                User.AddAmmo(AmmoType.Nato9, 60);
                CustomItem.TryGive(User, "MG-119");
                User.EnableEffect(EffectType.BodyshotReduction, 10);
                User.EnableEffect(EffectType.DamageReduction, 10);
                User.EnableEffect(EffectType.MovementBoost, 2);
                User.CustomInfo = "<b><color=#4DFFB8>Медик Реагирования</color></b>";
                User.Broadcast(10, "<b>Вы стали <color=#727472>медиком СБ</color>! Вы прошли обучение в мед. центре <color=#120a8f>Фонда</color>, и теперь готовы защищать сотрудников от <color=#ffa000>аномалий</color>.");
            });

        }
    }
}