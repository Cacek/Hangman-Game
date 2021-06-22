using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Hangman
{
    class StartGame
    {
        public string SecretWordToGuess { get; set; }

        public string Hint { get; set; }
        
        public List<string> LettersToGuess { get; set; } = new List<string>();

        public List<string> UnderscoreSecretWorld { get; set; } = new List<string>();

        public void Welcome()
        {
            Console.Write("WELCOME TO THE");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("   H A N G M A N   ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            //Thread.Sleep(1200);
            Console.WriteLine("Your have to guess the letters to reveal the hidden word.");
            Console.WriteLine();
            //Thread.Sleep(1200);
            Console.WriteLine("You may, at any time, attempt to guess the whole word.");
            Console.WriteLine();
            //Thread.Sleep(1200);
            Console.WriteLine("But be careful, if you don't guess the word, you lose 2 lives.");
            Console.WriteLine();
            //Thread.Sleep(800);
            Console.WriteLine("Let's get started!");
            //Thread.Sleep(800);
        }
        public void GetRandomSecretWord() 
        {

            Dictionary<string, string> dict = File.ReadAllLines(@"D:\countries_and_capitals.txt")
                                       .Select(x => x.Split(" | "))
                                       .ToDictionary(x => x[0], x => x[1]);

            Random random = new Random();

            //SecretWordToGuess = (dict.ElementAt(rand.Next(0, dict.Count)).Value).ToLower();

            int index = random.Next(dict.Count);

            string key = dict.Keys.ElementAt(index);
            string value = dict.Values.ElementAt(index);

            KeyValuePair<string, string> pair = dict.ElementAt(index);

            SecretWordToGuess = pair.Value.ToLower();
            Hint = pair.Key;


        }

    }
}

