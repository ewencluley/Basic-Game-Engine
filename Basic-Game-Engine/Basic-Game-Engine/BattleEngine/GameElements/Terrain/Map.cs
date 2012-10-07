using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleEngine.GameElements.Terrain
{
    class Map:DrawableGameComponent
    {
        //Texture2D heightMap;
        String heightMapName;
        TerrainBlock[] mapSegments;

        int segmentsAcross, segmentsDown;
        public int SegmentsAcross{ get { return segmentsAcross; } }
        public int SegmentsDown { get { return segmentsDown; } }

        Game game;

        public Map(Game game, String heightMapName)
            :base(game)
        {
            this.heightMapName = heightMapName;
            this.game = game;
        }

        protected override void LoadContent()
        {
            Texture2D heightMap = game.Content.Load<Texture2D>(heightMapName);
            mapSegments = splitMap(heightMap);
            base.LoadContent();
        }

        //Splits a large height map texture into a variety of smaller ones.
        //Size is defined by TerrainBlock and should be limited to the maximum drawable verticies in one pass
        private TerrainBlock[] splitMap(Texture2D heightMap)
        {
            segmentsAcross = heightMap.Width / TerrainBlock.BLOCK_VERT_WIDTH;
            segmentsDown = heightMap.Height / TerrainBlock.BLOCK_VERT_WIDTH;
            TerrainBlock[] terrainBlocks = new TerrainBlock[segmentsAcross * segmentsDown];
            for (int sD = 0; sD < segmentsDown; sD++)
            {
                for (int sA = 0; sA < segmentsAcross; sA++)
                {
                    Color[] sectionHeightMapData = new Color[TerrainBlock.BLOCK_VERT_WIDTH * TerrainBlock.BLOCK_VERT_WIDTH];
                    int offsetX =0 , offsetY = 0;
                    if (sA > 0) offsetX = 1;
                    if (sD > 0) offsetY = 1;
                    heightMap.GetData<Color>(0, new Rectangle((sA * TerrainBlock.BLOCK_VERT_WIDTH)-offsetX, (sD * TerrainBlock.BLOCK_VERT_WIDTH)-offsetY, TerrainBlock.BLOCK_VERT_WIDTH, TerrainBlock.BLOCK_VERT_WIDTH), sectionHeightMapData, 0, TerrainBlock.BLOCK_VERT_WIDTH * TerrainBlock.BLOCK_VERT_WIDTH);

                    Texture2D mapSegment = new Texture2D(game.GraphicsDevice, TerrainBlock.BLOCK_VERT_WIDTH, TerrainBlock.BLOCK_VERT_WIDTH);
                    mapSegment.SetData<Color>(sectionHeightMapData);

                    terrainBlocks[sD * segmentsAcross + sA] = new TerrainBlock(game,mapSegment);
                    terrainBlocks[sD * segmentsAcross + sA].Initialize();
                    terrainBlocks[sD * segmentsAcross + sA].InitalizeTerrain(new Vector3(sA*TerrainBlock.REAL_BLOCK_WIDTH, 0, sD*TerrainBlock.REAL_BLOCK_WIDTH));
                }
            }
            return terrainBlocks;
        }

        private TerrainBlock[] AdjustJoinVertices(TerrainBlock[] terrainSegments)
        {
            for (int sD = 0; sD < segmentsDown; sD++)
            {
                for (int sA = 0; sA < segmentsAcross; sA++)
                {

                    TerrainBlock currentBlock = terrainSegments[(sD * sA) + sA];
                    if(sA == 0)
                    {/*left hand edge piece*/
                        if (sD == 0)
                        {/*top left corner*/
                        }
                        else if (sD == segmentsDown - 1)
                        {/*bottom left corner*/
                        }
                        else
                        {/*left edge*/
                        }
                    }
                    else if (sA == segmentsAcross - 1)
                    { /*right hand edge piece*/
                        if (sD == 0)
                        {/*top right corner*/
                        }
                        else if (sD == segmentsDown - 1)
                        {/*bottom right corner*/
                        }
                        else
                        {/*right edge*/
                        }
                    }
                    else
                    {
                        if (sD == 0) { /*top edge piece*/}
                        else if (sD == segmentsDown - 1) { /*bottom edge piece*/}
                        else { /* middle piece*/
                            
                        }
                    }
                }
            }
            return terrainSegments;
        }

        public TerrainBlock[] GetTerrainBlocks()
        {
            return mapSegments;
        }

    }
}
