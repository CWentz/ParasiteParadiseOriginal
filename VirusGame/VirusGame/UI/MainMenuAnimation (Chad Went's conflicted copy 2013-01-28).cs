using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.UI
{
    public class MainMenuAnimation : VirusGame.SpriteClasses.MovingSprite
    {

        private byte alphaByte = 0;
        public bool active = false;
        public bool permanentlyDisable = false;
        private int activeCount;

        public MainMenuAnimation(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
        : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            position = new Vector2(512, 384);
            animation.Scale = 1f;
            animation.Depth = .00f;
            aniM.FramesPerSecond = 10;
            animation.IsLooping = true;
            aniM.AddAnimation("active", 1, _frames, animation.Copy());
            aniM.Animation = "active";
            Type = "loadscreen";
            rotates = false;
            body.CollidesWith = ~Category.All;
            active = true;
            IsVisible = false;
            body.Dispose();
            
        }

        public override void Update(GameTime gameTime)
        {

            //position = Globals.CameraPosition;
            //activeCount--;




            aniM.Update(gameTime);

        }

    }
}
