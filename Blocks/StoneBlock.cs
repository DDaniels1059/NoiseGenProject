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
        private static Texture2D texture;
        private static bool isTextureLoaded = false;

        public StoneBlock(ContentManager content) : base(content/*, Game1.miningTexture, 2*/)
        {
            if (!isTextureLoaded)
            {
                texture = content.Load<Texture2D>("Stone");
                isTextureLoaded = true;
            }

            isMinable = true;
            multiplier = 1;
            timeToMine = 200f;
        }

        protected override Texture2D GetTexture()
        {
            return texture;
        }

        public override void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            GameData.items.Add(new Health(itemPosition, Content));
            //Debug.WriteLine("Dropped Stone");
        }
    }
}
