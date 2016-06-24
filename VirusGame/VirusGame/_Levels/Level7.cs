using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame._Levels
{
    public class Level7 : LevelMain
    {
        private bool nerve1attacked;
        private bool nerve2attacked;
        private bool nerve3attacked;
        private bool switchBG;
        private bool screamReset;
        private bool screamedAgain;
        private bool brainSetup = false;
        protected SpriteClasses.NPCSprites.BrainSprite brains;

        public Level7(GraphicsDevice graphicDevice, String _levelGleedFile)
            : base(graphicDevice, _levelGleedFile)
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);

            if (!brainSetup)
            {
                lastLevel = true;
                brains = SpriteClasses.SpriteManager.addBrain(finish.position);
                brainSetup = true;
            }



            if (!switchBG)
            {
                paralaxBG.setTexture = SpriteClasses.SpriteManager.getImage("Levels/Brainbackground");
                switchBG = true;
            }

            #region bloodspawner unique for level setup

            foreach (SpriteClasses.MovingSprite ms in allCells)
            {

                switch (ms.Type)
                {
                    case "BloodSpawner":
                        {
                            //if (bloodsetupcount < 3)
                            //{
                            //    switch (((SpriteClasses.BloodControl.BloodSpawner)ms).nameForLevel)
                            //    {
                            //        case "BloodSpawner1":
                            //            {
                            //                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .05f;
                            //                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = 30f;
                            //                bloodsetupcount++;
                            //            }
                            //            break;
                            //        case "BloodSpawner2":
                            //            {
                            //                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .05f;
                            //                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = 30f;
                            //                bloodsetupcount++;
                            //            }
                            //            break;
                            //        case "BloodSpawner3":
                            //            {
                            //                ((SpriteClasses.BloodControl.BloodSpawner)ms).SpawnTimer = .05f;
                            //                ((SpriteClasses.BloodControl.BloodSpawner)ms).CellMass = 30f;
                            //                bloodsetupcount++;
                            //            }
                            //            break;
                            //        default:
                            //            break;

                            //    }
                            //}

                        }
                        break;
                    case "Makrophage":
                        {
                            #region makrophage
                            ((SpriteClasses.NPCSprites.MakrophageCell)ms).chasedistance = 700;
                            if (screamReset && !screamedAgain)
                            {
                                ((SpriteClasses.NPCSprites.MakrophageCell)ms).screamPlayed = false;
                                screamedAgain = true;
                            }
                            #endregion
                        }
                        break;
                    case "Nerve":
                        {
                            #region nerves
                            switch (((SpriteClasses.NPCSprites.NerveCell)ms).nameForLevel)
                            {
                                case "Nerve1":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).switchable = true;
                                        nerve1attacked = ((SpriteClasses.NPCSprites.NerveCell)ms).attacked;
                                        //nerve3On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                        if (nerve2attacked)
                                        {
                                            ((SpriteClasses.NPCSprites.NerveCell)ms).UpdateSwitch();
                                            nerve2attacked = false;
                                        }
                                    }
                                    break;
                                case "Nerve2":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).switchable = true;
                                        nerve2attacked = ((SpriteClasses.NPCSprites.NerveCell)ms).attacked;
                                        //nerve4On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve3":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).switchable = true;
                                        nerve3attacked = ((SpriteClasses.NPCSprites.NerveCell)ms).attacked;
                                        //nerve5On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                        if (nerve1attacked)
                                        {
                                            ((SpriteClasses.NPCSprites.NerveCell)ms).UpdateSwitch();
                                            nerve1attacked = false;
                                        }
                                    }
                                    break;
                                default:
                                    break;

                            }
                            #endregion
                        }
                        break;
                }
            }

            #endregion


            if (nerve1On)
            {
                bloodSpawn1Open = true;
            }
            else
            {
                bloodSpawn1Open = false;
            }

            if (nerve2On)
            {
                bloodSpawn2Open = true;
            }
            else
            {
                bloodSpawn2Open = false;
            }

            if (nerve3On)
            {
                bloodSpawn3Open = true;
            }
            else
            {
                bloodSpawn3Open = false;
            }

            if (nerve4On)
            {
                bloodSpawn4Open = true;
            }
            else
            {
                bloodSpawn4Open = false;
            }


            ///////////////////


            #region trigger
            if (trigger1On)
            {
                gefecht1Open = true;
                gefecht6Open = true;
            }
            else
            {
                gefecht1Open = false;
                gefecht6Open = false;
            }

            if (trigger2On)
            {
                gefecht2Open = true;
                gefecht3Open = true;
                screamReset = true;
            }
            else
            {
                gefecht2Open = false;
                gefecht3Open = false;
            }

            if (trigger3On)
            {
                gefecht4Open = true;
            }
            else
            {
                gefecht4Open = false;
            }


            if (trigger4On)
            {
                gefecht5Open = true;
            }
            else
            {
                gefecht5Open = false;
            }


            if (trigger5On)
            {
                gefecht8Open = true;
            }
            else
            {
                gefecht8Open = false;
            }
            if (trigger6On)
            {
                gefecht7Open = true;
            }
            else
            {
                gefecht7Open = false;
            }

            #endregion

            if (trigger2On)
                synTrig2 = gefecht3Pos;
            if (trigger1On)
                synTrig1 = gefecht1Pos;
            if (trigger3On)
                synTrig3 = gefecht4Pos;
            if (trigger4On)
                synTrig4 = gefecht5Pos;
            if (trigger5On)
                synTrig5 = gefecht8Pos;
            if (trigger6On)
                synTrig6 = gefecht7Pos;

            brains.Update(gameTime);
            if (brains.timer > 140)
            {
                levelDone = true;
                brainKilled = true;
            }
                
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (brainSetup)
            {
                brains.Draw(spriteBatch);
            }
            base.Draw(gameTime, spriteBatch);
        }
    }

    
}
