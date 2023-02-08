using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.Blocks
{
    internal class Block
    {

        protected Texture2D Texture { get; set; }
        protected Vector2 Position { get; set; }

        public bool isMinable = false;

        public Block(ContentManager content, string path)
        {
            Texture = content.Load<Texture2D>(path);
        }


        public void Draw(SpriteBatch _spriteBatch, Vector2 position)
        {
            _spriteBatch.Draw(Texture, position, null, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.000001f);
        }


        public virtual void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            throw new NotImplementedException();
        }
    }
}