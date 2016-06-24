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
using Microsoft.Xna.Framework.Storage;

namespace VirusGame
{
    public class Tooltip
    {
        string updateString = "";

        public bool panning = false;
        public int tooltipID = 0;
        Texture2D enterButton;
        Texture2D background;
        SpriteFont font;
        Texture2D arrowKeys;
        Texture2D arrow;
        Texture2D spacebar;

        Vector2 arrowPosition;
        //added
        Texture2D start;
        Texture2D back;
        Texture2D wASDKeys;
        Texture2D dPad;
        Texture2D leftThumb;
        Texture2D abutton;

        private bool controllerConnected;
        public bool nextStep = false;
        public bool showTooltip = false;

        bool canSkip = false;
        public int toolTipStep = 0;
        string text = "";


        public Tooltip(int _tooltipID, Texture2D _background, SpriteFont _font, Texture2D _enterButton, Texture2D _arrowKeys, Texture2D _arrow, Texture2D _spacebar)
        {

            tooltipID = _tooltipID;
            background = _background;
            font = _font;
            enterButton = _enterButton;
            arrowKeys = _arrowKeys;
            arrow = _arrow;
            spacebar = _spacebar;
            start = SpriteClasses.SpriteManager.getImage("UI/Tutorial/start");
            back = SpriteClasses.SpriteManager.getImage("UI/Tutorial/back");
            wASDKeys = SpriteClasses.SpriteManager.getImage("UI/Tutorial/wasd");
            dPad = SpriteClasses.SpriteManager.getImage("UI/Tutorial/dpad");
            leftThumb = SpriteClasses.SpriteManager.getImage("UI/Tutorial/leftthumbstick");
            abutton = SpriteClasses.SpriteManager.getImage("UI/Tutorial/abutton");
            
        }

        public void Update(Controls key, SpriteClasses.PlayerSprite player)
        {
            controllerConnected = key.controlsConnected;
            
            switch (tooltipID)
            {
                case 0:
                    {
                        showTooltip = true;
                        canSkip = true;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;

                            panning = true;
                            text = "Welcome to Parasite Paradise\n";
                            text += "In this Tutorial We will teach you what you need to know to survive.\n";
                            text += "Lets begin, shall we?";
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.moveDown() || key.moveUp() || key.moveLeft() || key.moveRight())
                                toolTipStep++;

                            canSkip = false;
                            panning = false;
                            text = "To move yourself around use the keyboard or controller.\n";
                            text += "-Keyboard: Arrow Keys or WASD \n-Controller: Left Thumbstick or D-Pad.";

                            
                        }
                        else if (toolTipStep == 2)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            canSkip = true;
                            text = "Great! Lets continue on...";
                        }

                    }
                    break;

                case 1:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (player.Lives == 4 && toolTipStep == 0)
                            toolTipStep = 1;

                        if (player.Lives == 3 && toolTipStep == 1)
                            toolTipStep = 2;

