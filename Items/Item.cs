using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using NoiseGenProject.Helpers;
using NoiseGenProject;

namespace NoiseGenProject.Items
{
    internal class Item
    {
        public Vector2 position;
        public bool Collided = false;
        public string itemName;
        protected SpriteAnimation anim;
        private static Texture2D itemTexture;
        private int itemOffset;
        public int itemTileSize;
        Rectangle hitBox;
        Helper help = new Helper();


        public Item(Vector2 position, ContentManager content, string path, int sheetLength, int sheetFPS, int itemTileSize, string itemName) 
        {
            this.position = position;
            itemTexture = content.Load<Texture2D>(path);
            itemOffset = itemTexture.Width / 8;
            anim = new SpriteAnimation(itemTexture, sheetLength, sheetFPS);
            this.itemName = itemName;
            this.itemTileSize = itemTileSize;
        }


        public void PickupItem(Player player)
        {
            if(player.PlayerCollisionBox.Intersects(hitBox))
            {
                UseItem(player);
            }
        }

        public virtual void UseItem(Player player) 
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, Player player)
        {
            hitBox = new Rectangle((int)position.X - itemOffset, (int)position.Y - itemOffset, 16, 16);
            anim.Position = new Vector2(position.X - itemOffset, position.Y - itemOffset);
            anim.Update(gameTime);
            PickupItem(player);
        }

        public void Draw(SpriteBatch _spriteBatch, Rectangle drawBounds, GraphicsDeviceManager _graphics)
        {
            int xStart = MathHelper.Max(0, (drawBounds.X) / GameData.TileSize);
            int xEnd = MathHelper.Min(GameData.MapSize, (drawBounds.Right) / GameData.TileSize + 1);
            int yStart = MathHelper.Max(0, (drawBounds.Y) / GameData.TileSize);
            int yEnd = MathHelper.Min(GameData.MapSize, (drawBounds.Bottom) / GameData.TileSize + 1);

            int itemX = (int)position.X / GameData.TileSize;
            int itemY = (int)position.Y / GameData.TileSize;

            if (itemX >= xStart && itemX <= xEnd && itemY >= yStart && itemY <= yEnd)
            {
                Vector2 origin = new Vector2(position.X - 8, position.Y - 8);
                float depth = help.GetDepth(origin, _graphics);
                anim.Draw(_spriteBatch, depth);
            }
        }
    }
}
