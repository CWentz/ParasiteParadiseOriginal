using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame._Levels
{
    public class Level3 : LevelMain
    {
        private bool bloodSpawnSetup = false;
        public Level3(GraphicsDevice graphicDevice, String _levelGleedFile)
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
                                    case "BloodSpawner2":
                                        {
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .05f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = 20f;
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

            if (!nerve1On)
            {
                bloodSpawn1Open = false;
                synNerve1 = bloodSpawn1Pos;
            }

            if (synNerve1 == new Vector2(0, 0))
            {
                bloodSpawn1Open = true;
            }
            if (!nerve2On)
            {
                bloodSpawn2Open = false;
                synNerve2 = bloodSpawn2Pos;
            }

            if (synNerve2 == new Vector2(0, 0))
            {
                bloodSpawn2Open = true;
            }
            //bloodSpawn1Open = nerve1On;
            //bloodSpawn2Open = nerve2On;

            if (trigger1On && trigger2On)
            {
                gefecht1Open = true;
                
                
            }

            if (trigger1On)
            {
                synTrig1 = gefecht1Pos;
            }
            if (trigger2On)
            {
                synTrig2 = gefecht1Pos;
            }

            //if(synTrig1 == new Vector2(0,0))
            //    gefecht1Open = false;


            
            if (trigger3On)// && synTrig3 == new Vector2(0,0))
            {
                gefecht4Open = gefecht3Open = gefecht2Open = trigger3On;
                synTrig3 = gefecht3Pos;
            }
        }
    }
}
