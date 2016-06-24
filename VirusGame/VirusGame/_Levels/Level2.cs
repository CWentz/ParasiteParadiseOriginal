using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame._Levels
{
    public class Level2 : LevelMain
    {

        public Level2(GraphicsDevice graphicDevice, String _levelGleedFile)
            : base(graphicDevice, _levelGleedFile)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);

            if (!nerve1On)
            {
                bloodSpawn1Open = nerve1On;
                synNerve1 = bloodSpawn1Pos;
            }
            else
            {
                bloodSpawn1Open = true;
            }

            if (!nerve2On)
                synNerve2 = bloodSpawn2Pos;
            if (!nerve3On)
                synNerve3 = bloodSpawn2Pos;

            if (nerve2On || nerve3On)
            {
                bloodSpawn2Open = true;
                
            }
            else
            {
                //synNerve2 = synNerve3 = bloodSpawn2Pos;
                bloodSpawn2Open = false;
            }


            if (trigger2On && trigger1On || trigger3On)
            {
                //synTrig2 = synTrig1 = synTrig3 = gefecht1Pos;
                gefecht1Open = true;
            }
            else
            {
                gefecht1Open = false;
            }

            if (trigger2On)
                synTrig2 = gefecht1Pos;
            if (trigger1On)
                synTrig1 = gefecht1Pos;
            if (trigger3On)
                synTrig3 = gefecht1Pos;
            //if (nerve3On)
            //    synNerve3 = bloodSpawn2Pos;


        }
    }
}
