using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleEngine.GameElements.Terrain
{
    class TreeVertexCollection
    {
        public VertexPositionNormalTexture[] Vertices;
        Vector3 position;
        int topSize, halfSize, vertexCount, scale;

        public VertexPositionNormalTexture this[int i]
        {
            get { return Vertices[i]; }
            set { Vertices[i] = value; }
        }

        public TreeVertexCollection(Vector3 position, Texture2D heightMap, int scale)
        {
            this.scale = scale;
            this.topSize = heightMap.Width - 1;
            this.halfSize = topSize / 2;
            this.position = position;
            this.vertexCount = heightMap.Width * heightMap.Width;

            Vertices = new VertexPositionNormalTexture[vertexCount];

            BuildVerticies(heightMap);

            CalculateAllNormals();
        }

        private void BuildVerticies(Texture2D heightMap)
        {
            Color[] heightMapColors = new Color[vertexCount];
            heightMap.GetData(heightMapColors);

            float x = position.X;
            float y = position.Y;
            float z = position.Z;
            float maxX = x + topSize;

            for (int i = 0; i < vertexCount; i++)
            {
                if (x > maxX)
                {
                    x = position.X;
                    z++;
                }

                y = position.Y + (heightMapColors[i].R / 5.0f);
                VertexPositionNormalTexture vert = new VertexPositionNormalTexture(new Vector3(x * scale, y * scale, z * scale), Vector3.Zero, Vector2.Zero);
                vert.TextureCoordinate = new Vector2((vert.Position.X - position.X) / topSize, (vert.Position.Z - position.Z) / topSize);
            }
        }

        private void CalculateAllNormals()
        {
            if (vertexCount < 9)
                return;

            int i = topSize + 2, j = 0, k = i + topSize;

            for (int n = 0; i <= (vertexCount - topSize) - 2; i += 2, n++, j += 2, k += 2)
            {

                if (n == halfSize)
                {
                    n = 0;
                    i += topSize + 2;
                    j += topSize + 2;
                    k += topSize + 2;
                }

                //Calculate normals for each of the 8 triangles
                SetNormals(i, j, j + 1);
                SetNormals(i, j + 1, j + 2);
                SetNormals(i, j + 2, i + 1);
                SetNormals(i, i + 1, k + 2);
                SetNormals(i, k + 2, k + 1);
                SetNormals(i, k + 1, k);
                SetNormals(i, k, i - 1);
                SetNormals(i, i - 1, j);
            }
        }

        private void SetNormals(int idx1, int idx2, int idx3)
        {
            if (idx3 >= Vertices.Length)
                idx3 = Vertices.Length - 1;

            var normal = Vector3.Cross(Vertices[idx2].Position - Vertices[idx1].Position, Vertices[idx1].Position - Vertices[idx3].Position);
            normal.Normalize();
            Vertices[idx1].Normal += normal;
            Vertices[idx2].Normal += normal;
            Vertices[idx3].Normal += normal;
        }

    }
}
