using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace FreshFarmMarket_211283E.Google
{
	public class GoogleCaptchaService
	{
		private readonly IOptionsMonitor<GoogleCaptchaConfig> _config;

		public GoogleCaptchaService(IOptionsMonitor<GoogleCaptchaConfig> config) { 
			_config = config;
		}
		public async Task<bool> VerifyToken(string token)
		{
			try
			{
				var url = $"https://www.google.com/recaptcha/api/siteverify?secret={ _config.CurrentValue.SecretKey }&response={ token }";
				using (HttpClient client = new HttpClient())
				{
					var httpResult = await client.GetAsync(url);
					if (httpResult.StatusCode != HttpStatusCode.OK) {
						return false;
					}

					var responseString = await httpResult.Content.ReadAsStringAsync();

					var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);
					return googleResult.success && googleResult.score >= 0.5;
				}
			}catch(Exception e)
			{
				return false;
			}
		}
	}
}
