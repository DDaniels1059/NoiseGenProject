using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoiseGenProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGenProject.UI
{
    internal class SettingsManager
    {
        private static Texture2D Menu;
        private static Texture2D Button;

        public void LoadContent(ContentManager Content)
        {
            Menu = Content.Load<Texture2D>("UI/Menu");
            Button = Content.Load<Texture2D>("Blocks/Stone");
        }

        public void Update(GraphicsDeviceManager _graphics, GameWindow Window, MouseState mState, MouseState mStateOld)
        {
            Vector2 mousePosScreen = new Vector2(mState.X, mState.Y);

            if (GameData.FullScreenButton.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
            {
                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    if (_graphics.PreferredBackBufferWidth == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    {
                        GameData.isFullscreen = false;
                        _graphics.PreferredBackBufferWidth = 1280;
                        _graphics.PreferredBackBufferHeight = 720;
                        Window.IsBorderless = false;
                    }
                    else
                    {
                        GameData.isFullscreen = true;
                        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                        Window.IsBorderless = true;
                    }
                    _graphics.ApplyChanges();
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont timerFont, Texture2D hoverTexture, Viewport viewport)
        {
            int screenWidth = viewport.Width;
            int screenHeight = viewport.Height;
            int settingsWidth = Menu.Width;
            int settingsHeight = Menu.Height;
            Vector2 SettingsMenuPosition = new Vector2(screenWidth / 2 - settingsWidth / 2, screenHeight / 2 - settingsHeight / 2);
            Vector2 fontScale = new Vector2(0.5f);

            if (GameData.showOptions)
            {
                _spriteBatch.Draw(Menu, SettingsMenuPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 1f);

                //FullScreen Button Draw
                if (!GameData.isFullscreen)
                {
                    _spriteBatch.Draw(Button, SettingsMenuPosition + new Vector2(+ 64, + 64), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                }
                else
                {
                    _spriteBatch.Draw(Button, SettingsMenuPosition + new Vector2(+64, +64), null, Color.Black, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                }

                //FullScreen Button Rectangle
                GameData.FullScreenButton = new Rectangle((int)SettingsMenuPosition.X + 64 , (int)SettingsMenuPosition.Y + 64, Button.Width, Button.Height);

                _spriteBatch.DrawString(timerFont, "FullScreen / Windowed", new Vector2((int)SettingsMenuPosition.X + 120, (int)SettingsMenuPosition.Y +64), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);

                _spriteBatch.Draw(hoverTexture, GameData.FullScreenButton, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
        }
    }
}
