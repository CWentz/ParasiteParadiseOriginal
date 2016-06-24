using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class MonozytCell : SpriteClasses.MovingSprite
    {

        public bool playingSound;
        public bool proximity = false;
        private bool start = true;
        public int weldHealth = 200;
        public bool weldedbool = false;
        public WeldJoint welded;
        public bool backtospawn = true;
        public bool chasing = false;
        public bool attacked = false;
        private int attackedHealth = 60;

        public MonozytCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Depth = 0.058f;
            animation.Scale = .25f;
            aniM.FramesPerSecond = 10;
            aniM.AddAnimation("spin", 1, _frames - 1, animation.Copy());
            aniM.AddAnimation("attack", 3, _frames, animation.Copy());
            animation.IsLooping = false;
            aniM.AddAnimation("die", 2, _frames - 1, animation.Copy());
            spawn = position;
            position = new Vector2(position.X + 10f, position.Y);
            body.IgnoreGravity = false;
            body.Mass = .1f;
            body.Restitution = 0f;
            body.LinearDamping = .75f;
            aniM.Animation = "spin";
            body.FixtureList[0].UserData = "monozyt";
            Type = "Monozyt";
            rotates = false;
            pointList = new Menu.Points(Type, 0, false);
        }


        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsVisible)
            {
                Points = weldHealth / 2;


                Vector2 tempVelocity = new Vector2(0, 0);

                if (attacked)
                {
                    attackedHealth -= 2;
                    playingSound = true;
                }




                if (!proximity)
                {
                    if (start)
                    {
                        //velocity.X = .1f;
                        //velocity.Y = .1f;
                        //chase(new Vector2(position.X + 10f, position.Y));
                        start = false;
                    }
                    else velocity = returnToSpawn();
                }

                tempVelocity += new Vector2(velocity.X, velocity.Y);



                if (backtospawn)
                    velocity = returnToSpawn();

                //0 no bounce 1 bounce set from 0.0 to 1.0


                velocity = velocity / 2f;
                body.ApplyForce(tempVelocity);

                if (testCollision)
                {
                    body.OnCollision += OnCollision;
                }

                testCollision = !testCollision;

                
                getPosition = new Vector2((float)ConvertUnits.ToDisplayUnits(body.Position.X), (float)ConvertUnits.ToDisplayUnits(body.Position.Y));


                position = new Vector2((float)ConvertUnits.ToDisplayUnits(body.Position.X), (float)ConvertUnits.ToDisplayUnits(body.Position.Y));
                //this.rectangle.X = (int)this.position.X - (this.rectangle.Width / 2);
                //this.rectangle.Y = (int)this.position.Y - (this.rectangle.Height / 2);
                body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));

                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;

                this.oldPosition = this.position;
                //this.position += this.velocity;

                if (weldedbool && !attacked)
                {
                    body.Mass = .4f;
                    weldHealth--;
                }

                if (weldHealth <= 60 && aniM.Animation != "die" || attacked && aniM.Animation != "die")
                {
                    aniM.FramesPerSecond = 10;
                    aniM.Animation = "die";
                }

                if (weldHealth <= 0 && IsVisible || attackedHealth <= 0)
                {

                    if (weldedbool && IsVisible)
                        level.RemoveJoint(welded);
                    IsVisible = false;
                    

                }
                
                this.aniM.Update(gameTime);
                if (rotates)
                    this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

            }
            if (!IsVisible && !bodyRemoved)
            {
                playingSound = false;
                pointList.PointValue = Points;
                pointList.Credit = true;
                credit = true;
                bodyRemoved = true;
                body.Dispose(); //level.RemoveBody(body);
            }
            pointList.Update(gameTime);
            //this.circle.Update(position);
            
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.UserData as string == "Pattack")
            {
                attacked = true;
                
                fixtureB.Body.Friction = (Points / 100f);
                
                
            }

            if (fixtureB.UserData as string == "Player")
            {
                if (!weldedbool)
                {
                    Vector2 weldA = new Vector2((float)ConvertUnits.ToSimUnits((float)Math.Cos(rotation - ((float)Math.PI / 2f))), (float)ConvertUnits.ToSimUnits((float)Math.Sin(rotation - ((float)Math.PI / 2f))));

                    Vector2 weldB = new Vector2((float)ConvertUnits.ToSimUnits((float)Math.Cos(fixtureB.Body.Rotation - ((float)Math.PI / 2f))), (float)ConvertUnits.ToSimUnits((float)Math.Sin(fixtureB.Body.Rotation - ((float)Math.PI / 2f))));


                    welded = new WeldJoint(fixtureA.Body, fixtureB.Body,
                        weldA * 20f, //_bodyA.Body.LocalCenter
                        weldB * 20f); //_bodyB.Body.LocalCenter
                    level.AddJoint(welded);
                    weldedbool = true;
                }

                return false;

            }
            return true;
        }

        /// <summary>
        /// Chase AI
        /// </summary>
        /// <param name="targetPosition">position of target</param>
        public void chase(Vector2 targetPosition)
        {
            if (!attacked)
            {
                if (Vector2.Distance(position, targetPosition) > 250 && weldedbool == false)
                {
                    if (aniM.Animation != "spin" && aniM.Animation != "die")
                        aniM.Animation = "spin";
                    backtospawn = true;
                    chasing = false;
                }

                if (Vector2.Distance(position, targetPosition) < 200 && Vector2.Distance(position, targetPosition) > 75 && weldedbool == false)
                {
                    backtospawn = false;
                    chasing = true;
                    if (aniM.Animation != "attack" && aniM.Animation != "die")
                        aniM.Animation = "attack";
                    proximity = true;
                    if (targetPosition.X < position.X)
                        velocity.X = -.5f;
                    if (targetPosition.X > position.X)
                        velocity.X = .5f;
                    if (targetPosition.Y < position.Y)
                        velocity.Y = -.5f;
                    if (targetPosition.Y > position.Y)
                        velocity.Y = .5f;
                }
            }
        }

        /// <summary>
        /// Chase AI
        /// </summary>
        /// <param name="targetPosition">position of target</param>
        public Vector2 returnToSpawn()
        {
            Vector2 tempVelo = new Vector2(0,0);
            if (spawn.X < position.X)
                tempVelo.X = -.5f;
            else if (spawn.X > position.X)
                tempVelo.X = .5f;
            if (spawn.Y < position.Y)
                tempVelo.Y = -.5f;
            else if (spawn.Y > position.Y)
                tempVelo.Y = .5f;

            return tempVelo;
        }
    }
}
