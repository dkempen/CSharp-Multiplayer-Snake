using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeServer.Data
{
    class Highscore
    {
        //  private List<Tuple<string, int>> highscores;

        private SortedDictionary<int, string> highscores;
        private const int maxListSize = 10;

        public Highscore()
        {
            highscores = new SortedDictionary<int, string>();
            highscores.Add(19, "AAA");
            highscores.Add(33, "AAB");
            highscores.Add(18, "AAC");
        }

        public void CheckScores(int score, string name)
        {
            highscores.Add(score, name);

            if (highscores.Count > maxListSize)
                highscores.Remove(highscores.First().Key);

            Console.WriteLine(highscores.First());
        }


        private class Score : IComparable
        {
            private int score;
            private string name;

            public int CompareTo(Object s)
            {
                Score scoreee = s as Score;
                return this.score - scoreee.score;
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
