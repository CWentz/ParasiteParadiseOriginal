using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

namespace VirusGame.SpriteClasses
{
    public class PlayerSprite : SpriteClasses.MovingSprite
    {
        #region declarations

        public bool invertControlMode = false;
        //temporary texture for confused state feedback
        //private Texture2D testConfused;
        private SpriteClasses.Player.ConfusionCloud cloud;

        public WeldJoint welded;
        public bool weldedbool = false;
        private int weldedKillTimer = 60;
        
        private Controls playerController = new Controls();
        private  SpriteClasses.Player.PlayerAttack attackSprite;
        private SpriteClasses.Player.PlayerConfuse confuseSprite;
        private SpriteClasses.Player.PlayerBleed bleedSprite;
        private SpriteClasses.Player.PlayerHeal healSprite;
        private SpriteClasses.Player.PlayerCollect collectSprite;
        //controller/keyboard states
        private KeyboardState key;
        private KeyboardState oldKey;
        private GamePadState controller;
        private GamePadState oldController;

        private bool normalControls = true;
        private int flippedCD = 60;
        private int attacktimer = 45;
       
        private int deathTimer = 60;
        public bool gameover = false;
        private bool wallWasHit;
        private int wallTimer = 30;
        private bool controlledRotation = false;
        private Vector2 oldLinearVelocity;
        private bool healed;
        private int healCD;
        private int score;
        private bool collectBool;
        private int collectTimer;

        private int kills;
        private int collected = 0;
        private int restored = 0;
        private int lives;
        private bool awardForFull = true;
        private bool cheated;
        private int cheatTimer;
        private float cheatSpeed;
        public bool cheatSound;


        public bool hitWallSoundActive;

        #endregion

        public PlayerSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            
            animation.Depth = 0.0711f;
            Type = "Player";
            animation.Scale = .25f;
            aniM.AddAnimation("move5", 1, _frames, animation.Copy());
            aniM.AddAnimation("move4", 2, _frames, animation.Copy());
            aniM.AddAnimation("move3", 3, _frames, animation.Copy());
            aniM.AddAnimation("move2", 4, _frames, animation.Copy());
            aniM.AddAnimation("move1", 5, _frames, animation.Copy());
            aniM.AddAnimation("death", 6, _frames, animation.Copy());
            aniM.Animation = "move5";
            aniM.FramesPerSecond = 10;
            lives = 5;
            confuseSprite = SpriteClasses.SpriteManager.addPlayerConfuse();
            bleedSprite = SpriteClasses.SpriteManager.addPlayerBleed();
            healSprite = SpriteClasses.SpriteManager.addPlayerHeal();
            collectSprite = SpriteClasses.SpriteManager.addPlayerCollect();
            cloud = new Player.ConfusionCloud(level, SpriteManager.Content.Load<Texture2D>("Images/Blindsheet1"), Vector2.Zero, Vector2.Zero, 1, 4);
            body.Restitution = .5f;
            body.Mass = .15f;
            body.LinearDamping = 1.5f;
            body.IgnoreGravity = false;
            body.FixtureList[0].UserData = "Player";
            setupAttack();
        }

        public bool WallWasHit
        {
            get { return wallWasHit; }
        }

        public int Collected
        {
            get { return collected; }
        }

        public int Restored
        {
            get { return restored; }
        }

        public int Score
        {
            get { return score; }
        }


        public bool Attacking
        {
            get { return attackSprite.IsVisible; }
        }
        public bool FlippedControls
        {
            get { return !normalControls; }
        }

        public int Lives
        {
            get { return lives; }
        }

        public void setupAttack()
        {

            attackSprite = SpriteManager.addPlayerAttack(position);
            attackSprite.body.CollisionCategories = Category.Cat5;
            //attackSprite.body.CollidesWith = Category.All & ~Category.Cat5 & ~Category.Cat15;
            Vector2 temp1 = attackSprite.body.Position - body.Position;
            Vector2 temp2 = body.Position - attackSprite.body.Position;
            body.CollisionCategories = Category.Cat5;
            body.CollidesWith = Category.All;
            body.SleepingAllowed = false;
            attackSprite.body.SleepingAllowed = false;
        }


