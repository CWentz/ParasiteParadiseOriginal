using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame
{
    public class VelocityRect
    {
        public Body body;

        int width = 143;
        int height = 162;

        public VelocityRect(World _level, Vector2 position, Vector2 scale, float rota, SpriteClasses.PlayerSprite player)
        {
            body = BodyFactory.CreateRectangle(_level, ConvertUnits.ToSimUnits(width * scale.X), ConvertUnits.ToSimUnits(height * scale.X), 1.0f, new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y)));
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.Rotation = rota;
            body.SleepingAllowed = true;
            body.CollisionCategories = Category.Cat10;
            body.FixtureList[0].UserData = "flow";
            body.CollidesWith = Category.Cat5;
            body.IgnoreGravity = true;
        }
    }
}
