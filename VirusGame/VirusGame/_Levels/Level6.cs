using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame._Levels
{
    public class Level6 : LevelMain
    {
        private bool bloodSpawnSetup = false;
        public Level6(GraphicsDevice graphicDevice, String _levelGleedFile)
            : base(graphicDevice, _levelGleedFile)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);

            #region bloodspawner unique for level setup

            foreach (SpriteClasses.MovingSprite ms in allCells)
            {
                if (!bloodSpawnSetup)
                {
                    switch (ms.Type)
                    {
                        case "BloodSpawner":
                            {
                                switch (((SpriteClasses.BloodControl.BloodSpawner)ms).nameForLevel)
                                {
                                    case "BloodSpawner1":
                                        {
                                            ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .05f;
                                            ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = 30f;
                                            bloodSpawnSetup = true;
                                        }
                                        break;
                                    default:
                                        break;

                                }

                            }
                            break;
                        default:
                            break;


                    }
                }
            }

            #endregion
            //t1 t2 g2 / t3 g1

            if (!nerve1On)
                synNerve1 = bloodSpawn1Pos;
            if (!nerve2On)
                synNerve2 = bloodSpawn1Pos;

            if (!nerve2On && !nerve1On)
            {
                bloodSpawn1Open = false;

            }
            else
            {
                //synNerve2 = synNerve3 = bloodSpawn2Pos;
                bloodSpawn1Open = true;
            }

            if (trigger3On || trigger4On)
            {
                gefecht1Open = true;
            }
            else
            {
                gefecht1Open = false;
            }

            if (trigger2On && trigger1On)
            {
                //synTrig2 = gefecht2Pos;
                gefecht2Open = true;
            }
            else
            {
                gefecht2Open = false;
            }

            if (trigger2On)
                synTrig2 = gefecht2Pos;
            if (trigger1On)
                synTrig1 = gefecht2Pos;
            if (trigger3On)
                synTrig3 = gefecht1Pos;
            if (trigger4On)
                synTrig4 = gefecht1Pos;

            ////if (nerve3On)
            //    synNerve3 = bloodSpawn2Pos;


        }

    }
}
