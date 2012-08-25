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
        Game1 theGame;
        BasicEffect effect; //the effect used to render the terrain
        Vector3 cameraPosition = new Vector3(0, 100, 0);
        
        int[] indices; //the array of indices
        VertexPositionNormalTexture[] vertices; //the array of vertices

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        
        String heightMapName; //name of the heightmap file
        int width, height; //the terrain's height and width

        Texture2D grassTexture;

        public TerrainBlock(Game game, String heightmapName)
            :base(game)
        {
            theGame = (Game1) game;
            this.heightMapName = heightmapName;

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = new BasicEffect(Game.GraphicsDevice);
            //tell the graphics device to mirror textures.
            

            //initial view and projection matrices 
            Matrix viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition - new Vector3(0f,50f,0f), new Vector3(0, 0, -1));
            Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, theGame.GraphicsDevice.Viewport.AspectRatio, 1.0f, 3000.0f);

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.World = Matrix.Identity;

            Texture2D theHeightMap = theGame.Content.Load<Texture2D>(heightMapName);//the heightmap texture
            grassTexture = theGame.Content.Load<Texture2D>("grass");

            height = theHeightMap.Height; //find height of terrain
            width = theHeightMap.Width; //find width of terrain

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
                    vertices[i].Position = new Vector3((float)x * 5, colorData[i].R / 2, (float)y * 5);
                    vertices[i].TextureCoordinate.X = (float)y /(64.0f);
                    vertices[i].TextureCoordinate.Y = (float)x /(64.0f);

                }


            }
            #endregion

            #region Calculate Indicies
            // Populate the array with references to indices in the vertex buffer
            indices = new int[(width - 1) * (height - 1) * 6];
            int j = 0;
            for (int i = 0; i < vertices.Length && i < (width * (height -1)); i++)
            {
                if (((i+1) % width) != 0 || i == 0)
                {

                    indices[j] = (int) i;                     // *----*
                    indices[++j] = (int) (i + 1);             //      |
                    indices[++j] = (int)(i + 1 + width);      //      *

                    indices[++j] = (int)i;                    // *
                    indices[++j] = (int)(i + width);          // |
                    indices[++j] = (int)(i + 1 + width);      // *----*
                    j++;

                }
            }
            #endregion

            #region Calculate Normals
            for(int i =0; i< vertices.Length; i++)
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
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            #region Camera Control
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) cameraPosition.Z += 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) cameraPosition.Z -= 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) cameraPosition.X -= 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) cameraPosition.X += 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) cameraPosition.Y += 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) cameraPosition.Y -= 1f;
            Matrix viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition - new Vector3(0f, 50f, 100f), new Vector3(0, 0, -1)); //make a new View Matrix based on the camera's new position
            effect.View = viewMatrix; //assign this to the effect
            #endregion
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SamplerState s = new SamplerState();
            s.AddressU = TextureAddressMode.Mirror; s.AddressV = TextureAddressMode.Mirror;
            theGame.GraphicsDevice.SamplerStates[0] = s;
            theGame.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            //effect.FogEnabled = true;
            //effect.FogStart = 120f;
            //effect.FogEnd = 150f;
            //effect.FogColor = Color.CornflowerBlue.ToVector3();
            effect.TextureEnabled = true;
            effect.Texture = grassTexture;
            effect.EnableDefaultLighting();
            effect.AmbientLightColor = new Vector3(0.5f, 0.5f,0.5f);
            //effect.CurrentTechnique = effect.Techniques["Textured"];
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                theGame.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                theGame.GraphicsDevice.Indices = indexBuffer;
                theGame.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);        
            }

            base.Draw(gameTime);
        }
    }
}
