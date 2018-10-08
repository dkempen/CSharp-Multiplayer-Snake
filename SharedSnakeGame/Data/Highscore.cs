using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSnakeGame.Data
{
    [Serializable]
    public class Highscore
    {
        public List<Tuple<int, string>> Highscores { get; set; }
        private const int maxListSize = 10;

        public Highscore()
        {
            Highscores = new List<Tuple<int, string>>();
        }

        private void CheckScores()
        {
            if (Highscores.Count > maxListSize)
                Highscores.Remove(Highscores.Last());
        }

        public void AddTestData()
        {
            for (int i = Highscores.Count; i < maxListSize; i++)
                AddHighScore(i, "AAA");
        }

        public void AddHighScore(int score, string name)
        {
            Highscores.Add(Tuple.Create(score, name));
            Highscores.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            CheckScores();
        }

        public void WriteHighScores()
        {
            WriteToBinaryFile("highscore.txt", this);
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

        public static Highscore ReadHighScores()
        {
            return ReadFromBinaryFile<Highscore>("highscore.txt");
        }

        public void PrintHighScores()
        {
            Console.WriteLine(ToString());
        }

        public string ToString()
        {
            var highScoreString = "";
            foreach (Tuple<int, string> score in Highscores)
                highScoreString += score.Item2 + ":   " + score.Item1 + "\n";
            return highScoreString;
        }
    }
}
