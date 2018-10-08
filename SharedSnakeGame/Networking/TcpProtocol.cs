using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Game;
using System.Drawing;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using SharedSnakeGame.Data;

namespace SharedSnakeGame.Networking
{
    static class TcpProtocol
    {
        public static JObject DataSend(GameData gamedata)
        {
            return new JObject
            {
                {"command", "data/send" },
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
                {"command", "direction/send" },
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
                {"command", "tick/send" },
            };
        }

        public static JObject IdSend(GameData gameData, int id)
        {
            return new JObject
            {
                {"command", "id/send" },
                {"id", id },
                {"data", JsonConvert.SerializeObject(gameData) }
            };
        }

        public static JObject NameSend(string name)
        {
            return new JObject
            {
                {"command", "name/send" },
                {"name", name }
            };
        }

        public static JObject EndSend()
        {
            return new JObject
            {
                {"command", "end/send" }
            };
        }

        public static JObject HighscoreSend(Highscore highscore)
        {
            return new JObject
            {
                {"command", "highscore/send" },
                {"data", JsonConvert.SerializeObject(highscore) }
            };
        }

        public static JObject CheckForDisconnectSend()
        {
            return new JObject
            {
                {"command", "disconnect/send" }
            };
        }

    }
}
