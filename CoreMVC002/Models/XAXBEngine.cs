using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CoreMVC002.Models
{
    public class XAXBEngine
    {
        [Newtonsoft.Json.JsonProperty]
        public string Secret { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public string Guess { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public string Result { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public List<string> GuessHistory { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public int GuessCount { get; set; }

        [Newtonsoft.Json.JsonConstructor]
        public XAXBEngine()
        {
            Secret = GenerateRandomSecret();
            Guess = null;
            Result = null;
            GuessHistory = new List<string>();
            GuessCount = 0;
        }

        public XAXBEngine(string secretNumber)
        {
            Secret = secretNumber;
            Guess = null;
            Result = null;
            GuessHistory = new List<string>();
            GuessCount = 0;
        }

        private string GenerateRandomSecret()
        {
            Random random = new Random();
            return new string(Enumerable.Range(0, 4)
                .Select(x => random.Next(0, 10).ToString()[0])
                .ToArray());
        }

        public int numOfA(string guessNumber)
        {
            int countA = 0;
            for (int i = 0; i < Secret.Length; i++)
            {
                if (Secret[i] == guessNumber[i])
                    countA++;
            }
            return countA;
        }

        public int numOfB(string guessNumber)
        {
            int countB = 0;
            for (int i = 0; i < guessNumber.Length; i++)
            {
                if (Secret.Contains(guessNumber[i]) && Secret[i] != guessNumber[i])
                    countB++;
            }
            return countB;
        }

        public bool IsGameOver(string guessNumber)
        {
            return numOfA(guessNumber) == 4;
        }

        public void AddGuessToHistory(string guess, string result)
        {
            GuessHistory.Add($"{guess} -> {result}");
            GuessCount++;
        }
    }
}