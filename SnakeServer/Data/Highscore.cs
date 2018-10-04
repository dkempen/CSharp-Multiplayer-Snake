using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeServer.Data
{
    [Serializable]
    class Highscore
    {
        public List<Score> highscores { get; set; }
        private const int maxListSize = 10;

        public Highscore()
        {
            highscores = new List<Score>();

           // AddHighScore(new Score(19, "AAA"));
           // AddHighScore(new Score(33, "AAB"));
           // AddHighScore(new Score(18, "AAC"));
        }

        public void CheckScores(int score, string name)
        {
            AddHighScore(new Score(19, "AAA"));
            AddHighScore(new Score(33, "AAB"));

            AddHighScore(new Score(score, name));

            if (highscores.Count > maxListSize)
                highscores.Remove(highscores.Last());
        }

        private void AddHighScore(Score highScore)
        {
            highscores.Add(new Score(highScore.Dick, highScore.Name));
            highscores.Sort();
        }

        public void WriteHighScores()
        {
            /*
            string json = JsonConvert.SerializeObject(this);
            //string json = JsonConvert.SerializeObject(highscores, Formatting.Indented);

            Console.WriteLine(json);

            Highscore test = JsonConvert.DeserializeObject<Highscore>(json);
            JToken j = JObject.Parse(json);
            test = j.ToObject<Highscore>();


            Console.WriteLine(test.highscores[0].Dick);
            */




            WriteToBinaryFile("highscore.txt",this,false);
         //   var highscore = ReadFromBinaryFile("highscore.txt");


            //  Console.WriteLine(json);

            //  List<Score> des = JsonConvert.DeserializeObject<List<Score>>(json);

            //  Console.WriteLine(des[0].HighScore);


            /*
            string message = JsonConvert.SerializeObject(this);
            Console.WriteLine(message);

            Highscore highScore = JsonConvert.DeserializeObject<Highscore>(message);

            Console.WriteLine(highScore.highscores[0].HighScore);

            */
            /*
            string message = JsonConvert.SerializeObject(new Score(15,"Jemam"));
            Console.WriteLine(message);

            Score highScore = JsonConvert.DeserializeObject<Score>(message);
            Console.WriteLine(highScore.HighScore);
*/

            //            highscores = des;
            //          WriteHighScores();

            //File.WriteAllText("highscores",);
        }


        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }



        public void ReadHighScores()
        {

        }

        public void PrintHighScores()
        {
            String highscoreString = "";

            foreach (Score score in highscores)
            {
                highscoreString += score.Name + ": " + score.Dick + "\n";
            }

            Console.WriteLine(highscoreString);

        }









        public class Score : IComparable<Score>
        {            
            public string Name { get; }
            public int Dick { get; }

            public Score(int score, string name)
            {
                this.Dick = score;
                this.Name = name;
            }

            public int CompareTo(Score s)
            {
                return s.Dick - this.Dick;
            }
        }
    }
    /*
    public static class ExtensionMethods
    {
        public static byte[] Combine(this byte[] bytes, byte[] b, int count)
        {
            byte[] data = new byte[bytes.Length + count];
            Buffer.BlockCopy(bytes, 0, data, 0, bytes.Length);
            Buffer.BlockCopy(b, 0, data, bytes.Length, count);
            return data;
        }
    }*/
}
