using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VirusGame.UI
{
    class Tips
    {
        
        string[] tipStrings = new string[12] {
        "You lose a life when you hit the walls! Be carefull!",
        "Makrophages will hunt you down and kill you instantly. They are the only enemies who can kill you. Steer clear of them!",
        "Plasma cells shoot antibodies at you. Blinding you when they strike.",
        "Helper cells explode. Catapulting you away when you're near them. Don't get too close.",
        "Monocytes grab you and slow you down. Kill them quick. Be fast to stay fast.",
        "There are two types of nerves for different types of barriers.",
        "Sometimes you'll need more than one nerve to open a barrier.",
        "Some triggers must be activated before you can use them.",
        "Bloodstreams can be turned on and off. Maybe that will be useful...",
        "Once a plexus is opened, it will stay open. Better think twice!",
        "You can see your current life points on your back. Pay attention to the lights!",
        "Collectibles can restore lost life points. You also get points for them, when you have full energy."
        };
        int RndNr;

        public Tips()
        {
            Random Rnd = new Random();
            RndNr = Rnd.Next(0, (tipStrings.Count() - 1));
        }

        public void drawTip(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            spriteBatch.DrawString(font, "Tip: " + tipStrings[RndNr], position, Color.White, 0f, new Vector2(font.MeasureString("Tip: " + tipStrings[RndNr]).X / 2, font.MeasureString("Tip: " + tipStrings[RndNr]).Y / 2), 1f, SpriteEffects.None, 0f);
        }

    }

}
