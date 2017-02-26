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
using Microsoft.Xna.Framework.Input;
using GameLibrary.NPC;
using GameLibrary.Particles;
using Microsoft.Xna.Framework.Audio;

namespace GameLibrary
{
    public class Elevator : AnimatedSprite
    {
        private int _currentFloor;

        private Building.Building _building;
        private Game _game;

        private bool _isCarryMode = false;

        private Person _carryingPerson;
        private int _startingFloor;

        ///////////////////////
        //Sound Effects

        private SoundEffect _denySound;

        public Elevator(Game game, Building.Building building)
            : base(game, "Sprites\\Elevator", new Rectangle(0, 0, 32, 32))
        {
            _game = game;
            _building = building;
            _currentFloor = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (_isCarryMode)
            {
                float y = _carryingPerson.Position.Y;
                _carryingPerson.Position = _building.GetRoomPosition(new Point(_building.ShaftPosition, _currentFloor));

                y -= _carryingPerson.Position.Y;

                foreach (TextParticle particle in _carryingPerson.TextParticles)
                    particle.Position.Y -= y;
            }

            if (InputHandler.IsKeyPressed(Keys.Up) || InputHandler.IsKeyPressed(Keys.W))
            {
                if (_currentFloor < _building.Floors - 1)
                {
                    this.Position.Y -= Building.Building.ROOM_SIZE + Building.Building.FLOOR_MARGIN;
                    _currentFloor++;
                }
            }
            else if (InputHandler.IsKeyPressed(Keys.Down) || InputHandler.IsKeyPressed(Keys.S))
            {
                if (_currentFloor > 0)
                {
                    this.Position.Y += Building.Building.ROOM_SIZE + Building.Building.FLOOR_MARGIN;
                    _currentFloor--;
                }
            }

            if (InputHandler.IsKeyPressed(Keys.Z) || InputHandler.IsKeyPressed(Keys.Enter))
            {
                if (!_isCarryMode)
                {
                    foreach (Person person in _building.Persons)
                        if (person.RoomPosition.X == _building.ShaftPosition && person.RoomPosition.Y == _currentFloor && !person.IsAnimating)
                        {
                            _isCarryMode = true;
                            _carryingPerson = person;
                            person.IsInElevator = true;
                            _startingFloor = _currentFloor;
                        }
                }
                else if (_isCarryMode && _startingFloor != _currentFloor)
                {
                    if (!_carryingPerson.HasVisitedThisFloor(_currentFloor))
                    {
                        _carryingPerson.VisitedFloors.Add(_currentFloor);
                        _carryingPerson.IsInElevator = false;
                        _carryingPerson.IsExploring = true;
                        _carryingPerson = null;
                        _isCarryMode = false;
                    }
                    else
                    {
                        _denySound.Play();
                    }
                }
                else if (_isCarryMode && _startingFloor == _currentFloor)
                {
                    _denySound.Play();
                }
            }

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            _denySound = _game.Content.Load<SoundEffect>("Sounds\\deny");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
