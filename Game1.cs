using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using NoiseGenProject.Blocks;
using NoiseGenProject.Helpers;
using NoiseGenProject.Items;
using System.Collections.Generic;

namespace NoiseGenProject
{

    enum Dir { Down, Up, Left, Right }


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kStateOld = Keyboard.GetState();
        MouseState mStateOld = Mouse.GetState();
        private Texture2D rectangleTexture;
        private Texture2D hoverTexture;

        private float hoverDistance;
        private Rectangle drawBounds;
        private Rectangle collisionBounds;


        SpriteFont timerFont;
        Color baseColor;
        Vector2 mouseHoverPos;

        Camera camera;
        Player player = new Player();
        Helper help = new Helper();
        FpsMonitor FPSM = new FpsMonitor();
        CreateMap createMap = new CreateMap();
        Random rand = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Window.AllowUserResizing = false;
            Window.IsBorderless = false;
            this.camera = new Camera(_graphics.GraphicsDevice);
            this.camera.Zoom = 2f;

            

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            baseColor = new Color(89, 86, 82, 255);
            rectangleTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new Color[] { Color.White });

            hoverTexture = Content.Load<Texture2D>("Hover");

            createMap.LoadContent(player, Content);
            player.LoadContent(Content);


            // Load the textures for the blocks
            timerFont = Content.Load<SpriteFont>("TextFont");

        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {

                KeyboardState kState = Keyboard.GetState();
                MouseState mState = Mouse.GetState();

                var initpos = player.Position;
                player.Update(gameTime, Content);
                var playerPos = player.Position;



                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                drawBounds = new Rectangle((int)player.Position.X - 400, (int)player.Position.Y - 400, 800, 800);
                collisionBounds = new Rectangle((int)player.Position.X - 50, (int)player.Position.Y - 50, 100, 100);

                mouseHoverPos = help.GetMousePos(mState, camera);
                hoverDistance = Vector2.Distance(player.Position, mouseHoverPos);


                FPSM.Update();  

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();




                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released && hoverDistance < 50)
                {
                    // Get the mouse position
                    Vector2 mousePos = help.GetMousePos(mState, camera);
                    Vector2 itemPosition = Vector2.Zero;
                    int x = (int)mousePos.X / GameData.TileSize;
                    int y = (int)mousePos.Y / GameData.TileSize;


                    if (x >= 0 && y >= 0 && x < GameData.MapSize && y < GameData.MapSize)
                    {
                        Block block = GameData.map[x, y];
                        Rectangle blockRect = new Rectangle(x * GameData.TileSize, y * GameData.TileSize, GameData.TileSize, GameData.TileSize);

                        int offset = rand.Next(0, 8);
                        int locOffset = rand.Next(0, 2);

                        if(locOffset == 0)
                        {
                            itemPosition = new Vector2((x * GameData.TileSize + 16) + offset, (y * GameData.TileSize + 16) + offset);
                        }
                        else
                        {
                            itemPosition = new Vector2((x * GameData.TileSize + 16) - offset, (y * GameData.TileSize + 16) - offset);
                        }

                        if(block != null)
                        {
                            if (blockRect.Contains((int)mousePos.X, (int)mousePos.Y) && block.isMinable)
                            {
                                // Remove the Block from the map and the collider from the list
                                block.DropItem(itemPosition, Content);
                                GameData.map[x, y] = null;


                                if (blockRect.Intersects(GameData.TLeftCell))
                                {
                                    GameData.TLeftCellColl.RemoveAll(c => c.X == x * GameData.TileSize && c.Y == y * GameData.TileSize);
                                }
                                else if (blockRect.Intersects(GameData.TRightCell))
                                {
                                    GameData.TRightCellColl.RemoveAll(c => c.X == x * GameData.TileSize && c.Y == y * GameData.TileSize);
                                }
                                else if (blockRect.Intersects(GameData.BLeftCell))
                                {
                                    GameData.BLeftCellColl.RemoveAll(c => c.X == x * GameData.TileSize && c.Y == y * GameData.TileSize);
                                }
                                else if (blockRect.Intersects(GameData.BRightCell))
                                {
                                    GameData.BRightCellColl.RemoveAll(c => c.X == x * GameData.TileSize && c.Y == y * GameData.TileSize);
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Not A Mineable or Valid Block");
                            }
                        }                        
                    }
                }


                if (kState.IsKeyDown(Keys.D1) && kStateOld.IsKeyUp(Keys.D1))
                {
                    this.camera.Zoom += 0.5f;
                }

                if (kState.IsKeyDown(Keys.D2) && kStateOld.IsKeyUp(Keys.D2))
                {
                    this.camera.Zoom -= 0.5f;
                }


                if (GameData.Curr_Cell != 0)
                {
                    List<Rectangle> cellCollisions = null;
                    switch (GameData.Curr_Cell)
                    {
                        case GameData.TLEFT_CELL:
                            cellCollisions = GameData.TLeftCellColl;
                            break;
                        case GameData.TRIGHT_CELL:
                            cellCollisions = GameData.TRightCellColl;
                            break;
                        case GameData.BLEFT_CELL:
                            cellCollisions = GameData.BLeftCellColl;
                            break;
                        case GameData.BRIGHT_CELL:
                            cellCollisions = GameData.BRightCellColl;
                            break;
                    }

                    if (cellCollisions != null)
                    {
                        foreach (var rect in cellCollisions)
                        {
                            if (rect.Intersects(player.PlayerCollisionBox) && collisionBounds.Contains(rect))
                            {
                                player.Position = initpos;
                                player.anim.setFrame(1);
                                break;
                            }
                        }
                    }
                }


                //if (player.PlayerCollisionBox.Intersects(GameData.TLeftCell))
                //{
                //    foreach (var rect in GameData.TLeftCellColl)
                //    {
                //        if (rect.Intersects(player.PlayerCollisionBox) && collisionBounds.Contains(rect))
                //        {
                //            player.Position = initpos;
                //            player.anim.setFrame(1);
                //        }
                //    }
                //}
                //else if (player.PlayerCollisionBox.Intersects(GameData.TRightCell))
                //{
                //    foreach (var rect in GameData.TRightCellColl)
                //    {
                //        if (rect.Intersects(player.PlayerCollisionBox) && collisionBounds.Contains(rect))
                //        {
                //            player.Position = initpos;
                //            player.anim.setFrame(1);
                //        }
                //    }
                //}
                //else if (player.PlayerCollisionBox.Intersects(GameData.BLeftCell))
                //{
                //    foreach (var rect in GameData.BLeftCellColl)
                //    {
                //        if (rect.Intersects(player.PlayerCollisionBox) && collisionBounds.Contains(rect))
                //        {
                //            player.Position = initpos;
                //            player.anim.setFrame(1);
                //        }
                //    }
                //}
                //else if (player.PlayerCollisionBox.Intersects(GameData.BRightCell))
                //{
                //    foreach (var rect in GameData.BRightCellColl)
                //    {
                //        if (rect.Intersects(player.PlayerCollisionBox) && collisionBounds.Contains(rect))
                //        {
                //            player.Position = initpos;
                //            player.anim.setFrame(1);
                //        }
                //    }
                //}

                //foreach (var rect in GameData.collisionObjects)
                //{
                //    if (rect.Intersects(player.PlayerCollisionBox) && collisionBounds.Contains(rect))
                //    {
                //        player.Position = initpos;
                //        player.anim.setFrame(1);
                //    }
                //}

                foreach (Item items in GameData.items)
                {
                    items.Update(gameTime, player);
                }

                kStateOld = kState;
                mStateOld = mState;

                GameData.items.RemoveAll(i => i.Collided);
                this.camera.Position = player.Position;
                this.camera.Update(gameTime);
                base.Update(gameTime);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(baseColor);

            #region DYNAMIC DISPLAY

            _spriteBatch.Begin(this.camera, SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            Vector2 playerOrigin = new Vector2(player.Position.X - 16, player.Position.Y - 1);
            player.Draw(_spriteBatch, rectangleTexture, help.GetDepth(playerOrigin, _graphics));
            createMap.Draw(_spriteBatch, drawBounds);

            Point mouseTile = new Point((int)(mouseHoverPos.X / GameData.TileSize), (int)(mouseHoverPos.Y / GameData.TileSize));
            if (hoverDistance <= 50)
            {
                if (mouseTile.X >= 0 && mouseTile.X < GameData.MapSize && mouseTile.Y >= 0 && mouseTile.Y < GameData.MapSize)
                {
                    Block block = GameData.map[mouseTile.X, mouseTile.Y];

                    if (block != null && block.isMinable)
                    {
                        Rectangle blockRect = new Rectangle(mouseTile.X * GameData.TileSize, mouseTile.Y * GameData.TileSize, GameData.TileSize, GameData.TileSize);
                        _spriteBatch.Draw(hoverTexture, blockRect, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                    }
                }
            }

            foreach (Item items in GameData.items)
            {
                items.Draw(_spriteBatch, drawBounds, _graphics);
            }

            _spriteBatch.Draw(hoverTexture, drawBounds, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(hoverTexture, collisionBounds, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);

            //_spriteBatch.Draw(hoverTexture, GameData.TLeftCell, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            //_spriteBatch.Draw(hoverTexture, GameData.TRightCell, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            //_spriteBatch.Draw(hoverTexture, GameData.BRightCell, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            //_spriteBatch.Draw(hoverTexture, GameData.BLeftCell, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);

            _spriteBatch.End();

            #endregion

            _spriteBatch.Begin();         
                FPSM.Draw(_spriteBatch, timerFont, new Vector2(25, 30), Color.White);
                _spriteBatch.DrawString(timerFont, "Player Pos: " + player.Position.X.ToString() + " " + player.Position.Y.ToString() + "       " + this.camera.Zoom, new Vector2(25, 60), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }


    public class FpsMonitor
    {
        public float Value { get; private set; }
        public TimeSpan Sample { get; set; }
        private Stopwatch sw;
        private int Frames;
        public FpsMonitor()
        {
            this.Sample = TimeSpan.FromSeconds(1);
            this.Value = 0;
            this.Frames = 0;
            this.sw = Stopwatch.StartNew();
        }
        public void Update()
        {
            if (sw.Elapsed > Sample)
            {
                this.Value = (float)(Frames / sw.Elapsed.TotalSeconds);
                this.sw.Reset();
                this.sw.Start();
                this.Frames = 0;
            }
        }
        public void Draw(SpriteBatch SpriteBatch, SpriteFont Font, Vector2 Location, Color Color)
        {
            this.Frames++;
            SpriteBatch.DrawString(Font, "FPS: " + this.Value.ToString(), Location, Color);
        }
    }
}