using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleEngine.GameElements.Terrain
{
    class TerrainBlock
    {
        VertexPositionColor[] vertices;
        Vector3 position;
        public TerrainBlock(Texture2D heightMap, float scale, Vector3 position)
        {
            this.position = position;
            vertices = new VertexPositionColor[heightMap.Width * heightMap.Height];
            Color[] colorData = new Color[heightMap.Width * heightMap.Height];
            heightMap.GetData<Color>(colorData);
            int row = 0;
            for (int i = 0; i < colorData.Length; i++)
            {
                vertices[i] = new VertexPositionColor(new Vector3(i - (row * heightMap.Width), colorData[i].R, (float)row), Color.Wheat); //colorData[i]
                if (i % heightMap.Width == 0) row++;
            }
        }

    }
}