        public override void Update(GameTime gameTime)
        {
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            key = Keyboard.GetState();
            controller = GamePad.GetState(PlayerIndex.One);

            if (IsVisible)
            {
                cloud.position = position;
                if (weldedbool)
                {
                    weldedKillTimer--;
                    if (weldedKillTimer <= 0)
                    {
                        lives = 0;
                    }
                }

                bool controlled = false;

                if (flippedCD < 0 && !normalControls)
                {
                    normalControls = true;
                    //flippedCD = 60;
                }

                velocity = new Vector2(0, 0);

                if (!normalControls)
                {
                    confuseSprite.Confused = true;
                }
                else
                {
                    confuseSprite.Confused = false;
                }

                #region controls

                if (playerController.Cheater() && !cheated)
                    cheated = true;

                if (cheated)
                {
                    cheatSpeed = .5f;
                    cheatTimer++;
                    if (cheatTimer == 1)
                    {
                        cheatSound = true;
                        lives--;
                    }
                    else cheatSound = false;
                    if (cheatTimer >= 180)
                    {
                        cheatSpeed = 0;
                        cheated = false;
                        cheatTimer = 0;
                    }
                }

                if (lives > 0)
                {
                    if (aniM.Animation != "move" + lives)
                        aniM.Animation = "move" + lives;
                }
                else if (aniM.Animation != "death")
                    aniM.Animation = "death";

                if (playerController.attack() && attacktimer <= 0)
                {
                    attack();
                    attacktimer = 45;
                }


                //attackSprite.rotation = rotation;
                


                if (normalControls)
                {
                    velocity += playerController.joystickMove();

                    if (playerController.moveRight())
                    {

                        velocity += new Vector2(1 + cheatSpeed, 0);
                    }

                    if (playerController.moveLeft())
                    {

                        velocity -= new Vector2(1 + cheatSpeed, 0);
                    }

                    if (playerController.moveUp())
                    {

                        velocity -= new Vector2(0, 1 + cheatSpeed);
                    }

                    if (playerController.moveDown())
                    {

                        velocity += new Vector2(0, 1 + cheatSpeed);
                    }
                    if (flippedCD < 30)
                        flippedCD++;
                }
                else
                {
                    velocity -= playerController.joystickMove();

                    
                    
                    flippedCD--;
                    if (playerController.moveRight())
                    {

                        velocity += new Vector2(-1, 0);
                    }

                    if (playerController.moveLeft())
                    {

                        velocity -= new Vector2(-1, 0);
                    }

                    if (playerController.moveUp())
                    {

                        velocity -= new Vector2(0, -1);
                    }

                    if (playerController.moveDown())
                    {

                        velocity += new Vector2(0, -1);
                    }

                }

                #endregion

                if (collectBool)
                {
                    collectSprite.Collect = true;
                    collectBool = false;

                }
                else
                {
                    collectSprite.Collect = false;
                }


                if (velocity != Vector2.Zero)
                {
                    controlled = true;
                }

                body.ApplyForce(velocity);


                if (testCollision)
                {
                    body.OnCollision += body_OnCollision;
                }

                testCollision = !testCollision;

                if (attackSprite.Killed)
                {
                    kills++;
                    score += (int)(attackSprite.body.Friction * 100f);
                    attackSprite.body.Friction = 0f;
                }

                position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;

                if (healed && healCD < 0)
                    healCD = 1;
                if (healCD == 0)
                    healed = false;

                healCD--;


                if (lives <= 0)
                {
                    if (deathTimer > 0)
                        deathTimer--;
                    else
                    {
                        IsVisible = false;
                        gameover = true;
                    }
                }

                //if (lives == 5)
                //{
                //    attackSprite.body.Friction = .1f;
                //}
                //else
                //{
                //    attackSprite.body.Friction = .2f;
                //}


                if (!controlled && !controlledRotation)
                {
                    oldLinearVelocity = body.LinearVelocity * .5f;
                    controlledRotation = true;
                }

                if (rotates && controlled)
                {
                    if (controlledRotation)
                    {
                        body.LinearVelocity = oldLinearVelocity;
                        controlledRotation = false;
                    }
                    this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);
                    rotation = rotation + ((float)Math.PI / 2f);
                }

                if (wallTimer > 45)
                    wallWasHit = true;
                else wallWasHit = false;

                attackSprite.position = position + ((new Vector2((float)Math.Cos(rotation - ((float)Math.PI / 2f)), (float)Math.Sin(rotation - ((float)Math.PI / 2f)))) * ((60f * Globals.GlobalScale) * Globals.GlobalScale));


                body.Rotation = rotation;
                attackSprite.rotation = rotation;
                attackSprite.body.Rotation = rotation;
                bleedSprite.rotation = rotation;
                bleedSprite.position = position;
                healSprite.rotation = rotation;
                healSprite.position = position;
                confuseSprite.position = position;
                confuseSprite.rotation = rotation;
                collectSprite.position = position;
                collectSprite.rotation = rotation;

                attacktimer--;
                wallTimer--;

                if (attacktimer < 0)
                    attackSprite.IsVisible = false;

                oldPosition = position;
                playerController.UpdateControls(key, oldKey, controller, oldController);
                attackSprite.Update(gameTime);
                bleedSprite.Update(gameTime);
                healSprite.Update(gameTime);
                confuseSprite.Update(gameTime);
                collectSprite.Update(gameTime);
                cloud.Update(gameTime);
                aniM.Update(gameTime);
            }
            else
            {
                if (!bodyRemoved)
                {
                    if(weldedbool)
                        level.RemoveJoint(welded);
                    bodyRemoved = true;
                    body.Dispose(); //level.RemoveBody(body);
                }
                bleedSprite.IsVisible = false;
                attackSprite.IsVisible = false;
                healSprite.IsVisible = false;
            }
            if (key.IsKeyDown(Keys.F11) && oldKey.IsKeyUp(Keys.F11))
            {
                invertControlMode = !invertControlMode;
            }

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();

        }




        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.UserData as string)
            {
                case "PlasmaShot":
                    {
                        if (invertControlMode)
                        {
                            flipControls();
                        }
                        else
                        {
                            cloud.Activate = true;
                        }
                        return true;
                    }
                case "Prop":
                    {
                        
                        return false;
                    }
                case "Makrophage":
                    {
                        IsVisible = false;
                        lives = 0;
                        //IsVisible = false;
                        //if (!weldedbool)
                        //{
                        //    Vector2 weldA = new Vector2((float)ConvertUnits.ToSimUnits((float)Math.Cos(rotation - ((float)Math.PI / 2f))), (float)ConvertUnits.ToSimUnits((float)Math.Sin(rotation - ((float)Math.PI / 2f))));

                        //    Vector2 weldB = new Vector2((float)ConvertUnits.ToSimUnits((float)Math.Cos(fixtureB.Body.Rotation)), (float)ConvertUnits.ToSimUnits((float)Math.Sin(fixtureB.Body.Rotation)));


                        //    welded = new WeldJoint(fixtureA.Body, fixtureB.Body,
                        //        weldA, //_bodyA.Body.LocalCenter
                        //        weldB); //_bodyB.Body.LocalCenter
                        //    level.AddJoint(welded);
                        //    weldedbool = true;
                        //}

                        return true;
                    }
                    

                case "flow":
                    {
                        BloodStreamVelocity(fixtureB.Body.Rotation);
                        return false;
                    }
                    

                case "collectable":
                    {
                        if (lives == 5 && fixtureB.Friction != .1f)
                        {
                            fixtureB.Body.Friction = .1f;
                            collected++;
                            if (!collectBool)
                            {
                                collectTimer = 0;
                                collectBool = true;
                            }
                            score += 100;
                            
                        }

                        if (lives < 5)
                        {

                            fixtureB.Body.Friction = .11f;
                            heal();
                        }
                        return false;
                    }
                    
                case "Wall":
                    {
                        WallHit();
                        return true;
                    }
                case "BloodCellDespawner":
                    {
                        return false;
                    }

                default : return true;

            }
            
        }

        private void BloodStreamVelocity(float _rotation)
        {
            float X = (float)(Math.Cos(_rotation));
            float Y = (float)(Math.Sin(_rotation));
            Vector2 newPos = new Vector2(X, Y);
            level.Gravity = newPos;
        }

        private Vector2 attackRay(float _rotation)
        {
            float X = (float)(Math.Cos(_rotation));
            float Y = (float)(Math.Sin(_rotation));
            return new Vector2(X, Y);
        }

        private void attack()
        {
            attackSprite.IsVisible = true;
            attackSprite.animationManager.Animation = "attack";
            attacktimer = 30;
        }

        private void flipControls()
        {
            if (flippedCD > 29)
            {
                normalControls = false;
            }
        }

        private void heal()
        {
            if (!healed)
            {
                healSprite.Healed = true;
                lives++;
                restored++;
                healed = true;
            }

        }

        private void attacked(int _damageTaken)
        {
            lives -= _damageTaken;
        }

 
        private void WallHit()
        {
            if (lives > 0 && wallTimer <= 0)
            {
                hitWallSoundActive = true;
                lives--;
                wallTimer = 60;
                bleedSprite.Damaged = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                confuseSprite.Draw(spriteBatch);
                attackSprite.Draw(spriteBatch);
                bleedSprite.Draw(spriteBatch);
                healSprite.Draw(spriteBatch);
                collectSprite.Draw(spriteBatch);
                cloud.Draw(spriteBatch);
                aniM.Draw(spriteBatch, texture, position, this.rotation);
            }
        }

    }
}
