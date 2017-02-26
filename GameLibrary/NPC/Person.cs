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
using GameLibrary.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GameLibrary.NPC
{
    public enum TravelDirection { Left, Right }

    public class Person : AnimatedSprite
    {
        private const int SPEED = 50;

        private static string[] SPRITES = new string[] { "Sprites\\NPC_x2", "Sprites\\NPC2_x2", "Sprites\\NPC3_x2", "Sprites\\NPC4_x2" };

        private Building.Building _building;
        private Game _game;

        public Point RoomPosition = new Point(0, 0);
        public Point RoomHeading { get; set; }
        public bool Dispose { get; set; }

        private List<TextParticle> _textParticles;
        public List<TextParticle> TextParticles { get { return _textParticles; } }
        private LinkedList<TextParticle> _disposeParticles;
        private Explosion _explosion;

        public bool IsEnabled { get; set; }

        //////////////////
        //Gameplay Properties
        private double _stressLevel = 0;
        public double StressLevel { get { return _stressLevel; } private set { _stressLevel = value; if (value < 0) _stressLevel = 0; } }
        private int _hp = 100;

        private double _stressMultiplier;

        private int _highestInflictedStress = 0;
        public int HighestInflictedStress { get { return _highestInflictedStress; } }

        private int _totaltStressInflicted = 0;
        public int TotalStressInflicted { get { return _totaltStressInflicted; } }

        private TimeSpan _refreshStressTime = new TimeSpan(0, 0, 2);
        private TimeSpan _currentTime;

        public HashSet<int> VisitedFloors { get; set; }

        /////////////////
        //AI
        private bool _isTraveling;
        private TravelDirection _direction;

        private bool IsOnRightFloor { get { return RoomPosition.Y == RoomHeading.Y; } }
        private bool IsWaitingForElevator { get { return RoomPosition.Y != RoomHeading.Y && RoomPosition.X == _building.ShaftPosition; } }

        public bool IsInElevator { get; set; }
        public bool IsExploring { get; set; }

        /////////////////
        //Sound Effects

        private SoundEffect _killSound;
        private SoundEffect _stressedSound;
        private SoundEffect _relievedSound;

        public Person(Game game, Building.Building building, double stressMultiplier)
            : base(game, SPRITES[RandomHelper.RandomInt(0, SPRITES.Length-1)], new Rectangle(0, 0, 32, 32))
        {
            IsEnabled = true;

            _stressMultiplier = stressMultiplier;

            _game = game;
            _building = building;
            _textParticles = new List<TextParticle>();
            _disposeParticles = new LinkedList<TextParticle>();
            VisitedFloors = new HashSet<int>();

            CreateAnimations();
            SourceRectangle = Animations[CurrentAnimation].CurrentFrame;

            int x = RandomHelper.RandomInt(0, _building.Rooms - 1);
            int y = RandomHelper.RandomInt(0, _building.Floors - 1);

            if (x == building.ShaftPosition)
                if (RandomHelper.RandomBool())
                    x++;
                else
                    x--;

            Spawn(new Point(x, y));
            SetNewDestination();

            _isTraveling = false;
            IsExploring = false;
        }

        private void CreateAnimations()
        {
            Animation animation = new Animation(3, 32, 32, 0, 32 * 0, AnimationType.LoopAndReverse, 1);
            Animations.Add(0, animation);

            animation = new Animation(3, 32, 32, 0, 32 * 1, AnimationType.LoopAndReverse, 1);
            Animations.Add(1, animation);

            animation = new Animation(3, 32, 32, 0, 32 * 2, AnimationType.LoopAndReverse, 1);
            Animations.Add(2, animation);

            animation = new Animation(3, 32, 32, 0, 32 * 3, AnimationType.LoopAndReverse, 1);
            Animations.Add(3, animation);
        }

        public void Spawn(Point room)
        {
            RoomPosition = room;
            RoomHeading = room;
            Position = new Vector2(
                _building.BasePosition.X + Building.Building.ROOM_SIZE + (Building.Building.ROOM_SIZE * room.X),
                _building.BasePosition.Y - (Building.Building.ROOM_SIZE * room.Y) - (Building.Building.FLOOR_MARGIN * room.Y));
        }

        public bool HasVisitedThisFloor(int floor)
        {
            return VisitedFloors.Contains(floor);
        }

        public override void Update(GameTime gameTime)
        {
            if (_explosion != null)
            {
                _explosion.Update(gameTime);

                if (_explosion.Dispose)
                    _explosion = null;
            }

            //handle particle Updates
            foreach (TextParticle particle in _textParticles)
            {
                particle.Update(gameTime);

                if (particle.Dispose)
                    _disposeParticles.AddLast(particle);
            }

            foreach (TextParticle particle in _disposeParticles)
                _textParticles.Remove(particle);
            _disposeParticles.Clear();


            //handle person update
            if (!IsEnabled)
                return;

            if (!IsInElevator)
            {
                //doesn't know where to go
                if (IsExploring)
                {
                    if (RoomPosition.X < RoomHeading.X)
                        _direction = TravelDirection.Right;
                    else
                        _direction = TravelDirection.Left;
                    _isTraveling = true;

                    if (_isTraveling)
                    {
                        Travel(gameTime);
                        CheckRoom(RoomHeading);
                    }
                }
                else //knows where he wants to go
                {
                    if (IsWaitingForElevator)
                    {
                        StressLevel -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _currentTime += gameTime.ElapsedGameTime;

                        if (_currentTime > _refreshStressTime)
                        {
                            _hp += 1;
                            _textParticles.Add(new TextParticle(_game, "+" + "1", Color.Yellow, this));
                            _currentTime = TimeSpan.Zero;
                        }
                    }
                    else if (!_isTraveling && RoomPosition != RoomHeading)
                    {
                        if (RoomPosition.Y == RoomHeading.Y)
                        {
                            if (RoomPosition.X < RoomHeading.X)
                                _direction = TravelDirection.Right;
                            else
                                _direction = TravelDirection.Left;
                            _isTraveling = true;
                        }
                        else
                        {
                            if (RoomPosition.X < _building.ShaftPosition)
                                _direction = TravelDirection.Right;
                            else
                                _direction = TravelDirection.Left;
                            _isTraveling = true;
                        }
                    }

                    if (_isTraveling)
                    {
                        _currentTime = TimeSpan.Zero;
                        Travel(gameTime);

                        if (IsOnRightFloor) //check for correct Room
                            CheckRoom(RoomHeading);
                        else //check for elevator
                            CheckRoom(new Point(_building.ShaftPosition, RoomHeading.Y));
                    }
                }
            }

            RoomPosition = _building.GetRoomFromPosition(Position);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            _killSound = _game.Content.Load<SoundEffect>("Sounds\\explosion");
            _stressedSound = _game.Content.Load<SoundEffect>("Sounds\\stressed");
            _relievedSound = _game.Content.Load<SoundEffect>("Sounds\\relieved");

            base.LoadContent();
        }

        private void Travel(GameTime gameTime)
        {
            IsAnimating = true;
            int motion;
            if (_direction == TravelDirection.Right)
            {
                motion = 1;
                CurrentAnimation = 0;
            }
            else
            {
                motion = -1;
                CurrentAnimation = 1;
            }

            Position.X += SPEED * motion * (float)gameTime.ElapsedGameTime.TotalSeconds;
            StressLevel += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds * _stressMultiplier;
        }

        private void CheckRoom(Point room)
        {
            bool arrived = false;
            if (_direction == TravelDirection.Right)
            {
                if (Position.X > _building.GetRoomPosition(room).X)
                    arrived = true;
            }
            else if (_direction == TravelDirection.Left)
            {
                if (Position.X < _building.GetRoomPosition(room).X)
                    arrived = true;
            }


            if (arrived)
            {
                _isTraveling = false;
                IsAnimating = false;
                Position.X = _building.GetRoomPosition(room).X;
                RoomPosition = _building.GetRoomFromPosition(Position);

                if (IsExploring)
                {
                    IsExploring = false;

                    if (RandomHelper.RandomBool())
                    {
                        int x = RandomHelper.RandomInt(0, _building.Rooms - 1);
                        while (x == _building.ShaftPosition)
                            x = RandomHelper.RandomInt(0, _building.Rooms - 1);
                        RoomHeading = new Point(x, RoomHeading.Y);
                    }

                    if (!IsOnRightFloor)
                    {
                        InflictStress();
                    }
                }

                if (RoomPosition == RoomHeading)
                {
                    _hp += (int)(StressLevel * (_stressMultiplier / 2.0));
                    _textParticles.Add(new TextParticle(_game, "+" + ((int)StressLevel).ToString(), Color.Yellow, this));
                    _relievedSound.Play();
                    StressLevel = 0;
                    SetNewDestination();
                    VisitedFloors.Clear();
                }
            }
        }

        private void InflictStress()
        {
            int stressMultiplier;
            if (RoomPosition.Y > RoomHeading.Y)
                stressMultiplier = RoomPosition.Y - RoomHeading.Y + 1;
            else
                stressMultiplier = RoomHeading.Y - RoomPosition.Y + 1;
            StressLevel *= stressMultiplier / 2.0;

            _hp -= (int)(StressLevel * _stressMultiplier);
            _totaltStressInflicted += (int)StressLevel;
            if (_hp < 0)
                Kill();
            else
            {
                _textParticles.Add(new TextParticle(_game, "-" + ((int)StressLevel).ToString(), Color.Red, this));
                _stressedSound.Play();
            }

            if (StressLevel > _highestInflictedStress)
                _highestInflictedStress = (int)StressLevel;
        }

        public void Kill()
        {
            IsEnabled = false;
            _killSound.Play();
            _explosion = new Explosion(_game, new Vector2(Position.X + (base.SourceRectangle.Width / 2), Position.Y + base.SourceRectangle.Height / 2));
        }

        public void SetNewDestination()
        {
            int x = RandomHelper.RandomInt(0, _building.Rooms - 1);
            while (x == _building.ShaftPosition)
                x = RandomHelper.RandomInt(0, _building.Rooms - 1);

            int y = RandomHelper.RandomInt(0, _building.Floors - 1);
            while (y == RoomPosition.Y)
                y = RandomHelper.RandomInt(0, _building.Floors - 1);

            RoomHeading = new Point(x, y);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsEnabled)
                base.Draw(gameTime, spriteBatch);

            foreach (TextParticle particle in _textParticles)
                particle.Draw(gameTime, spriteBatch);

            if (_explosion != null)
                _explosion.Draw(gameTime, spriteBatch);
        }
    }
}
