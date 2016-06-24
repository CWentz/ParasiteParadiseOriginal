using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame
{
    public class VelocityRect
    {
        public Body body;
        SpriteClasses.PlayerSprite player;

        public VelocityRect(World _level, Vector2 position, float rotation, SpriteClasses.PlayerSprite player)
        {

            body = BodyFactory.CreateRectangle(_level, ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(100), 1.0f, new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y)));
            body.BodyType = BodyType.Dynamic;
            body.OnCollision += Body_OnCollision;


        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            
            return true;
        }
    }
}
