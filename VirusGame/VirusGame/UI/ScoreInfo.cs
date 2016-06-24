using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusGame.UI
{
    public class ScoreInfo
    {
        private string name;
        private int pointValue;
        private bool killed;
        private bool collected;
        

        public ScoreInfo(String _name, int _pointValue)
        {
            name = _name;
            pointValue = _pointValue;
        }
    }
}
