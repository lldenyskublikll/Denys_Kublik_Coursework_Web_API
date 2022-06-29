using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Models
{
    public class Ranking_parameters // модель вхідних даних для отримання ранкінгу аніме/манги
    {
        public string Ranking_model { get; set; }
        public int Limit { get; set; }
    }

    public class Search_by_ID_parameters // модель вхідних даних для отримання повної інформації про тайтл 
    {
        public int ID { get; set; }
    }

    public class Search_by_name_parameters // модель вхідних даних для отримання списку з певною кількістю тайтлів, назва яхи схожа або містить у собі введену користувачем стрінгу
    { 
        public string Q { get; set; }
        public int Limit { get; set; }
    }

    public class DB_parameters // модель вхідних даних для пошуку/видалення елемента у/з бази даних
    {
        public int Telegram_ID { get; set; }
        public int Title_ID { get; set; }
    }
    
}
