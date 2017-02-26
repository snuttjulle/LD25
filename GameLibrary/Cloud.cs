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

namespace GameLibrary
{
    public class Cloud
    {
        private static string[] _assetNames = new string[] { "Background\\clouds1", "Background\\clouds2", "Background\\clouds3", "Background\\clouds4", "Background\\clouds5", "Background\\clouds6", "Background\\clouds7", "Background\\clouds8" };
        private static Texture2D[] _textures;

        private Game _game;

        private int _floatVelocity;
        private Vector2 Position;

        public bool Dispose { get; set; }

        private int _cloudId;

        public Cloud(Game game)
        {
            _game = game;

            if (_textures == null)
                LoadContent();

            _cloudId = RandomHelper.RandomInt(0, _assetNames.Length - 1);
            Position = new Vector2(-(_textures[_cloudId].Width), RandomHelper.RandomInt(0, 150));
            _floatVelocity = RandomHelper.RandomInt(5, 25);

            Dispose = false;
        }

        public void StartupPosition()
        {
            Position = new Vector2(RandomHelper.RandomInt(-(_textures[_cloudId].Width), _game.GraphicsDevice.Viewport.Width), RandomHelper.RandomInt(0, 150));
        }

        public void Update(GameTime gameTime)
        {
            Position.X += _floatVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X > _game.GraphicsDevice.Viewport.Width)
                Dispose = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textures[_cloudId], Position, Color.White);
        }

        public void LoadContent()
        {
            _textures = new Texture2D[_assetNames.Length];

            for (int i = 0; i < _textures.Length; i++)
                _textures[i] = _game.Content.Load<Texture2D>(_assetNames[i]);
        }
    }
}
