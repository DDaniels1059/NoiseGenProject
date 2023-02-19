using Microsoft.Xna.Framework;
using NoiseGenProject.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.Helpers
{
    internal class GameState
    {
        public Block[,] Map { get; set; }
        public Vector2 PlayerPosition { get; set; }
    }
}
