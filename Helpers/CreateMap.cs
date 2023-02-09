using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Blocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.Helpers
{
    internal class CreateMap
    {
        public static FastNoiseLite noise = new FastNoiseLite();
        private static Texture2D ground;
        private static bool isTextureLoaded = false;
        Random rand = new Random();

        public void LoadContent(Player player, ContentManager Content)
        {
            if(!isTextureLoaded) 
            {
                ground = Content.Load<Texture2D>("Ground");
                isTextureLoaded = true;
            }

            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.060f);
            noise.SetSeed(rand.Next(1, 100000000));

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
                    if (value <= -0.9)
                    {
                        GameData.map[x, y] = new CoalBlock(Content);
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
                    else if (value <= 0.3)
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
                    else
                    {
                        GameData.map[x, y] = null;
                        if (!positionSet)
                        {
                            player.Position = new Vector2(x * GameData.TileSize + GameData.TileSize / 2, y * GameData.TileSize + 16);
                            positionSet = true;
                        }
                    }
                }
            }
        }


        public void Draw(SpriteBatch _spriteBatch, Rectangle bounds)
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
                        Vector2 blockPosition = new Vector2(x * GameData.TileSize, y * GameData.TileSize);
                        block.Draw(_spriteBatch, blockPosition);
                    }
                    else
                    {
                        Vector2 blockPosition = new Vector2(x * GameData.TileSize, y * GameData.TileSize);
                        _spriteBatch.Draw(ground, blockPosition, Color.White);
                    }
                }
            }
        }
    }
}
