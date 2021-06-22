using System;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            
            game.Welcome(); 
            
            Console.WriteLine(); 
            
            game.GetRandomSecretWord();
            game.PrintUnderscorelines(game.SecretWordToGuess);  
            
            Console.WriteLine(); 
            Console.WriteLine(); 
            
            game.AddLettersToTheGuessingList(game.SecretWordToGuess);  
            game.Guessing(game.SecretWordToGuess);
            
            Console.WriteLine(); 
            Console.WriteLine(); 
            
        }
    }
}
