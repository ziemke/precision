using System;
using System.Collections.Generic;
using System.Text;

namespace Precision.Classes
{
    internal class HighScore
    {
        private string userName;

        internal string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int score;

        internal int Score
        {
            get { return score; }
            set { score = value; }
        }


        private int level;

        internal int Level
        {
            get { return level; }
            set { level = value; }
        }

    }
}
