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
        public IronBlock() : base(GameData.Textures["Blocks/Iron"])
        {
            isMinable = true;
            multiplier = 3;
        }

        public override void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            Sounds.Pop.Play(0.2f, 0f, 0f);
            GameData.items.Add(new Health(itemPosition, Content));
        }
    }
}
