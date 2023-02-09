using Microsoft.Xna.Framework;
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
        public static int TileSize = 32;
        public static int MapSize = 850;
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
        public static int Curr_Cell = 0;

        public static List<Rectangle> TLeftCellColl = new List<Rectangle>();
        public static List<Rectangle> TRightCellColl = new List<Rectangle>();
        public static List<Rectangle> BLeftCellColl = new List<Rectangle>();
        public static List<Rectangle> BRightCellColl = new List<Rectangle>();
    }
}
