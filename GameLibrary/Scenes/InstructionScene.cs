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

namespace GameLibrary.Scenes
{
    public class InstructionScene : Scene
    {
        private SpriteFont _text;
        private SpriteFont _heading;

        private string _headText = "INSTRUCTIONS";
        private string _expl = "You are an evil elevator! Your goal is drop the mans of at \nthe wrong floor so they get stressed out and "
            + "\neventually explode!\n\nWhen the mans realise that they're on the wrong floor they \nwill be damaged by stress. The more stressed out they \nare, the more damage they take. \n\nThis is indicated by the number over their head. \nThe further away from the correct floor they are, "
            + "\nthe more stress they take. If you drop them at the \ncorrect floor though, the will be relieved and lose stress.";

        private string _contrText = "CONTROLS";
        private string _arrows = "ARROWS or W and S";
        private string _contrExpl = " - move the elevator ";
        private string _button = "Z or ENTER";
        private string _contrExpl2 = " - picks up a man/drop off a man";
        private string _contrExpl3 = "You can only pick up one man at a time and you can only \ndrop the man at a floor he has not yet visited since \nlast time he was \"relieved\".";
        private string _contrExpl4 = "Q";
        private string _contrExpl5 = " - mute music";

        private string _esc1 = "Press ";
        private string _esc2 = "ENTER";
        private string _esc3 = " to go back";

        public InstructionScene(Game game, SceneHandler handler)
            : base(game, handler)
        { }

        public override void LoadContent()
        {
            _text = Game.Content.Load<SpriteFont>("Subtitle");
            _heading = Game.Content.Load<SpriteFont>("Title");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputHandler.IsKeyReleased(Keys.Enter))
                Handler.SwitchToScene((int)SceneNames.TitleScene);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            spriteBatch.DrawString(_text, _headText, new Vector2(20, 20), Color.Red);
            spriteBatch.DrawString(_text, _expl, new Vector2(20, 80), Color.White);
            Vector2 marginY = new Vector2(0, 30);

            spriteBatch.DrawString(_text, _contrText, new Vector2(20, 430)+marginY, Color.Red);
            spriteBatch.DrawString(_text, _arrows, new Vector2(20, 490)+marginY, Color.Red);
            spriteBatch.DrawString(_text, _contrExpl, new Vector2(20 + _text.MeasureString(_arrows).X, 490)+marginY, Color.White);
            spriteBatch.DrawString(_text, _button, new Vector2(20, 490 + _text.MeasureString(_contrExpl).Y)+marginY, Color.Red);


            spriteBatch.DrawString(_text, _contrExpl2, new Vector2(20 + _text.MeasureString(_button).X, 490 + _text.MeasureString(_contrExpl).Y) + marginY, Color.White);
            spriteBatch.DrawString(_text, _contrExpl4, new Vector2(20, 490 + _text.MeasureString(_contrExpl).Y + _text.MeasureString(_contrExpl2).Y) + marginY, Color.Red);
            spriteBatch.DrawString(_text, _contrExpl5, new Vector2(20 + _text.MeasureString(_contrExpl4).X, 490 + _text.MeasureString(_contrExpl).Y + _text.MeasureString(_contrExpl2).Y) + marginY, Color.White);

            spriteBatch.DrawString(_text, _contrExpl3, new Vector2(20, 640)+marginY, Color.White);

            spriteBatch.DrawString(_text, _esc1, new Vector2(600, 20), Color.White);
            spriteBatch.DrawString(_text, _esc2, new Vector2(600 + _text.MeasureString(_esc1).X, 20), Color.Red);
            spriteBatch.DrawString(_text, _esc3, new Vector2(600 + _text.MeasureString(_esc1).X + _text.MeasureString(_esc2).X, 20), Color.White);
        }
    }
}
