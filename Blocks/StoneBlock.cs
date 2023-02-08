using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public StoneBlock(ContentManager content) : base(content, "Stone")
        {
            isMinable = true;
        }

        public override void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            GameData.items.Add(new Health(itemPosition, Content));

            Debug.WriteLine("Dropped Stone Item");
        }
    }
}
