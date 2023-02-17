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

        public int multiplier = 1;
        public bool isMinable = false;
        public bool isMining = false;
        public float elaspedTime = 0;


        public virtual void Draw(SpriteBatch _spriteBatch, Vector2 position)
        {
            _spriteBatch.Draw(GetTexture(), position, null, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.000001f);

            if (isMining)
            {
                Game1.anim.Position = position;
                Game1.anim.Draw(_spriteBatch, 0.000002f);
            }
        }

        public void Update(float dt)
        {
            if (isMining)
            {
                elaspedTime += 100 * dt;

                if (elaspedTime <= 25 * multiplier)
                {
                    if (elaspedTime > 1.5)
                    {
                        if (Sounds.MinePlayed == false)
                            Sounds.Mine.Play(0.3f, 0f, 0f);
                    }
                    Game1.anim.setFrame(0);
                    Sounds.MinePlayed = true;
                }
                else if (elaspedTime <= 50 * multiplier)
                {
                    Game1.anim.setFrame(1);
                    if (Sounds.MinePlayed == true)
                        Sounds.Mine.Play(0.3f, 0f, 0f);
                    Sounds.MinePlayed = false;
                }
                else if (elaspedTime <= 75 * multiplier)
                {
                    Game1.anim.setFrame(2);
                    if (Sounds.MinePlayed == false)
                        Sounds.Mine.Play(0.3f, 0f, 0f);
                    Sounds.MinePlayed = true;
                }
                else if (elaspedTime <= 100 * multiplier)
                {
                    Game1.anim.setFrame(3);
                    if (Sounds.MinePlayed == true)
                        Sounds.Mine.Play(0.3f, 0f, 0f);
                    Sounds.MinePlayed = false;
                }
                else
                {
                    Game1.anim.setFrame(0);
                }
            }
            else
            {
                Sounds.MinePlayed = false;
                elaspedTime = 0;
            }
        }

        protected virtual Texture2D GetTexture()
        {
            throw new NotImplementedException();
        }

        public virtual void DropItem(Vector2 itemPosition, ContentManager Content)
        {
            throw new NotImplementedException();
        }
    }
}