using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleEngine.GameElements.Units
{
    class Soldier:Asset3D
    {
        Vector3 position;
        Model model;
        Quaternion direction;

        public Soldier(Model model, Vector3 position)
        {
            this.model = model;
            this.position = position;
        }

        public Model GetModel(){ return model; }

        public Vector3 GetPosition() { return position; }

        public Quaternion GetDirection() { return direction; }
    }
}
