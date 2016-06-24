using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses.Valves
{
    public class Gefecht : MovingSprite
    {
        public bool playSound;
        private int closedTimer = 50;
        private int openingTimer = 50;
        public String nameForLevel;
        public FixedRevoluteJoint top;
        public FixedRevoluteJoint bottom;
        

        public bool open;

        public Gefecht(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, float _rotation, float _scale)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Depth = 0.06f;
            animation.Scale = _scale;
            animation.IsLooping = false;
            aniM.AddAnimation("closed", 1, 1, animation.Copy());
            aniM.AddAnimation("opening", 1, _frames, animation.Copy());
            aniM.AddAnimation("closing", 2, _frames, animation.Copy());
            aniM.FramesPerSecond = 8;
            aniM.Animation = "closed";
            firstRun = true;
            body.Dispose();
            

            body = BodyFactory.CreateRectangle(level, ConvertUnits.ToSimUnits(40 * _scale), ConvertUnits.ToSimUnits(280 * _scale), 100f);
            body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            body.BodyType = BodyType.Dynamic;
            
            body.Enabled = true;
            body.Rotation = _rotation;
            rotation = _rotation;
            body.Mass = 10f;
            body.FixedRotation = false;
            body.Restitution = .25f;
            body.IgnoreGravity = true;
            
            body.CollidesWith = Category.All | ~Category.Cat15 | ~Category.Cat3;
            

            position = _position + ((new Vector2((float)Math.Cos(rotation - (float)MathHelper.PiOver2), (float)Math.Sin(rotation - (float)MathHelper.PiOver2))) * 140f);
            top = JointFactory.CreateFixedRevoluteJoint(
                level,
                body,
                new Vector2((float)ConvertUnits.ToSimUnits(0),
                    (float)ConvertUnits.ToSimUnits(-140)),
                new Vector2((float)ConvertUnits.ToSimUnits(position.X),
                    (float)ConvertUnits.ToSimUnits(position.Y)));

            position = _position + ((new Vector2((float)Math.Cos(rotation + (float)MathHelper.PiOver2), (float)Math.Sin(rotation + (float)MathHelper.PiOver2))) * 140f);
            bottom = JointFactory.CreateFixedRevoluteJoint(
                level,
                body,
                new Vector2((float)ConvertUnits.ToSimUnits(0),
                    (float)ConvertUnits.ToSimUnits(140)),
                new Vector2((float)ConvertUnits.ToSimUnits(position.X),
                    (float)ConvertUnits.ToSimUnits(position.Y)));

            Type = "Gefecht";
            body.FixtureList[0].UserData = "gefecht";

        }


        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (testCollision)
            {
                body.OnCollision += OnCollision;
            }
            testCollision = !testCollision;

            if (firstRun)
            {

                firstRun = false;
            }

            if (open)
            {
                closedTimer = 50;
                body.CollidesWith = ~Category.All;
                if (aniM.Animation != "opening")
                    aniM.Animation = "opening";
                openingTimer--;
                if (openingTimer <= 0)
                    IsVisible = false;
            }
            if (!open)
            {
                IsVisible = true;
                body.CollidesWith = Category.All | ~Category.Cat15 | ~Category.Cat3;
                openingTimer = 50;
                if (aniM.Animation != "closing" && closedTimer == 50 && aniM.Animation != "closed")
                    aniM.Animation = "closing";

                closedTimer--;

                if (aniM.Animation != "closed" && closedTimer <= 0)
                    aniM.Animation = "closed";


                
            }

            Vector2 tempVelocity = new Vector2(0, 0);

            rotation = body.Rotation;

            tempVelocity += new Vector2(velocity.X, velocity.Y);

            position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            if (position != oldPosition)
            {
                HasMoved = true;
            }
            else HasMoved = false;

            this.oldPosition = this.position;

            this.aniM.Update(gameTime);


        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.UserData as string)
            {
                case ("Wall"):
                    return false;
                case ("gefecht"):
                    return false;
                case ("flow"):
                    return false;
                case ("Prop"):
                    return false;
                default:
                    return true;

            }
            //if (fixtureB.UserData as string == "Wall")
            //    return false;
            //if (fixtureB.UserData as string == "gefecht")
            //    return false;
            //if (fixtureB.UserData as string == "flow")
            //    return false;
            //return true;

        }
    }
}
