using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoiseGenProject.Helpers;

namespace NoiseGenProject
{
    class Player
    {
        private Vector2 position;
        private Dir direction = Dir.Down;
        private int speed = 250;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        private Rectangle playerCollisionBox = new Rectangle();
        public SpriteAnimation anim;
        Texture2D walkDown;
        Texture2D walkUp;
        Texture2D walkRight;
        Texture2D walkLeft;




        public SpriteAnimation[] animations = new SpriteAnimation[4];
        public Vector2 Position { get { return position; } set { position = value; } }
        public Rectangle PlayerCollisionBox { get { return playerCollisionBox; } }

        public void LoadContent(ContentManager Content)
        {
            walkDown = Content.Load<Texture2D>("Player/walkDown");
            walkUp = Content.Load<Texture2D>("Player/walkUp");
            walkRight = Content.Load<Texture2D>("Player/walkRight");
            walkLeft = Content.Load<Texture2D>("Player/walkLeft");

            animations[0] = new SpriteAnimation(walkDown, 4, 8);
            animations[1] = new SpriteAnimation(walkUp, 4, 8);
            animations[2] = new SpriteAnimation(walkLeft, 4, 8);
            animations[3] = new SpriteAnimation(walkRight, 4, 8);
            anim = animations[0];
        }

        public void Update(GameTime gameTime, ContentManager Content)
        {
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            isMoving = false;

            if (kState.IsKeyDown(Keys.D))
            {
                direction = Dir.Right;
                isMoving = true;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                direction = Dir.Left;
                isMoving = true;
            }
            if (kState.IsKeyDown(Keys.W))
            {
                direction = Dir.Up;
                isMoving = true;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                direction = Dir.Down;
                isMoving = true;
            }

            if (isMoving)
            {
                switch (direction)
                {
                    case Dir.Right:
                        if (position.X < 850 * 32)
                            position.X += speed * dt;
                        break;
                    case Dir.Left:
                        if (position.X > 0)
                            position.X -= speed * dt;
                        break;
                    case Dir.Up:
                        if (position.Y > 10)
                            position.Y -= speed * dt;
                        break;
                    case Dir.Down:
                        if (position.Y < 850 * 32)
                            position.Y += speed * dt;
                        break;
                }
            }

            anim = animations[(int)direction];
            anim.Position = new Vector2(position.X - 16, position.Y - 16);

            if (isMoving)
                anim.Update(gameTime);
            else
                anim.setFrame(1);




            playerCollisionBox = new Rectangle((int)position.X - 8, (int)position.Y + 4, 16, 16);
            kStateOld = kState;
        }

        public void Draw(SpriteBatch _spriteBatch, Texture2D rectangleTexture, float depth)
        {

            anim.Draw(_spriteBatch, depth);
            //_spriteBatch.Draw(rectangleTexture, playerCollisionBox, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.8f);

            //if (showDebugCollider)
            //    _spriteBatch.Draw(debugTexture, PlayerCollisionBox, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.004f);

            //if (showDebugCollider)
            //    _spriteBatch.Draw(originTexture, playerOrigin, null, Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.005f);

        }
    }
}
