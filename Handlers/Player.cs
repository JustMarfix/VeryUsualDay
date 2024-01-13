using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;

namespace VeryUsualDay.Handlers
{
    public class Player
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(5f, () =>
            {
                if (VeryUsualDay.Instance.IsEnabledInRound && ev.NewRole == RoleTypeId.Spectator && ev.Reason != SpawnReason.ForceClass)
                {
                    if (VeryUsualDay.Instance.Is008Leaked)
                    {
                        if (ev.Player.Role.Type == RoleTypeId.Scp0492)
                        {
                            return;
                        }
                    }
                    if (ev.Reason == SpawnReason.Died || ev.Reason == SpawnReason.Destroyed)
                    {
                        string bc = "<b>Вы были перемещены в <color=purple>Обучение</color>, т.к. ивент находится в процессе.</b>";
                        ev.Player.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                        ev.Player.Broadcast(10, bc);
                    }
                    else
                    {
                        string bc = "<color=#98FB98><b>Вы</b></color> на закрытом рп ивенте <color=#666699>\"Слишком обычный день\"</color>. Можете ожидать своей роли и появления на территории комплекса. Возможен лимит участвующих.";
                        ev.Player.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                        ev.Player.Broadcast(10, bc);
                    }
                }
            });
        }
        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound && VeryUsualDay.Instance.LockerPlayers.Contains(ev.Player.Id))
            {
                if (ev.Door.IsGate)
                {
                    if (ev.Door.IsOpen)
                    {
                        ev.Door.IsOpen = true;
                        ev.Door.Lock(float.PositiveInfinity, DoorLockType.AdminCommand);
                    }
                    else
                    {
                        ev.Door.Unlock();
                    }
                }
                else
                {
                    if (ev.Door.IsFullyClosed)
                    {
                        ev.Door.Unlock();
                    }
                    else
                    {
                        ev.Door.IsOpen = true;
                        ev.Door.Lock(float.PositiveInfinity, DoorLockType.AdminCommand);
                    }
                }
            }
        }
        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound)
            {
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
                if (ev.Player.Role.Type == RoleTypeId.Scientist && VeryUsualDay.Instance.Config.ForbiddenForScientists.Contains(ev.Pickup.Type))
                {
                    ev.IsAllowed = false;
                }
                if (ev.Player.Role.Type == RoleTypeId.ClassD && ev.Player.CustomName.ToLower().Contains("уборщик") && VeryUsualDay.Instance.Config.ForbiddenForJanitors.Contains(ev.Pickup.Type))
                {
                    ev.IsAllowed = false;
                }
                if ((VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Green || VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Emerald) && ev.Player.Role.Type == RoleTypeId.ClassD && !ev.Player.CustomName.ToLower().Contains("уборщик") && ev.Pickup.Type.IsWeapon() && !VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
                {
                    VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Blue;
                    Cassie.Message("<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#005EBC>Синий Код</color>. Зафиксированы малые нарушения. Персоналу следует принимать меры предосторожности. <size=0> pitch_0.1 .G1 .G2 . pitch_1.0 . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
                }
            }
        }
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp0762)
                {
                    ev.IsAllowed = false;
                }
                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 && ev.Item.Type == ItemType.GunRevolver)
                {
                    ev.IsAllowed = false;
                }
            }
        }
        public void OnHurting(HurtingEventArgs ev)
        {
            try
            {
                if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
                {
                    if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp0762)
                    {
                        if (ev.Attacker.CurrentItem.As<Jailbird>()?.WearState == InventorySystem.Items.Jailbird.JailbirdWearState.AlmostBroken)
                        {
                            ev.Attacker.CurrentItem?.Destroy();
                            Item jailbird = ev.Attacker.AddItem(ItemType.Jailbird);
                            Timing.CallDelayed(0.1f, () =>
                            {
                                ev.Attacker.CurrentItem = jailbird;
                            });
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public void OnDied(DiedEventArgs ev)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound)
            {
                ev.Player.CustomInfo = "Человек";
                ev.Player.MaxHealth = 100f;
                ev.Player.Scale = new UnityEngine.Vector3(1f, 1f, 1f);
                if (VeryUsualDay.Instance.Zombies.Contains(ev.Player.Id))
                {
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
        }
        public void OnLeft(LeftEventArgs ev)
        {
            if (VeryUsualDay.Instance.LockerPlayers.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.LockerPlayers.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                VeryUsualDay.Instance.ScpPlayers.Remove(ev.Player.Id);
            }
            if (VeryUsualDay.Instance.Zombies.Contains(ev.Player.Id))
            {
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

        public void OnShooting(ShootingEventArgs ev)
        {
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 && ev.Firearm.Type == ItemType.GunRevolver)
                {
                    ev.Firearm.Ammo += 1;
                }
            }
        }

        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id))
            {
                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 && ev.Usable.Type == ItemType.SCP500 || ev.Usable.Type == ItemType.SCP207 || ev.Usable.Type == ItemType.AntiSCP207)
                {
                    ev.IsAllowed = false;
                    ev.Item?.Destroy();
                    return;
                }
            }
        }
    }
}
