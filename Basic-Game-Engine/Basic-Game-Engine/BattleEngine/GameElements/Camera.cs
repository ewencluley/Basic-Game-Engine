using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleEngine.GameElements.Terrain;
using Microsoft.Xna.Framework.Input;

namespace BattleEngine.GameElements
{
    class Camera:GameComponent
    {
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Vector3 position;
        BasicEffect effect;

        Game game;

        public Camera(Game game) :
            base(game)
        {
            this.game = game;
            position = new Vector3(1000.0f,0.0f, 0.0f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 300000.0f);
            viewMatrix = Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
            effect = new BasicEffect(Game.GraphicsDevice);
            effect.World = Matrix.Identity;
        }

        public override void Update(GameTime gameTime)
        {
            #region Camera Control
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) position.Z += 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) position.Z -= 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) position.X -= 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) position.X += 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) position.Y += 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) position.Y -= 1f;
            viewMatrix = Matrix.CreateLookAt(position, position - new Vector3(0f, 50f, 100f), new Vector3(0, 0, -1)); //make a new View Matrix based on the camera's new position
            game.Window.Title = "X:" + position.X + ", Y:" + position.Y + ", Z:" + position.Z;
            #endregion
            base.Update(gameTime);
        }

        public void Draw3DAsset(Asset3D asset)
        {
            //viewMatrix = Matrix.CreateLookAt(position,
                         //Vector3.Zero, Vector3.Up);
            Model model = asset.GetModel();   
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] 
                        * Matrix.CreateFromQuaternion(asset.GetDirection())
                        * Matrix.CreateTranslation(asset.GetPosition());
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        public void DrawTerrainAsset(TerrainBlock asset)
        {
            
            
            SamplerState s = new SamplerState();
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            s.AddressU = TextureAddressMode.Wrap; s.AddressV = TextureAddressMode.Wrap;
            Game.GraphicsDevice.SamplerStates[0] = s;
            Game.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            //effect.FogEnabled = true;
            //effect.FogStart = 120f;
            //effect.FogEnd = 150f;
            //effect.FogColor = Color.CornflowerBlue.ToVector3();
            effect.TextureEnabled = true;
            effect.Texture = asset.GetTexture();
            effect.EnableDefaultLighting();
            effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.SetVertexBuffer(asset.GetVertexBuffer());
                game.GraphicsDevice.Indices = asset.GetIndexBuffer();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, asset.GetVerticesLength(), 0, asset.GetIndicesLength() / 3);
            }

            //base.Draw(gameTime);
        }
    }
}
