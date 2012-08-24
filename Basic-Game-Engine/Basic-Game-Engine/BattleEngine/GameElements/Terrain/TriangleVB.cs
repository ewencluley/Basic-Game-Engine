using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleEngine.GameElements.Terrain
{
    class TriangleVB: DrawableGameComponent
    {
        Game1 theGame;
        BasicEffect effect; //the effect used to render the terrain
        Vector3 cameraPosition = new Vector3(0, 50, 0);
        
        short[] indices; //the array of indices
        VertexPositionColor[] vertices; //the array of vertices

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        
        String heightMapName; //name of the heightmap file
        int width, height; //the terrain's height and width

        public TriangleVB(Game game, String heightmapName)
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
            effect.VertexColorEnabled = true;

            //initial view and projection matrices 
            Matrix viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition - new Vector3(0f,50f,0f), new Vector3(0, 0, -1));
            Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, theGame.GraphicsDevice.Viewport.AspectRatio, 1.0f, 3000.0f);

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.World = Matrix.Identity;

            Texture2D theHeightMap = theGame.Content.Load<Texture2D>(heightMapName);//the heghtmap texture
            height = theHeightMap.Height; //find height of terrain
            width = theHeightMap.Width; //find width of terrain
            vertices = new VertexPositionColor[(theHeightMap.Width ) * (theHeightMap.Height )];
            Color[] colorData = new Color[theHeightMap.Width * theHeightMap.Height]; //extract colour data from the height map
            theHeightMap.GetData<Color>(colorData);
            for (int y = 0; y < theHeightMap.Height; y++)
            {
                for (int x = 0; x < theHeightMap.Width; x++)
                {
                    int i = y * theHeightMap.Width + x;
                    vertices[i] = new VertexPositionColor(new Vector3((float)x*5,colorData[i].R/5,(float)y*5), Color.White); //create each new vertex
                }


            }

            // Populate the array with references to indices in the vertex buffer
            indices = new short[(width - 1) * (height - 1) * 6];
            int j = 0;
            for (int i = 0; i < vertices.Length && i < (width * (height -1)); i++)
            {
                if (((i+1) % width) != 0 || i == 0)
                {
                    indices[j] = (short) i;                     // *----*
                    indices[++j] = (short) (i + 1);             //      |
                    indices[++j] = (short)(i + 1 + width);      //      *

                    indices[++j] = (short)i;                    // *
                    indices[++j] = (short)(i + width);          // |
                    indices[++j] = (short)(i + 1 + width);      // *----*
                    j++;

                }
            }


            #region Vertex and Index Buffers
            vertexBuffer = new VertexBuffer(theGame.GraphicsDevice, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);
            indexBuffer = new IndexBuffer(theGame.GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
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
            Matrix viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition - new Vector3(0f, 50f, 100f), new Vector3(0, 0, -1)); //make a new View Matrix based on the camera's new position
            effect.View = viewMatrix; //assign this to the effect
            #endregion
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            theGame.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            effect.FogEnabled = true;
            effect.FogStart = 120f;
            effect.FogEnd = 150f;
            effect.FogColor = Color.CornflowerBlue.ToVector3();
            //effect.EnableDefaultLighting();
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
