using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Exiled.API.Enums;
using MEC;
using Newtonsoft.Json;
using PlayerRoles;

namespace VeryUsualDay
{
    public static class PrisonController
    {
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", VeryUsualDay.Instance.Config.AuthToken);
                var response = client.PostAsync($"{VeryUsualDay.Instance.Config.BaseApiUrl}/aban", content).Result;
                if (response.IsSuccessStatusCode && VeryUsualDay.Instance.IsEnabledInRound)
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
                            player.Teleport(VeryUsualDay.PrisonPosition);
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", VeryUsualDay.Instance.Config.AuthToken);
                var response = client.GetAsync($"{VeryUsualDay.Instance.Config.BaseApiUrl}/aban?steamId={player.UserId}").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<List<string>>(content);
                return (response.IsSuccessStatusCode, int.Parse(json[0]), json[1]);
            }
        }
    }
}