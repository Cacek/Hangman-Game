using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading;

namespace Hangman
{
    class Game
    {
        private List<string> WrongGuess { get; set; } = new List<string>();
        private List<string> Keyboard { get; set; }
        private List<string> KeyboardBorder { get; set; }

        private readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";

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
            Thread.Sleep(1200);
            Console.WriteLine("Your have to guess the letters to reveal the hidden word.");
            Console.WriteLine();
            Thread.Sleep(1200);
            Console.WriteLine("You may, at any time, attempt to guess the whole word.");
            Console.WriteLine();
            Thread.Sleep(1200);
            Console.WriteLine("But be careful, if you don't guess the word, you lose 2 lives.");
            Console.WriteLine();
            Thread.Sleep(800);
            Console.WriteLine("Let's get started!");
            Thread.Sleep(800);
        }
        public void GetRandomSecretWord()
        {

            Dictionary<string, string> dict = File.ReadAllLines(@"D:\countries_and_capitals.txt")
                                       .Select(x => x.Split(" | "))
                                       .ToDictionary(x => x[0], x => x[1]);

            Random random = new Random();

            int index = random.Next(dict.Count);

            string key = dict.Keys.ElementAt(index);
            string value = dict.Values.ElementAt(index);

            KeyValuePair<string, string> pair = dict.ElementAt(index);

            SecretWordToGuess = pair.Value.ToLower();
            Hint = pair.Key;


        }
        public void AddLettersToTheGuessingList(string wordToGuess) 
        {
            
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                LettersToGuess.Add(wordToGuess.Substring(i, 1));
            }
        }
        public void PrintUnderscorelines(string wordToGuess)
        {
            foreach (char character in wordToGuess)
            {
                if (character == ' ')
                {
                    UnderscoreSecretWorld.Add(" ");
                }
                else
                {
                    UnderscoreSecretWorld.Add(" _ ");
                }

            }
            Console.WriteLine();
        }


        public void Guessing(string WordToGuess)
        {
            bool win = true;
            int frequency;
            int duration;
            int wrongGuessCounts = 0;
            int lives = 5;
            int letters = 0;

            StartGame startGame = new StartGame();

            createKeyboardView();
           
            Console.WriteLine($"You have {lives} lives.");
            Console.WriteLine();
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            do
            {
                bool correctAnswer = UnderscoreSecretWorld.SequenceEqual(LettersToGuess);
                
                     
                if (correctAnswer)
                {
                    win = true;
                    WinnerSound();
                    Console.WriteLine("CONGRATS, YOU WON");
                    Console.WriteLine();

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    string elapsedTime = String.Format("{00}", ts.Seconds);

                    Console.WriteLine("You guessed the capital after " + letters + " tries. It took you " + elapsedTime + " seconds.");

                    AnotherRound(); 
                }

                Console.WriteLine("Enter a letter, or guess a word:");
                string userInput = Console.ReadLine();


                if (userInput.ToLower() == WordToGuess)
                {
                    letters++;
                    stopWatch.Stop();
                    WinnerSound();
                    Console.WriteLine("CONGRATS, YOU WON!!!");
                    Console.WriteLine();
                    
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{00}", ts.Seconds);

                    Console.WriteLine("You guessed the capital after " + letters + " tries. It took you " + elapsedTime + " seconds.");
                    Console.WriteLine();
                    Console.WriteLine("Please enter your name for a Highscore: ");
                    string username = Console.ReadLine();

                    string path = @"D:\";
                    string file = Path.Combine(path, "highscore.txt");

                    if(!File.Exists(file))
                    {
                        DateTime now = DateTime.Now;
                        File.WriteAllText(file, username + "|" + now + "|" + elapsedTime + "|" + letters + "|" + SecretWordToGuess + Environment.NewLine);
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        File.AppendAllText(file, username + "|" + now + "|" + elapsedTime + "|" + letters + "|" + SecretWordToGuess + Environment.NewLine);
                    }
                    AnotherRound();
                    break;
                }

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("You must type a single letter, or a whole word");
                    Console.WriteLine();
                    continue;
                }

                if (userInput.ToLower() != WordToGuess && !alphabet.Contains(userInput.ToLower()) && userInput.ToLower().Length == 1) 
                {
                    Console.WriteLine("You must type a single letter, or a whole word");
                    Console.WriteLine(); 
                    continue;
                }

                if (LetterRepeats(userInput))
                {
                    Console.WriteLine();
                    Console.WriteLine($"You already tried letter '{userInput}'");
                    Console.WriteLine();
                }
                 

                if (WrongLetter(userInput) && !LetterRepeats(userInput) && userInput.ToLower().Length == 1)
                {
                    Console.WriteLine("Wrong letter!");
                    Console.WriteLine();
                    wrongGuessCounts++;
                    letters++;
                    frequency = 300;
                    duration = 900;
                    Console.Beep(frequency, duration);
                    HangmanDrawing(wrongGuessCounts); 
                    Console.WriteLine(); 
                    Console.WriteLine(); 
                }

                if(userInput.ToLower().Length > 1 && userInput.ToLower() != WordToGuess)
                {
                    Console.WriteLine("Wrong word. You lost 2 lives!");
                    wrongGuessCounts += 2;
                    letters++;
                    Console.Beep(300, 900);
                    HangmanDrawing(wrongGuessCounts);
                    Console.WriteLine();
                    Console.WriteLine();

                }

                if (wrongGuessCounts >= 5)
                {
                    
                    Console.Beep(280, 400);
                    Console.Beep(474, 200);
                    Console.Beep(344, 700);
                    Console.WriteLine("SORRY, you lose!");
                    Console.WriteLine(); 
                    Console.WriteLine($"HINT: It was the capital of " + Hint);
                    Console.WriteLine();

                    AnotherRound();          
                    break;
                }
                for (int i = 0; i < LettersToGuess.Count; i++)
                {
                    if (string.Equals(LettersToGuess[i], userInput, StringComparison.OrdinalIgnoreCase))
                    {
                        UnderscoreSecretWorld[i] = LettersToGuess[i]; 
                    }
                    Console.Write($"{UnderscoreSecretWorld[i]} "); 
                }

                Console.WriteLine();
                Console.WriteLine(); 

                if (!correctAnswer)
                {
                    Console.WriteLine($"You have {lives - wrongGuessCounts} lives!");
                    letters++;
                }

                Console.WriteLine(); 

                ShowLetters(0, alphabet.Length / 2, userInput.ToLower()); 
                ShowLetters(alphabet.Length / 2, alphabet.Length / 2, userInput.ToLower()); 

                Console.WriteLine(); 

            } while (win);
        }
        private bool LetterRepeats(string userInput) 
        {
            return UnderscoreSecretWorld.Contains(userInput.ToLower())
                   || WrongGuess.Contains(userInput.ToLower()); 
        }
        private bool WrongLetter(string userInput) 
        {
            if (LettersToGuess.Contains(userInput.ToLower()))
            {
                return false;
            }
            return true;
        }
        private void ShowLetters(int x, int y, string userInput)
        {
            for (int i = 0; i < y; i++)  
            {
                Console.Write($" {KeyboardBorder[i]}  ");
            }
            Console.WriteLine(); 

            List<string> keyboardPart = Keyboard.GetRange(x, y).ToList();  

            if (Keyboard.Contains(userInput) && !LettersToGuess.Contains(userInput)) 
            {
                WrongGuess.Add(userInput);
            }
            foreach (var item in keyboardPart)
            {
                if (UnderscoreSecretWorld.Contains(item))
                {
                    Console.ForegroundColor = ConsoleColor.Green; 
                }
                if (WrongGuess.Contains(item))
                {
                    Console.ForegroundColor = ConsoleColor.Red; 
                }
                Console.Write($"| {item.ToUpper()} | "); 
                Console.ResetColor();  
            }
            Console.WriteLine(); 
            for (int i = 0; i < y; i++)
            {
                Console.Write($" {KeyboardBorder[i]}  "); 
            }
            Console.WriteLine();
        }
        private void createKeyboardView()
        {
            Keyboard = new List<string>();

            for (int i = 0; i < alphabet.Length; i++)
            {
                Keyboard.Add(alphabet.Substring(i, 1));
            }

            KeyboardBorder = new List<string>();

            for (int i = 0; i < alphabet.Length; i++)
            {
                KeyboardBorder.Add("---");
            }
        }

        private void HangmanDrawing(int WrongGuessCount)
        {
            if (WrongGuessCount == 1)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine("      +---+        ");
                Console.WriteLine("      |   |        ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      +----+       ");
                Console.ResetColor();
            }
            else if (WrongGuessCount == 2)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("      +---+        ");
                Console.WriteLine("      |   |        ");
                Console.WriteLine("      |   0        ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      +----+       ");
                Console.ResetColor();
            }
            else if (WrongGuessCount == 3)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("      +---+        ");
                Console.WriteLine("      |   |        ");
                Console.WriteLine("      |   0        ");
                Console.WriteLine("      |   |        ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      +----+       ");
                Console.ResetColor();
            }
            else if (WrongGuessCount == 4)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("      +---+        ");
                Console.WriteLine("      |   |        ");
                Console.WriteLine("      |   0        ");
                Console.WriteLine("      |  /|\\       ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      +----+       ");
                Console.ResetColor();
            }
            else if (WrongGuessCount == 5)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("      +---+        ");
                Console.WriteLine("      |   |        ");
                Console.WriteLine("      |   0        ");
                Console.WriteLine("      |  /|\\       ");
                Console.WriteLine("      |  / \\       ");
                Console.WriteLine("      |            ");
                Console.WriteLine("      +----+       ");
                Console.ResetColor();
            }
        }
        private void WinnerSound()
        {
            Console.Beep(523, 200);
            Console.Beep(523, 100);
            Console.Beep(659, 700);
            Console.Beep(659, 100);
            Console.Beep(659, 400);
        }
        public void AnotherRound() 
        {
            Console.WriteLine(); 
            Console.WriteLine("Press ENTER to continue with the next round,");
            Console.WriteLine(); 
            Console.WriteLine("(or press any other key to exit the game)");
            ConsoleKey keyPressed = Console.ReadKey().Key;

            if (keyPressed == ConsoleKey.Enter)
            {
                UnderscoreSecretWorld.Clear();
                WrongGuess.Clear();
                LettersToGuess.Clear();
                Console.WriteLine(); 
                Console.WriteLine("Great!");
                Console.WriteLine(); 
                Console.WriteLine();
                Console.Clear();
                GetRandomSecretWord();
                PrintUnderscorelines(SecretWordToGuess);
                Console.WriteLine();
                Console.WriteLine();
                AddLettersToTheGuessingList(SecretWordToGuess);
                Guessing(SecretWordToGuess);
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(); 
                Console.WriteLine();
                Console.WriteLine("See you next time!");
                Environment.Exit(0);
            }
        }
    }
}
