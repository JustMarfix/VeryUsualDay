using System;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Jailbird;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace VeryUsualDay.Handlers
{
    public static class Player
    {
        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(5f, () =>
            {
                if (!VeryUsualDay.Instance.IsEnabledInRound || ev.NewRole != RoleTypeId.Spectator ||
                    ev.Reason == SpawnReason.ForceClass) return;
                if (VeryUsualDay.Instance.Is008Leaked)
                {
                    if (ev.Player.Role.Type == RoleTypeId.Scp0492)
                    {
                        return;
                    }
                }
                if (ev.Reason == SpawnReason.Died || ev.Reason == SpawnReason.Destroyed)
                {
                    var bc = "<b>Вы были перемещены в <color=purple>Обучение</color>, т.к. ивент находится в процессе.</b>";
                    ev.Player.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    ev.Player.Broadcast(10, bc);
                }
                else
                {
                    var bc = "<color=#98FB98><b>Вы</b></color> на закрытом рп ивенте <color=#666699>\"Слишком обычный день\"</color>. Можете ожидать своей роли и появления на территории комплекса. Возможен лимит участвующих.";
                    ev.Player.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    ev.Player.Broadcast(10, bc);
                }
            });
        }
        public static void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp0762)
                {
                    ev.IsAllowed = false;
                }
                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 && VeryUsualDay.Instance.Config.Scp035ForbiddenItems.Contains(ev.Pickup.Type))
                {
                    ev.IsAllowed = false;
                }
            }
            switch (ev.Player.Role.Type)
            {
                case RoleTypeId.Scientist when VeryUsualDay.Instance.Config.ForbiddenForScientists.Contains(ev.Pickup.Type):
                case RoleTypeId.ClassD when ev.Player.CustomName.ToLower().Contains("уборщик") && VeryUsualDay.Instance.Config.ForbiddenForJanitors.Contains(ev.Pickup.Type):
                    ev.IsAllowed = false;
                    break;
            }

            if ((VeryUsualDay.Instance.CurrentCode != VeryUsualDay.Codes.Green &&
                 VeryUsualDay.Instance.CurrentCode != VeryUsualDay.Codes.Emerald) ||
                ev.Player.Role.Type != RoleTypeId.ClassD || ev.Player.CustomName.ToLower().Contains("уборщик") ||
                !ev.Pickup.Type.IsWeapon() || VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id)) return;
            VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Blue;
            Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#005EBC>Синий Код</color>. Зафиксированы малые нарушения. Персоналу следует принимать меры предосторожности. <size=0> pitch_0.1 .G1 .G2 . pitch_1.0 . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
        }
        public static void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id)) return;
            if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp0762)
            {
                ev.IsAllowed = false;
            }
            if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 && ev.Item.Type == ItemType.GunRevolver)
            {
                ev.IsAllowed = false;
            }
        }
        public static void OnHurting(HurtingEventArgs ev)
        {
            try
            {
                if (!VeryUsualDay.Instance.ScpPlayers.TryGetValue(ev.Player.Id, out var player)) return;
                if (player != VeryUsualDay.Scps.Scp0762) return;
                if (ev.Attacker.CurrentItem.As<Jailbird>()?.WearState != JailbirdWearState.AlmostBroken) return;
                ev.Attacker.CurrentItem?.Destroy();
                var jailbird = ev.Attacker.AddItem(ItemType.Jailbird);
                Timing.CallDelayed(0.1f, () =>
                {
                    ev.Attacker.CurrentItem = jailbird;
                });
            }
            catch
            {
                // ignored
            }
        }
        public static void OnDied(DiedEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            ev.Player.CustomInfo = "Человек";
            ev.Player.MaxHealth = 100f;
            ev.Player.Scale = new Vector3(1f, 1f, 1f);
            if (VeryUsualDay.Instance.Zombies.Contains(ev.Player.Id))
            {
                ev.Player.UnMute();
                VeryUsualDay.Instance.Zombies.Remove(ev.Player.Id);
            }
            else if (!VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id) && VeryUsualDay.Instance.Is008Leaked)
            {
                Timing.CallDelayed(2f, () =>
                {
                    ev.Player.Role.Set(RoleTypeId.Scp0492, RoleSpawnFlags.AssignInventory);
                    Timing.CallDelayed(2f, () =>
                    {
                        ev.Player.MaxHealth = 1850f;
                        ev.Player.Health = 1850f;
                        ev.Player.EnableEffect(EffectType.Stained);
                        ev.Player.CustomInfo = "<b><color=#960018>SCP-008-2</color></b>";
                        ev.Player.Broadcast(10, "<b>Вы стали <color=#DC143C>SCP-008-2</color>!\n<color=#960018>Атакуйте людей</color> до конца жизни!</b>");
                        ev.Player.Mute();
                        VeryUsualDay.Instance.Zombies.Add(ev.Player.Id);
                    });
                });
            }
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                VeryUsualDay.Instance.ScpPlayers.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.DBoysQueue.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.DBoysQueue.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.JoinedDboys.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.JoinedDboys.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Green || VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Emerald)
            {
                Timing.CallDelayed(5f, () =>
                {
                    Ragdoll.GetLast(ev.Player).Destroy();
                });
            }
        }
        public static void OnLeft(LeftEventArgs ev)
        {
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                VeryUsualDay.Instance.ScpPlayers.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.Zombies.Contains(ev.Player.Id))
            {
                ev.Player.UnMute();
                VeryUsualDay.Instance.Zombies.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.DBoysQueue.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.DBoysQueue.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.JoinedDboys.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.JoinedDboys.Remove(ev.Player.Id);
            }
        }

        public static void OnShooting(ShootingEventArgs ev)
        {
            if (!VeryUsualDay.Instance.ScpPlayers.TryGetValue(ev.Player.Id, out var player)) return;
            if (player == VeryUsualDay.Scps.Scp035 && ev.Firearm.Type == ItemType.GunRevolver)
            {
                ev.Firearm.Ammo += 1;
            }
        }

        public static void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!VeryUsualDay.Instance.ScpPlayers.TryGetValue(ev.Player.Id, out var player)) return;
            if ((player != VeryUsualDay.Scps.Scp035 || ev.Usable.Type != ItemType.SCP500) &&
                ev.Usable.Type != ItemType.SCP207 && ev.Usable.Type != ItemType.AntiSCP207) return;
            ev.IsAllowed = false;
            ev.Item?.Destroy();
        }
    }
}
