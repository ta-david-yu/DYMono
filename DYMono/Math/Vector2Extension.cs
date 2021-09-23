using System;
using System.Collections.Generic;
using System.Text;

namespace DYMono.Math
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Rotate a vector by an angle in degree.
        /// </summary>
        /// <param name="value">The source vector</param>
        /// <param name="angle">The angle in degree</param>
        /// <returns>The rotated vector.</returns>
        public static Vector2 Rotate(Vector2 value, float angle)
        {
            double resultX, resultY;
            double cosA = System.Math.Cos((double)Util.DegreesToRadians(angle));
            double sinA = System.Math.Sin((double)Util.DegreesToRadians(angle));

            resultX = cosA * value.X - sinA * value.Y;
            resultY = cosA * value.Y + sinA * value.X;

            return new Vector2((float)resultX, (float)resultY);
        }
    }
}
