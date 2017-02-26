// <copyright file="Building.cs" company="Treptow Art Studio">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Anders Treptow</author>
// <date>12/17/2012 01:33:58 AM </date>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLibrary.Statistics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Scenes
{
    public class ResultScene : Scene
    {
        private const int X_MARGIN = 20;
        private const int Y_MARGIN = 40;

        private Score _score;
        private TimeSpan _time;
        private SpriteFont _font;

        ///////////////////////
        // Text
        private string _sKilled = "People Killed:";
        private string _killed;

        private string _sHighestStress = "Highest Overall Stress:";
        private string _highestStress;

        private string _sHighestInflictedStress = "Highest Stress Inflicted:";
        private string _highestInflictedStress;

        private string _sTotalStressInflicted = "Total Stress Inflicted:";
        private string _totalInflictedStress;

        private string _sTimeLeft = "Time remaining:";
        private string _timeLeft;

        private string _sRoundScore = "Round Score:";
        private string _roundScore;

        private string _sTotalScore = "Total Score:";

        private string _sContinue = "Press ENTER to continue...";

        public int TotalScore { get; set; }
        public int Round { get; set; }

        ///////////////////////
        //Positions
        private int _leftPos;
        private int _rightPos;

        public ResultScene(Game game, SceneHandler handler)
            : base(game, handler)
        { }

        public void RefreshScore(Score score, TimeSpan timeLeft, int round)
        {
            Round = round;
            _score = score;
            _time = timeLeft;

            _killed = _score.PeopleKilled.ToString();
            _highestStress = ((int)_score.HighestOverallStress).ToString();
            _highestInflictedStress = ((int)_score.HighestInflictedStress).ToString();
            _totalInflictedStress = _score.TotalStressInflicted.ToString();
            _timeLeft = string.Format("{0:D2}:{1:D2}", _time.Duration().Minutes, _time.Duration().Seconds);

            int roundscore = _score.PeopleKilled * 10;
            roundscore += (int)(_score.HighestOverallStress * 6);
            roundscore += _score.HighestInflictedStress * 7;
            roundscore += _score.TotalStressInflicted * 2;
            roundscore *= (int)(0.5 * (_time.TotalSeconds)) + 10;
            roundscore /= 100;

            _roundScore = roundscore.ToString();
            TotalScore += roundscore;
        }

        public override void LoadContent()
        {
            _font = Game.Content.Load<SpriteFont>("Subtitle");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _leftPos = (int)(Game.GraphicsDevice.Viewport.Width / 2) - 50;
            _rightPos = (int)(Game.GraphicsDevice.Viewport.Width / 2) + 250;

            if (InputHandler.IsKeyReleased(Keys.Enter))
                Handler.NewGame();
        }

        private void DrawText(string text1, string text2, int top, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, text1, new Vector2(_leftPos - _font.MeasureString(text1).X, top), Color.White);
            spriteBatch.DrawString(_font, text2, new Vector2(_leftPos + X_MARGIN, top), Color.Red);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            spriteBatch.DrawString(_font, "Round: " + Round, new Vector2(700, 50), Color.White);

            int top = 200;

            //killed
            DrawText(_sKilled, _killed, top, spriteBatch);

            top += Y_MARGIN;

            //highest overall stress
            DrawText(_sHighestStress, _highestStress, top, spriteBatch);

            top += Y_MARGIN;

            //highest inflicted stress
            DrawText(_sHighestInflictedStress, _highestInflictedStress, top, spriteBatch);

            top += Y_MARGIN;

            //total inflicted stress
            DrawText(_sTotalStressInflicted, _totalInflictedStress, top, spriteBatch);

            top += Y_MARGIN;

            //remaining time
            DrawText(_sTimeLeft, _timeLeft, top, spriteBatch);

            top += Y_MARGIN;
            top += Y_MARGIN;
            top += Y_MARGIN;
            top += Y_MARGIN;

            //round score
            spriteBatch.DrawString(_font, _sRoundScore, new Vector2(_rightPos - _font.MeasureString(_sRoundScore).X, top), Color.White);
            spriteBatch.DrawString(_font, _roundScore, new Vector2(_rightPos + X_MARGIN, top), Color.Yellow);

            top += Y_MARGIN;

            spriteBatch.DrawString(_font, _sTotalScore, new Vector2(_rightPos - _font.MeasureString(_sTotalScore).X, top), Color.White);
            spriteBatch.DrawString(_font, TotalScore.ToString(), new Vector2(_rightPos + X_MARGIN, top), Color.Yellow);

            //Continue Text...
            spriteBatch.DrawString(_font, _sContinue, new Vector2(_rightPos - _font.MeasureString(_sContinue).X + _font.MeasureString(_roundScore).X + X_MARGIN, 650), Color.White);
        }
    }
}
