using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.SpriteClasses.Valves
{
    public class Valve
    {
        public Claps left;
        public Claps right;
        public float rotation;
        public Vector2 position;
        public bool open = true;
        private float leftValveRotation;
        private float rightValveRotation;
        public String nameForLevel;
        private float scale;

        public Valve(Vector2 _position, float _rotation, int _distance, float _scale)
        {
            scale = _scale;
            position = _position;
            rotation = _rotation;
            leftValveRotation = rotation;
            rightValveRotation = rotation + (float)MathHelper.Pi; 
            
            position = _position + ((new Vector2((float)Math.Cos(rotation - (float)MathHelper.PiOver2), (float)Math.Sin(rotation - (float)MathHelper.PiOver2))) * (float)_distance);
            left = SpriteManager.addClap(position, "left", leftValveRotation, _scale + (.3f * _scale));

            position = _position + ((new Vector2((float)Math.Cos(rotation + (float)MathHelper.PiOver2), (float)Math.Sin(rotation + (float)MathHelper.PiOver2))) * (float)_distance);
            right = SpriteManager.addClap(position, "right", rightValveRotation, _scale + (.3f * _scale));

            left.body.IgnoreCollisionWith(right.body);
            right.body.IgnoreCollisionWith(left.body);

            
        }

        public void Update(GameTime gameTime)
        {
            if (open)
            {
                left.body.CollisionCategories = Category.Cat2 & ~Category.Cat15;
                right.body.CollisionCategories = Category.Cat2 & ~Category.Cat15;
                
                left.speed =  MathHelper.Pi/2f;
                right.speed = -MathHelper.Pi/2f;

                //if (right.body.Rotation <= rightValveRotation)
                //    right.body.Rotation = rightValveRotation;
                //if (left.body.Rotation >= leftValveRotation)
                //    left.body.Rotation = leftValveRotation;

                if(right.body.Rotation <= rightValveRotation)
                    right.body.Rotation = rightValveRotation;
                if (left.body.Rotation >= leftValveRotation)
                    left.body.Rotation = leftValveRotation;
            }
            if (!open)
            {
                //
                

                left.body.CollisionCategories = Category.Cat3 & ~Category.Cat15;
                right.body.CollisionCategories = Category.Cat3 & ~Category.Cat15;

                //
                //left.body.CollisionCategories = Category.Cat3 & ~Category.Cat15;
                //right.body.CollisionCategories = Category.Cat3 & ~Category.Cat15;

                left.speed = -MathHelper.Pi/2f;
                right.speed = MathHelper.Pi/2f;

                if (right.body.Rotation <= rightValveRotation - MathHelper.Pi / 2.5f)
                {
                    right.body.Rotation = rightValveRotation - MathHelper.Pi / 2.5f;
                }
                if (left.body.Rotation >= leftValveRotation + MathHelper.Pi / 2.5f)
                {
                    left.body.Rotation = leftValveRotation + MathHelper.Pi / 2.5f;
                }
                
                
            }
            left.Update(gameTime);
            right.Update(gameTime);
            left.body.IgnoreCollisionWith(right.body);
            right.body.IgnoreCollisionWith(left.body);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            left.Draw(spriteBatch);
            right.Draw(spriteBatch);
        }
    }
}