                        if (toolTipStep == 0)
                        {
                            text = "What you need to know is, that when you hit the wall,\n";
                            text += "you will loose a life. Try it out, pay attention to your character!";
                        }
                        else if (toolTipStep == 1)
                        {
							toolTipStep = 2;  // who wants to hit the wall again anyways?
                            text = "Wait! Did you see that? Try it again...";
                        }
                        else if (toolTipStep == 2)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            text = "Yeah, you only have four lives left now. Sorry about that.";
                        }
                        else if (toolTipStep == 3)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            panning = true;
                            canSkip = true;
                            text = "You can see your current lives on the back of the character.\n";
                            text += "Each light is one life point. Pay attention so you do not die.";
                        }
                    }
                    break;

                case 2:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            panning = true;
                            canSkip = true;
                            text = "Ohh, the blood stream is blocking our path.\n";
                            text += "You are smart, it will not be a problem for you...";
                            arrowPosition = new Vector2(640, 430);
                        }
                        else if (toolTipStep == 1)
                        {
                            if (updateString == "Nerve1")
                                toolTipStep++;
                            text = "Attack this nerve to close this blood valve. To attack use:\n";
                            text += "-Keyboard: SPACE\n-Controller: A";
                        }
                        else if (toolTipStep == 2)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            canSkip = true;
                            text = "Good Job! Your path is now clear!";
                        }
                    }
                    break;
                case 3:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            panning = true;
                            canSkip = true;
                            text = "Sometimes your path will be blocked by a plexus.";
                            arrowPosition = new Vector2(640, 430);
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;
                            panning = true;
                            canSkip = true;
                            text = "Try to find a way to open the plexus!";
                        }
                        else if (toolTipStep == 2)
                        {
                            panning = false;
                            canSkip = false;
                            showTooltip = false;
                        }
                        else if (toolTipStep == 3)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            panning = true;
                            canSkip = true;
                            text = "Awesome! You are really good at this! Lets continue on...!";
                        }
                    }
                    break;
                case 4:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            panning = true;
                            canSkip = true;
                            text = "Monocytes will grab you and slow you down.\n";
                            text += "Try to kill them before they grab you.\n You can also kill them after, but you get less points then.";
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;
                            panning = true;
                            canSkip = true;
                            text = "Kill the monocytes quick!";
                        }
                        else if (toolTipStep == 2)
                        {
                            panning = false;
                            canSkip = false;
                            showTooltip = false;
                        }
                        else if (toolTipStep == 3)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            panning = false;
                            canSkip = true;
                            text = "Good job! The monocytes are gone!\n";
                        }

                    }
                    break;
                case 5:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Collectibles will restore lost lives. If you are full, you get bonus points.";
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Collect a few! watch your lost lives get restored.";
                        }
                        else if (toolTipStep == 2)
                        {
                            showTooltip = false;
                            canSkip = false;
                            panning = false;
                            if (player.Restored > 1 || player.Collected > 1)
                                toolTipStep++;
                        }
                        else if (toolTipStep == 3)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            canSkip = true;
                            text = "Great. You are at full health again!";
                        }
                    }
                    break;
                case 6:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Plasmacells will shoot antigens at you. Watch out!\n";
                            text += "Each time you are hit, you will become more blind.\nYou can also attack and destroy the antigens.";
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Quick, try to kill it before it blinds you!";
                        }
                        else if (toolTipStep == 2)
                        {
                            showTooltip = false;
                            canSkip = false;
                            panning = false;
                        }
                        else if (toolTipStep == 3)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            canSkip = true;
                            text = "Alright. Lets continue on with this level!\nLets see how fast you can go, give it some gas!";
                        }
                    }
                    break;
                case 7:
                    {
                        showTooltip = false;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;

                            canSkip = true;
                            showTooltip = true;
                            panning = true;
                            text = "You probably noticed that there is a blood stream blocking your path.\n";
                            text += "That flow is way too strong, it will just throw you away.";
                        }
                        if (toolTipStep == 2)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            canSkip = true;
                            showTooltip = true;
                            panning = true;
                            text = "Lets see whats farther down the road...\n";
                        }
                    }
                    break;
                case 8:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Helper cells, BLAST IT ALL! They explode. When you get close they arm!\n";
                            text += "Once armed try to get away! They will explode and catault you away.\nNOTE: points for evading explosions.";
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Try and sneak by these infernal cells...";
                        }
                        else if (toolTipStep == 2)
                        {
                            showTooltip = false;
                            canSkip = false;
                            panning = false;
                        }
                        else if (toolTipStep == 3)
                        {
                            if (key.accept())
                            {
                                nextStep = true;
                                toolTipStep = 0;
                            }
                            canSkip = true;
                            text = "That was fun, was it not?! Now lets finish this.";
                        }
                    }
                    break;
                case 9:
                    {
                        showTooltip = true;
                        canSkip = false;
                        panning = false;

                        if (toolTipStep == 0)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "WATCH OUT! That is a macrophage the most dangerous of all!\n Heed my words, remember that horrible scream.\nMacrophages are slow but they will kill you if they catch you.";
                        }
                        else if (toolTipStep == 1)
                        {
                            if (key.accept())
                                toolTipStep++;
                            canSkip = true;
                            panning = true;
                            text = "Hit that dendrite over there to open up this plexus. Watch out! \nSometimes a denrite is hooked up to more than one plexus.";
                        }
                        else if (toolTipStep == 2)
                        {
                            showTooltip = false;
                            canSkip = false;
                            panning = false;
                        }
                    }
                    break;
                default: break;
            }

        }

        public void DoUpdate(string _updateString)
        {
            updateString = _updateString;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showTooltip)
            {
                spriteBatch.Draw(background, new Vector2(Globals.CameraPosition.X - 513, Globals.CameraPosition.Y - 415 + 440), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .00001f);
                spriteBatch.DrawString(font, text, new Vector2((Globals.CameraPosition.X - 513) + 400, (Globals.CameraPosition.Y - 415) + 650), Color.White, 0f, new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2), 1f, SpriteEffects.None, 0f);
                if (canSkip)
                {
                    if (controllerConnected)
                    {
                        spriteBatch.Draw(abutton, new Vector2((Globals.CameraPosition.X - 513) + 750, (Globals.CameraPosition.Y - 415) + 600), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(enterButton, new Vector2((Globals.CameraPosition.X - 513) + 750, (Globals.CameraPosition.Y - 415) + 600), Color.White);
                    }
                    
                    
                }
                if (tooltipID == 0 && toolTipStep == 1)
                {
                    if (controllerConnected)
                    {
                        spriteBatch.Draw(leftThumb, new Vector2((Globals.CameraPosition.X - 513) + 750, (Globals.CameraPosition.Y - 415) + 600), Color.White);
                        spriteBatch.Draw(dPad, new Vector2((Globals.CameraPosition.X - 513) + 845, (Globals.CameraPosition.Y - 415) + 600), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(arrowKeys, new Vector2((Globals.CameraPosition.X - 513) + 720, (Globals.CameraPosition.Y - 415) + 575), Color.White);
                        spriteBatch.Draw(wASDKeys, new Vector2((Globals.CameraPosition.X - 513) + 845, (Globals.CameraPosition.Y - 415) + 575), Color.White);
                    }
                    
                    
                }
                if (tooltipID == 2 && toolTipStep == 1)
                {
                    if (controllerConnected)
                    {
                        spriteBatch.Draw(arrow, arrowPosition, Color.White);
                        spriteBatch.Draw(abutton, new Vector2((Globals.CameraPosition.X - 513) + 750, (Globals.CameraPosition.Y - 415) + 600), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(arrow, arrowPosition, Color.White);
                        spriteBatch.Draw(spacebar, new Vector2((Globals.CameraPosition.X - 513) + 750, (Globals.CameraPosition.Y - 415) + 600), Color.White);
                    }
                }
            }
        }
    }
}
