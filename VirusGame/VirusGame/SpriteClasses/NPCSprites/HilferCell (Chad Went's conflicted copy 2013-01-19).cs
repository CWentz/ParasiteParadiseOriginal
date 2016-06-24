using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common.PhysicsLogic;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class HilferCell : SpriteClasses.MovingSprite
    {
        public FarseerPhysics.Common.PhysicsLogic.Explosion explosion;

        public bool proximity = false;
        public bool hasBody = true;
        private int timer = 80;
        public bool backtospawn = true;
        private int explodeTimer = 0;
        private bool countdown = false;
        private int contactCountdown = 0;

        public HilferCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            body.CollidesWith = Category.All;
            animation.Scale = .25f;
            animation.Depth = 0.07f;
            aniM.FramesPerSecond = 10;
            aniM.AddAnimation("idle", 1, 8, animation.Copy());
            aniM.AddAnimation("proximity", 2, _frames, animation.Copy());
            aniM.AddAnimation("explode", 3, _frames, animation.Copy());
            aniM.Animation = "idle";
            Type = "Helper";
            rotates = false;
            spawn = position;
            Points = 50;
            body.Mass = 1f;
            body.Restitution = .75f;
            body.LinearDamping = .75f;
            //explosion = new Explosion(_level);
            //explosion.IsActiveOn(body);
            //explosion.AddEnabledCategory(Category.Cat5);
            //explosion.IgnoreWhenInsideShape = true;
            
        }

        public int ExplodeTimer
        {
            get { return explodeTimer; }
        }

        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (IsVisible)
            {
                
                Vector2 tempVelocity = new Vector2(0, 0);


                

                if (backtospawn)
                    velocity = returnToSpawn();

                tempVelocity += new Vector2(velocity.X, velocity.Y);


                body.OnCollision += OnCollision;



                body.ApplyForce(tempVelocity);

                position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
                body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));


                if (countdown)
                {
                    explodeTimer++;
                    explodeTimer += contactCountdown;
                }
                if (explodeTimer > 1 && explodeTimer <= 60)
                {
                    aniM.FramesPerSecond = explodeTimer;
                }
                if (explodeTimer >= 60)
                {
                    proximity = true;
                }


                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;
                
                oldPosition = position;

                if (rotates)
                    this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);
                if (proximity)
                {
                    if (aniM.Animation != "explode")
                    {
                        aniM.FramesPerSecond = 15;
                        aniM.Animation = "explode";
                    }
                    if (timer > 0)
                        timer = 0;
                    timer--;

                    if (timer == -13)
                    {
                        explode();
                        explodeTimer = 61;
                    }
                    if (timer == -30)
                        IsVisible = false;
                }
                aniM.Update(gameTime);
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
                explosion.IsActiveOn(fixtureB.Body);
                contactCountdown = 10;
                return true;
            }
            return true;
        }


        public void explode()
        {
            if (Vector2.Distance(position, playerPosition) > ConvertUnits.ToDisplayUnits(2f) * Globals.GlobalScale)
            {
                credit = true;
            }
            Vector2 min = body.Position - new Vector2(1.5f, 1.5f);
            Vector2 max = body.Position + new Vector2(1.5f, 1.5f);

            min = min * Globals.GlobalScale;
            max = max * Globals.GlobalScale;

            AABB aabb = new AABB(ref min, ref max);


            level.QueryAABB(fixture =>
            {
                Vector2 fv = fixture.Body.WorldCenter;
                fv.Normalize();
                fv *= 1.5f;
                fixture.Body.ApplyLinearImpulse(ref fv);
                return true;
            }, ref aabb);
            
            //explosion.Activate(body.Position, 10f, 1.5f);
            
        }

        /// <summary>
        /// Chase AI
        /// </summary>
        /// <param name="targetPosition">position of target</param>
        public void chase(Vector2 targetPosition, bool soundAbfrage = false)
        {
            PlayerPosition = targetPosition;
            if (Vector2.Distance(position, targetPosition) < 150 * Globals.GlobalScale)
            {
                if (aniM.Animation != "proximity" && aniM.Animation != "explode")
                    aniM.Animation = "proximity";

                if (Vector2.Distance(position, targetPosition) < 120 * Globals.GlobalScale)
                {
                    countdown = true;
                }
            }
            else if (aniM.Animation != "idle" && aniM.Animation != "explode" && !countdown)
                aniM.Animation = "idle";
        }


        public Vector2 returnToSpawn()
        {
            Vector2 tempVelo = new Vector2(0, 0);
            if (Vector2.Distance(position, spawn) < 50 * Globals.GlobalScale)
            {
                if (spawn.X < position.X)
                    tempVelo.X = -1f;
                if (spawn.X > position.X)
                    tempVelo.X = 1f;
                if (spawn.Y < position.Y)
                    tempVelo.Y = -1f;
                if (spawn.Y > position.Y)
                    tempVelo.Y = 1f;
            }
            return tempVelo;
        }
    }
}
