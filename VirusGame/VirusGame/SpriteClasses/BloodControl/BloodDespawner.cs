using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses.BloodControl
{
    public class BloodDespawner : MovingSprite
    {
        public FixedRevoluteJoint top;
        public FixedRevoluteJoint bottom;
        private float scale;
        public String nameForLevel;
        private bool testCol;


        /// <summary>
        /// despawns the bloodcells when they have contact with the body.
        /// </summary>
        /// <param name="_level">farseer world</param>
        /// <param name="_texture">texture</param>
        /// <param name="_position">center position</param>
        /// <param name="_velocity">n/a</param>
        /// <param name="_frames">animation frames</param>
        /// <param name="_animations">animation rows</param>
        /// <param name="_rotation">rotation radians</param>
        /// <param name="_scale">display scale</param>
        public BloodDespawner(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, float _rotation, float _scale)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Depth = .074f;
            depth = .074f;
            animation.Scale = .70f * _scale;
            scale = _scale;
            animation.IsLooping = false;
            aniM.AddAnimation("nothing", 1, _frames, animation.Copy());
            
            aniM.FramesPerSecond = 1;
            aniM.Animation = "nothing";
            firstRun = true;
            body.Dispose();


            body = BodyFactory.CreateRectangle(level, ConvertUnits.ToSimUnits(40), ConvertUnits.ToSimUnits(160 * scale * Globals.GlobalScale), 100f);
            body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            body.BodyType = BodyType.Dynamic;
            
            body.Enabled = true;
            body.Rotation = _rotation;
            rotation = _rotation;
            body.Mass = 10f;
            body.FixedRotation = false;
            body.Restitution = .25f;
            body.IgnoreGravity = true;

            body.CollidesWith = ~Category.All | Category.Cat3 | Category.Cat2;

            position = _position + ((new Vector2((float)Math.Cos(rotation - (float)MathHelper.PiOver2), (float)Math.Sin(rotation - (float)MathHelper.PiOver2))) * (45f * scale * Globals.GlobalScale));
            top = JointFactory.CreateFixedRevoluteJoint(
                level,
                body,
                new Vector2((float)ConvertUnits.ToSimUnits(0),
                    (float)ConvertUnits.ToSimUnits(-45  * scale * Globals.GlobalScale)),
                new Vector2((float)ConvertUnits.ToSimUnits(position.X),
                    (float)ConvertUnits.ToSimUnits(position.Y)));

            position = _position + ((new Vector2((float)Math.Cos(rotation + (float)MathHelper.PiOver2), (float)Math.Sin(rotation + (float)MathHelper.PiOver2))) * (45f * scale * Globals.GlobalScale));
            bottom = JointFactory.CreateFixedRevoluteJoint(
                level,
                body,
                new Vector2((float)ConvertUnits.ToSimUnits(0),
                    (float)ConvertUnits.ToSimUnits(45 * scale * Globals.GlobalScale)),
                new Vector2((float)ConvertUnits.ToSimUnits(position.X),
                    (float)ConvertUnits.ToSimUnits(position.Y)));

            Type = "BloodCellDespawner";
            body.FixtureList[0].UserData = "BloodCellDespawner";
            body.IsSensor = true;

        }


        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (testCollision)
            {
                body.OnCollision += OnCollision;
            }
            testCollision = !testCollision;

            Vector2 bodposition = position + ((new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))) * ((40f * scale * Globals.GlobalScale) * Globals.GlobalScale));

            //body.Position = body.Position;

            body.Position = Globals.getWorldPosition(bodposition);
            //position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            this.aniM.Update(gameTime);


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //aniM.Draw(spriteBatch, texture, (position - new Vector2(aniM.Width, aniM.Height)) - ((new Vector2((float)Math.Cos(rotation - (float)Math.PI - .1f), (float)Math.Sin(rotation - (float)Math.PI - .1f))) * ((50f * scale * Globals.GlobalScale) * Globals.GlobalScale))
            //    , rotation + (float)Math.PI - .1f);
            spriteBatch.Draw(SpriteClasses.SpriteManager.Content.Load<Texture2D>("Test/lower"), position, null, Color.White, rotation - .1f, new Vector2(153 / scale, 181), scale * .35f * Globals.GlobalScale, SpriteEffects.None, depth + .001f);
            spriteBatch.Draw(SpriteClasses.SpriteManager.Content.Load<Texture2D>("Test/upper"), position, null, Color.White, rotation - .1f, new Vector2(153 / scale, 181), scale * .35f * Globals.GlobalScale, SpriteEffects.None, depth - .001f);
        }



        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.UserData as string)
            {
                case ("Wall"):
                    return false;
                case ("bloodcell"):
                    return true;
                case ("flow"):
                    return false;
                case ("Player"):
                    return false;
                case ("Prop"):
                    return false;
                default:
                    return false;

            }
            
        }
    }
}
