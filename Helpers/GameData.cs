using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Blocks;
using NoiseGenProject.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.Helpers
{
    internal class GameData
    {
        public static bool isFullscreen = false;
        public static bool showOptions = false;

        public static int mapSeed = 0;
        public static int TileSize = 32;
        public static int MapSize = 500;
        public static int CellSize = ((MapSize * TileSize) / 2) + 200; //Add 200 Pixels To Each Cell For OverLap.
        public static List<Rectangle> collisionObjects = new List<Rectangle>();
        public static Block[,] map = new Block[MapSize, MapSize];
        public static List<Item> items = new List<Item>();

        public static Rectangle TLeftCell = new Rectangle(0, 0, CellSize, CellSize);
        public static Rectangle TRightCell = new Rectangle(CellSize, 0, CellSize, CellSize);
        public static Rectangle BLeftCell = new Rectangle(0, CellSize, CellSize, CellSize);
        public static Rectangle BRightCell = new Rectangle(CellSize, CellSize, CellSize, CellSize);

        public const int TLEFT_CELL = 1;
        public const int TRIGHT_CELL = 2;
        public const int BLEFT_CELL = 3;
        public const int BRIGHT_CELL = 4;
        public static int CURR_CELL = 0;

        public static List<Rectangle> TLeftCellColl = new List<Rectangle>();
        public static List<Rectangle> TRightCellColl = new List<Rectangle>();
        public static List<Rectangle> BLeftCellColl = new List<Rectangle>();
        public static List<Rectangle> BRightCellColl = new List<Rectangle>();

        public static SpriteFont font;
        public static Dictionary<string, Texture2D> Textures;
        public static void LoadTextures(ContentManager content)
        {
            Textures = new Dictionary<string, Texture2D>();
            Textures["Blocks/Ground"] = content.Load<Texture2D>("Blocks/Ground");
            Textures["Blocks/Coal"] = content.Load<Texture2D>("Blocks/Coal");
            Textures["Blocks/Iron"] = content.Load<Texture2D>("Blocks/Iron");
            Textures["Blocks/Stone"] = content.Load<Texture2D>("Blocks/Stone");
            Textures["UI/textBox"] = content.Load<Texture2D>("UI/textBox");

            font = content.Load<SpriteFont>("timerFont");
        }
    }
}
