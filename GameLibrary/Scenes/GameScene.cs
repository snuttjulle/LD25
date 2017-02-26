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
using Microsoft.Xna.Framework.Content;
using GameLibrary.NPC;
using GameLibrary.Statistics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Scenes
{
    public class GameScene : Scene
    {
        //Resources
        private SpriteFont _font;
        private Texture2D _meadow;
        private Texture2D _sky;

        //Clouds
        private List<Cloud> _clouds;
        private TimeSpan _currentCloudTimer;
        private TimeSpan _cloudSpawnTimer = new TimeSpan(0, 0, RandomHelper.RandomInt(5, 20));

        private Building.Building _building;
        private Score _score;

        private TimeSpan _timer = new TimeSpan(0, 2, 30);
        private TimeSpan _delayTimer;

        private int _currentDifficulty;

        public GameScene(Game game, SceneHandler handler, int difficulty)
            : base(game, handler)
        {
            _currentDifficulty = difficulty;
            SetFromDifficulty(difficulty);
            _score = new Score(game, _building);
            _clouds = new List<Cloud>();
            _delayTimer = TimeSpan.Zero;
        }

        private void SetFromDifficulty(int difficulty)
        {
            int persons, floors, rooms;
            double stressMultiplier;

            switch (difficulty)
            {
                case 1:
                    persons = RandomHelper.RandomInt(1, 2);
                    floors = RandomHelper.RandomInt(5, 8);
                    rooms = 8;
                    break;
                case 2:
                    persons = RandomHelper.RandomInt(2, 3);
                    floors = RandomHelper.RandomInt(5, 8);
                    rooms = 8;
                    break;
                case 3:
                    persons = RandomHelper.RandomInt(3, 4);
                    floors = RandomHelper.RandomInt(5, 7);
                    rooms = RandomHelper.RandomInt(8, 10);
                    break;
                case 4:
                    persons = RandomHelper.RandomInt(3, 8);
                    floors = RandomHelper.RandomInt(5, 13);
                    rooms = RandomHelper.RandomInt(8, 10);
                    break;
                case 5:
                    persons = RandomHelper.RandomInt(5, 8);
                    floors = RandomHelper.RandomInt(5, 15);
                    rooms = RandomHelper.RandomInt(8, 16);
                    break;
                case 6:
                    persons = RandomHelper.RandomInt(5, 10);
                    floors = RandomHelper.RandomInt(5, 13);
                    rooms = RandomHelper.RandomInt(8, 20);
                    break;
                case 7:
                    persons = RandomHelper.RandomInt(7, 12);
                    floors = RandomHelper.RandomInt(5, 17);
                    rooms = RandomHelper.RandomInt(8, 20);
                    break;
                case 8:
                    persons = RandomHelper.RandomInt(10, 15);
                    floors = RandomHelper.RandomInt(5, 20);
                    rooms = RandomHelper.RandomInt(8, 20);
                    break;
                case 9:
                    persons = 3;
                    floors = 5;
                    rooms = 12;
                    break;
                default: // after round 9
                    persons = RandomHelper.RandomInt(5, 15);
                    floors = RandomHelper.RandomInt(5, 20);
                    rooms = RandomHelper.RandomInt(5, 20);
                    break;
            }

            if (difficulty < 9)
            {
                stressMultiplier = 1 / (double)difficulty;
                stressMultiplier *= 3;
            }
            else
            {
                stressMultiplier = (1 / 9.0) * 3;

                int topLevel = difficulty - 10;
                if (topLevel < 12)
                    _timer = new TimeSpan(0, 0, (int)(_timer.TotalSeconds - (topLevel * 5)));
                else
                    _timer = new TimeSpan(0, 1, 30);
            }

            _building = new Building.Building(Game, floors, rooms, persons, stressMultiplier, difficulty);
        }

        public override void LoadContent()
        {
            for (int i = 0; i < RandomHelper.RandomInt(1, 5); i++)
            {
                Cloud cloud = new Cloud(Game);
                cloud.StartupPosition();
                _clouds.Add(cloud);
            }

            _font = Game.Content.Load<SpriteFont>("Subtitle");
            _meadow = Game.Content.Load<Texture2D>("Background\\meadow");
            _sky = Game.Content.Load<Texture2D>("Background\\sky");
            _building.LoadContent();
            _score.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_score.PeopleKilled < _building.Persons.Count())
                _timer -= gameTime.ElapsedGameTime;

            _building.Update(gameTime);
            _score.Update(gameTime);

            if (_timer <= TimeSpan.Zero)
            {
                GameOverScene go = ((GameOverScene)Handler.Scenes[(int)SceneNames.GameOverScene]);
                go.RefreshScore(_currentDifficulty);
                Handler.SwitchToScene((int)SceneNames.GameOverScene);
            }
            else if (_score.PeopleKilled >= _building.Persons.Count())
            {
                _delayTimer += gameTime.ElapsedGameTime;

                if (_delayTimer > new TimeSpan(0, 0, 3))
                {
                    ResultScene resultScene = (ResultScene)Handler.Scenes[(int)SceneNames.ResultScene];
                    resultScene.RefreshScore(_score, _timer, _currentDifficulty);
                    Handler.SwitchToScene((int)SceneNames.ResultScene);
                }
            }

            LinkedList<Cloud> _disposeList = new LinkedList<Cloud>();
            //clouds
            foreach (Cloud cloud in _clouds)
            {
                cloud.Update(gameTime);
                if (cloud.Dispose)
                    _disposeList.AddLast(cloud);
            }

            foreach (Cloud cloud in _disposeList)
                _clouds.Remove(cloud);


            _currentCloudTimer += gameTime.ElapsedGameTime;
            if (_currentCloudTimer > _cloudSpawnTimer)
            {
                Cloud cloud = new Cloud(Game);
                _clouds.Add(cloud);
                _cloudSpawnTimer = new TimeSpan(0, 0, RandomHelper.RandomInt(30, 60));
                _currentCloudTimer = TimeSpan.Zero;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Game.GraphicsDevice.Clear(Color.AliceBlue);

            spriteBatch.Draw(_sky, new Vector2(0, 0), Color.White);
            foreach (Cloud cloud in _clouds)
                cloud.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(_meadow, new Rectangle(0, Game.GraphicsDevice.Viewport.Height - _meadow.Height, _meadow.Width, _meadow.Height), Color.White);
            _building.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, string.Format("{0:D2}:{1:D2}", _timer.Duration().Minutes, _timer.Duration().Seconds), new Vector2(10, 10), Color.Blue);
            _score.Draw(gameTime, spriteBatch);
        }
    }
}
