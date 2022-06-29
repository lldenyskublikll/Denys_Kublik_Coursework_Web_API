using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Models
{
    public class DB_object // модель для вхідних даних при додаванні елементу до бази даних
    { 
        public int Telegram_ID { get; set; }
        public int Title_ID { get; set; }
        public string Title_name { get; set; }
        public string Title_type { get; set; }    
    }

    public class DB_response // модель для отримуваних даних з БД
    {
        public int Title_ID { get; set; }
        public string Title_name { get; set; }
        public string Title_type { get; set; }
    }
}
