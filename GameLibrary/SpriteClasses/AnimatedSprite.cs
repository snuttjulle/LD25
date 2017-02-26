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

namespace GameLibrary.SpriteClasses
{
    public abstract class AnimatedSprite : BaseSprite
    {
        protected Dictionary<int, Animation> Animations { get; set; }
        protected int CurrentAnimation { get; set; }

        private bool _isAnimating;
        public bool IsAnimating { get { return _isAnimating; } set { _isAnimating = value; if (!_isAnimating && Animations.Count > 0) Animations[CurrentAnimation].Reset(); UpdateSourceRectangle(); } }

        public AnimatedSprite(Game game, string assetName, Rectangle sourceRectangle)
            : base(game, assetName, sourceRectangle)
        {
            Animations = new Dictionary<int, Animation>();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Animations.Count > 0 && IsAnimating)
            {
                Animations[CurrentAnimation].Update(gameTime);
                SourceRectangle = Animations[CurrentAnimation].CurrentFrame;
            }
        }

        public void UpdateSourceRectangle()
        {
            if (Animations.Count > 0)
            {
                SourceRectangle = Animations[CurrentAnimation].CurrentFrame;
            }
        }
    }
}
