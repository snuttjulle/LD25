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
using GameLibrary.NPC;

namespace GameLibrary.Building
{
    public class Building
    {
        private Game _game;

        private Texture2D _roofTexture;
        private static Texture2D _primitiveTexture;

        private Elevator _elevator;
        private int _shaftPosition;

        public const int ROOM_SIZE = 32;
        public const int FLOOR_MARGIN = 2;

        private Floor[] _floors;
        private Vector2 _base; //bottom_left

        private Person[] _persons;

        public readonly int Floors;
        public readonly int Rooms;
        public Vector2 BasePosition { get { return _base; } }
        public int ShaftPosition { get { return _shaftPosition; } }
        public Person[] Persons { get { return _persons; } }

        public Building(Game game, int floors, int rooms, int persons, double stressMultiplier, int difficulty)
        {
            _game = game;
            _elevator = new Elevator(game, this);
            _shaftPosition = RandomHelper.RandomInt(1, rooms - 1);

            Floors = floors;
            Rooms = rooms;
            _floors = new Floor[floors];

            Vector2 basePosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height - ROOM_SIZE - 70);
            Vector2 position = basePosition;
            position.Y -= (ROOM_SIZE * floors) + (FLOOR_MARGIN * floors);
            position.X = (game.GraphicsDevice.Viewport.Width / 2) - (((ROOM_SIZE * rooms) + (2 * rooms)) / 2);
            _base = position;
            _base.Y = basePosition.Y;

            for (int i = 0; i < floors; i++)
            {
                position.Y += ROOM_SIZE + FLOOR_MARGIN;
                _floors[i] = new Floor(game, rooms, position, _shaftPosition);
            }

            _elevator.Position.X = _base.X + ((_shaftPosition + 1) * ROOM_SIZE);
            _elevator.Position.Y = _base.Y;

            //create the people
            _persons = new Person[persons];

            for (int i = 0; i < _persons.Length; i++)
            {
                _persons[i] = new Person(game, this, stressMultiplier);
            }
        }

        public void Update(GameTime gameTime)
        {
            _elevator.Update(gameTime);

            foreach (Person person in _persons)
            {
                person.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Floor floor in _floors)
            {
                floor.Draw(gameTime, spriteBatch);
            }

            _elevator.Draw(gameTime, spriteBatch);

            DrawBuldingEdges(spriteBatch);
            DrawBuildingBorders(spriteBatch);

            foreach (Person person in _persons)
            {
                person.Draw(gameTime, spriteBatch);
            }
        }

        public Vector2 GetRoomPosition(Point room)
        {
            float x = BasePosition.X + ROOM_SIZE + (ROOM_SIZE * room.X);
            float y = BasePosition.Y - (ROOM_SIZE * room.Y) - (FLOOR_MARGIN * room.Y);

            return new Vector2(x, y);
        }

        public Point GetRoomFromPosition(Vector2 position)
        {
            int x = (int)((position.X - BasePosition.X) / ROOM_SIZE) - 1;
            int y = (int)((BasePosition.Y - position.Y) / (ROOM_SIZE + FLOOR_MARGIN));

            return new Point(x, y);
        }

        private void DrawBuildingBorders(SpriteBatch spriteBatch)
        {
            Rectangle dest = new Rectangle((int)_base.X + ROOM_SIZE, (int)_base.Y - 2, ROOM_SIZE * Rooms, 2);
            for (int i = 0; i < Floors - 1; i++)
            {
                if (i != 0)
                {
                    dest.Y -= ROOM_SIZE + FLOOR_MARGIN;
                }
                spriteBatch.Draw(_primitiveTexture, dest, new Color(130, 130, 130));
            }
        }

        //WOW!!! WHAT A FUCKING STUPID WAY TO DO THIS BUT IF YOU'RE UNDER PRESSURE YOU ARE UNDER PRESSURE
        private void DrawBuldingEdges(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)_base.X + ROOM_SIZE, (int)(_base.Y) - ((ROOM_SIZE * Floors) + ((Floors) * FLOOR_MARGIN)), ROOM_SIZE, ROOM_SIZE);
            Rectangle leftSource = new Rectangle(32 * 0, 0, ROOM_SIZE, ROOM_SIZE);
            Rectangle middleSource = new Rectangle(32 * 1, 0, ROOM_SIZE, ROOM_SIZE);
            Rectangle rightSource = new Rectangle(32 * 2, 0, ROOM_SIZE, ROOM_SIZE);
            Rectangle sideSource = new Rectangle(32 * 0, 32 * 1, ROOM_SIZE, ROOM_SIZE);
            Rectangle middle = new Rectangle(32 * 0, 32 * 1, ROOM_SIZE, FLOOR_MARGIN);

            for (int i = 0; i < Rooms + 1; i++)
            {
                if (i == 0)
                {
                    destinationRectangle.Y += FLOOR_MARGIN;
                }
                //You can use switch case here
                if (i > 0 && i < Rooms)
                {
                    spriteBatch.Draw(_roofTexture, destinationRectangle, middleSource, Color.White);
                }
                else if (i == 0)
                {
                    spriteBatch.Draw(_roofTexture, destinationRectangle, leftSource, Color.White);
                }
                else
                {
                    spriteBatch.Draw(_roofTexture, destinationRectangle, rightSource, Color.White);
                }

                destinationRectangle.X += ROOM_SIZE;
            }

            destinationRectangle.X -= ROOM_SIZE;

            for (int i = 0; i < Floors - 1; i++)
            {

                destinationRectangle.Y += ROOM_SIZE;
                spriteBatch.Draw(_roofTexture, destinationRectangle, sideSource, Color.White);

                destinationRectangle.Y += FLOOR_MARGIN;

                spriteBatch.Draw(_roofTexture, destinationRectangle, sideSource, Color.White);

            }

            destinationRectangle.Y += ROOM_SIZE;

            Rectangle bottomRightCorner = new Rectangle(32 * 1, 32 * 1, ROOM_SIZE, ROOM_SIZE);
            spriteBatch.Draw(_roofTexture, destinationRectangle, bottomRightCorner, Color.White);
        }

        internal void LoadContent()
        {
            _roofTexture = _game.Content.Load<Texture2D>("Sprites\\roof");

            foreach (Floor floor in _floors)
            {
                floor.LoadContent();
            }

            _elevator.LoadContent();

            if (_primitiveTexture == null)
            {
                _primitiveTexture = new Texture2D(_game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _primitiveTexture.SetData(new[] { Color.White });
            }

            foreach (Person person in _persons)
            {
                person.LoadContent();
            }
        }
    }
}
