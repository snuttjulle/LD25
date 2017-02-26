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
    public enum AnimationType { Once, Loop, LoopAndReverse }

    public class Animation
    {
        private Rectangle[] _frames;
        private int _frameWidth;
        private int _frameHeight;
        private int _fps;
        private int _currentFrame;
        private int _resetFrame = 0;

        private int _animationDirection = 1;

        protected readonly int xOffset;
        protected readonly int yOffset;

        private TimeSpan _frameTimer;
        private TimeSpan _frameLength;

        public Rectangle CurrentFrame { get { return _frames[_currentFrame]; } }
        public AnimationType AnimationType { get; set; }
        public int FPS
        {
            get { return _fps; }
            set
            {
                if (value < 1)
                    _fps = 1;
                else if (value > 60)
                    _fps = 60;
                else
                    _fps = value;
                _frameLength = TimeSpan.FromSeconds(1 / (double)_fps);
            }
        }

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset, AnimationType type)
            : this(frameCount, frameWidth, frameHeight, xOffset, yOffset, type, 0)
        { }


        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset, AnimationType type, int resetFrame)
        {
            _resetFrame = resetFrame;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            FPS = 10;
            AnimationType = type;

            _frames = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                _frames[i] = new Rectangle(xOffset + (frameWidth * i), yOffset, frameWidth, frameHeight);
            Reset();
        }

        public void Update(GameTime gameTime)
        {
            _frameTimer += gameTime.ElapsedGameTime;
            if (_frameTimer >= _frameLength)
            {
                _frameTimer = TimeSpan.Zero;

                if (AnimationType == AnimationType.Loop)
                    _currentFrame = (_currentFrame + 1) % _frames.Length;
                else if (AnimationType == AnimationType.LoopAndReverse)
                {
                    int nextFrame = _currentFrame + _animationDirection;
                    if (nextFrame >= _frames.Length)
                    {
                        _animationDirection = -1;
                        nextFrame = _currentFrame + _animationDirection;
                    }
                    else if (nextFrame < 0)
                    {
                        _animationDirection = 1;
                        nextFrame = 1;
                    }

                    _currentFrame = nextFrame;
                }
            }
        }

        public void Reset()
        {
            _currentFrame = _resetFrame;
            _frameTimer = TimeSpan.Zero;
        }
    }
}
