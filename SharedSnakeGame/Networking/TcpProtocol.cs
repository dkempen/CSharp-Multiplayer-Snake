using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SharedSnakeGame.Networking
{
    static class TcpProtocol
    {
        public static JObject DataSend(GameData gamedata)
        {
            return new JObject
            {
                {"data", JsonConvert.SerializeObject(gamedata) }
            };
        }
        

        public static GameData DataReceived(string message)
        {
            dynamic json = ConvertToJson(message);
            return (GameData)json.data;
        }

        private static dynamic ConvertToJson(string message)
        {
            return JsonConvert.DeserializeObject<dynamic>(message);
        }

        public static JObject DirectionSend(Point point)
        {
            return new JObject
            {
                {"pointX", point.X },
                {"pointY", point.Y }
            };
        }
        public static Point DirectionReceived(dynamic json)
        {
            return new Point((int)json.pointX, (int)json.pointY);
        } 

        public static JObject TickSend()
        {
            return new JObject
            {
                {"newTick", true }
            };
        }

        public static JObject IdSend(GameData gameData, int id)
        {
            return new JObject
            {
                {"id", id },
                {"data", JsonConvert.SerializeObject(gameData) }
            };
        }
    }
}
