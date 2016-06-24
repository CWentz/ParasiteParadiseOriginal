using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses
{
    public class CollectCell : SpriteClasses.MovingSprite
    {
        public bool isCollected = false;
        public bool wasFull = false;

        public CollectCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Scale = .15f;
            animation.Depth = 0.2f;
            aniM.FramesPerSecond = 9;
            aniM.AddAnimation("float", 1, _frames, animation.Copy());
            aniM.Animation = "float";
            body.FixtureList[0].UserData = "collectable";
            body.IsSensor = true;
            Type = "collectable";
            rotates = false;
            Points = 100;
            body.Mass = 0.01f;
            body.Restitution = 0f;
            //slows the object when no force applied
            body.LinearDamping = .75f;

            body.CollidesWith = ~Category.All | Category.Cat5;
            pointList = new Menu.Points(Type, Points, true);
            JointFactory.CreateFixedRevoluteJoint(level, body, body.LocalCenter,
                new Vector2((float)ConvertUnits.ToSimUnits(_position.X), (float)ConvertUnits.ToSimUnits(_position.Y)));

        }


        public override void Update(GameTime gameTime)
        {


            if (IsVisible)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


                Vector2 tempVelocity = new Vector2(0, 0);


                tempVelocity += new Vector2(velocity.X, velocity.Y);


                //0 no bounce 1 bounce set from 0.0 to 1.0
                
                body.ApplyForce(velocity);

                rotation += .05f;

                if (testCollision)
                {
                    body.OnCollision += OnCollision;
                }
                testCollision = !testCollision;

                getPosition = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

                position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
                //this.rectangle.X = (int)this.position.X - (this.rectangle.Width / 2);
                //this.rectangle.Y = (int)this.position.Y - (this.rectangle.Height / 2);

                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;

                this.oldPosition = this.position;
                if (rotates)
                    this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

            }

            if (!IsVisible && !bodyRemoved)
            {
                
                bodyRemoved = true;
                if (body.Friction == .1f)
                {
                    wasFull = true;
                    pointList.Restored = false;
                }
                if(body.Friction == .11f)
                {
                    wasFull = true;
                    pointList.Restored = true;
                }
                pointList.Credit = credit = isCollected = true;
                //isCollected = true;
                body.Dispose(); //level.RemoveBody(body);
            }
            pointList.Update(gameTime);
            this.aniM.Update(gameTime);

        }
        

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if(fixtureB.UserData as string == "Player")
            {
                body.CollidesWith = ~Category.All;

                IsVisible = false;
                return false;
            }
            return false;
        }

    }
}
