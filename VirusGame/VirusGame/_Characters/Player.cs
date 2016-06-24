using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;

namespace VirusGame
{

    public class Player
    {
        #region Declarations

        public SpriteClasses.PlayerSprite playerTexture;
        Controller Controls;
        KeyboardState previousKeyboardState = Keyboard.GetState();
        World level;
        ContentManager content;
        Body body;
        public Body wheel;
        public Fixture fixture;
        private float centerOffset;
        Vector2 av;

        public Vector2 Position
        {
            get
            {
                return (ConvertUnits.ToDisplayUnits(body.Position) + Vector2.UnitY * centerOffset);
            }
        }
        public Vector2 position;
        public Vector2 getPosition;
        public float gravAngle;

        #endregion

        #region Constructor
        public Player(World level, ContentManager serviceProvider, Vector2 position, float mass)
        {
            content = serviceProvider;
            this.level = level;
            this.position = position;
            this.getPosition = position;
            LoadContent();

            body = BodyFactory.CreateCircle(level, ConvertUnits.ToSimUnits(20), 1.0f);
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0.1f;
            body.Friction = 5f;
            body.Position = new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y));
            body.IgnoreGravity = false;

            centerOffset = position.Y - (float)ConvertUnits.ToDisplayUnits(body.Position.Y); //remember the offset from the center for drawing


        }
        #endregion

        #region Load Content
        public void LoadContent()
        {
            //playerTexture = new SpriteClasses.PlayerSprite(content.Load<Texture2D>("Sprites/anim_sp"), getPosition, new Vector2(0, 0), 4, 1);
            playerTexture.setupAttack();
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;



            Vector2 velocity = new Vector2(0, 0); ;

            if (keyboardState.IsKeyDown(Keys.Right))
            {

                velocity += new Vector2(1, 0);
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {

                velocity -= new Vector2(1, 0);
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {

                velocity -= new Vector2(0, 1);
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {

                velocity += new Vector2(0, 1);
            }

            //0 no bounce 1 bounce set from 0.0 to 1.0
            body.Restitution = .2f;
            
            body.Mass = .2f;
            //slows the object when no force applied
            body.LinearDamping = .75f;
            
            body.ApplyForce(velocity);
            body.ApplyForce(new Vector2(.1f,.1f));
            body.UserData = "Player";

            getPosition = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
            float gravAngle = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

            playerTexture.Update(gameTime);
            playerTexture.position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
            playerTexture.rotation = gravAngle;

        }
        #endregion

        #region Draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerTexture.Draw(spriteBatch);
        }

        #endregion

    }
}
