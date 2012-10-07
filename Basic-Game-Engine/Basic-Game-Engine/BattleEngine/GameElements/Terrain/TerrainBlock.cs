using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleEngine.GameElements.Terrain
{
    class TerrainBlock: DrawableGameComponent
    {
        Game theGame;
        int[] indices; //the array of indices
        VertexPositionNormalTexture[] vertices; //the array of vertices

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        
        String heightMapName; //name of the heightmap file
        Texture2D theHeightMap;
        
        Texture2D grassTexture;

        public static int BLOCK_VERT_WIDTH = 256;
        public static int VERT_SCALE = 5;
        public static int REAL_BLOCK_WIDTH = (BLOCK_VERT_WIDTH-1) * VERT_SCALE;

        public TerrainBlock(Game game, String heightmapName)
            :base(game)
        {
            theGame = (Game1) game;
            this.heightMapName = heightmapName;

        }

        public TerrainBlock(Game game, Texture2D heightMapSegment)
            : base(game)
        {
            theGame = (Game1)game;
            theHeightMap = heightMapSegment;

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            
            grassTexture = theGame.Content.Load<Texture2D>("grass");
            if (theHeightMap == null)
            {
                theHeightMap = theGame.Content.Load<Texture2D>(heightMapName);//the heightmap texture
                InitalizeTerrain(Vector3.Zero);
            }
            base.LoadContent();
        }

        public void InitalizeTerrain(Vector3 blockOffset)
        {
            int height = theHeightMap.Height; //find height of terrain
            int width = theHeightMap.Width; //find width of terrain

            #region Calculate Vertices
            vertices = new VertexPositionNormalTexture[(theHeightMap.Width) * (theHeightMap.Height)]; //setup the vertices array
            Color[] colorData = new Color[theHeightMap.Width * theHeightMap.Height]; //extract colour data from the height map
            theHeightMap.GetData<Color>(colorData);
            for (int y = 0; y < theHeightMap.Height; y++)
            {
                for (int x = 0; x < theHeightMap.Width; x++)
                {
                    int i = y * theHeightMap.Width + x;
                    //vertices[i] = new VertexPositionColor(new Vector3((float)x*5,colorData[i].R/5,(float)y*5), Color.White); //create each new vertex
                    vertices[i].Position = new Vector3((float)x * VERT_SCALE, colorData[i].R / 2, (float)y * VERT_SCALE) + blockOffset;
                    vertices[i].TextureCoordinate.X = (float)y / (30.0f);
                    vertices[i].TextureCoordinate.Y = (float)x / (30.0f);

                }


            }
            #endregion

            #region Calculate Indicies
            // Populate the array with references to indices in the vertex buffer
            indices = new int[(width - 1) * (height - 1) * 6];
            int j = 0;
            for (int i = 0; i < vertices.Length && i < (width * (height - 1)); i++)
            {
                if (((i + 1) % width) != 0 || i == 0)
                {

                    indices[j] = (int)i;                     // *----*
                    indices[++j] = (int)(i + 1);             //      |
                    indices[++j] = (int)(i + 1 + width);      //      *

                    indices[++j] = (int)i;                    // *
                    indices[++j] = (int)(i + width);          // |
                    indices[++j] = (int)(i + 1 + width);      // *----*
                    j++;

                }
            }
            #endregion

            #region Calculate Normals
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = Vector3.Zero;
            }


            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
            #endregion

            #region Vertex and Index Buffers
            vertexBuffer = new VertexBuffer(theGame.GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);
            indexBuffer = new IndexBuffer(theGame.GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
            indexBuffer.SetData(indices);
            #endregion
        }

        public VertexPositionNormalTexture GetVertex(int col, int row)
        {
            return vertices[row * BLOCK_VERT_WIDTH + col];
        }
        public VertexPositionNormalTexture[] GetVertexRow(int r)
        {
            VertexPositionNormalTexture[] row = new VertexPositionNormalTexture[BLOCK_VERT_WIDTH];
            for (int i = 0; i < BLOCK_VERT_WIDTH; i++)
            {
                row[i] = vertices[r * BLOCK_VERT_WIDTH + i];
            }
            return row;
        }
        public VertexPositionNormalTexture[] GetVertexColumn(int c)
        {
            VertexPositionNormalTexture[] col = new VertexPositionNormalTexture[BLOCK_VERT_WIDTH];
            for (int i = 0; i < BLOCK_VERT_WIDTH; i++)
            {
                col[i] = vertices[i * BLOCK_VERT_WIDTH + c];
            }
            return col;
        }

        public Texture2D GetTexture() { return grassTexture; }

        public VertexBuffer GetVertexBuffer() { return vertexBuffer; }

        public IndexBuffer GetIndexBuffer() { return indexBuffer; }

        public int GetVerticesLength() { return vertices.Length; }

        public int GetIndicesLength() { return indices.Length; }
    }
}
