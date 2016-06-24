using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame._Levels
{
    public class Level5 : LevelMain
    {

        public Level5(GraphicsDevice graphicDevice, String _levelGleedFile)
            : base(graphicDevice, _levelGleedFile)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);
            //
            if (!nerve1On)
            {
                bloodSpawn1Open = nerve1On;
                synNerve1 = bloodSpawn1Pos;
            }
            else
            {
                bloodSpawn1Open = true;
            }
            //
            if (!nerve2On)
            {
                synNerve2 = bloodSpawn2Pos;
                bloodSpawn2Open = nerve2On;
            }
            else
            {
                bloodSpawn2Open = true;
            }

            //
            if (trigger1On)
            {
                gefecht1Open = true;
                synTrig1 = gefecht1Pos;
            }
            else
            {
                gefecht1Open = false;
            }

            //
            if (trigger2On)
            {
                gefecht2Open = true;
                synTrig2 = gefecht2Pos;
            }
            else
            {
                gefecht2Open = false;
            }
            //
            if (trigger3On)
            {
                synTrig3 = gefecht3Pos;
                gefecht3Open = true;
            }
            else
            {
                gefecht3Open = false;
            }
            

        }
    }
}
