using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Extensions
{
    public static class Extension
    {
        public static T ToClass<T>(this Dictionary<string, AttributeValue> dict) // конвертація інформації про шуканий у БД тайтл з отриманого файлу
        {
            var type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                var property = type.GetProperty(kv.Key);
                if (property != null)
                {
                    if (!string.IsNullOrEmpty(kv.Value.S))
                    {
                        property.SetValue(obj, kv.Value.S);
                    }
                    else if (!string.IsNullOrEmpty(kv.Value.N))
                    {
                        property.SetValue(obj, int.Parse(kv.Value.N));
                    }
                }
            }
            return (T)obj;
        }
    }
}
