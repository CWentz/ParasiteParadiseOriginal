using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VirusGame.UI
{
    class ScoreBoard_Percent
    {
        float alphaFloat = 1.0f;
        Vector2 startPosition = new Vector2(425, 650);
        string text;
        int timer = 60;
        bool updated = false;
        bool highScore = false;
        bool dead = false;

        public ScoreBoard_Percent(Vector2 start, string txt)
        {
            startPosition = startPosition - start;
            text = txt;
        }

        public void Update(bool check = false, bool _highScore = false, bool _dead = false)
        {
            highScore = _highScore;
            dead = _dead;

            if (!dead)
            {
                if (check)
                    timer = 0;

                if (timer > 0)
                    timer--;

                if (timer == 0)
                {
                    alphaFloat -= 0.01f;

                    updated = true;
                    startPosition += new Vector2(0.4f, -1f);
                }
            }
            else { startPosition = new Vector2(180, 30); alphaFloat = 1f; updated = true;  }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if(updated)
                if(highScore)
                    if(dead)
                        spriteBatch.DrawString(font, text, startPosition, Color.Red * alphaFloat, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                    else
                        spriteBatch.DrawString(font, text, startPosition, Color.Green * alphaFloat, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                else
                    spriteBatch.DrawString(font, text, startPosition, Color.White * alphaFloat, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
        }
    }
}
