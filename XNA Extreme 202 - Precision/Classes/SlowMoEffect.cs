using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Precision.Classes
{
    class SlowMoEffect : Actor
    {
        internal SlowMoEffect(Texture2D texture) : base(texture)
        {
            this.Visible = false;
        }
    }
}
