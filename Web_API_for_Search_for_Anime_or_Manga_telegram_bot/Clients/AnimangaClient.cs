using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Models;
using System.Net.Http.Headers;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Clients
{
    public class AnimangaClient
    {
        private readonly HttpClient _client;        
        private static string _address;
        public static string _apikey;

        public AnimangaClient() 
        {
            _address = Constants.adress;
            _apikey = Constants.apikey;

            _client = new HttpClient();
            
            _client.BaseAddress = new Uri(_address);
            
            _client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", _apikey);

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Anime_Methods
        public async Task<Anime_by_name_model> Get_anime_ID_by_anime_name(string q, int limit)  // отримання списку аніме, назва яких схожа на введену користувачем стрінгу
        {
            var response = await _client.GetAsync($"/v2/anime?q={q}&limit={limit}");           
            //response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }

            // convert from json

            var result = JsonConvert.DeserializeObject<Anime_by_name_model>(content);

            return result;
        }

        public async Task<Anime_by_ID_Model> Get_anime_info_by_anime_ID(int ID) // отримання повної інформації про аніме за його айді
        {
            var response = await _client.GetAsync($"/v2/anime/{ID}?fields=title,main_picture,alternative_titles,id,mean,rank,popularity,media_type,genres,rating,num_episodes,start_season,status,source,studios,related_anime,recommendations");
            //response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }

            // convert from json

            var result = JsonConvert.DeserializeObject<Anime_by_ID_Model>(content);

            return result;
        }

        public async Task<Anime_ranking_Model> Get_anime_ranking(string ranking_type, int limit) // отримання ранкінгу аніме
        {
            var response = await _client.GetAsync($"/v2/anime/ranking?ranking_type={ranking_type}&limit={limit}");
            //response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }

            // convert from json

            var result = JsonConvert.DeserializeObject<Anime_ranking_Model>(content);

            return result;
        }
        #endregion


        #region Manga_Methods
        public async Task<Manga_by_name_model> Get_manga_ID_by_manga_name(string q, int limit) // отримання списку манги, назва якої схожа на введену користувачем стрінгу
        {
            var response = await _client.GetAsync($"/v2/manga?q={q}&limit={limit}");
            //response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }

            // convert from json

            var result = JsonConvert.DeserializeObject<Manga_by_name_model>(content);

            return result;
        }

        public async Task<Manga_by_ID_Model> Get_manga_info_by_manga_ID(int ID) // отримання повної інформації про мангу за її айді
        {
            var response = await _client.GetAsync($"/v2/manga/{ID}?fields=title,main_picture,"+"alternative_titles,id,genres,mean,rank,popularity,authors{first_name,last_name},serialization,media_type,status,num_volumes,num_chapters,related_manga,recommendations");
            //response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }

            // convert from json

            var result = JsonConvert.DeserializeObject<Manga_by_ID_Model>(content);

            return result;
        }

        public async Task<Manga_ranking_Model> Get_manga_ranking(string ranking_type, int limit) // отримання ранкінгу манги
        {
            var response = await _client.GetAsync($"/v2/manga/ranking?ranking_type={ranking_type}&limit={limit}");
            //response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
           
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }

            // convert from json

            var result = JsonConvert.DeserializeObject<Manga_ranking_Model>(content);

            return result;
        }
        #endregion
    }
}
