using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Extensions;
using Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Models;
using Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Clients;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimangaController : ControllerBase
    {
        private readonly ILogger<AnimangaController> _logger;
        private readonly AnimangaClient _animangaClient;
        private readonly DynamoDataBaseClient _dynamoDataBaseClient;

        public AnimangaController(ILogger<AnimangaController> logger, AnimangaClient animangaClient, DynamoDataBaseClient dynamoDataBaseClient)
        {
            _logger = logger;
            _animangaClient = animangaClient;
            _dynamoDataBaseClient = dynamoDataBaseClient;
        }



        #region GET_requests_for_anime

        // знаходить тайтли (аніме), назва яких схожа або містить у собі введену вами стрінгу
        // вхіжні дані:
        // -- стрінга (бажано вводити з англ. розкладки)
        [HttpGet("Search_by_anime_name")]
        public async Task<IActionResult> Get_anime_ID_by_anime_name([FromQuery] Search_by_name_parameters parameters)
        {
            var anime_id = await _animangaClient.Get_anime_ID_by_anime_name(parameters.Q, parameters.Limit);

            if (anime_id == null)
            {
                return NotFound("Not found");
            }

            return Ok(anime_id);
        }


        // знаходить повну інформацію про тайтл (аніме) за його ID
        // вхідні дані:
        // -- ID тайтлу
        [HttpGet("Get_anime_by_id_info")]
        public async Task<IActionResult> Get_anime_info_by_ID([FromQuery] Search_by_ID_parameters parameters)
        {
            var anime_info = await _animangaClient.Get_anime_info_by_anime_ID(parameters.ID);

            if (anime_info == null)
            {
                return NotFound("Not found");
            }

            return Ok(anime_info);
        }


        // повертає ранкінг аніме за популярністю (можна обрати рейтинг за типом аніме)
        // вхідні дані:
        // -- кількість результуючих даних;
        // -- типи аніме, за яким буде проводитись ранкінг:
        //                                                 * all (усі)
        //                                                 * airing (ті, що зараз виходять)
        //                                                 * upcoming (анонси)
        //                                                 * TV (ТВ)
        //                                                 * OVA (ОВА)
        //                                                 * movie (фільм)
        //                                                 * special (спешл)
        //                                                 * bypopularity (за популярністю на MAL)
        //                                                 * favorite (за додаванням до улюблених на МАL)
        [HttpGet("Get_anime_ranking")]
        public async Task<IActionResult> Get_anime_ranking([FromQuery] Ranking_parameters parameters)
        {
            var anime_ranking = await _animangaClient.Get_anime_ranking(parameters.Ranking_model, parameters.Limit);

            if (anime_ranking == null)
            {
                return NotFound("Not found");
            }

            return Ok(anime_ranking);
        }

        #endregion



        #region GET_requests_for_manga

        // знаходить тайтли (мангу), назва яких схожа або містить у собі введену вами стрінгу
        // вхіжні дані:
        // -- стрінга (бажано вводити з англ. розкладки)
        [HttpGet("Search_by_manga_name")]        
        public async Task<IActionResult> Get_manga_ID_by_manga_name([FromQuery] Search_by_name_parameters parameters)
        {
            var manga_id = await _animangaClient.Get_manga_ID_by_manga_name(parameters.Q, parameters.Limit);

            if (manga_id == null)
            {
                return NotFound("Not found");
            }

            return Ok(manga_id);
        }


        // знаходить повну інформацію про тайтл (мангу) за його ID
        // вхідні дані:
        // -- ID тайтлу
        [HttpGet("Get_manga_by_id_info")]
        public async Task<IActionResult> Get_manga_info_by_ID([FromQuery] Search_by_ID_parameters parameters)
        {
            var manga_info = await _animangaClient.Get_manga_info_by_manga_ID(parameters.ID);

            if (manga_info == null)
            {
                return NotFound("Not found");
            }

            return Ok(manga_info);
        }


        // повертає ранкінг манги за популярністю; можна обрати рейтинг за типом манги
        // вхідні дані:
        // -- кількість результуючих даних;
        // -- типи манги, за яким буде проводитись ранкінг:
        //                                                 * all (усі)
        //                                                 * manga (манга)
        //                                                 * novels (новели)
        //                                                 * oneshots (ваншоти)
        //                                                 * doujin (додзін)
        //                                                 * manhwa (манхва)
        //                                                 * manhua (маньхуа)
        //                                                 * bypopularity (за популярністю на MAL)
        //                                                 * favorite (за додаванням до улюблених на МАL)
        [HttpGet("Get_manga_ranking")]
        public async Task<IActionResult> Get_manga_ranking([FromQuery] Ranking_parameters parameters)
        {
            var manga_ranking = await _animangaClient.Get_manga_ranking(parameters.Ranking_model, parameters.Limit);

            if (manga_ranking == null)
            {
                return NotFound("Not found");
            }

            return Ok(manga_ranking);
        }

        #endregion




        #region Requests_for_Dynamo_DB

        // перевіряє, чи існує в базі даних елемент з заданими параметрами
        // вхідні дані:
        // -- айді телеграм-аккаунта юзера
        // -- айді тайтлу, який знаходиться, або не знаходиться у базі даних
        [HttpGet("Get_info_about item_from_DB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get_Data_from_DB([FromQuery] DB_parameters parameters)
        {
            var result = await _dynamoDataBaseClient.Get_info_about_user_favourites(parameters.Telegram_ID, parameters.Title_ID);

            if (result == null)
            {
                return NotFound("This item doesn't exist in your 'Favourites list'");
            }

            var DB_response = new DB_object
            {
                Telegram_ID = result.Telegram_ID,
                Title_ID = result.Title_ID,
                Title_name = result.Title_name,
                Title_type = result.Title_type
            };

            return Ok(DB_response);
        }



        // перевіряє, чи існують у базі даних елементи юзера
        // вхідні дані:
        // -- айді телеграм-аккаунта юзера       
        [HttpGet("Get_ALL_user_favourites_from_DB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get_All_user_favourites_from_DB(int Telegram_user_ID)
        {
            var response = await _dynamoDataBaseClient.Get_ALL_user_data_from_DynamoDB(Telegram_user_ID);

            if (response == null)
            {
                return NotFound("There are no records in your 'Favourites list'");
            }

            var result = response
                .Select(i => new DB_response()
                {
                    Title_ID = i.Title_ID,
                    Title_name = i.Title_name,
                    Title_type = i.Title_type
                })
                .ToList();

            return Ok(result);
        }



        // додає певний елемент до бази даних 
        // вхідні дані:
        // -- айді телеграм-аккаунта юзера
        // -- айді тайтлу, який юзер бажає додати
        // -- назва тайтлу
        // -- тип тайтлу (аніме або манга)
        [HttpPost("Add_favourite_item_to_DB")]
        public async Task<IActionResult> Add_item_to_favourites_database([FromBody] DB_object db_object)
        {
            var data = new DB_object
            {
                Telegram_ID = db_object.Telegram_ID,
                Title_ID = db_object.Title_ID,
                Title_name = db_object.Title_name,
                Title_type = db_object.Title_type
            };

            var result = await _dynamoDataBaseClient.Post_Data_to_DynamoDB(data);

            if (result == false)
            {
                return BadRequest("Unablle to add this item to your 'Favourites list'");
            }

            return Ok("The item was successfully added to your 'Favourites list'");
        }



        // видаляє певний елемент з бази даних
        // вхідні дані:
        // -- айді телеграм-аккаунта юзера
        // -- айді тайтлу, який юзер бажає видалити
        [HttpDelete("Delete_item_from_DB")]
        public async Task<IActionResult> Delete_item_fron_favourites_database([FromQuery] DB_parameters parameters)
        {
            var result = await _dynamoDataBaseClient.Delete_Data_from_DynamoDB(parameters.Telegram_ID, parameters.Title_ID);

            if (result == false)
            {
                return BadRequest("You don't have this item in your 'Favourites list'");
            }

            return Ok("The item was successfully deleted from your 'Favourites list'");
        }



        // видаляє певний елемент з бази даних
        // вхідні дані:
        // -- айді телеграм-аккаунта юзера
        // спочатку перевіряє наявність елементів у даного юзера
        // якщо елементи є, то видаляє їх
        [HttpDelete("Delete_ALL_user_data_from_DB")]
        public async Task<IActionResult> Delete_ALL_user_items_from_favourites_database(int Telegram_user_ID)
        {
            var result = await _dynamoDataBaseClient.Delete_ALL_user_data_from_DynamoDB(Telegram_user_ID);

            if (result == false)
            {
                return BadRequest("You don't have any items in your 'Favourites list'");
            }

            return Ok("All items were successfully deleted from your 'Favourites list'");
        }
        #endregion








        //[HttpPost("info_to_DB")]
        //public async Task<IActionResult> Add_item_to_favourites_database([FromBody] DB_object db_object) 
        //{
        //    var data = new DB_object
        //    {
        //        Telegram_ID = db_object.Telegram_ID,
        //        Title_ID = db_object.Title_ID,
        //        Title_name = db_object.Title_name,
        //        Title_type = db_object.Title_type

        //    };

        //    await _dynamoDataBaseClient.Post_Data_to_DynamoDB(data);

        //    return Ok();
        //}
    }
}
