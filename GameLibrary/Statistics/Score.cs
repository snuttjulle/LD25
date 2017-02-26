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

namespace GameLibrary.Statistics
{
    public class Score
    {
        private const int HUD_MARGIN = 10;

        private SpriteFont _font;
        private Building.Building _building;
        private Game _game;

        /////////////////////
        //Score properties
        private int _peopleKilled;
        private double _overallStress;

        public double HighestOverallStress { get; private set; }
        public int HighestInflictedStress { get; private set; }
        public int PeopleKilled { get { return _peopleKilled; } }
        public int TotalStressInflicted { get { int total = 0; foreach (Person person in _building.Persons) total += person.TotalStressInflicted; return total; } }

        public Score(Game game, Building.Building building)
        {
            _game = game;
            _building = building;
            _peopleKilled = 0;
            _overallStress = 0.0;
            HighestOverallStress = 0.0;
        }

        public void LoadContent()
        {
            _font = _game.Content.Load<SpriteFont>("Subtitle");
        }

        public void Update(GameTime gameTime)
        {
            //Calculate Overall Stress
            int _alivePersons = 0;
            double _sumStress = 0;

            foreach (Person person in _building.Persons)
            {
                if (person.HighestInflictedStress > HighestInflictedStress)
                    HighestInflictedStress = person.HighestInflictedStress;

                if (person.IsEnabled)
                {
                    _alivePersons++;
                    _sumStress += person.StressLevel;
                }
            }
            _overallStress = _sumStress / _alivePersons;

            if (double.IsNaN(_overallStress))
                _overallStress = 0;

            if (_overallStress > HighestOverallStress)
                HighestOverallStress = _overallStress;

            //People killed
            int _totalPersons = _building.Persons.Count();
            _peopleKilled = _totalPersons - _alivePersons;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string stressText = string.Format("{0:N0}", _overallStress);

            int offsetRight = _game.GraphicsDevice.Viewport.Width;
            float offsetPeopleKilledText = offsetRight - _font.MeasureString("People Killed").X - HUD_MARGIN;
            float offsetPeopleKilled = offsetRight - _font.MeasureString(_peopleKilled.ToString()).X - HUD_MARGIN;
            float offsetOverallStressText = offsetRight - _font.MeasureString("Overall Stress").X - HUD_MARGIN;
            float offsetOverallStress = offsetRight - _font.MeasureString(string.Format("{0:N0}", _overallStress)).X - HUD_MARGIN;

            spriteBatch.DrawString(_font, "People Killed", new Vector2(offsetPeopleKilledText, 10), Color.Blue);
            spriteBatch.DrawString(_font, _peopleKilled.ToString(), new Vector2(offsetPeopleKilled, 40), Color.Blue);

            spriteBatch.DrawString(_font, "Overall Stress", new Vector2(offsetOverallStressText, 70), Color.Blue);
            spriteBatch.DrawString(_font, stressText, new Vector2(offsetOverallStress, 100), Color.Blue);
        }
    }
}
