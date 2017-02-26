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

namespace GameLibrary.Building
{
    public class Room : BaseSprite
    {
        public Room(Game game, Vector2 position, string assetName, Rectangle sourceRectangle)
            : base(game, assetName, sourceRectangle)
        {
            base.Position = position;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        } 

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
