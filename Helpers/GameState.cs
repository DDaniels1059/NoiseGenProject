using Microsoft.Xna.Framework;
using NoiseGenProject.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.Helpers
{
    internal class GameState
    {
        public Block[,] Map { get; set; }

        public int Seed { get; set; }

        public Vector2 PlayerPosition { get; set; }
        
        public List<Rectangle> TLeftCellColl { get; set; }
        public List<Rectangle> TRightCellColl { get; set; }
        public List<Rectangle> BLeftCellColl { get; set; }
        public List<Rectangle> BRightCellColl { get; set; }
    }
}
