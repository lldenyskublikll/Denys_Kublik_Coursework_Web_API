using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Extensions;
using Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Models;

namespace Web_API_for_Search_for_Anime_or_Manga_telegram_bot.Clients
{
    public class DynamoDataBaseClient: IDisposable
    {
        public static string _tableName;
        private readonly IAmazonDynamoDB _dynamoDB;

        public DynamoDataBaseClient(IAmazonDynamoDB dynamoDB) 
        {
            _dynamoDB = dynamoDB;
            _tableName = Constants.TableName;            
        }


        // отримання інформації про наявність або відсутність певного елементу у БД
        public async Task<DB_object> Get_info_about_user_favourites(int telegram_id, int title_id)  
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Telegram_ID", new AttributeValue{N = $"{telegram_id}" } },
                    {"Title_ID", new AttributeValue{N = $"{title_id}" } }                   
                }
            };

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
            {
                return null;
            }
            var result = response.Item.ToClass<DB_object>();

            return result;
        }



        // додавання об'єкту до БД
        public async Task<bool> Post_Data_to_DynamoDB(DB_object db) 
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"Telegram_ID", new AttributeValue {N = $"{db.Telegram_ID}"}},
                    {"Title_ID", new AttributeValue {N = $"{db.Title_ID}"}},
                    {"Title_name", new AttributeValue {S = $"{db.Title_name}"}},
                    {"Title_type", new AttributeValue {S = $"{db.Title_type}"}}
                }
            };

            try
            {
                var response = await _dynamoDB.PutItemAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unablle to add this item to data base\n" + ex);

                return false;
            }              
        }



        // видалення об'єкту з БД
        public async Task<bool> Delete_Data_from_DynamoDB(int telegram_id, int title_id) 
        {
            var check_item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Telegram_ID", new AttributeValue{N = $"{telegram_id}" } },
                    {"Title_ID", new AttributeValue{N = $"{title_id}" } }
                }
            };

            var check_item_response = await _dynamoDB.GetItemAsync(check_item);

            if (check_item_response.Item == null || !check_item_response.IsItemSet)
            {
                return false;
            }            

            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Telegram_ID", new AttributeValue{N = $"{telegram_id}" } },
                    {"Title_ID", new AttributeValue{N = $"{title_id}" } }
                }
            };

            try
            {
                var response = await _dynamoDB.DeleteItemAsync(request);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unablle to delete this item from data base\n" + ex);

                return false;
            }            
        }



        // отримання усіх об'єктів конкретного юзера
        public async Task<List<DB_object>> Get_ALL_user_data_from_DynamoDB(int telegram_user_id)
        {
            var result = new List<DB_object>();     

            var request = new QueryRequest
            {
                TableName = _tableName,
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "Telegram_ID = :v_replyTelegram_ID",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> 
                {
                    {":v_replyTelegram_ID", new AttributeValue{N = $"{telegram_user_id}"} }
                }
            };

            var response = await _dynamoDB.QueryAsync(request);

            if (response.Items == null || response.Items.Count == 0)
            {
                return null;
            }

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                result.Add(item.ToClass<DB_object>());
            }

            return result;
        }



        // видалення усіх елементів юзера       
        public async Task<bool> Delete_ALL_user_data_from_DynamoDB(int telegram_user_id)
        {
            var check_item = new List<DB_object>();

            var check_item_request = new QueryRequest
            {
                TableName = _tableName,
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "Telegram_ID = :v_replyTelegram_ID",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_replyTelegram_ID", new AttributeValue{N = $"{telegram_user_id}"} }
                }
            };

            var check_item_response = await _dynamoDB.QueryAsync(check_item_request);

            if (check_item_response.Items == null || check_item_response.Items.Count == 0)
            {
                return false;
            }

            foreach (Dictionary<string, AttributeValue> item in check_item_response.Items)
            {
                check_item.Add(item.ToClass<DB_object>());
            }

            foreach (DB_object db_object in check_item)
            {
                var request = new DeleteItemRequest
                {
                    TableName = _tableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {"Telegram_ID", new AttributeValue{N = $"{db_object.Telegram_ID}" } },
                        {"Title_ID", new AttributeValue{N = $"{db_object.Title_ID}" } }
                    }
                };

                await _dynamoDB.DeleteItemAsync(request);                
            }

            return true;
        }












        public void Dispose()
        {
            _dynamoDB.Dispose();
        }







        //// отримання усіх об'єктів конкретного юзера
        //public async Task<List<DB_object>> Get_ALL_user_data_from_DynamoDB(int telegram_user_id)
        //{
        //    var result = new List<DB_object>();

        //    //Dictionary<string, AttributeValue> lastKeyEvaluated = null;     

        //    var request = new ScanRequest
        //    {
        //        TableName = _tableName,
        //        //ExclusiveStartKey = lastKeyEvaluated,
        //        //ExpressionAttributeValues = new Dictionary<string, AttributeValue>
        //        //{
        //        //    {":val", new AttributeValue{N = $"{telegram_user_id}"}}
        //        //},
        //        //FilterExpression = "Telegran_ID = :val",
        //        //ProjectionExpression = "Telegran_ID, Title_ID, Title_name, Title_type"
        //    };

        //    var response = await _dynamoDB.ScanAsync(request);

        //    if (response.Items == null || response.Items.Count == 0)
        //    {
        //        return null;
        //    }

        //    foreach (Dictionary<string, AttributeValue> item in response.Items)
        //    {
        //        result.Add(item.ToClass<DB_object>());
        //    }

        //    return result;
        //}


     

    }
}
