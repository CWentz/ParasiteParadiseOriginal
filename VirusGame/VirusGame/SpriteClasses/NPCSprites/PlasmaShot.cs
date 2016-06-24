using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class PlasmaShot : SpriteClasses.MovingSprite
    {
        
        #region Declarations

        Char direction;
        public int timer = 60;

        #endregion

        public PlasmaShot(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, char _direction)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            position = _position;
            Type = "plasmashot";
            animation.Depth = 0.08f;
            animation.Scale = .5f;
            aniM.AddAnimation("spin", 1, _frames, animation.Copy());
            aniM.Animation = "spin";
            
            direction = _direction;
            rotates = false;
            body.CollidesWith = Category.Cat5 | Category.Cat15;
            body.Restitution = 0f;
            body.Mass = .3f;
            body.LinearDamping = 2f;
            body.FixtureList[0].UserData = "PlasmaShot";
            

            
        }

        public Char Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsVisible)
            {
                if (testCollision)
                {
                    body.OnCollision += OnCollision;
                }

                testCollision = !testCollision;


                shotDirection();

                Vector2 tempVelocity = new Vector2(0, 0);
                tempVelocity += new Vector2((float)(Math.Cos(rotation + (Math.PI / 2.0))), (float)(Math.Sin(rotation + (Math.PI / 2.0))));

                body.ApplyForce((tempVelocity * 2f));

                position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));


                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;

                if (body.Restitution == .1f && IsVisible)
                    IsVisible = false;

                this.oldPosition = this.position;

                this.aniM.Update(gameTime);

            }

            if (!IsVisible && !bodyRemoved)
            {
                bodyRemoved = true;
                body.Dispose(); //level.RemoveBody(body);
            }
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.UserData as string == "Player")
            {
                IsVisible = false;
                return false;
            }
            if (fixtureB.UserData as string == "PlasmaCell")
            {
                return false;
            }
            if (fixtureB.UserData as string == "gefect")
            {
                IsVisible = false;
                return false;
            }
            IsVisible = false;
            if (fixtureB.UserData as string == "Prop")
            {
                IsVisible = true;
                return false;
            }
            
            return true;
        }


       
        
        public void shotDirection()
        {
            switch (direction)
            {
                case 'l':
                    {
                        //rotation = 3.14f;
                        velocity = new Vector2(-45f, 0);
                    }
                    break;
                case 'r':
                    {
                        //rotation = 0;
                        velocity = new Vector2(45f, 0);
                    }
                    break;

                case 'u':
                    {
                        //rotation = 1.52f;
                        velocity = new Vector2(0, -45f);
                    }
                    break;
                case 'd':
                    {
                        //rotation = -1.52f;
                        velocity = new Vector2(0, 45f);
                    }
                    break;
                default:
                    {
                        direction = 'd';
                    }
                    break;
            }
        }
    }
}
