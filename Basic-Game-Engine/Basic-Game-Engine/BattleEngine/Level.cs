using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleEngine.GameElements.Terrain;
using BattleEngine.GameElements.Units;
using BattleEngine.GameElements;
using BattleEngine.GameElements.Cameras;

namespace BattleEngine
{
    class Level:DrawableGameComponent
    {
        Soldier myModel;
        MouseCamera cam;

        //TerrainBlock lineList;
        Map map;

        public Level(Game game)
            : base(game)
        {
            cam = new MouseCamera(game);
            map = new Map(game, "heightmap");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            map.Initialize();
            //lineList = new TerrainBlock(Game, "heightmap");
            //lineList.Initialize();
            //this. .Components.Add(lineList);
            myModel = new Soldier(Game.Content.Load<Model>("ship"), new Vector3(100f, 100f, 0f));
            //aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            cam.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            System.Console.WriteLine("game running:" + gameTime.TotalGameTime.Milliseconds);
            cam.Draw3DAsset(myModel);
            foreach (TerrainBlock t in map.GetTerrainBlocks())
            {
                cam.DrawTerrainAsset(t);
            }
            //cam.DrawTerrainAsset(lineList);
            base.Draw(gameTime);
        }


    }
}
