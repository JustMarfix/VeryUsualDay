using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using Newtonsoft.Json;
using PlayerRoles;
using UnityEngine;
using Player = VeryUsualDay.Handlers.Player;
using PlayerHandler = Exiled.Events.Handlers.Player;
using Random = UnityEngine.Random;
using Server = VeryUsualDay.Handlers.Server;
using ServerHandler = Exiled.Events.Handlers.Server;

namespace VeryUsualDay
{
    public class VeryUsualDay : Plugin<Config>
    {
        public static VeryUsualDay Instance { get; private set; }

        public override string Author => "JustMarfix";
        public override string Name => "VeryUsualDay (FX Version)";

        public override Version Version => new Version(4, 5, 1);

        public bool IsEnabledInRound { get; set; }
        public bool IsLunchtimeActive { get; set; }
        public bool IsDboysSpawnAllowed { get; set; }
        public bool IsTeslaEnabled { get; set; }
        public List<int> JoinedDboys { get; set; } = new List<int>();
        public List<int> DBoysQueue { get; set; } = new List<int>();
        public List<int> Shakheds { get; set; } = new List<int>();
        public List<RoomType> ChaosRooms { get; set; } = new List<RoomType>();
        public int BuoCounter { get; set; } = 1;
        public int SpawnedDboysCounter { get; set; } = 1;
        public int SpawnedWorkersCounter { get; set; } = 1;
        public int SpawnedScientistCounter { get; set; } = 1;
        public int SpawnedSecurityCounter { get; set; } = 1;
        
        private readonly Vector3 _armedPersonnelTowerCoords = new Vector3(-16f, 1014.5f, -32f);
        private readonly Vector3 _civilianPersonnelTowerCoords = new Vector3(44.4f, 1014.5f, -51.6f);
        public readonly Vector3 SpawnPosition = new Vector3(139.487f, 995.392f, -16.762f);
        public static readonly Vector3 PrisonPosition = new Vector3(130.233f, 993.766f, 21.049f);
        public Vector3 SupplyBoxCoords = new Vector3();

        public readonly List<Vector3> EmfSupplyCoords = new List<Vector3>
        {
            new Vector3(-10.682f, 1003f, -32.115f),
            new Vector3(-6.873f, 1003f, -32f)
        };
        
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
            Scp035,
            Scp0352,
            Scp049,
            Scp0762,
            Scp372,
            Scp682,
            Scp966,
            Scp999
        }

        public Codes CurrentCode { get; set; } = Codes.Green;
        public Dictionary<int, Scps> ScpPlayers { get; set; } = new Dictionary<int, Scps>();

