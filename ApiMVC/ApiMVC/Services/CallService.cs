using ApiMVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ApiMVC.Services
{
    public class CallService: Controller
    {
        private readonly HttpClient _httpClient;

        public CallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Make a call and save to database ( GET METHOD )
        public async Task<List<Call>> MakePhoneCall()
        {
            var response = await _httpClient.GetAsync("https://dial.voip24h.vn/dial?voip=76af0a0d5f8445fa649525123d713c6bc2b2f9b8&secret=1366b46c23edb28f61aeae42fd571e00&sip=124&phone=0812195764");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(jsonResponse);
            var eventJson = jsonObject["events"].ToString();
            var events = JsonConvert.DeserializeObject<List<Call>>(eventJson);
            return events;
        }
    }
}