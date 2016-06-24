using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

namespace VirusGame.SpriteClasses.Valves
{

    public class Claps : MovingSprite
    {
        public Body rectBody;
        public FixedRevoluteJoint j;
        public float speed = MathHelper.Pi;

        public Claps(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, String _side, float _rotation, float _scale)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Depth = 0.074f;
            animation.Scale = _scale * .3f;
            aniM.AddAnimation("left", 1, _frames, animation.Copy());
            animation.SpriteEffect = SpriteEffects.FlipHorizontally;
            aniM.AddAnimation("right", 1, _frames, animation.Copy());
            aniM.FramesPerSecond = 2;
            aniM.Animation = _side;
            firstRun = true;
            body.Dispose();


            body = BodyFactory.CreateRectangle(level, ConvertUnits.ToSimUnits(100 * _scale), ConvertUnits.ToSimUnits(20 * _scale), 100f);
            body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            body.BodyType = BodyType.Dynamic;
            //fixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(10), 1, body.Position, body);
            
            body.Enabled = true;
            body.Rotation = _rotation;
            body.Mass = 10f;
            body.FixedRotation = false;
            body.Restitution = .25f;
            body.IgnoreGravity = true;
            //
            //body.CollisionCategories = Category.Cat2;
            //
            body.CollidesWith = Category.All & ~Category.Cat15 & ~Category.Cat3;
            if (_side == "left")
            {
                j = JointFactory.CreateFixedRevoluteJoint(
                    level,
                    body,
                    new Vector2((float)ConvertUnits.ToSimUnits(-30 * _scale),
                        (float)ConvertUnits.ToSimUnits(0)),
                    new Vector2((float)ConvertUnits.ToSimUnits(position.X),
                        (float)ConvertUnits.ToSimUnits(position.Y)));
                j.MotorTorque = 100f;
                j.MotorSpeed = -speed;
                
            }
            if (_side == "right")
            {
                j = JointFactory.CreateFixedRevoluteJoint(
                    level,
                    body,
                    new Vector2((float)ConvertUnits.ToSimUnits(30 * _scale),
                        (float)ConvertUnits.ToSimUnits(0)),
                    new Vector2((float)ConvertUnits.ToSimUnits(position.X),
                        (float)ConvertUnits.ToSimUnits(position.Y)));
                j.MotorTorque = 100f;
                j.MotorSpeed = speed;
                
                
            }
            // rotate 1/4 of a circle per second
            body.FixtureList[0].UserData = "valve";
            // have little torque (power) so it can push away a few blocks
            j.MaxMotorTorque = 100f;
            j.Breakpoint = 999999f;
            j.MotorEnabled = true;
            
            
               

        }

        public override void Update(GameTime gameTime)
        {
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (firstRun)
            {
                
                firstRun = false;
            }


            Vector2 tempVelocity = new Vector2(0, 0);

            rotation = body.Rotation;
            
            tempVelocity += new Vector2(velocity.X, velocity.Y);

            position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            //body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            if (position != oldPosition)
            {
                HasMoved = true;
            }
            else HasMoved = false;

            this.oldPosition = this.position;

            this.aniM.Update(gameTime);
           
        }
    }
}
