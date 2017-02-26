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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Statistics;

namespace GameLibrary.Scenes
{
    public class GameOverScene : Scene
    {
        private const string GAME_OVER_TEXT = "GAME OVER";
        private const string SUBTITLE_TEXT = "People became too productive...";
        private const string TRY_AGAIN_TEXT1 = "Press ";
        private const string TRY_AGAIN_TEXT2 = "START";
        private const string TRY_AGAIN_TEXT3 = " to try again";
        private const string TOTAL_SCORE = "Total Score: ";

        private SpriteFont _h1Font;
        private Vector2 _titleVector;

        private SpriteFont _h3Font;
        private Vector2 _subVector;
        private Vector2 _tryVector;

        private int _opacity = 255;

        private int _totalScore;
        private int _atRound;

        public GameOverScene(Game game, SceneHandler handler)
            : base(game, handler)
        { }

        public void RefreshScore(int round)
        {
            _totalScore = ((ResultScene)Handler.Scenes[(int)SceneNames.ResultScene]).TotalScore;
            _atRound = round;

            Handler.Reset();
        }

        public override void LoadContent()
        {
            float offsetX = base.Game.GraphicsDevice.Viewport.Width / 2;

            _h1Font = Game.Content.Load<SpriteFont>("Title");
            _titleVector = new Vector2(offsetX - (_h1Font.MeasureString(GAME_OVER_TEXT) / 2).X, 50);

            _h3Font = Game.Content.Load<SpriteFont>("Subtitle");
            _subVector = new Vector2(offsetX - (_h3Font.MeasureString(SUBTITLE_TEXT) / 2).X, 200);
            _tryVector = new Vector2(offsetX - ((_h3Font.MeasureString(TRY_AGAIN_TEXT1) + _h3Font.MeasureString(TRY_AGAIN_TEXT2) + _h3Font.MeasureString(TRY_AGAIN_TEXT3)).X / 2), 600);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputHandler.IsKeyReleased(Keys.Enter))
                Handler.SwitchToScene((int)SceneNames.TitleScene);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.DrawString(_h1Font, GAME_OVER_TEXT, _titleVector, new Color(_opacity, 0, 0));
            spriteBatch.DrawString(_h3Font, SUBTITLE_TEXT, _subVector, new Color(_opacity, 0, 0));

            Vector2 tryV = _tryVector;
            spriteBatch.DrawString(_h3Font, TRY_AGAIN_TEXT1, tryV, new Color(_opacity, _opacity, _opacity));
            tryV.X += _h3Font.MeasureString(TRY_AGAIN_TEXT1).X;
            spriteBatch.DrawString(_h3Font, TRY_AGAIN_TEXT2, tryV, new Color(_opacity, 0, 0));
            tryV.X += _h3Font.MeasureString(TRY_AGAIN_TEXT2).X;
            spriteBatch.DrawString(_h3Font, TRY_AGAIN_TEXT3, tryV, new Color(_opacity, _opacity, _opacity));

            float offsetX = base.Game.GraphicsDevice.Viewport.Width / 2;
            float width = _h3Font.MeasureString(TOTAL_SCORE + _totalScore.ToString()).X;
            float fdskkfds = offsetX - (width / 2);

            spriteBatch.DrawString(_h3Font, TOTAL_SCORE, new Vector2(fdskkfds, 100), Color.White);
            spriteBatch.DrawString(_h3Font, _totalScore.ToString(), new Vector2(fdskkfds + _h3Font.MeasureString(TOTAL_SCORE).X, 100), Color.Yellow);
        }
    }
}
