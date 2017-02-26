// <copyright file="Building.cs" company="Treptow Art Studio">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Anders Treptow</author>
// <date>12/17/2012 01:33:58 AM </date>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public static class RandomHelper
    {
        private static Random random = new Random();

        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static bool RandomBool()
        {
            if (random.Next(1, 100) > 50)
                return true;
            else
                return false;
        }
    }
}
