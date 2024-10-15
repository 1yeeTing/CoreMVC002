using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CoreMVC002.Models;
using Newtonsoft.Json;

namespace CoreMVC002.Controllers
{
    public class GameController : Controller
    {
        private const string GameStateKey = "GameState";

        [HttpGet]
        public ActionResult Index()
        {
            var model = GetOrCreateGameState();
            return View(model);
        }

        [HttpPost]
        public ActionResult Guess(XAXBEngine model)
        {
            var gameModel = GetOrCreateGameState();
            gameModel.Guess = model.Guess;

            if (!IsValidGuess(model.Guess))
            {
                ModelState.AddModelError("Guess", "請輸入四個數字。");
                return View("Index", gameModel);
            }

            int numA = gameModel.numOfA(gameModel.Guess);
            int numB = gameModel.numOfB(gameModel.Guess);
            gameModel.Result = $"{numA}A{numB}B";

            gameModel.AddGuessToHistory(gameModel.Guess, gameModel.Result);

            if (gameModel.IsGameOver(gameModel.Guess))
            {
                TempData["GameOver"] = "true";
                TempData["PlayAgainMessage"] = "你猜對了！是否要重新開始？";
                TempData.Remove(GameStateKey);
            }
            else
            {
                SaveGameState(gameModel);
            }

            return View("Index", gameModel);
        }

        private XAXBEngine GetOrCreateGameState()
        {
            if (TempData[GameStateKey] is string serializedState)
            {
                TempData.Keep(GameStateKey);
                return JsonConvert.DeserializeObject<XAXBEngine>(serializedState);
            }

            var newGame = new XAXBEngine(GenerateSecretNumber());
            SaveGameState(newGame);
            return newGame;
        }

        private void SaveGameState(XAXBEngine gameState)
        {
            TempData[GameStateKey] = JsonConvert.SerializeObject(gameState);
        }

        private string GenerateSecretNumber()
        {
            Random random = new Random();
            return new string(Enumerable.Range(0, 4)
                .Select(x => random.Next(0, 10).ToString()[0])
                .ToArray());
        }

        private bool IsValidGuess(string guess)
        {
            return !string.IsNullOrEmpty(guess) && guess.Length == 4 && guess.All(char.IsDigit);
        }
    }
}