#region File Description
//-----------------------------------------------------------------------------
// Tile.cs
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
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,

        /// <summary>
        /// ladder tile is like platform: impassable above it but can be passed from below. Can be also passed from above 
        /// if player moves down
        /// </summary>
        Ladder = 3,

        /// <summary>
        /// temporarily impassable object, will change to passable after few times of collision
        /// </summary>
        Breakable = 4
    }

    /// <summary>
    /// Stores the appearance and collision behavior of a rectangle.
    /// </summary>
    public struct Tile
    {

        public TileCollision Collision;
        public Body body;

        public Vector2 Position
        {
            get { return body.Position; }
        }

        private Vector2 position;

        /// <summary>
        /// Constructs a new tile body.
        /// </summary>
        public Tile(TileCollision collision, float width, float height, Vector2 position, float rotation, World world)
        {
            Collision = collision;

            this.position = position + new Vector2(width, height) / 2;
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(height), 64f, ConvertUnits.ToSimUnits(this.position));
            body.BodyType = BodyType.Static;
            body.Rotation = rotation;
            body.FixtureList[0].UserData = "Wall";
            body.Restitution = 0.3f;
            body.Friction = 2f;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {

            //this call uses scaling but for some reason textures are drawn black
            //spriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(Position), new Rectangle((int)ConvertUnits.ToDisplayUnits(Position.X), (int)ConvertUnits.ToDisplayUnits(Position.Y), Texture.Width, Texture.Height), Color.White, body.Rotation, Origin,
            //scale, SpriteEffects.None, 0f);

            //spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(Position.X), (int)ConvertUnits.ToDisplayUnits(Position.Y), texture.Width, texture.Height), null, Color.White, rotation, Origin,
            //     SpriteEffects.None, 0f);

            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(Position), null, Color.White);
        }

    }
}