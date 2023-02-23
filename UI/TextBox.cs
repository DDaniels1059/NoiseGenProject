using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoiseGenProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.UI
{
    internal class TextBox
    {
        public static Texture2D texture;
        public Rectangle rectangle;
        public bool isPressed;
        private string text = "";
        public string data = "";
        Vector2 fontScale = new Vector2(0.5f);


        public TextBox(string text)
        {
            texture = GameData.Textures["UI/textBox"];
            this.text = text;
        }

        public void Draw(SpriteBatch _spriteBatch, int rectX, int rectY, int textX, int textY)
        {
            rectangle = new Rectangle(rectX, rectY, texture.Width, texture.Height);

            if (isPressed)
            {
                _spriteBatch.Draw(texture, rectangle, null, Color.SlateGray, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
            else
            {
                _spriteBatch.Draw(texture, rectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            }

            if (text.Length > 0)
            {
                _spriteBatch.DrawString(GameData.font, text + data, new Vector2(textX, textY), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
            }
        }
    }
}
