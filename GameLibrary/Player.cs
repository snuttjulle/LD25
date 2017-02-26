using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.SpriteClasses;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
    public class Player : AnimatedSprite
    {
        public Player(Game game)
            : base(game, "Sprites\\Player", new Rectangle())
        {
            IsAnimating = true;

            Animation animation = new Animation(3, 32, 32, 0, 32 * 0, AnimationType.LoopAndReverse);
            Animations.Add(0, animation);

            animation = new Animation(3, 32, 32, 0, 32 * 1, AnimationType.LoopAndReverse);
            Animations.Add(1, animation);

            animation = new Animation(3, 32, 32, 0, 32 * 2, AnimationType.LoopAndReverse);
            Animations.Add(2, animation);

            animation = new Animation(3, 32, 32, 0, 32 * 3, AnimationType.LoopAndReverse);
            Animations.Add(3, animation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
