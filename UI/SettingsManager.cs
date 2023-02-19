using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
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
        private const int MaxDigits = 8;
        private float timer = 100;
        private bool createMapIsPressed = false;
        private static Rectangle FullScreenButton = new Rectangle(0, 0, 0, 0);
        private static Rectangle CreateNewMap = new Rectangle(0, 0, 0, 0);

        public void LoadContent(ContentManager Content)
        {
            Menu = Content.Load<Texture2D>("UI/Menu");
            Button = Content.Load<Texture2D>("Blocks/Stone");
        }

        public void Update(GraphicsDeviceManager _graphics, GameWindow Window, float dt, ContentManager Content, CreateMap createMap, Player player, MouseState mState, MouseState mStateOld, KeyboardState kState, KeyboardState kStateOld)
        {
            Vector2 mousePosScreen = new Vector2(mState.X, mState.Y);
            //If the Player Clicks the FullScreen Button
            if (FullScreenButton.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
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

            //Enter Seed
            for (int i = 0; i < 10; i++)
            {
                if (kState.IsKeyDown(Keys.D0 + i) && kStateOld.IsKeyUp(Keys.D0 + i))
                {
                    // If the player pressed a number key, add it to the seed, Make sure it doesnt add more if its too Long
                    if (GameData.mapSeed.ToString().Length < MaxDigits)
                    {
                        GameData.mapSeed = GameData.mapSeed * 10 + i;
                    }
                }
            }

            if (kState.IsKeyDown(Keys.Back) && kStateOld.IsKeyUp(Keys.Back))
            {
                // Remove the last digit from the seed value
                GameData.mapSeed = GameData.mapSeed / 10;

                if (GameData.mapSeed == 0)
                {
                    GameData.mapSeed = 1;
                }
            }

            if (kState.IsKeyDown(Keys.Add) && kStateOld.IsKeyUp(Keys.Add))
            {
                GameState gameState = new GameState
                {
                    Map = GameData.map,
                    PlayerPosition = player.Position
                };

                string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
                //File.WriteAllText("gameData.json", json);

            }


            if (createMapIsPressed)
            {
                timer -= 100 * dt;

                if(timer <= 0)
                {
                    createMapIsPressed = false;
                    timer = 100;
                }
            }

            if (kState.IsKeyDown(Keys.Enter) && kStateOld.IsKeyUp(Keys.Enter))
            {
                // Make sure the seed is within the range of 1 to 99999999
                GameData.mapSeed = MathHelper.Clamp(GameData.mapSeed, 1, 99999999);
            }

            if(CreateNewMap.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
            {
                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    timer = 100;
                    createMapIsPressed = true;
                    createMap.CreateNew(player, Content);
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
                FullScreenButton = new Rectangle((int)SettingsMenuPosition.X + 64 , (int)SettingsMenuPosition.Y + 64, Button.Width, Button.Height);
                _spriteBatch.DrawString(timerFont, "FullScreen / Windowed", new Vector2((int)SettingsMenuPosition.X + 120, (int)SettingsMenuPosition.Y + 64), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);



                //Create Button Draw
                if(!createMapIsPressed)
                {
                    _spriteBatch.Draw(Button, SettingsMenuPosition + new Vector2(+64, +114), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                }
                else
                {
                    _spriteBatch.Draw(Button, SettingsMenuPosition + new Vector2(+64, +114), null, Color.Black, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                }
                CreateNewMap = new Rectangle((int)SettingsMenuPosition.X + 64, (int)SettingsMenuPosition.Y + 114, Button.Width, Button.Height);
                _spriteBatch.DrawString(timerFont, "Seed: " + GameData.mapSeed.ToString(), new Vector2((int)SettingsMenuPosition.X + 320, (int)SettingsMenuPosition.Y + 114), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
                _spriteBatch.DrawString(timerFont, "Create New Map", new Vector2((int)SettingsMenuPosition.X + 120, (int)SettingsMenuPosition.Y + 114), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);



                //_spriteBatch.Draw(hoverTexture, CreateNewMap, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                //_spriteBatch.Draw(hoverTexture, FullScreenButton, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
        }
    }
}