        public override void OnEnabled()
        {
            Instance = this;
            if (Instance.Config.AuthToken == "")
            {
                Log.Error("AuthToken пуст - функционал тюрьмы и БД будет недоступен.");
            }
            PlayerHandler.ChangingRole += Player.OnChangingRole;
            PlayerHandler.PickingUpItem += Player.OnPickingUpItem;
            PlayerHandler.DroppingItem += Player.OnDroppingItem;
            PlayerHandler.Hurting += Player.OnHurting;
            PlayerHandler.Died += Player.OnDied;
            PlayerHandler.Left += Player.OnLeft;
            PlayerHandler.Shooting += Player.OnShooting;
            PlayerHandler.UsingItem += Player.OnUsingItem;
            PlayerHandler.TriggeringTesla += Player.OnTriggeringTesla;
            PlayerHandler.Verified += Player.OnVerified;
            PlayerHandler.Hurt += Player.OnHurt;
            PlayerHandler.Healed += Player.OnHealed;
            ServerHandler.WaitingForPlayers += Server.OnWaitingForPlayers;
            ServerHandler.RoundStarted += Server.OnRoundStarted;
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
            PlayerHandler.TriggeringTesla -= Player.OnTriggeringTesla;
            PlayerHandler.Verified -= Player.OnVerified;
            PlayerHandler.Hurt -= Player.OnHurt;
            PlayerHandler.Healed -= Player.OnHealed;
            ServerHandler.WaitingForPlayers -= Server.OnWaitingForPlayers;
            ServerHandler.RoundStarted -= Server.OnRoundStarted;
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
        
        public IEnumerator<float> _prisonTimer()
        {
            for (;;)
            {
                foreach (var player in Exiled.API.Features.Player.List)
                {
                    if (player.TryGetSessionVariable("isInPrison", out bool prisonState) && prisonState)
                    {
                        player.TryGetSessionVariable("prisonTime", out Int32 time);
                        // Log.Info($"Игроку {player.UserId} осталось находиться в тюрьме {time} секунд.");
                        time -= 1;
                        player.SessionVariables.Remove("prisonTime");
                        player.SessionVariables.Add("prisonTime", time);
                        if (time == 0)
                        {
                            SendToPrison(player, 0, "Выпущен из тюрьмы.");
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }

        public IEnumerator<float> _chaos()
        {
            for (;;)
            {
                foreach (var roomType in ChaosRooms)
                {
                    var room = Room.Get(roomType);
                    foreach (var door in room.Doors.Where(p => !p.IsElevator && !p.IsGate && !p.IsKeycardDoor && !p.IsLocked).Shuffle())
                    {
                        Timing.CallDelayed(Random.Range(0f, 1.5f), () =>
                        {
                            door.IsOpen = !door.IsOpen;
                        });
                    }

                    foreach (var player in room.Players)
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            var score = Random.Range(0, 100);
                            if (score < 25)
                            {
                                player.PlayGunSound(ItemType.GunCrossvec, (byte)Random.Range(175, 256));
                            }
                            else if (score < 50)
                            {
                                player.PlayGunSound(ItemType.GunFSP9, (byte)Random.Range(175, 256));
                            }
                            else if (score < 75)
                            {
                                player.PlayGunSound(ItemType.GunE11SR, (byte)Random.Range(175, 256));
                            }
                            else 
                            {
                                player.PlayGunSound(ItemType.GunShotgun, (byte)Random.Range(175, 256));
                            }
                        }
                        else if (Random.Range(0, 100) < 30)
                        {
                            player.PlayBeepSound();
                        }

                        else if (Random.Range(0, 100) < 30)
                        {
                            player.PlayShieldBreakSound();
                        }
                        else
                        {
                            player.EnableEffect(EffectType.Scanned, duration: 10f);
                        }
                    }
                }
                yield return Timing.WaitForSeconds(0.75f);
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

        private static async Task<string> HttpGetUser(string steamid)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Instance.Config.AuthToken);
                var content = await client.GetStringAsync($"http://justmeow.ru:9000/get_user/{steamid}");
                return content;
            }
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
            player.CustomInfo = new StringBuilder().Append($"\"{splitted[0]}\" ").Append(string.Join(" ", splitted.Skip(1))).ToString(); // add quotes to the name and conc. to the main string
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

                        if (Instance.Config.ScienceHealth.ContainsKey(json[4]))
                        {
                            player.MaxHealth = Instance.Config.ScienceHealth[json[4]];
                            player.Health = Instance.Config.ScienceHealth[json[4]];
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
                        player.MaxHealth = Instance.Config.WorkersHealth[json[4]];
                        player.Health = Instance.Config.WorkersHealth[json[4]];
                        player.Teleport(_civilianPersonnelTowerCoords);
                    });
                    break;
                case "ГОР":
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
                case "ОВБ":
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
        
        public static TimeSpan ConvertToTimeSpan(string timeSpan)
        {
            var l = timeSpan.Length - 1;
            var value = timeSpan.Substring(0, l);
            var type = timeSpan.Substring(l, 1);

            switch (type)
            {
                case "d": return TimeSpan.FromDays(double.Parse(value));
                case "h": return TimeSpan.FromHours(double.Parse(value));
                case "m": return TimeSpan.FromMinutes(double.Parse(value));
                case "s": return TimeSpan.FromSeconds(double.Parse(value));
                default: return TimeSpan.FromSeconds(double.Parse(value));
            }
        }

        public static bool SendToPrison(Exiled.API.Features.Player player, int durationSeconds, string reason)
        {
            using (var client = new HttpClient())
            {
                var data = new Dictionary<string, string>
                {
                    { "steamId", player.UserId },
                    { "time", durationSeconds.ToString() },
                    { "reason", reason }
                };
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Instance.Config.AuthToken);
                var response = client.PostAsync($"http://justmeow.ru:9000/aban", content).Result;
                if (response.IsSuccessStatusCode && Instance.IsEnabledInRound)
                {
                    player.Role.Set(RoleTypeId.Tutorial);
                    Timing.CallDelayed(0.5f, () =>
                    {
                        if (durationSeconds == 0)
                        {
                            player.UnMute();
                            player.DisableEffect(EffectType.SilentWalk);
                            player.SessionVariables.Remove("isInPrison");
                            player.SessionVariables.Remove("prisonTime");
                            player.SessionVariables.Remove("prisonReason");
                        }
                        else
                        {
                            player.Mute();
                            player.EnableEffect(EffectType.SilentWalk, 255);
                            player.Teleport(PrisonPosition);
                            player.SessionVariables.Add("isInPrison", true);
                            player.SessionVariables.Add("prisonTime", durationSeconds);
                            player.SessionVariables.Add("prisonReason", reason);
                        }
                    });
                }
                return response.IsSuccessStatusCode;
            }
        }

        public static (bool, int, string) CheckIfPlayerInPrison(Exiled.API.Features.Player player)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Instance.Config.AuthToken);
                var response = client.GetAsync($"http://justmeow.ru:9000/aban?steamId={player.UserId}").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<List<string>>(content);
                return (response.IsSuccessStatusCode, int.Parse(json[0]), json[1]);
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
        
        public static bool In<T>(this T val, params T[] vals) => vals.Contains(val);

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable.ToList();
            var newEnum = Enumerable.Empty<T>();
            while (enumerable1.Count != 0)
            {
                var index = Random.Range(0, enumerable1.Count);
                newEnum = newEnum.Append(enumerable1[index]);
                enumerable1.RemoveAt(index);
            }
            return newEnum;
        }
    }
}
