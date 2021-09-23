using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYMono.Math
{
    public static class Vector2Helper
    {
        /// <summary>
        /// Rotate a vector by an angle in degree.
        /// </summary>
        /// <param name="value">The source vector</param>
        /// <param name="angle">The angle in degree</param>
        /// <returns>The rotated vector.</returns>
        public static Vector2 Rotate(Vector2 value, float angle)
        {
            double angleInRad = MathHelper.ToRadians(angle);
            float cosA = (float)System.Math.Cos(angleInRad);
            float sinA = (float)System.Math.Sin(angleInRad);

            float resultX, resultY;
            resultX = cosA * value.X - sinA * value.Y;
            resultY = cosA * value.Y + sinA * value.X;

            return new Vector2(resultX, resultY);
        }

        /// <summary>
        /// Return the unsigned angle between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Unsigned angle in degree. The value is always between 0 and 180 degree</returns>
        public static float Angle(Vector2 a, Vector2 b)
        {
            return MathHelper.ToDegrees((float)System.Math.Atan2(b.Y - a.Y, b.X - a.X));
        }

        /// <summary>
        /// Return the signed angle from the first vector to the second vector.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Signed angle in degree ranging from -180 to 180 degree.</returns>
        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            float result = (float)(System.Math.Atan2(to.Y, to.X) - System.Math.Atan2(from.Y, from.X));
            result = MathHelper.ToDegrees(result);

            if (result > 180) { result -= 2 * 180; }
            else if (result <= -180) { result += 2 * 180; }

            return result;
        }
    }
}
