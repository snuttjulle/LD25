// <copyright file="Building.cs" company="Treptow Art Studio">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Anders Treptow</author>
// <date>12/17/2012 01:33:58 AM </date>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameLibrary.Particles
{
    public class Explosion
    {
        private const int NO_PARTICLES = 500;
        private const int GRAVITY = 1500;

        private Game _game;
        private static Texture2D _primitiveTexture;

        private Vector2[] _particles;
        private Vector2[] _velocities;

        public bool Dispose { get; set; }

        public Explosion(Game game, Vector2 origin)
        {
            _game = game;
            Dispose = false;

            if (_primitiveTexture == null)
            {
                _primitiveTexture = new Texture2D(_game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _primitiveTexture.SetData(new[] { Color.White });
            }

            //create particles
            _particles = new Vector2[NO_PARTICLES];
            _velocities = new Vector2[NO_PARTICLES];

            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i] = origin + new Vector2(RandomHelper.RandomInt(-5, 5));
            }

            for (int i = 0; i < _particles.Length; i++)
            {
                _velocities[i].X = RandomHelper.RandomInt(-300, 300);
                _velocities[i].Y = RandomHelper.RandomInt(200, 450);
            }
        }

        public void Update(GameTime gameTime)
        {
            bool dispose = true;

            for (int i = 0; i < _particles.Length; i++)
            {
                if (_particles[i].X < 0 
                    || _particles[i].Y < 0 
                    || _particles[i].X > _game.GraphicsDevice.Viewport.Width 
                    || _particles[i].Y > _game.GraphicsDevice.Viewport.Height)
                    continue;

                _particles[i].X += _velocities[i].X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _particles[i].Y -= _velocities[i].Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _velocities[i].Y -= GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                dispose = false;
            }

            Dispose = dispose;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(0, 0, 2, 2);
            foreach (Vector2 v in _particles)
            {
                if (v.X < 0 || v.Y < 0 || v.X > _game.GraphicsDevice.Viewport.Width || v.Y > _game.GraphicsDevice.Viewport.Height)
                    continue;

                spriteBatch.Draw(_primitiveTexture, new Rectangle((int)v.X, (int)v.Y, 2, 2), source, Color.Yellow);
            }
        }
    }
}
