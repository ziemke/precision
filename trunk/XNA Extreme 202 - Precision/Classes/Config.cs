using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Config
    {
        internal const float CELL_DEATH_TIME = 2f;

        internal const float TITLE_SCREEN_FLICK_DURATION = 1.5f;

        internal const int POWERUP_PLACEMENT_RETRYCOUNT = 100;

        internal const int ACTIVE_TIMEBAR_WIDTH = 400;
        internal const int ACTIVE_TIMEBAR_HEIGHT = 10;
        internal const int ACTIVE_TIMEBAR_ICON_SIZE = 30;
        //protected const int PICKUP_TIMEBAR_WIDTH = 50;
        internal const int PICKUP_TIMEBAR_HEIGHT = 10;
        internal static readonly Color lifeBarColorDefault = Color.Green;

        internal const float TIME_POWERUP_DURATION = 3f;
        internal const float TIME_POWERUP_RESPAWN_TIME = 1f;
        internal const float TIME_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER = 0f;
        internal const float TIME_POWERUP_PICKUP_TIME = 3f;
        internal static readonly Color timePowerupActiveTimeBarColor = new Color(0, 128, 255);
        internal static readonly Color timePowerupPickupTimeBarColor = Color.White;
        internal const float TIME_POWERUP_SLOWDOWN = 0.1f;

        internal const float EXTRALIFE_POWERUP_RESPAWN_TIME = 15f;
        internal const float EXTRALIFE_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER = 10f;
        internal const float EXTRALIFE_POWERUP_PICKUP_TIME = 5f;
        internal static readonly Color extralifePowerupPickupTimeBarColor = Color.White;

        internal const float SCORE_POWERUP_DURATION = 3f;
        internal const float SCORE_POWERUP_RESPAWN_TIME = 20f;
        internal const float SCORE_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER = 10f;
        internal const float SCORE_POWERUP_PICKUP_TIME = 5f;
        internal static readonly Color scorePowerupActiveTimeBarColor = Color.Orange;
        internal static readonly Color scorePowerupPickupTimeBarColor = Color.White;

        internal const float SHIELD_POWERUP_DURATION = 3f;
        internal const float SHIELD_POWERUP_RESPAWN_TIME = 10f;
        internal const float SHIELD_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER = 10f;
        internal const float SHIELD_POWERUP_PICKUP_TIME = 5f;
        internal static readonly Color shieldPowerupActiveTimeBarColor = Color.LightBlue;
        internal static readonly Color shieldPowerupPickupTimeBarColor = Color.White;

        internal const float DESTRUCTION_POWERUP_DURATION = 3f;
        internal const float DESTRUCTION_POWERUP_RESPAWN_TIME = 30f;
        internal const float DESTRUCTION_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER = 10f;
        internal const float DESTRUCTION_POWERUP_PICKUP_TIME = 5f;
        internal static readonly Color destructionPowerupActiveTimeBarColor = Color.Red;
        internal static readonly Color destructionPowerupPickupTimeBarColor = Color.White;
        
        internal const float SPEED_POWERDOWN_DURATION = 10f;
        internal const float SPEED_POWERDOWN_RESPAWN_TIME = 5f;
        internal const float SPEED_POWERDOWN_RESPAWN_TIME_RANDOM_MODIFIER = 10f;
        internal const float SPEED_POWERDOWN_PICKUP_TIME = CELL_DEATH_TIME;
        internal static readonly Color speedPowerdownActiveTimeBarColor = Color.Red;
        internal static readonly List<Color> speedPowerdownPickupTimeBarColors;
        internal const float SPEED_POWERDOWN_SLOWDOWN = 0.4f;
        
        
        static Config()
        {
            speedPowerdownPickupTimeBarColors = new List<Color>();
            speedPowerdownPickupTimeBarColors.Add(Color.Red);
            speedPowerdownPickupTimeBarColors.Add(Color.Yellow);
            speedPowerdownPickupTimeBarColors.Add(Color.Green);
        }
    }
}
