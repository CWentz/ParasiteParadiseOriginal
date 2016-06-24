using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame._Levels
{
    public class Level1 : LevelMain
    {
        //private bool cameraPanNerve1;
        //private bool cameraPanTrigger1;
        //private bool cameraPanNerve2;
        //private bool cameraPanTrigger2;
        //private bool cameraPanTrigger3;
        //private int panTimer;

        public Level1(GraphicsDevice graphicDevice, String _levelGleedFile) :base(graphicDevice, _levelGleedFile)
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);



            if (!nerve1On)
            {
                bloodSpawn1Open = false;
                synNerve1 = bloodSpawn1Pos;
            }
            else
            {
                bloodSpawn1Open = true;
            }

            if (!nerve2On)
            {
                synNerve2 = bloodSpawn2Pos;
                bloodSpawn2Open = false;
            }
            else
            {
                bloodSpawn2Open = true;
            }

            if (trigger1On)
            {
                synTrig1 = gefecht1Pos;
                gefecht1Open = true;
            }


            if (trigger2On)
            {
                synTrig2 = gefecht4Pos;
                gefecht4Open = true;
                gefecht3Open = true;
            }

            if (trigger3On && trigger4On)
            {
                gefecht2Open = true;
            }

            if (trigger3On)
            {
                synTrig3 = gefecht2Pos;
            }
            if (trigger4On)
            {
                synTrig4 = gefecht2Pos;
            }




            #region old camera panning
            //if (trigger4On)
            //{
            //    //if (cameraPanTimer < 60 && !gefecht2Open && !cameraPanTrigger3)
            //    //    cameraDestination = gefecht2Pos;
            //    //if (!cameraPanTrigger3)
            //    //{

            //        synTrig4 = gefecht2Pos;
            //        //panning = true;
            //        //cameraPanTrigger3 = true;
            //    //}
            //    //if (cameraPanTimer >= 30 && trigger3On)
            //    //{
            //        //synapse4On = true;
            //        gefecht2Open = true;
            //    //}
            //    //if (cameraPanTimer < 60 && !gefecht2Open && !cameraPanTrigger3)
            //    //    cameraDestination = gefecht2Pos;
            //    //PanCameraToPosition(bloodSpawn2Pos, 50f);

            //}

            //if (nerve1On && !cameraPanNerve1)
            //    bloodSpawn1Open = true;

            //if (!nerve1On)
            //{
            //    if (!cameraPanNerve1)
            //    {
            //        //synapse1On = true;
            //        synNerve1 = bloodSpawn1Pos;
            //        //cameraDestination = bloodSpawn1Pos;
            //        panning = true;
            //        cameraPanNerve1 = true;
            //    }
            //    if (cameraPanTimer >= 30)
            //    {

            //        bloodSpawn1Open = false;
            //    }
            //    if (cameraPanTimer < 60 && bloodSpawn1Open)
            //        cameraDestination = bloodSpawn1Pos;
            //        //PanCameraToPosition(bloodSpawn1Pos, 50f);
                
            //}

            //if (nerve2On && !cameraPanNerve2)
            //{
            //    bloodSpawn2Open = true;
            //}
            //if (!nerve2On)
            //{
            //    if (!cameraPanNerve2)
            //    {

            //        synNerve2 = bloodSpawn2Pos;
            //        //synapse2On = true;
            //        //cameraDestination = bloodSpawn2Pos;
            //        panning = true;
            //        cameraPanNerve2 = true;
            //    }
            //    if (cameraPanTimer >= 30)
            //    {
                    
            //        bloodSpawn2Open = false;
            //    }
            //    if (cameraPanTimer < 60 && bloodSpawn2Open)
            //        cameraDestination = bloodSpawn2Pos;
            //        //PanCameraToPosition(bloodSpawn2Pos, 50f);

            //}


            //if (!trigger1On && !cameraPanTrigger1)
            //    gefecht1Open = false;

            //if (trigger1On)
            //{
            //    if (!cameraPanTrigger1)
            //    {
            //        synTrig1 = gefecht1Pos;
            //        ////synapse3On = true;
            //        //cameraDestination = bloodSpawn2Pos;
            //        panning = true;
            //        cameraPanTrigger1 = true;
            //    }
            //    if (cameraPanTimer >= 30)
            //    {
            //        gefecht1Open = true;
            //    }
            //    if (cameraPanTimer < 60 && !gefecht1Open)
            //        cameraDestination = gefecht1Pos;
            //    //PanCameraToPosition(bloodSpawn2Pos, 50f);

            //}


            //if (!trigger3On && !trigger4On && !cameraPanTrigger2)
            //    gefecht2Open = false;

            //if (trigger3On)
            //{
            //    if (!cameraPanTrigger2)
            //    {

            //        synTrig3 = gefecht2Pos;
            //        //cameraDestination = bloodSpawn2Pos;
            //        panning = true;
            //        cameraPanTrigger2 = true;
            //    }
            //    if (cameraPanTimer >= 30 && trigger4On)
            //    {
            //        //synapse4On = true;
            //        gefecht2Open = true;
            //    }
            //    if (cameraPanTimer < 60 && !gefecht2Open)
            //        cameraDestination = gefecht2Pos;
            //    //PanCameraToPosition(bloodSpawn2Pos, 50f);

            //}



            ////if (!trigger1On && !cameraPanTrigger1)
            ////    gefecht1Open = false;

            ////if (trigger1On)
            ////{
            ////    if (!cameraPanTrigger1)
            ////    {
            ////        synTrig1 = gefecht1Pos;
            ////        //cameraDestination = bloodSpawn2Pos;
            ////        panning = true;
            ////        cameraPanTrigger1 = true;
            ////    }
            ////    if (cameraPanTimer >= 30)
            ////    {
            ////        gefecht1Open = true;
            ////    }
            ////    if (cameraPanTimer < 60 && !gefecht1Open)
            ////        cameraDestination = gefecht1Pos;
            ////    //PanCameraToPosition(bloodSpawn2Pos, 50f);

            ////}



            
            //if (trigger2On)
            //{
            //    gefecht3Open = trigger2On;
            //    gefecht4Open = trigger2On;
            //}
            #endregion
        }
    }
}
