using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Helpers;
using NoiseGenProject.Items;
using System.Diagnostics;

namespace NoiseGenProject.Blocks
{
    internal class CoalBlock : Block
    {
        private static Texture2D texture;
        private static bool isTextureLoaded = false;

        public CoalBlock(ContentManager content) : base(content/*, Game1.miningTexture, 1*/)
        {
            if (!isTextureLoaded)
            {
                texture = content.Load<Texture2D>("Coal");
                isTextureLoaded = true;
            }
            isMinable = true;
            multiplier = 2;
            timeToMine = 400;
        }

        protected override Texture2D GetTexture()
        {
            return texture;
        }

        public override void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            GameData.items.Add(new Health(itemPosition, Content));
            //Debug.WriteLine("Dropped Coal");
        }
    }
}
