using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace DYMono.Math
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Rotate the current vector by an angle in degree.
        /// </summary>
        /// <param name="angle">The angle in degree</param>
        public static void Rotate(this Vector2 value, float angle)
        {
            double angleInRad = MathHelper.ToRadians(angle);
            float cosA = (float)System.Math.Cos(angleInRad);
            float sinA = (float)System.Math.Sin(angleInRad);

            value.X = cosA * value.X - sinA * value.Y;
            value.Y = cosA * value.Y + sinA * value.X;
        }
    }
}
