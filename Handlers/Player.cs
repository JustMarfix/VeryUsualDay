using System.Runtime.CompilerServices;
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
        public static void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (VeryUsualDay.Instance.IsTeslaEnabled) return;
            ev.IsTriggerable = false;
            ev.IsAllowed = false;
        }
        
        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            ev.Player.TryGetSessionVariable("isInPrison", out bool prisonState);
            if (prisonState) return;
            Timing.CallDelayed(5f, () =>
            {
                if (ev.NewRole != RoleTypeId.Spectator ||
                    ev.Reason == SpawnReason.ForceClass) return;

                if (ev.Reason == SpawnReason.Died || ev.Reason == SpawnReason.Destroyed)
                {
                    var bc =
                        "<b>Вы были перемещены в <color=purple>Обучение</color>, т.к. ивент находится в процессе.</b>";
                    ev.Player.Role.Set(RoleTypeId.Tutorial, reason: SpawnReason.ForceClass);
                    ev.Player.Broadcast(10, bc);
                }
                else
                {
                    var bc =
                        "<color=#98FB98><b>Вы</b></color> на закрытом рп ивенте <color=#666699>\"Foundation-X\"</color>. Можете ожидать своей роли и появления на территории комплекса. Возможен лимит участвующих.";
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

                if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 &&
                    VeryUsualDay.Instance.Config.Scp035ForbiddenItems.Contains(ev.Pickup.Type))
                {
                    ev.IsAllowed = false;
                }
            }

            switch (ev.Player.Role.Type)
            {
                case RoleTypeId.Scientist
                    when VeryUsualDay.Instance.Config.ForbiddenForScientists.Contains(ev.Pickup.Type):
                case RoleTypeId.FacilityGuard
                    when VeryUsualDay.Instance.Config.ForbiddenForSecurity.Contains(ev.Pickup.Type):
                case RoleTypeId.ClassD when ev.Player.CustomName.ToLower().Contains("рабочий") &&
                                            VeryUsualDay.Instance.Config.ForbiddenForWorkers.Contains(ev.Pickup.Type):
                case RoleTypeId.ClassD when !ev.Player.CustomName.ToLower().Contains("рабочий") &&
                                            VeryUsualDay.Instance.Config.ForbiddenForClassD.Contains(ev.Pickup.Type):
                case RoleTypeId.Tutorial when ev.Player.CustomName.ToLower().Contains("агент") &&
                                              VeryUsualDay.Instance.Config.ForbiddenForAgency.Contains(ev.Pickup.Type):
                    ev.IsAllowed = false;
                    break;
            }

            if ((VeryUsualDay.Instance.CurrentCode != VeryUsualDay.Codes.Green &&
                 VeryUsualDay.Instance.CurrentCode != VeryUsualDay.Codes.Emerald) ||
                ev.Player.Role.Type != RoleTypeId.ClassD || ev.Player.CustomName.ToLower().Contains("рабочий") ||
                !ev.Pickup.Type.IsWeapon() || VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id)) return;
            VeryUsualDay.Instance.CurrentCode = VeryUsualDay.Codes.Blue;
            Cassie.Message(
                "<b><color=#727472>[Рабочий режим]</color></b>: объявлен <color=#005EBC>Синий Код</color>. Зафиксированы малые нарушения. Персоналу следует принимать меры предосторожности. <size=0> pitch_0.1 .G1 .G2 . pitch_1.0 . . . . . . . . . . . . . .",
                isNoisy: false, isSubtitles: true);
        }

        public static void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            if (!VeryUsualDay.Instance.ScpPlayers.ContainsKey(ev.Player.Id)) return;
            if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp0762)
            {
                ev.IsAllowed = false;
            }

            if (VeryUsualDay.Instance.ScpPlayers[ev.Player.Id] == VeryUsualDay.Scps.Scp035 &&
                ev.Item.Type == ItemType.GunRevolver)
            {
                ev.IsAllowed = false;
            }
        }

        public static void OnHurting(HurtingEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            try
            {
                if (!VeryUsualDay.Instance.ScpPlayers.TryGetValue(ev.Attacker.Id, out var avel) ||
                    avel != VeryUsualDay.Scps.Scp0762) return; // checks if attacker is not avel
                if (ev.Player != null && Random.Range(0, 100) < 40) ev.Attacker.Heal(25f);
                if (ev.Attacker.CurrentItem.As<Jailbird>()?.WearState != JailbirdWearState.AlmostBroken) return;
                ev.Attacker.CurrentItem?.Destroy();
                var jailbird = ev.Attacker.AddItem(ItemType.Jailbird);
                Timing.CallDelayed(0.1f, () => { ev.Attacker.CurrentItem = jailbird; });
            }
            catch
            {
                // ignored, just stfu about how bad it is
            }
        }

        public static void OnDied(DiedEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            ev.Player.CustomInfo = "Человек";
            ev.Player.MaxHealth = 100f;
            ev.Player.Scale = new Vector3(1f, 1f, 1f);
            
            if (VeryUsualDay.Instance.DBoysQueue.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.DBoysQueue.Remove(ev.Player.Id);
            }

            if (VeryUsualDay.Instance.JoinedDboys.Contains(ev.Player.Id))
            {
                VeryUsualDay.Instance.JoinedDboys.Remove(ev.Player.Id);
            }

            if (VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Green ||
                VeryUsualDay.Instance.CurrentCode == VeryUsualDay.Codes.Emerald)
            {
                Timing.CallDelayed(5f, () => { Ragdoll.GetLast(ev.Player).Destroy(); });
            }
        }

        public static void OnLeft(LeftEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
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
        }

        public static void OnShooting(ShootingEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            if (!VeryUsualDay.Instance.ScpPlayers.TryGetValue(ev.Player.Id, out var player)) return;
            if (player == VeryUsualDay.Scps.Scp035 && ev.Firearm.Type == ItemType.GunRevolver)
            {
                ev.Firearm.Ammo += 1;
            }
        }

        public static void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!VeryUsualDay.Instance.IsEnabledInRound) return;
            if (!VeryUsualDay.Instance.ScpPlayers.TryGetValue(ev.Player.Id, out var player)) return;
            if ((player != VeryUsualDay.Scps.Scp035 || ev.Usable.Type != ItemType.SCP500) &&
                ev.Usable.Type != ItemType.SCP207 && ev.Usable.Type != ItemType.AntiSCP207) return;
            ev.IsAllowed = false;
            ev.Item?.Destroy();
        }

        public static void OnVerified(VerifiedEventArgs ev)
        {
            if (VeryUsualDay.Instance.IsEnabledInRound)
            {
                var userData = (ITuple)VeryUsualDay.CheckIfPlayerInPrison(ev.Player);
                if ((bool)userData[0])
                {
                    ev.Player.Mute();
                    ev.Player.EnableEffect(EffectType.SilentWalk, 255);
                    ev.Player.Teleport(VeryUsualDay.PrisonPosition);
                    ev.Player.SessionVariables.Add("isInPrison", true);
                    ev.Player.SessionVariables.Add("prisonTime", (int)userData[1]);
                    ev.Player.SessionVariables.Add("prisonReason", (string)userData[2]);
                }
            }
        }
    }
}