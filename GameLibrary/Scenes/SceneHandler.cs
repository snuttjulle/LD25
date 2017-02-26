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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Scenes
{
    public enum SceneNames { TitleScene, GameScene, GameOverScene, ResultScene, InstructionScene }

    public class SceneHandler
    {
        private Game _game;

        private bool _gameStartup = true;
        private bool _isPlaying = false;
        private SoundEffect _bgs;
        private SoundEffectInstance _bgsInstance;

        public Scene[] Scenes;
        private Scene _activeScene;

        private int _difficulty = 0;

        public SceneHandler(Game game)
        {
            _game = game;
            Scenes = new Scene[10];
            Scenes[0] = new TitleScene(_game, this);
            Scenes[2] = new GameOverScene(_game, this);
            Scenes[3] = new ResultScene(_game, this);
            Scenes[4] = new InstructionScene(_game, this);

            _activeScene = Scenes[(int)SceneNames.TitleScene];
        }

        public void NewGame()
        {
            _difficulty++;
            Scenes[1] = new GameScene(_game, this, _difficulty);
            Scenes[1].LoadContent();
            SwitchToScene((int)SceneNames.GameScene);

            if (_gameStartup)
            {
                _bgsInstance = _bgs.CreateInstance();
                _bgsInstance.IsLooped = true;
                _bgsInstance.Volume = .2f;
                _bgsInstance.Play();
                _isPlaying = true;
                _gameStartup = false;
            }
        }

        public void Reset()
        {
            _difficulty = 0;
            ResultScene rs = Scenes[3] as ResultScene;
            rs.TotalScore = 0;
            rs.Round = 0;
        }

        public void SwitchToScene(int index)
        {
            if (index > Scenes.Length || index < 0)
                throw new ArgumentOutOfRangeException("Invalid index");

            if (index == (int)SceneNames.GameOverScene) //game over means restart
                _difficulty = 0;

            _activeScene = Scenes[index];
        }

        public void LoadContent(ContentManager content)
        {
            _bgs = _game.Content.Load<SoundEffect>("Music\\bgs");

            //loads content for all scenes at the getgo
            foreach (Scene scene in Scenes)
                if (scene != null)
                    scene.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (InputHandler.IsKeyReleased(Keys.Q))
            {
                if (_bgsInstance != null)
                {
                    if (_isPlaying)
                    {
                        _bgsInstance.Pause();
                        _isPlaying = false;
                    }
                    else
                    {
                        _bgsInstance.Play();
                        _isPlaying = true;
                    }
                }
            }

            if (_activeScene != null)
                _activeScene.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_activeScene != null)
                _activeScene.Draw(gameTime, spriteBatch);
        }
    }
}
