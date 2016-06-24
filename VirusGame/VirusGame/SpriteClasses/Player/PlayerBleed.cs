using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.Player
{
     public class PlayerBleed : MovingSprite
    {
         private int animationTimer = 0;
         private bool damaged = false;

         public PlayerBleed(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
             : base(_level, _texture, _position, _velocity, _frames, _animations)
         {
             animation.Scale = .4375f;
             animation.Depth = 0.059f;
             aniM.FramesPerSecond = 10;
             aniM.AddAnimation("bleed", 1, _frames, animation.Copy());
             aniM.Animation = "bleed";

             Type = "bleed";
             rotates = false;
             body.Dispose();
             IsVisible = false;
         }

         public bool Damaged
         {
             set { damaged = value; }
         }

         public override void Update(GameTime gameTime)
         {

             //body.Position = Globals.getWorldPosition(position);
             //body.Rotation = rotation;

             if (damaged)
             {
                 animationTimer++;
                 IsVisible = true;
                 if (animationTimer == 1)
                 {
                     aniM.Animation = "bleed";
                 }   
             }
             if (animationTimer >= 29)
             {
                 damaged = false;
                 animationTimer = 0;
                 IsVisible = false;
             }

             aniM.Update(gameTime);

         }

    }
}
