using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Helpers;
using NoiseGenProject.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.Blocks
{
    internal class StoneBlock : Block
    {
        public StoneBlock() : base(GameData.Textures["Blocks/Stone"])
        {
            isMinable = true;
            multiplier = 1;
        }
        public override void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            Sounds.Pop.Play(0.2f, 0f, 0f);
            GameData.items.Add(new Health(itemPosition, Content));
        }
    }
}
