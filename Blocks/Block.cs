using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoiseGenProject.Helpers;
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

        protected Vector2 Position { get; set; }
        //public static SpriteAnimation anim;
        private bool isTextureLoaded = false;
        public bool isMinable = false;
        public bool isMining = false;
        protected int multiplier = 1;
        public float elaspedTime = 0;
        public float timeToMine = 0;


        public Block(ContentManager content)
        {
            //if (!isTextureLoaded)
            //{
            //    anim = new SpriteAnimation(miningTexture, 4, miningTextureFPS);
            //    anim.IsLooping = false;
            //    isTextureLoaded = true;
            //}
        }

        protected virtual Texture2D GetTexture()
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(SpriteBatch _spriteBatch, Vector2 position)
        {
            _spriteBatch.Draw(GetTexture(), position, null, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.000001f);

            if (isMining)
            {
                Game1.anim.Position = position;
                Game1.anim.Draw(_spriteBatch, 0.000002f);
            }
            else
            {

            }
        }

        public void Update(GameTime gameTime, float dt)
        {
            if (isMining)
            {
                if (elaspedTime <= 50 * multiplier)
                {
                    Game1.anim.setFrame(0);
                }
                else if (elaspedTime <= 100 * multiplier)
                {
                    Game1.anim.setFrame(1);
                }
                else if (elaspedTime <= 150 * multiplier)
                {
                    Game1.anim.setFrame(2);
                }
                else if (elaspedTime <= 200 * multiplier)
                {
                    Game1.anim.setFrame(3);
                }
                else
                {
                    Game1.anim.setFrame(0);
                }
            }
            else
            {
                elaspedTime = 0;
            }
        }

        public virtual void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            throw new NotImplementedException();
        }
    }
}