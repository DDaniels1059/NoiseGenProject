using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Helpers;
using NoiseGenProject.Items;
using System.Diagnostics;

namespace NoiseGenProject.Blocks
{
    internal class IronBlock : Block
    {
        private static Texture2D texture;
        private static bool isTextureLoaded = false;

        public IronBlock(ContentManager content) : base(content)
        {
            if (!isTextureLoaded)
            {
                texture = content.Load<Texture2D>("Blocks/Iron");
                isTextureLoaded = true;
            }
            isMinable = true;
            multiplier = 3;
            timeToMine = 600;
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
