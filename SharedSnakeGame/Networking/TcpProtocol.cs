using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Game;
using System.Drawing;
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

        public static JObject NameSend(string name, Color color)
        {
            return new JObject
            {
                {"command", "name/send" },
                {"name", name },
                {"color", JToken.FromObject(color) }
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
