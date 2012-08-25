using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleEngine.GameElements.Terrain;

namespace BattleEngine
{
    class Level:DrawableGameComponent
    {
        Model myModel;
        SpriteBatch spriteBatch;
        float modelRotation = 0.0f;
        Vector3 modelPosition = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(-1000, 0, 0);
        float aspectRatio;

        TerrainBlock lineList;

        public Level(Game game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            lineList = new TerrainBlock(Game, "heightmap");
            lineList.Initialize();
            //this. .Components.Add(lineList);
            myModel = Game.Content.Load<Model>("ship");
            aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //modelRotation += (float) gameTime.ElapsedGameTime.TotalSeconds;
            lineList.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            System.Console.WriteLine("game running:" + gameTime.TotalGameTime.Milliseconds);
            //Matrix[] transforms = new Matrix[myModel.Bones.Count];
            //myModel.CopyAbsoluteBoneTransformsTo(transforms);
            //// Draw the model. A model can have multiple meshes, so loop.
            //foreach (ModelMesh mesh in myModel.Meshes)
            //{
            //    // This is where the mesh orientation is set, as well 
            //    // as our camera and projection.
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.World = transforms[mesh.ParentBone.Index] *
            //            Matrix.CreateRotationY(modelRotation)
            //            * Matrix.CreateTranslation(modelPosition);
            //        effect.View = Matrix.CreateLookAt(cameraPosition,
            //            Vector3.Zero, Vector3.Up);
            //        effect.Projection = Matrix.CreatePerspectiveFieldOfView(
            //            MathHelper.ToRadians(45.0f), aspectRatio,
            //            1.0f, 10000.0f);
            //    }
            //    // Draw the mesh, using the effects set above.
            //    mesh.Draw();
            //}
            //base.Draw(gameTime);

            lineList.Draw(gameTime);

            base.Draw(gameTime);
        }


    }
}
