using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Newtonsoft.Json;
using PlayerRoles;
using UnityEngine;
using Player = VeryUsualDay.Handlers.Player;
using PlayerHandler = Exiled.Events.Handlers.Player;
using Server = VeryUsualDay.Handlers.Server;
using ServerHandler = Exiled.Events.Handlers.Server;

namespace VeryUsualDay
{
    public class VeryUsualDay : Plugin<Config>
    {
        public static VeryUsualDay Instance { get; private set; }

        public override string Author => "JustMarfix";
        public override string Name => "VeryUsualDay";

        public override Version Version => new Version(3, 1, 0);

        public bool IsEnabledInRound { get; set; }
        public bool Is008Leaked { get; set; }
        public bool IsLunchtimeActive { get; set; }
        public bool IsDboysSpawnAllowed { get; set; }
        public List<int> Zombies { get; set; } = new List<int>();
        public List<int> JoinedDboys { get; set; } = new List<int>();
        public List<int> DBoysQueue { get; set; } = new List<int>();
        public int BuoCounter { get; set; } = 1;
        public int SpawnedDboysCounter { get; set; } = 1;
        public int SpawnedJanitorsCounter { get; set; } = 1;
        public int SpawnedScientistCounter { get; set; } = 1;
        public int SpawnedSecurityCounter { get; set; } = 1;
        private readonly Vector3 _armedPersonnelTowerCoords = new Vector3(-16f, 1014.5f, -32f);
        private readonly Vector3 _civilianPersonnelTowerCoords = new Vector3(44.4f, 1014.5f, -51.6f);
        
        public enum Codes
        {
            [Description("Зелёный")]
            Green,
            [Description("Изумрудный")]
            Emerald,
            [Description("Синий")]
            Blue,
            [Description("Оранжевый")]
            Orange,
            [Description("Жёлтый")]
            Yellow,
            [Description("Красный")]
            Red
        }

        public enum Scps
        {
            Scp0082,
            Scp035,
            Scp0352,
            Scp049,
            Scp0762,
            Scp372,
            Scp575,
            Scp682,
            Scp966,
            Scp999
        }

        public Codes CurrentCode { get; set; } = Codes.Green;
        public Dictionary<int, Scps> ScpPlayers { get; set; } = new Dictionary<int, Scps>();

        public override void OnEnabled()
        {
            Instance = this;
            PlayerHandler.ChangingRole += Player.OnChangingRole;
            PlayerHandler.PickingUpItem += Player.OnPickingUpItem;
            PlayerHandler.DroppingItem += Player.OnDroppingItem;
            PlayerHandler.Hurting += Player.OnHurting;
            PlayerHandler.Died += Player.OnDied;
            PlayerHandler.Left += Player.OnLeft;
            PlayerHandler.Shooting += Player.OnShooting;
            PlayerHandler.UsingItem += Player.OnUsingItem;
            ServerHandler.WaitingForPlayers += Server.OnWaitingForPlayers;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerHandler.ChangingRole -= Player.OnChangingRole;
            PlayerHandler.PickingUpItem -= Player.OnPickingUpItem;
            PlayerHandler.DroppingItem -= Player.OnDroppingItem;
            PlayerHandler.Hurting -= Player.OnHurting;
            PlayerHandler.Died -= Player.OnDied;
            PlayerHandler.Left -= Player.OnLeft;
            PlayerHandler.Shooting -= Player.OnShooting;
            ServerHandler.WaitingForPlayers -= Server.OnWaitingForPlayers;
            Instance = null;
            base.OnDisabled();
        }

