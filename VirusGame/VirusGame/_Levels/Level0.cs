using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VirusGame._Levels
{
    public class Level0 : LevelMain
    {
        private bool bloodSpawnSetup = false;

        Texture2D toolTipBG;
        SpriteFont toolTipFont;
        Texture2D enterButton;
        Texture2D arrowKeys;
        
        
        Texture2D arrow;

        Texture2D spacebar;


        KeyboardState key;
        KeyboardState oldKey;
        GamePadState controller;
        GamePadState oldController;

        public Tooltip toolTip;

        Controls playerController = new Controls();

        Camera.Camera2D cam;

        public bool doTutorial = true;
        private int tutorialStep = 0;

        public Level0(GraphicsDevice graphicDevice, String _levelGleedFile)
            : base(graphicDevice, _levelGleedFile)
        {
            if (doTutorial)
                autoPanningBack = false;

            toolTipBG = Content.Load<Texture2D>("UI/Tutorial/background");
            toolTipFont = Content.Load<SpriteFont>("TutorialFont");
            enterButton = SpriteClasses.SpriteManager.getImage("UI/Tutorial/enter");
            arrowKeys = SpriteClasses.SpriteManager.getImage("UI/Tutorial/pfeiltasten");
            arrow = SpriteClasses.SpriteManager.getImage("UI/Tutorial/arrow");
            spacebar = SpriteClasses.SpriteManager.getImage("UI/Tutorial/spacebar");
             
            
            toolTip = new Tooltip(tutorialStep, toolTipBG, toolTipFont, enterButton, arrowKeys, arrow, spacebar);

            playerController.UpdateControls(key, oldKey, controller, oldController);

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);

            key = Keyboard.GetState();
            controller = GamePad.GetState(PlayerIndex.One);

            

            if (doTutorial)
            {
                toolTip.Update(playerController, player);

                switch (tutorialStep)
                {
                    case 0: // Steuerung erklären
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 1: // Wände machen Aua
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 2: // Blutstrom stoppen
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (!nerve1On && toolTip.toolTipStep == 1)
                                toolTip.toolTipStep++;

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 3: // Geflecht
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0)
                                cameraDestination = new Vector2(40, 400);
                            if (toolTip.toolTipStep == 1)
                                cameraDestination = player.position;

                            if (trigger1On && trigger2On && toolTip.toolTipStep == 2)
                                toolTip.toolTipStep++;

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 4: // Monozyt
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0)
                                cameraDestination = new Vector2(-100, 400);

                            if (toolTip.toolTipStep == 1)
                                cameraDestination = player.position;

                            if (toolTip.toolTipStep == 2)
                            {
                                #region MonozytCheck
                                foreach (SpriteClasses.MovingSprite ms in allCells)
                                {
                                    switch (ms.Type)
                                    {
                                        case "Monozyt":
                                            {
                                                if (!ms.IsVisible)
                                                    toolTip.toolTipStep++;
                                            }
                                            break;
                                        default:
                                            break;


                                    }
                                }
                                #endregion
                            }

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 5: // Collectibles
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0)
                                cameraDestination = new Vector2(-150, 500);

                            if (toolTip.toolTipStep == 1)
                                cameraDestination = player.position;

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 6: // Plasmacell
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0)
                                cameraDestination = new Vector2(-550, 100);

                            if (toolTip.toolTipStep == 1)
                                cameraDestination = player.position;

                            if (toolTip.toolTipStep == 2)
                            {
                                #region PlasmaCheck
                                foreach (SpriteClasses.MovingSprite ms in allCells)
                                {
                                    switch (ms.Type)
                                    {
                                        case "Plasma":
                                            {
                                                if (!ms.IsVisible)
                                                    toolTip.toolTipStep++;
                                            }
                                            break;
                                        default:
                                            break;


                                    }
                                }
                                #endregion
                            }

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 7: // Bloodstream
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0 && !toolTip.nextStep)
                            {
                                if (Vector2.Distance(new Vector2(-800, -400), player.position) <= 200)
                                    toolTip.toolTipStep = 1;
                            }
                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 8: // Helpercell
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0)
                                cameraDestination = new Vector2(0, -500);

                            if (toolTip.toolTipStep == 1)
                                cameraDestination = player.position;

                            if (toolTip.toolTipStep == 2)
                            {
                                #region HelpercellCheck
                                foreach (SpriteClasses.MovingSprite ms in allCells)
                                {
                                    switch (ms.Type)
                                    {
                                        case "Helper":
                                            {
                                                if (!ms.IsVisible)
                                                    toolTip.toolTipStep++;
                                            }
                                            break;
                                        default:
                                            break;


                                    }
                                }
                                #endregion
                            }

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;
                    case 9: // Makrophage
                        {
                            toolTip.tooltipID = tutorialStep;

                            if (toolTip.toolTipStep == 0)
                                cameraDestination = new Vector2(600, -400);

                            if (toolTip.toolTipStep == 1)
                                cameraDestination = player.position;

                            if (toolTip.nextStep)
                                tutorialStep++;
                            toolTip.nextStep = false;
                        }
                        break;

                    default: break;
                }
            }

            if (toolTip.panning)
                panning = true;
            else panning = false;



            #region Level-Setup
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

            if (!nerve1On)
            {
                bloodSpawn1Open = false;
                synNerve1 = bloodSpawn1Pos;
            }
            else
            {
                bloodSpawn1Open = true;
            }

            if (!nerve2On && !nerve3On)
            {
                bloodSpawn2Open = false;
            }
            else
            {
                bloodSpawn2Open = true;
            }

            if (!nerve2On)
            {
                synNerve2 = bloodSpawn2Pos;
            }
            if (!nerve3On)
            {
                synNerve3 = bloodSpawn2Pos;
            }

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

            if (trigger3On)// && synTrig3 == new Vector2(0,0))
            {
                gefecht3Open = gefecht2Open = trigger3On;
                synTrig3 = gefecht3Pos;
            }
            #endregion

            playerController.UpdateControls(key, oldKey, controller, oldController);

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)//, LineBatch lineBatch)
        {

            if (doTutorial)
            {
                toolTip.Draw(spriteBatch);
            }


            base.Draw(gameTime, spriteBatch); //, lineBatch);
        }

    }
}
