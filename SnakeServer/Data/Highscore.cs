using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeServer.Data
{
    class Highscore
    {
        private List<Score> highscores;
        private const int maxListSize = 10;

        public Highscore()
        {
            highscores = new List<Score>();

            AddHighScore(new Score(19, "AAA"));
            AddHighScore(new Score(33, "AAB"));
            AddHighScore(new Score(18, "AAC"));
        }

        public void CheckScores(int score, string name)
        {
            AddHighScore(new Score(score, name));

            if (highscores.Count > maxListSize)
                highscores.Remove(highscores.Last());

            PrintHighScores();
        }

        private void AddHighScore(Score highScore)
        {
            highscores.Add(new Score(highScore.HighScore, highScore.Name));
            highscores.Sort();
        }

        public void WriteHighScores()
        {

        }

        public void ReadHighScores()
        {

        }

        public void PrintHighScores()
        {
            String highscoreString = "";

            foreach (Score score in highscores)
            {
                highscoreString += score.Name + ": " + score.HighScore + "\n";
            }

            Console.WriteLine(highscoreString);

        }









        private class Score : IComparable
        {
            public int HighScore { get; }
            public string Name { get; }

            public Score(int score, string name)
            {
                this.HighScore = score;
                this.Name = name;
            }

            public int CompareTo(Object s)
            {
                Score secondScore = s as Score;
                return secondScore.HighScore - this.HighScore;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static byte[] Combine(this byte[] bytes, byte[] b, int count)
        {
            byte[] data = new byte[bytes.Length + count];
            Buffer.BlockCopy(bytes, 0, data, 0, bytes.Length);
            Buffer.BlockCopy(b, 0, data, bytes.Length, count);
            return data;
        }
    }
}
