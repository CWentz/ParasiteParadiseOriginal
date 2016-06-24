using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.Parallax
{
    public class Background : SpriteClasses.Parallax.BackgroundSprite
    {
        private Vector2 image1Pos;
        private Vector2 image2Pos;
        private Vector2 oldCenter;

        public Background(Texture2D _texture, float _speed) : base(_texture, _speed)
        {
            speed = _speed;
            texture = _texture;
            image1Pos = new Vector2(-texture.Width, -texture.Height / 2f);
            image2Pos = new Vector2(center.X, -texture.Height / 2f);
            limit = new Vector2(((texture.Height) - 2100f) / 2f, ((texture.Width) - 1600f) / 2f);
            maxPosition = new Vector2(500, 300);
            minPosition = new Vector2(-500, -300);
            Type = "Background";
            Depth = .9f;
            
        }

        public Texture2D setTexture
        {
            set { texture = value; }
            get { return texture;}
        }

        public override void Update(GameTime gameTime, Vector2 _positionChange)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float xMove = 0;
            float yMove = 0;

            center = new Vector2(0, 0);

            //if the position change isn't 0 it will get a direction
            //it divides by its absolute value to get(1 or -1).
            if (_positionChange.X != 0)
            {
                if (_positionChange.X > 0)
                    xMove = 1;
                else xMove = -1;
            }
            if (_positionChange.Y != 0)
            {
                if (_positionChange.Y > 0)
                    yMove = 1;
                else yMove = -1;
            }
            //while the center is within the bounds it will scroll
            if(center.Y < maxPosition.Y && center.Y > minPosition.Y)
                changedY += yMove * yDirection * speed * timeDelta;
            if(center.X < maxPosition.X && center.X > minPosition.X)
                changedX += xMove * xDirection * speed * timeDelta;

            if (center.X < maxPosition.X && center.X > minPosition.X && center.Y < maxPosition.Y && center.Y > minPosition.Y)
                center += (changedX + changedY);


            // if the center point goes beyond the bounds it will reset to the old position.
            if (center.X >= maxPosition.X || center.X <= minPosition.X)
                center.X = oldCenter.X;
            if (center.Y >= maxPosition.Y || center.Y <= minPosition.Y)
                center.Y = oldCenter.Y;

            

            image1Pos = new Vector2(-texture.Width + center.X, (-texture.Height / 2f) + center.Y);
            image2Pos = new Vector2(center.X, (-texture.Height / 2f) + center.Y);

            oldCenter = center;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, image1Pos, null, Color.White, rotation, new Vector2(0, 0), 1f, SpriteEffects.None, depth);
            spriteBatch.Draw(texture, image2Pos, null, Color.White, rotation, new Vector2(0, 0), 1f, SpriteEffects.None, depth);
            //spriteBatch.Draw(Globals.Pixel, new Rectangle((int)center.X, (int)center.Y, 40,40), Color.White);
            //spriteBatch.Draw(texture, image2Pos, Color.White);
        }
    }
}
