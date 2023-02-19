using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Blocks;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace NoiseGenProject.Helpers
{
    internal class CreateMap
    {
        public static FastNoiseLite noise = new FastNoiseLite();
        private static Texture2D ground;
        private static bool isTextureLoaded = false;
        private Random rand = new Random();
        private static bool firstLoad = true;
        public void LoadContent(Player player, ContentManager Content)
        {
            if(!isTextureLoaded) 
            {
                ground = Content.Load<Texture2D>("Blocks/Ground");
                isTextureLoaded = true;
            }

            NoiseData(player, Content);

        }

        public void CreateNew(Player player, ContentManager Content)
        {
            //Set Current Map and Collider to Null/New
            for (int x = 0; x < GameData.MapSize; x++)
            {
                for (int y = 0; y < GameData.MapSize; y++)
                {
                    GameData.map[x, y] = null;
                }
            }

            GameData.TLeftCellColl = new List<Rectangle>();
            GameData.TRightCellColl = new List<Rectangle>();
            GameData.BLeftCellColl = new List<Rectangle>();
            GameData.BRightCellColl = new List<Rectangle>();

            NoiseData(player, Content);
        }

        private void NoiseData(Player player, ContentManager Content)
        {
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.060f);
            if (firstLoad)
            {
                int randomSeed = rand.Next(1, 999999999);
                noise.SetSeed(randomSeed);
                GameData.mapSeed = randomSeed;
                firstLoad = false;
            }
            else
            {
                noise.SetSeed(GameData.mapSeed);
            }            

             

            float[] noiseData = new float[GameData.MapSize * GameData.MapSize];
            int index = 0;

            for (int y = 0; y < GameData.MapSize; y++)
            {
                for (int x = 0; x < GameData.MapSize; x++)
                {
                    noiseData[index++] = noise.GetNoise(x, y);
                }
            }

            bool positionSet = false;

            for (int y = 0; y < GameData.MapSize; y++)
            {
                for (int x = 0; x < GameData.MapSize; x++)
                {
                    float value = noise.GetNoise(x, y);

                    //Pad The Maps First 15 blocks on the inward Edges with Stone that is NotMinable to Hide the Edge of the Map
                    bool isFirst15Top = (y < 15);
                    bool isFirst15Left = (x < 15);
                    bool isFirst15Right = (x >= GameData.MapSize - 15);
                    bool isFirst15Bottom = (y >= GameData.MapSize - 15);

                    if (isFirst15Top || isFirst15Left || isFirst15Right || isFirst15Bottom)
                    {
                        GameData.map[x, y] = new StoneBlock(Content);
                        GameData.map[x, y].isMinable = false;
                        Rectangle rect = new Rectangle(x * GameData.TileSize, y * GameData.TileSize, GameData.TileSize, GameData.TileSize);

                        if (rect.Intersects(GameData.TLeftCell))
                        {
                            GameData.TLeftCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.TRightCell))
                        {
                            GameData.TRightCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.BLeftCell))
                        {
                            GameData.BLeftCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.BRightCell))
                        {
                            GameData.BRightCellColl.Add(rect);
                        }
                    }
                    else if (value <= -.5)
                    {
                        // Ground Tile && Set Player Initial Position
                        GameData.map[x, y] = null;
                        if (!positionSet)
                        {
                            player.Position = new Vector2(x * GameData.TileSize + GameData.TileSize / 2, y * GameData.TileSize + 10);
                            positionSet = true;
                        }
                    }
                    else if (value > .86 && value <= 1)
                    {
                        int rarityChance = rand.Next(1, 900);
                        if (rarityChance <= 880)
                        {
                            GameData.map[x, y] = new CoalBlock(Content);
                        }
                        else
                        {
                            //This Will be Diamonds
                            GameData.map[x, y] = new IronBlock(Content);
                        }
                        Rectangle rect = new Rectangle(x * GameData.TileSize, y * GameData.TileSize, GameData.TileSize, GameData.TileSize);

                        if (rect.Intersects(GameData.TLeftCell))
                        {
                            GameData.TLeftCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.TRightCell))
                        {
                            GameData.TRightCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.BLeftCell))
                        {
                            GameData.BLeftCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.BRightCell))
                        {
                            GameData.BRightCellColl.Add(rect);
                        }
                    }
                    else
                    {
                        GameData.map[x, y] = new StoneBlock(Content);
                        Rectangle rect = new Rectangle(x * GameData.TileSize, y * GameData.TileSize, GameData.TileSize, GameData.TileSize);

                        if (rect.Intersects(GameData.TLeftCell))
                        {
                            GameData.TLeftCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.TRightCell))
                        {
                            GameData.TRightCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.BLeftCell))
                        {
                            GameData.BLeftCellColl.Add(rect);
                        }
                        else if (rect.Intersects(GameData.BRightCell))
                        {
                            GameData.BRightCellColl.Add(rect);
                        }
                    }
                }
            }
        }



        public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics, Camera camera, Rectangle bounds)
        {
            //What is bounds.Right / bounds.Bottom ?
            int xStart = MathHelper.Max(0, (bounds.X) / GameData.TileSize);
            int xEnd = MathHelper.Min(GameData.MapSize, (bounds.Right) / GameData.TileSize + 1);
            int yStart = MathHelper.Max(0, (bounds.Y) / GameData.TileSize);
            int yEnd = MathHelper.Min(GameData.MapSize, (bounds.Bottom) / GameData.TileSize + 1);           

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    // Calculate the position of the block on the screen
                    Block block = GameData.map[x, y];
                    if (block != null)
                    {
                        _spriteBatch.Begin(camera, SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
                        Vector2 blockPosition = new Vector2(x * GameData.TileSize, y * GameData.TileSize);
                        block.Draw(_spriteBatch, blockPosition);
                        _spriteBatch.End();
                    }
                    else
                    {
                        _spriteBatch.Begin(camera, SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
                        Vector2 blockPosition = new Vector2(x * GameData.TileSize, y * GameData.TileSize);
                        _spriteBatch.Draw(ground, blockPosition, Color.White);
                        _spriteBatch.End();
                    }
                }
            }
        }
    }
}
