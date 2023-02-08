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
        public static List<Rectangle> collisionObjects = new List<Rectangle>();
        public static Block[,] map = new Block[MapSize, MapSize];
        public static List<Item> items = new List<Item>();

    }
}
