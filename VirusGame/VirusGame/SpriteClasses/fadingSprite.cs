using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses
{
    public class fadingSprite : MovingSprite
    {

        private Color overlay;
        public byte alphaByte = 0;
        public bool active = false;
        private Texture2D fader;
        //private int countdown = 51;

        public fadingSprite(World _level, Texture2D _texture, Texture2D _faderTexture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
        : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            fader = _faderTexture;
            overlay = new Color(0, 0, 0, alphaByte);
            animation.Scale = 1.4f;
            animation.Depth = 0f;
            aniM.FramesPerSecond = 8;
            animation.IsLooping = true;
            aniM.AddAnimation("active1", 1, _frames, animation.Copy());
            aniM.AddAnimation("active2", 2, _frames, animation.Copy());

            aniM.AddAnimation("active3", 3, _frames, animation.Copy());
            aniM.AddAnimation("active4", 4, _frames, animation.Copy());

            aniM.Animation = "active1";
            
            Type = "Fading";
            rotates = false;
            body.CollidesWith = ~Category.All;
            IsVisible = false;

            Depth = 0f;
            body.Dispose();
            
        }

        public override void Update(GameTime gameTime)
        {
            //alphaByte is ment to reset past 255 currently
            overlay = new Color(255, 255, 255, alphaByte);
            //if (active)
            if (IsVisible)
            {
                if (alphaByte == 0)
                    aniM.Animation = "active1";

                if (alphaByte == 60)
                    aniM.Animation = "active2";

                if (alphaByte == 120)
                    aniM.Animation = "active3";

                if (alphaByte == 180)
                    aniM.Animation = "active4";

                //if (alphaByte == 248)
                //    aniM.Animation = "active4";

                //if (alphaByte == 255)
                //    alphaByte = 0;

                alphaByte += 2;
                //if (!active)
                //    alphaByte = 0;

                
                aniM.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                aniM.Draw(spriteBatch, texture, Globals.CameraPosition, this.rotation);
                //spriteBatch.Draw(fader, position,null, overlay, 0f, new Vector2(fader.Width/2, fader.Height/2), 4f * Globals.GlobalScale, SpriteEffects.None, depth); 
                
            }
        }

    }
}
