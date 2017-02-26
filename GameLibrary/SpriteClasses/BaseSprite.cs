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

namespace GameLibrary.SpriteClasses
{
    public abstract class BaseSprite
    {
        private Game _game;
        private string _assetName;

        protected Texture2D Texture;
        protected Rectangle SourceRectangle;

        public Vector2 Position;

        protected int Width { get { return SourceRectangle.Width; } }
        protected int Height { get { return SourceRectangle.Height; } }

        protected bool Visible { get; set; }

        public BaseSprite(Game game, string assetName, Rectangle sourceRectangle)
        {
            _game = game;
            _assetName = assetName;
            SourceRectangle = sourceRectangle;
            Visible = true;
        }

        public virtual void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>(_assetName);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                Vector2 drawPosition = Position;
                drawPosition.X = (float)Math.Ceiling(drawPosition.X);
                drawPosition.Y = (float)Math.Ceiling(drawPosition.Y);

                if (drawPosition.X + Width < 0
                    || drawPosition.X > _game.GraphicsDevice.Viewport.Width
                    || drawPosition.Y + Height < 0
                    || drawPosition.Y > _game.GraphicsDevice.Viewport.Height)
                    return;

                spriteBatch.Draw(Texture, drawPosition, SourceRectangle, Color.White);
            }
        }
    }
}
