using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace VirusGame.SpriteClasses.Menu
{
    public class Points
    {
        public ArrayList info = new ArrayList();
        //public List<> info;
        //public dynamic[] info; 

        private float seconds;
        private int minutes;
        private bool collect;
        private bool restored;
        private bool kill;
        private int pointValue;
        private String name;
        private bool credit;
        private bool creditGiven;
        public bool counted;


        /// <summary>
        /// score for individual 
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_pointValue"></param>
        /// <param name="_collect"></param>
        public Points(String _name, int _pointValue, bool _collect)
        {
            info = new ArrayList(8);
            name = _name;
            pointValue = _pointValue;
            kill = !_collect;
            collect = _collect;

        }

        public bool Restored
        {
            set { restored = value; }
        }

        public bool Credit
        {
            set { credit = value; }
        }

        public int PointValue
        {
            set { pointValue = value; }
        }

        //public bool Collect
        //{
        //    set { collect = value; }
        //}
        //public int Minutes
        //{
        //    set { minutes = value; }
        //}
        //public float Seconds
        //{
        //    set { seconds = value; }
        //}
        //public bool Restored
        //{
        //    set { restored = value; }
        //}
        //public bool Kill
        //{
        //    set { kill = value; }
        //}

      



        public void Update(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }


            if (credit && !creditGiven)
            {
                info.Add(name);
                info.Add(pointValue);
                info.Add(kill);
                info.Add(collect);
                info.Add(restored);
                info.Add(minutes);
                info.Add(seconds);
                info.Add(false);
                
            }

        }

        public ArrayList getInfo()
        {
            if (!creditGiven)
            {
                creditGiven = true;
                return info;
            }
            return null;
        }
    }
}
