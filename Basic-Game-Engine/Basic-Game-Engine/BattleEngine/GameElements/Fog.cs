using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BattleEngine.GameElements
{
    static class Fog
    {
        public static Vector3 Colour = Color.Black.ToVector3();
        public static bool On = true;
        public static float Start = 400f;
        public static float End = 600f;
    }
}
