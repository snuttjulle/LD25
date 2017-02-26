// <copyright file="Building.cs" company="Treptow Art Studio">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Anders Treptow</author>
// <date>12/17/2012 01:33:58 AM </date>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.SpriteClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.NPC;

namespace GameLibrary.Particles
{
    public class TextParticle
    {
        private const float SPEED = 5.0f;

        private static SpriteFont _font;

        private TimeSpan _currentTimer;
        private TimeSpan _displayTime = new TimeSpan(0, 0, 2);
        private Person _owner;

        public Vector2 Position;

        public bool Dispose { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }

        public TextParticle(Game game, string text, Color color, Person owner)
        {
            _owner = owner;
            Position = owner.Position;
            Text = text;
            Color = color;

            if(_font == null)
                _font = game.Content.Load<SpriteFont>("ParticleText");
        }

        public void Update(GameTime gameTime)
        {
            _currentTimer += gameTime.ElapsedGameTime;

            if (_currentTimer > _displayTime)
            {
                Dispose = true;
                return;
            }

            Position.Y -= SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.X = _owner.Position.X;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Text, Position + new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(_font, Text, Position, Color);
        }
    }
}
