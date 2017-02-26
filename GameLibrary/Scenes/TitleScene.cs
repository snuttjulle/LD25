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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace GameLibrary.Scenes
{
    public class TitleScene : Scene
    {
        private const string TITLE_TEXT = "Elevator Villain";
        private const string SUBTITLE_TEXT1 = "Press ";
        private const string SUBTITLE_TEXT2 = "ENTER";
        private const string SUBTITLE_TEXT3 = " to start";

        private const string INSTRUCTION_TEXT1 = "i";
        private const string INSTRUCTION_TEXT2 = " - instructions";

        private Texture2D _title;

        private TimeSpan _flashTime = new TimeSpan(0, 0, 0, 0, 700);

        private SpriteFont _h1Font;
        private Vector2 _titleVector;

        private SpriteFont _h3Font;
        private Vector2 _subVector;

        private Vector2 _inVector;

        private SpriteFont _disc;

        private TimeSpan _currentTime;
        private bool _display = true;

        private bool _isStartingGame = false;
        private TimeSpan _startupTime = new TimeSpan(0, 0, 0, 1);
        private TimeSpan _currentStartupTime;

        private int _opacity = 255;

        private TimeSpan _currentTransitionTime;
        private TimeSpan _transitionDelay = new TimeSpan(0, 0, 1);

        private SoundEffect _acceptSound;

        public TitleScene(Game game, SceneHandler handler)
            : base(game, handler)
        {
        }

        public override void LoadContent()
        {
            float offsetX = base.Game.GraphicsDevice.Viewport.Width / 2;

            _h1Font = Game.Content.Load<SpriteFont>("Title");
            _titleVector = new Vector2(offsetX - (_h1Font.MeasureString(TITLE_TEXT) / 2).X, 50);

            _h3Font = Game.Content.Load<SpriteFont>("Subtitle");
            _subVector = new Vector2(offsetX - ((_h3Font.MeasureString(SUBTITLE_TEXT1) + _h3Font.MeasureString(SUBTITLE_TEXT2) + _h3Font.MeasureString(SUBTITLE_TEXT3)) / 2).X, 650);

            _inVector = new Vector2(offsetX - ((_h3Font.MeasureString(INSTRUCTION_TEXT1) + _h3Font.MeasureString(INSTRUCTION_TEXT2)) / 2).X, 700);

            _acceptSound = Game.Content.Load<SoundEffect>("Sounds\\accept");

            _title = Game.Content.Load<Texture2D>("Sprites\\title");

            _disc = Game.Content.Load<SpriteFont>("Disclaimer");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.IsKeyReleased(Keys.Enter) && !_isStartingGame)
            {
                _flashTime = new TimeSpan(0, 0, 0, 0, 150);
                _isStartingGame = true;
                _acceptSound.Play();
            }
            else if (InputHandler.IsKeyReleased(Keys.I) && !_isStartingGame)
            {
                Handler.SwitchToScene((int)SceneNames.InstructionScene);
            }

            _currentTime += gameTime.ElapsedGameTime;

            if (_currentTime > _flashTime)
            {
                _display = !_display;
                _currentTime = TimeSpan.Zero;
            }

            if (_isStartingGame)
            {
                _currentStartupTime += gameTime.ElapsedGameTime;
                if (_currentStartupTime > _startupTime)
                    _opacity -= 5;
            }

            if (_opacity < 0) //end case
            {
                _currentTransitionTime += gameTime.ElapsedGameTime;

                if (_currentTransitionTime > _transitionDelay)
                    Handler.NewGame();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Game.GraphicsDevice.Clear(Color.Black);
            //spriteBatch.DrawString(_h1Font, TITLE_TEXT, _titleVector, new Color(_opacity, 0, 0));
            spriteBatch.Draw(_title, new Rectangle((int)(Game.GraphicsDevice.Viewport.Width / 2) - (_title.Width / 2), (int)_titleVector.Y, _title.Width, _title.Height), Color.White);

            if (_display)
            {
                Vector2 sub = _subVector;

                spriteBatch.DrawString(_h3Font, SUBTITLE_TEXT1, sub, new Color(_opacity, _opacity, _opacity));
                sub.X += _h3Font.MeasureString(SUBTITLE_TEXT1).X;
                spriteBatch.DrawString(_h3Font, SUBTITLE_TEXT2, sub, new Color(_opacity, 0, 0));
                sub.X += _h3Font.MeasureString(SUBTITLE_TEXT2).X;
                spriteBatch.DrawString(_h3Font, SUBTITLE_TEXT3, sub, new Color(_opacity, _opacity, _opacity));
            }

            Vector2 inv = _inVector;

            spriteBatch.DrawString(_h3Font, INSTRUCTION_TEXT1, inv, new Color(_opacity, 0, 0));
            inv.X += _h3Font.MeasureString(INSTRUCTION_TEXT1).X;
            spriteBatch.DrawString(_h3Font, INSTRUCTION_TEXT2, inv, new Color(_opacity, _opacity, _opacity));

            string disc = "By: Anders \"Snutt\" Treptow";
            spriteBatch.DrawString(_disc, disc,new Vector2( Game.GraphicsDevice.Viewport.Width-_disc.MeasureString(disc).X, Game.GraphicsDevice.Viewport.Height-_disc.MeasureString(disc).Y), Color.Yellow);
        }
    }
}
