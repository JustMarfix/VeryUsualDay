using PlayerHandler = Exiled.Events.Handlers.Player;
using ServerHandler = Exiled.Events.Handlers.Server;

using Exiled.API.Features;
using System.Collections.Generic;
using MEC;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Exiled.API.Enums;
using UnityEngine;
using System;

namespace VeryUsualDay
{
    public class VeryUsualDay : Plugin<Config>
    {
        public static VeryUsualDay Instance { get; set; } = new VeryUsualDay();
        public VeryUsualDay() { }

        public override string Author => "JustMarfix";
        public override string Name => "VeryUsualDay";

        public override Version Version => new Version(1, 2, 0);

        public bool IsEnabledInRound { get; set; } = false;
        public bool Is008Leaked { get; set; } = false;
        public bool IsLunchtimeActive { get; set; } = false;
        public List<int> LockerPlayers { get; set; } = new List<int>() { };
        public List<int> Avels { get; set; } = new List<int>() { };
        public List<int> Zombies { get; set; } = new List<int>() { };
        public int BUOCounter { get; set; } = 0;
        public enum Codes
        {
            Green,
            Emerald,
            Blue,
            Yellow,
            Red
        }

        public Codes CurrentCode { get; set; } = Codes.Green;

        private Handlers.Player player;
        private Handlers.Server server;

        public override void OnEnabled()
        {
            player = new Handlers.Player();
            server = new Handlers.Server();
            PlayerHandler.ChangingRole += player.OnChangingRole;
            PlayerHandler.InteractingDoor += player.OnInteractingDoor;
            PlayerHandler.PickingUpItem += player.OnPickingUpItem;
            PlayerHandler.DroppingItem += player.OnDroppingItem;
            PlayerHandler.Hurting += player.OnHurting;
            PlayerHandler.Died += player.OnDied;
            PlayerHandler.Left += player.OnLeft;
            ServerHandler.WaitingForPlayers += server.OnWaitingForPlayers;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerHandler.ChangingRole -= player.OnChangingRole;
            PlayerHandler.InteractingDoor -= player.OnInteractingDoor;
            PlayerHandler.PickingUpItem -= player.OnPickingUpItem;
            PlayerHandler.DroppingItem -= player.OnDroppingItem;
            PlayerHandler.Hurting -= player.OnHurting;
            PlayerHandler.Died -= player.OnDied;
            PlayerHandler.Left -= player.OnLeft;
            ServerHandler.WaitingForPlayers -= server.OnWaitingForPlayers;
            player = null;
            server = null;
            base.OnDisabled();
        }

        public IEnumerator<float> _avel()
        {
            for (;;)
            {
                foreach (int i in Avels)
                {
                    try
                    {
                        if (Player.TryGet(i, out Player avel) && avel.Health < 5000)
                        {
                            avel.Heal(50f);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        Log.Error(e.Message);
                    }
                }
                yield return Timing.WaitForSeconds(7f);
            }
        }

        public IEnumerator<float> _008_poisoning()
        {
            for (;;)
            {
                foreach (Player target in Player.Get(player => player.Zone == ZoneType.HeavyContainment && !Instance.Config.DoNotPoisonRoles.Contains(player.Role.Type) && player.Role.Team != PlayerRoles.Team.SCPs && (player.CustomInfo is null || !player.CustomInfo.ToLower().Contains("scp"))))
                {
                    target.EnableEffect(EffectType.Poisoned);
                }
                yield return Timing.WaitForSeconds(10f);
            }
        }

        static async Task<string> HttpGetUser(string steamid)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync($"http://justmeow.ru:9000/get_user/{steamid}");
            return content;
        }

        public async void SetUserRole(Player player)
        {
            string json_string = await HttpGetUser(player.UserId.ToString());
            List<string> json = JsonConvert.DeserializeObject<List<string>>(json_string);
            if (json.Count == 0)
            {
                return;
            }
            player.CustomInfo = $"\"{json[1]}\"";
            player.CustomName = json[2];
            if (json[3] == "СБ")
            {
                player.Role.Set(PlayerRoles.RoleTypeId.FacilityGuard, PlayerRoles.RoleSpawnFlags.None);
                Timing.CallDelayed(2f, () =>
                {
                    foreach (ItemType item in Instance.Config.SecurityItems[json[4]])
                    {
                        player.AddItem(item);
                    }
                    foreach (AmmoType ammo in Instance.Config.SecurityAmmo[json[4]])
                    {
                        player.AddAmmo(ammo, 60);
                    }
                    player.MaxHealth = Instance.Config.SecurityHealth[json[4]];
                    player.Health = Instance.Config.SecurityHealth[json[4]];
                    player.Teleport(new UnityEngine.Vector3(-16f, 1014.5f, -32f));
                });
            }
            else if (json[3] == "НС")
            {
                player.Role.Set(PlayerRoles.RoleTypeId.Scientist, PlayerRoles.RoleSpawnFlags.None);
                Timing.CallDelayed(2f, () =>
                {
                    foreach (ItemType item in Instance.Config.ScientificItems[json[4]])
                    {
                        player.AddItem(item);
                    }
                    player.Teleport(new Vector3(44.4f, 1014.5f, -51.6f));
                });
            }
            else if (json[3] == "Уборщики")
            {
                player.Role.Set(PlayerRoles.RoleTypeId.ClassD, PlayerRoles.RoleSpawnFlags.None);
                Timing.CallDelayed(2f, () =>
                {
                    foreach (ItemType item in Instance.Config.JanitorsItems[json[4]])
                    {
                        player.AddItem(item);
                    }
                    player.Teleport(new UnityEngine.Vector3(44.4f, 1014.5f, -51.6f));
                });
            }
            else if (json[3] == "ЭВС")
            {
                player.Role.Set(Instance.Config.EMFRoles[json[4]], PlayerRoles.RoleSpawnFlags.None);
                Timing.CallDelayed(2f, () =>
                {
                    foreach (ItemType item in Instance.Config.EMFItems[json[4]])
                    {
                        player.AddItem(item);
                    }
                    foreach (AmmoType ammo in Instance.Config.EMFAmmo[json[4]])
                    {
                        player.AddAmmo(ammo, 60);
                    }
                    player.MaxHealth = Instance.Config.EMFHealth[json[4]];
                    player.Health = Instance.Config.EMFHealth[json[4]];
                    player.Teleport(new Vector3(-16f, 1014.5f, -32f));
                });
            }
            else if (json[3] == "Агентство")
            {
                player.Role.Set(PlayerRoles.RoleTypeId.Tutorial, PlayerRoles.RoleSpawnFlags.None);
                Timing.CallDelayed(2f, () =>
                {
                    player.Scale = new UnityEngine.Vector3(1.3f, 0.8f, 1f);
                    foreach (ItemType item in Instance.Config.AgencyItems[json[4]])
                    {
                        player.AddItem(item);
                    }
                    foreach (AmmoType ammo in Instance.Config.AgencyAmmo[json[4]])
                    {
                        player.AddAmmo(ammo, 60);
                    }
                    player.MaxHealth = Instance.Config.AgencyHealth;
                    player.Health = Instance.Config.AgencyHealth;
                    player.IsGodModeEnabled = false;
                    player.Teleport(new UnityEngine.Vector3(-16f, 1014.5f, -32f));
                });
            }
        }
        public void RoleDistribution()
        {
            foreach (Player player in Player.Get(PlayerRoles.RoleTypeId.Tutorial))
            {
                if (player.CustomInfo == "Человек" || player.CustomInfo is null)
                {
                    SetUserRole(player);
                }
            }
        }
    }
}
