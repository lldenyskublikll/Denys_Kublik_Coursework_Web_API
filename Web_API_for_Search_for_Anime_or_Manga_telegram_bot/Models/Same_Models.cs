using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Models // моделі, що використовуються у моделях отримуваних даних аніме та манги
{
    public class Node
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }

    #region Ranking
    public class Data_1
    {
        public Ranking Ranking { get; set; }
        public Node Node { get; set; }        
    }
    public class Ranking 
    {
        public int Rank { get; set; }    
    }
    #endregion

    public class Data_2 
    { 
        public Node Node { get; set; }    
    }

    public class Alternative_titles
    {
        //public List<Synonyms> Synonyms { get; set; }
        public string En { get; set; }
        public string Ja { get; set; }        
    }  

    public class Genres
    {
        public string Name { get; set; }        
    }

    public class Recommendations
    {
        public Node Node { get; set; }
    }

    public class Main_picture
    {
        public string Large { get; set; }
    }
}
