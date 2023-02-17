using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NoiseGenProject.Items
{
    internal class Health : Item
    {
        public Health(Vector2 position, ContentManager content) : base(position, content, "healthpickup", 4, 5, 16, "Health Pickup")
        {

        }

        public override void UseItem(Player player)
        {
            Collided = true;
        }
    }
}
