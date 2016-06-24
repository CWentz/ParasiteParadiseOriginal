#region File Description
//-----------------------------------------------------------------------------
// MyTexture.cs
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame
{

    /// <summary>
    /// Stores the appearance and collision behavior of a rectangle.
    /// </summary>
    public struct MyTexture
    {

        Texture2D texture;
        public Vector2 Origin;
        public float depth;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        

        private Vector2 position;

        private float rotation;
        private Vector2 scale;

        /// <summary>
        /// Constructs a new tile with body and texture.
        /// </summary>
        public MyTexture(Texture2D tex, Vector2 pos, float rot, Vector2 sca, float dep)
        {
            depth = dep;
            texture = tex;
            rotation = rot;
            scale = sca;
            Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            this.position = pos;
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            //this call uses scaling but for some reason textures are drawn black
            //spriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(Position), new Rectangle((int)ConvertUnits.ToDisplayUnits(Position.X), (int)ConvertUnits.ToDisplayUnits(Position.Y), Texture.Width, Texture.Height), Color.White, body.Rotation, Origin,
            //scale, SpriteEffects.None, 0f);

            //spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(Position.X), (int)ConvertUnits.ToDisplayUnits(Position.Y), texture.Width, texture.Height), null, Color.White, rotation, Origin,
            //     SpriteEffects.None, 0f);

            spriteBatch.Draw(texture, Position, null, Color.White, rotation, Origin, scale, SpriteEffects.None, depth);
        }
    }
}
