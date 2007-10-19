using System;
using System.Collections.Generic;
using System.Text;

namespace Precision.Classes
{
    internal class HighScore
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }


        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

    }
}
