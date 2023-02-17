﻿using Microsoft.Xna.Framework;
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

        public StoneBlock(ContentManager Content)
        {
            if (!isTextureLoaded)
            {
                texture = Content.Load<Texture2D>("Blocks/Stone");
                isTextureLoaded = true;
            }

            isMinable = true;
            multiplier = 1;
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
