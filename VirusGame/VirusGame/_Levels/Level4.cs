using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame._Levels
{
    public class Level4 : LevelMain
    {
        //private bool spawnerSetup;
        private int bloodSpawnerCounter;


        public Level4(GraphicsDevice graphicDevice, String _levelGleedFile) :base(graphicDevice, _levelGleedFile)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {

            base.Update(gameTime, keyboardState);

            #region custom bloodspawners for level
            if (bloodSpawnerCounter < 3)
            {
                foreach (SpriteClasses.MovingSprite ms in allCells)
                {

                    switch (ms.Type)
                    {
                        case "BloodSpawner":
                            {
                                switch (((SpriteClasses.BloodControl.BloodSpawner)ms).nameForLevel)
                                {
                                    case "BloodSpawner3":
                                        {
                                            if (!((SpriteClasses.BloodControl.BloodSpawner)ms).wallCollide)
                                            {
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .15f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = .07f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).Timer = 400;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).wallCollide = true;
                                                bloodSpawnerCounter++;
                                            }
                                        }
                                        break;
                                    case "BloodSpawner4":
                                        {
                                            if (!((SpriteClasses.BloodControl.BloodSpawner)ms).wallCollide)
                                            {
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = 0.15f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = .07f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).Timer = 400;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).wallCollide = true;
                                                bloodSpawnerCounter++;
                                            }
                                        }
                                        break;
                                    case "BloodSpawner5":
                                        {
                                            if (!((SpriteClasses.BloodControl.BloodSpawner)ms).wallCollide)
                                            {
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .15f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = .07f;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).Timer = 400;
                                                ((SpriteClasses.BloodControl.BloodSpawner)ms).wallCollide = true;
                                                bloodSpawnerCounter++;
                                            }
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
                bloodSpawn1Open = nerve1On;
                synNerve1 = bloodSpawn1Pos;
            }
            else
            {
                bloodSpawn1Open = true;
            }
            //bloodSpawn1Open = nerve1On;
            if (!nerve2On)
            {
                bloodSpawn2Open = nerve2On;
                synNerve2 = bloodSpawn2Pos;
            }
            else
            {
                bloodSpawn2Open = true;
            }

            //bloodSpawn2Open = nerve2On;
            if (trigger1On && trigger4On)
            {
                gefecht1Open = true;
                synTrig1 = gefecht1Pos;
                synTrig4 = gefecht1Pos;
            }

            if(trigger1On)
                synTrig1 = gefecht1Pos;
            if(trigger4On)
                synTrig4 = gefecht1Pos;

            if (trigger2On)
            {
                gefecht4Open = true;
                synTrig2 = gefecht4Pos;
            }


            //gefecht1Open = trigger1On;
            if (trigger3On)
            {
                gefecht3Open = gefecht2Open = true;
                synTrig3 = gefecht2Pos;
            }

            
            //gefecht2Open = gefecht3Open = trigger3On;
            //gefecht4Open = trigger2On;

            bloodSpawn3Open = bloodSpawn4Open = bloodSpawn5Open = true;

        }
    }
}
