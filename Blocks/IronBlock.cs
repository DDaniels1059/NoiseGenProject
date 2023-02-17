﻿using Microsoft.Xna.Framework;
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

        public IronBlock(ContentManager content)
        {
            if (!isTextureLoaded)
            {
                texture = content.Load<Texture2D>("Blocks/Iron");
                isTextureLoaded = true;
            }
            isMinable = true;
            multiplier = 3;
        }

        public override void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            Sounds.Pop.Play(0.2f, 0f, 0f);
            GameData.items.Add(new Health(itemPosition, Content));
        }
        protected override Texture2D GetTexture()
        {
            return texture;
        }

    }
}
