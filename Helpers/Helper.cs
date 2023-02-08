using Comora;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace NoiseGenProject.Helpers
{
    class Helper
    {
        public Vector2 GetMousePos(MouseState mstate, Camera camera)
        {
            mstate = Mouse.GetState();
            Vector2 mpos = new Vector2(mstate.X, mstate.Y);
            return mpos = Vector2.Transform(mpos, camera.ViewportOffset.Absolute);
        }

        public float GetDepth(Vector2 origin, GraphicsDeviceManager _graphics)
        {
            float depth = origin.Y / _graphics.PreferredBackBufferHeight;
            depth = depth * 0.01f; // multiply the depth by a small value
            return depth;
        }
    }
}
