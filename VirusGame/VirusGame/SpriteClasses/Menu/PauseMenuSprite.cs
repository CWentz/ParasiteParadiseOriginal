using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.Menu
{
    public class PauseMenuSprite : MovingSprite
    {
        private int count = 0;

        public PauseMenuSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            texture = _texture;
            position = _position;
            IsVisible = false;
            body.CollidesWith = Category.All;
            animation.Scale = 1f;
            animation.Depth = 0f;
            aniM.FramesPerSecond = 12;
            aniM.AddAnimation("line1", 1, _frames, animation.Copy());
            aniM.AddAnimation("line2", 2, _frames, animation.Copy());
            aniM.AddAnimation("line3", 3, 1, animation.Copy());
            aniM.AddAnimation("play", 4, 1, animation.Copy());
            aniM.AddAnimation("exit", 5, 1, animation.Copy());
            aniM.Animation = "line1";
            Type = "PauseMenu";

            body.Dispose(); //level.RemoveBody(body);

        }

        public void Update(GameTime gameTime, int _selection)
        {

                if (count >35 && _selection == 1)
                    aniM.Animation = "play";
                if (count > 35 && _selection == 2)
                    aniM.Animation = "exit";


                switch (count)
                {
                    case 0:
                        {
                            aniM.Animation = "line1";
                        }
                        break;
                    case 14:
                        {
                            aniM.Animation = "line2";
                        }
                        break;
                    case 28:
                        {
                            aniM.Animation = "line3";
                        }
                        break;
                    case 34:
                        {
                            aniM.Animation = "play";
                        }
                        break;
                    default:
                        break;
                }
                count++;


                aniM.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            aniM.Draw(spriteBatch, 
                texture, 
                new Vector2(1024 /2f, 768 /2f), // position.X+(aniM.Width/2f), position.Y + (aniM.Height/2f)), 
                this.rotation);
        }
    }
}
