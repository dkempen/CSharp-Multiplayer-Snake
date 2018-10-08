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
            highscores.Add(new Score(highScore.HighScore, highScore.Name));
            highscores.Sort();
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
            var highScoreString = "";
            foreach (Score score in highscores)
                highScoreString += score.Name + ": " + score.HighScore + "\n";
            Console.WriteLine(highScoreString);
        }

        [Serializable]
        public class Score : IComparable<Score>
        {
            public string Name { get; }
            public int HighScore { get; }

            public Score(int score, string name)
            {
                HighScore = score;
                Name = name;
            }

            public int CompareTo(Score s)
            {
                return s.HighScore - this.HighScore;
            }
        }
    }
}