        public IEnumerator<float> _joining()
        {
            for (;;)
            {
                if (CurrentCode == Codes.Green)
                {
                    var counter = 0;
                    foreach (var i in DBoysQueue.ToList())
                    {
                        if (!Exiled.API.Features.Player.TryGet(i, out var dboy)) continue;
                        if ((dboy.CustomInfo == "Человек" || dboy.CustomInfo is null) && dboy.Role.Type == RoleTypeId.Tutorial)
                        {
                            if (JoinedDboys.Count >= 5)
                            {
                                break;
                            }
                            if (counter == 3)
                            {
                                break;
                            }
                            dboy.CustomName = $"Испытуемый - ##-{SpawnedDboysCounter}";
                            JoinedDboys.Add(i);
                            dboy.Role.Set(RoleTypeId.ClassD);
                            Timing.CallDelayed(1f, () =>
                            {
                                dboy.ClearInventory();
                                dboy.Handcuff();
                                dboy.Broadcast(10, "<b>Вы стали <color=#EE7600>Испытуемым</color>! Можете сотрудничать с <color=#120a8f>фондом</color> или принимать попытки <color=#ff2b2b>побега</color> при первой возможности. </b>");
                            });
                            counter += 1;
                            SpawnedDboysCounter += 1;
                        }
                        DBoysQueue.Remove(i);
                    }
                    switch (counter)
                    {
                        case 0:
                            break;
                        case 1:
                            Cassie.Message("<b><color=#FF0090>O5</color> >>> <color=#008080>Комплекс</color></b>: один испытуемый прибыл в блок D. <size=0> . . . . . . . . . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
                            break;
                        case 2:
                            Cassie.Message("<b><color=#FF0090>O5</color> >>> <color=#008080>Комплекс</color></b>: двое испытуемых прибыло в блок D<size=0> . . . . . . . . . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
                            break;
                        case 3:
                            Cassie.Message("<b><color=#FF0090>O5</color> >>> <color=#008080>Комплекс</color></b>: трое испытуемых прибыло в блок D<size=0> . . . . . . . . . . . . . . . . . . . . . .", isNoisy: false, isSubtitles: true);
                            break;
                    }
                }
                yield return Timing.WaitForSeconds(300f);
            }
        }

        /*
        public IEnumerator<float> _avel()
        {
            for (;;)
            {
                foreach (var pair in ScpPlayers.Where(p => p.Value == Scps.Scp0762))
                {
                    try
                    {
                        if (Exiled.API.Features.Player.TryGet(pair.Key, out var avel) && avel.Health < 5000)
                        {
                            avel.Heal(50f);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
                yield return Timing.WaitForSeconds(7f);
            }
        }*/

        public IEnumerator<float> _008_poisoning()
        {
            for (;;)
            {
                foreach (var target in Exiled.API.Features.Player.Get(player => player.Zone == ZoneType.HeavyContainment && !Instance.Config.DoNotPoisonRoles.Contains(player.Role.Type) && player.Role.Team != Team.SCPs && (player.CustomInfo is null || !player.CustomInfo.ToLower().Contains("scp"))))
                {
                    target.EnableEffect(EffectType.Poisoned);
                }
                yield return Timing.WaitForSeconds(10f);
            }
        }

        private static async Task<string> HttpGetUser(string steamid)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync($"http://justmeow.ru:9000/get_user/{steamid}");
            return content;
        }

        private async void SetUserRole(Exiled.API.Features.Player player)
        {
            var jsonString = await HttpGetUser(player.UserId);
            var json = JsonConvert.DeserializeObject<List<string>>(jsonString);
            if (json.Count == 0)
            {
                return;
            }

            var splitted = json[1].Split(' ');
            player.CustomInfo = new StringBuilder().Append($"\"{splitted[0]}\" ").Append(string.Join(" ", splitted.Skip(1))).ToString();
            player.CustomName = json[2];
            switch (json[3])
            {
                case "СБ":
                    player.Role.Set(RoleTypeId.FacilityGuard, RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        foreach (var item in Instance.Config.SecurityItems[json[4]])
                        {
                            player.AddItem(item);
                        }
                        foreach (var ammo in Instance.Config.SecurityAmmo[json[4]])
                        {
                            player.AddAmmo(ammo, 60);
                        }
                        player.MaxHealth = Instance.Config.SecurityHealth[json[4]];
                        player.Health = Instance.Config.SecurityHealth[json[4]];
                        player.Teleport(_armedPersonnelTowerCoords);
                    });
                    break;
                case "НС":
                    player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        foreach (var item in Instance.Config.ScientificItems[json[4]])
                        {
                            player.AddItem(item);
                        }
                        player.Teleport(_civilianPersonnelTowerCoords);
                    });
                    break;
                case "Рабочие":
                    player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        foreach (var item in Instance.Config.WorkersItems[json[4]])
                        {
                            player.AddItem(item);
                        }
                        foreach (var pair in Instance.Config.WorkersEffects[json[4]])
                        {
                            player.EnableEffect(pair.Key);
                            player.ChangeEffectIntensity(pair.Key, pair.Value);
                        }
                        player.MaxHealth = Instance.Config.SecurityHealth[json[4]];
                        player.Health = Instance.Config.SecurityHealth[json[4]];
                        player.Teleport(_civilianPersonnelTowerCoords);
                    });
                    break;
                case "ЭВС":
                    player.Role.Set(Instance.Config.EmfRoles[json[4]], RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        foreach (var item in Instance.Config.EmfItems[json[4]])
                        {
                            player.AddItem(item);
                        }
                        foreach (var ammo in Instance.Config.EmfAmmo[json[4]])
                        {
                            player.AddAmmo(ammo, 60);
                        }
                        foreach (var pair in Instance.Config.EmfEffects[json[4]])
                        {
                            player.EnableEffect(pair.Key);
                            player.ChangeEffectIntensity(pair.Key, pair.Value);
                        }
                        player.MaxHealth = Instance.Config.EmfHealth[json[4]];
                        player.Health = Instance.Config.EmfHealth[json[4]];
                        player.Teleport(_armedPersonnelTowerCoords);
                    });
                    break;
                case "Агентство":
                    player.Role.Set(RoleTypeId.Tutorial, RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        player.Scale = new Vector3(1.3f, 0.8f, 1f);
                        foreach (var item in Instance.Config.AgencyItems[json[4]])
                        {
                            player.AddItem(item);
                        }
                        foreach (var ammo in Instance.Config.AgencyAmmo[json[4]])
                        {
                            player.AddAmmo(ammo, 60);
                        }
                        player.MaxHealth = Instance.Config.AgencyHealth;
                        player.Health = Instance.Config.AgencyHealth;
                        player.IsGodModeEnabled = false;
                        player.Teleport(_armedPersonnelTowerCoords);
                    });
                    break;
                case "Административный персонал":
                    player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        foreach (var item in Instance.Config.AdministrativeItems[json[4]])
                        {
                            player.AddItem(item);
                        }
                        player.MaxHealth = Instance.Config.AdministrativeHealth[json[4]];
                        player.Health = Instance.Config.AdministrativeHealth[json[4]];
                        player.Teleport(_civilianPersonnelTowerCoords);
                    });
                    break;
                case "Испытуемые":
                    player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.None);
                    Timing.CallDelayed(2f, () =>
                    {
                        player.ClearInventory();
                        player.Handcuff();
                        player.MaxHealth = 130f;
                        player.Health = 130f;
                    });
                    break;
            }
        }
        public void RoleDistribution()
        {
            foreach (var player in Exiled.API.Features.Player.Get(RoleTypeId.Tutorial))
            {
                if (player.CustomInfo == "Человек" || player.CustomInfo is null)
                {
                    SetUserRole(player);
                }
            }
        }
    }
    public static class ReflectionHelpers
    {
        private static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
}
