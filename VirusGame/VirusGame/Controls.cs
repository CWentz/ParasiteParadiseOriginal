using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace VirusGame
{
    public class Controls
    {

        #region Declarations
        public bool controlsConnected;
        
        KeyboardState key; 
        KeyboardState oldKey; 
        GamePadState controller; 
        GamePadState oldController;

        private float leftMotor;
        private float rightMotor;
        private int vibrateTimer;
        private float hilferExploding;

        #endregion

        public int HelperExploding
        {
            set { hilferExploding = value / 60f; }
        }


        /// <summary>
        /// Saves the current keystates and controller states in this control class.
        /// </summary>
        /// <param name="_key">keyboard state</param>
        /// <param name="_oldKey">old keyboard state</param>
        /// <param name="_controller">controller pad state</param>
        /// <param name="_oldController">old controller pad state</param>
        public void UpdateControls(KeyboardState _key, KeyboardState _oldKey, GamePadState _controller, GamePadState _oldController)
        {
            key = _key;
            oldKey = _oldKey;
            controller = _controller;
            oldController = _oldController;

            controlsConnected = controller.IsConnected;
            

            


            if (hilferExploding > 0f && hilferExploding < .9f)
                setVibrate(hilferExploding, hilferExploding, 10);


            if (vibrateTimer > 0)
            {
                GamePad.SetVibration(PlayerIndex.One, leftMotor, rightMotor);
            }

            if (vibrateTimer < 0)
            {
                vibrateTimer = 0;
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            }

            vibrateTimer--;

        }
        public void setVibrate(float _left, float _right, int _timeMS)
        {
            leftMotor = _left;
            rightMotor = _right;
            vibrateTimer = _timeMS;
        }

        #region get set
   
        #endregion 

        #region movement methods
        public Vector2 joystickMove()
        {
            return (controller.ThumbSticks.Left * new Vector2(1, -1));
        }


        public bool Cheater()
        {
            if (key.IsKeyDown(Keys.Up) && key.IsKeyDown(Keys.W) || controller.Buttons.LeftShoulder == ButtonState.Pressed && controller.Buttons.RightShoulder == ButtonState.Pressed)
                return true;
            else return false;
        }





        #region menu movement
        /// <summary>
        /// tests if the up key or dpad up is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool menuUp()
        {
            if (key.IsKeyDown(Keys.Up) && oldKey.IsKeyUp(Keys.Up) || controller.DPad.Up == ButtonState.Pressed && oldController.DPad.Up == ButtonState.Released || key.IsKeyDown(Keys.W) && oldKey.IsKeyUp(Keys.W))
                return true;
            else return false;

        }

        /// <summary>
        /// tests if the down key or dpad down is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool menuDown()
        {
            if (key.IsKeyDown(Keys.Down) && oldKey.IsKeyUp(Keys.Down) || controller.DPad.Down == ButtonState.Pressed && oldController.DPad.Down == ButtonState.Released || key.IsKeyDown(Keys.S) && oldKey.IsKeyUp(Keys.S))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the right key or dpad right is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool menuRight()
        {
            if (key.IsKeyDown(Keys.Right) && oldKey.IsKeyUp(Keys.Right) || 
                controller.DPad.Right == ButtonState.Pressed && oldController.DPad.Right == ButtonState.Released || 
                key.IsKeyDown(Keys.D) && oldKey.IsKeyUp(Keys.D))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the left key or dpad left is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool menuLeft()
        {
            if (key.IsKeyDown(Keys.Left) && oldKey.IsKeyUp(Keys.Left) || controller.DPad.Left == ButtonState.Pressed && oldController.DPad.Left == ButtonState.Released || key.IsKeyDown(Keys.A) && oldKey.IsKeyUp(Keys.A))
                return true;
            else return false;
        }
        #endregion

        /// <summary>
        /// tests if the up key or dpad up is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool moveUp()
        {
            if (key.IsKeyDown(Keys.Up) || controller.DPad.Up == ButtonState.Pressed || key.IsKeyDown(Keys.W))
                return true;
            else return false;

        }

        /// <summary>
        /// tests if the down key or dpad down is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool moveDown()
        {
            if (key.IsKeyDown(Keys.Down) || controller.DPad.Down == ButtonState.Pressed|| key.IsKeyDown(Keys.S))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the right key or dpad right is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool moveRight()
        {
            if (key.IsKeyDown(Keys.Right) || controller.DPad.Right == ButtonState.Pressed || key.IsKeyDown(Keys.D))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the left key or dpad left is pushed.
        /// </summary>
        /// <returns>bool</returns>
        public bool moveLeft()
        {
            if (key.IsKeyDown(Keys.Left) || controller.DPad.Left == ButtonState.Pressed || key.IsKeyDown(Keys.A))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the player attacks key is down and oldKey is up.
        /// </summary>
        /// <returns>bool</returns>
        public bool attack()
        {
            if (key.IsKeyDown(Keys.Space) && oldKey.IsKeyUp(Keys.Space) 
                || (controller.Buttons.A == ButtonState.Pressed && oldController.Buttons.A == ButtonState.Released))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the right control key is pushed and oldkey is up.
        /// </summary>
        /// <returns>bool</returns>
        public bool split()
        {
            if (key.IsKeyDown(Keys.RightControl) && oldKey.IsKeyUp(Keys.RightControl) 
                || (controller.Buttons.X == ButtonState.Pressed && oldController.Buttons.X == ButtonState.Released))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the escape key or controller back is pushed and last state was released/up.
        /// </summary>
        /// <returns>bool</returns>
        public bool escape()
        {
            if (key.IsKeyDown(Keys.Escape) && oldKey.IsKeyUp(Keys.Escape) 
                || (controller.Buttons.Back == ButtonState.Pressed && oldController.Buttons.Back == ButtonState.Released))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the enter key or controller button a is pushed and oldstate was released/up.
        /// </summary>
        /// <returns>bool</returns>
        public bool accept()
        {
            if (key.IsKeyDown(Keys.Enter) && oldKey.IsKeyUp(Keys.Enter) 
                || (controller.Buttons.A == ButtonState.Pressed && oldController.Buttons.A == ButtonState.Released))
                return true;
            else return false;
        }

        /// <summary>
        /// tests if the p key or controller start is pushed and oldstate was released/up.
        /// </summary>
        /// <returns>bool</returns>
        public bool pause()
        {
            if (key.IsKeyDown(Keys.P) && oldKey.IsKeyUp(Keys.P)
                || (controller.Buttons.Start == ButtonState.Pressed && oldController.Buttons.Start == ButtonState.Released))
                return true;
            else return false;
        }

        /// <summary>
        /// toogle the debugmode
        /// </summary>
        /// <returns>bool</returns>
        public bool debugToogle()
        {
            if (key.IsKeyDown(Keys.F12) && oldKey.IsKeyUp(Keys.F12))
                return true;
            else return false;
        }

        #endregion
    }
}
