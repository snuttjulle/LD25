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
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Building
{
    public class Floor
    {
        private const string SPRITE_ASSET = "Sprites\\rooms2";

        private Room[] _rooms;

        public Floor(Game game, int rooms, Vector2 startPosition, int shaft)
        {
            if (rooms == 1)
                throw new ArgumentException("There cannot be only 1 room");

            _rooms = new Room[rooms];

            Vector2 position = startPosition;

            for (int i = 0; i < _rooms.Length; i++)
            {
                position.X += 32;
                if (i == shaft)
                    _rooms[i] = new Room(game, position, SPRITE_ASSET, new Rectangle(32 * 2, 32 * 2, 32, 32));
                else if (i != 0)
                    _rooms[i] = new Room(game, position, SPRITE_ASSET, new Rectangle(32 * RandomHelper.RandomInt(0, 3), 32 * RandomHelper.RandomInt(0, 1), 32, 32));
                else
                    _rooms[i] = new Room(game, position, SPRITE_ASSET, new Rectangle(32 * 0, 32 * 2, 32, 32));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Room room in _rooms)
                room.Draw(gameTime, spriteBatch);
        }

        internal void LoadContent()
        {
            foreach (Room room in _rooms)
                room.LoadContent();
        }
    }
}
