using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoiseGenProject.Blocks;
using NoiseGenProject.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace NoiseGenProject.UI
{
    internal class SettingsManager
    {
        private static Texture2D Menu;
        private static Texture2D Button;
        private const int MaxDigits = 9;
        private const int MaxSaveName = 30;
        private float timer = 100;
        private bool createMapIsPressed = false;
        private static Rectangle FullScreenButton = new Rectangle(0, 0, 0, 0);
        private static Rectangle CreateNewMap = new Rectangle(0, 0, 0, 0);
        private TextBox seedTextBox;
        private TextBox saveNameTextBox;
        private List<LoadSaveButton> loadSaveButtons = new List<LoadSaveButton>();

        private string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private string miningProjectFolder;
        private string savesFolder;
        private string defaultFilePath;
        private string[] saveFiles;

        private string saveName = "";

        public void LoadMap(Player player, string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(gzipStream))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.All;
                    GameState gameState = serializer.Deserialize<GameState>(jsonReader);

                    GameData.map = gameState.Map;
                    GameData.mapSeed = gameState.Seed;
                    player.Position = gameState.PlayerPosition;
                    GameData.TLeftCellColl = gameState.TLeftCellColl;
                    GameData.TRightCellColl = gameState.TRightCellColl;
                    GameData.BLeftCellColl = gameState.BLeftCellColl;
                    GameData.BRightCellColl = gameState.BRightCellColl;
                }
            }
        }

        public void LoadContent(ContentManager Content)
        {
            Menu = Content.Load<Texture2D>("UI/Menu");
            Button = Content.Load<Texture2D>("Blocks/Stone");
            appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            miningProjectFolder = Path.Combine(appDataFolder, "MiningProject");
            savesFolder = Path.Combine(miningProjectFolder, "Saves");
            saveFiles = Directory.GetFiles(savesFolder, "*.json");

            seedTextBox = new TextBox("Seed: ");
            saveNameTextBox = new TextBox("Save Name: ");

            foreach (string saveFile in saveFiles)
            {
                string saveFileName = Path.GetFileName(saveFile);
                string saveFileNameWOE = Path.GetFileNameWithoutExtension(saveFile);
                LoadSaveButton button = new LoadSaveButton();
                button.FilePath = Path.Combine(savesFolder, saveFileName);
                button.FileName = saveFileName;
                button.FileNameWOE = saveFileNameWOE;
                loadSaveButtons.Add(button);
            }
        }

        public void Update(GraphicsDeviceManager _graphics, GameWindow Window, float dt, ContentManager Content, CreateMap createMap, Player player, MouseState mState, MouseState mStateOld, KeyboardState kState, KeyboardState kStateOld)
        {
            Vector2 mousePosScreen = new Vector2(mState.X, mState.Y);
            defaultFilePath = Path.Combine(savesFolder, saveName + ".json");

            seedTextBox.data = GameData.mapSeed.ToString();
            saveNameTextBox.data = saveName.ToString();

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

            foreach (LoadSaveButton button in loadSaveButtons)
            {
                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    if (button.Rectangle.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
                    {
                        LoadMap(player, button.FilePath);
                    }
                }
            }

            //Enter Seed
            if (seedTextBox.isPressed)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (kState.IsKeyDown(Keys.D0 + i) && kStateOld.IsKeyUp(Keys.D0 + i) || kState.IsKeyDown(Keys.NumPad0 + i) && kStateOld.IsKeyUp(Keys.NumPad0 + i))
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

                    if (GameData.mapSeed < 0)
                    {
                        GameData.mapSeed = 0;
                    }
                }
            }


            if (saveNameTextBox.isPressed)
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    if ((kState.IsKeyDown((Keys)c) && kStateOld.IsKeyUp((Keys)c)) || (kState.IsKeyDown((Keys)(c + 32)) && kStateOld.IsKeyUp((Keys)(c + 32))))
                    {
                        // If the player pressed a letter key, add it to the save name, Make sure it doesnt add more if its too Long
                        if (saveName.Length < MaxSaveName)
                        {
                            saveName += c;
                        }
                    }
                }

                if (kState.IsKeyDown(Keys.Back) && kStateOld.IsKeyUp(Keys.Back))
                {
                    // Remove the last digit from the seed value
                    if (saveName.Length <= 0)
                    {
                        saveName = "";
                    }
                    else
                    {
                        saveName = saveName.Remove(saveName.Length - 1);
                    }
                }
            }

            HashSet<string> existingFilePaths = new HashSet<string>(saveFiles);
            string[] newSaveFiles = Directory.GetFiles(savesFolder, "*.json");
            foreach (string saveFile in newSaveFiles)
            {
                if (!existingFilePaths.Contains(saveFile))
                {
                    string saveFileName = Path.GetFileName(saveFile);
                    string saveFileNameWOE = Path.GetFileNameWithoutExtension(saveFile);
                    LoadSaveButton button = new LoadSaveButton();
                    button.FilePath = saveFile;
                    button.FileName = saveFileName;
                    button.FileNameWOE = saveFileNameWOE;
                    loadSaveButtons.Add(button);

                    existingFilePaths.Add(saveFile);
                }
            }
            saveFiles = existingFilePaths.ToArray();

            

            if (kState.IsKeyDown(Keys.Add) && kStateOld.IsKeyUp(Keys.Add))
            {
                if (!Directory.Exists(miningProjectFolder))
                {
                    Directory.CreateDirectory(miningProjectFolder);
                }
                if (!Directory.Exists(savesFolder))
                {
                    Directory.CreateDirectory(savesFolder);
                }

                using (FileStream fileStream = new FileStream(defaultFilePath, FileMode.Create))
                using (GZipStream gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
                using (StreamWriter writer = new StreamWriter(gzipStream))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.All;
                    serializer.Serialize(jsonWriter, new GameState
                    {
                        Map = GameData.map,
                        Seed = GameData.mapSeed,
                        PlayerPosition = player.Position,
                        TLeftCellColl = GameData.TLeftCellColl,
                        TRightCellColl = GameData.TRightCellColl,
                        BLeftCellColl = GameData.BLeftCellColl,
                        BRightCellColl = GameData.BRightCellColl
                    });
                }
            }

            if (createMapIsPressed)
            {
                timer -= 400 * dt;

                if(timer <= 0)
                {
                    createMapIsPressed = false;
                    timer = 100;
                }
            }
            if (CreateNewMap.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
            {
                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    timer = 100;
                    createMapIsPressed = true;
                    createMap.CreateNew(player);
                }
            }


            if (seedTextBox.rectangle.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
            {
                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    saveNameTextBox.isPressed = false;
                    seedTextBox.isPressed = true;
                }
            }
            if (saveNameTextBox.rectangle.Contains(mousePosScreen.X, mousePosScreen.Y) && GameData.showOptions)
            {
                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    seedTextBox.isPressed = false;
                    saveNameTextBox.isPressed = true;
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
                _spriteBatch.DrawString(timerFont, "FullScreen / Windowed ", new Vector2((int)SettingsMenuPosition.X + 120, (int)SettingsMenuPosition.Y + 64), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);

                //Create Button Draw
                if (!createMapIsPressed)
                {
                    _spriteBatch.Draw(Button, SettingsMenuPosition + new Vector2(+64, +114), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                }
                else
                {
                    _spriteBatch.Draw(Button, SettingsMenuPosition + new Vector2(+64, +114), null, Color.Black, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                }


                //Seed TextBox Draw
                seedTextBox.Draw(_spriteBatch, (int)SettingsMenuPosition.X + 361, (int)SettingsMenuPosition.Y + 111, (int)SettingsMenuPosition.X + 320, (int)SettingsMenuPosition.Y + 114);
                saveNameTextBox.Draw(_spriteBatch, (int)SettingsMenuPosition.X + 661, (int)SettingsMenuPosition.Y + 111, (int)SettingsMenuPosition.X + 575, (int)SettingsMenuPosition.Y + 114);


                CreateNewMap = new Rectangle((int)SettingsMenuPosition.X + 64, (int)SettingsMenuPosition.Y + 114, Button.Width, Button.Height);
                _spriteBatch.DrawString(timerFont, "Create New Map ", new Vector2((int)SettingsMenuPosition.X + 120, (int)SettingsMenuPosition.Y + 114), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
                int yPos = (int)SettingsMenuPosition.Y + 164;

                foreach (LoadSaveButton button in loadSaveButtons)
                {
                    Rectangle saveFileRect = new Rectangle((int)SettingsMenuPosition.X + 120, yPos, (int)80, (int)20);
                    button.Rectangle = saveFileRect;

                    _spriteBatch.DrawString(timerFont, button.FileNameWOE, new Vector2((int)SettingsMenuPosition.X + 120, yPos), Color.White, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
                    yPos += 30;
                }




                //foreach (SaveFileLoad button in saveFileButtons)
                //{
                //    _spriteBatch.Draw(hoverTexture, button.Rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                //}
                //_spriteBatch.Draw(hoverTexture, CreateNewMap, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                //_spriteBatch.Draw(hoverTexture, FullScreenButton, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            }
        }
    }
}
