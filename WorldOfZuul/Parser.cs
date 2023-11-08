using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class Parser
    {
        private readonly CommandWords commandWords = new();

        public Command? GetCommand(string inputLine)
        {
            string[] words = inputLine.Split();

            if (words.Length == 0 || !commandWords.IsValidCommand(words[0]))
            {
                return null;
            }

            if (words.Length >= 1)
            {
                List<string> secondWord = new();
                foreach(string _ in words) secondWord.Add(_);
                secondWord.RemoveAt(0);
                string[] secondWordArray = secondWord.ToArray();
                return new Command(words[0], secondWordArray);
            }

            return new Command(words[0]);
        }
    }

}
