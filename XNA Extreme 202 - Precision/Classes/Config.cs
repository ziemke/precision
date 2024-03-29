using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Config
    {
        internal const float CELL_DEATH_TIME_GENERIC = 2f;
        internal const float CELL_DEATH_TIME_EASY = 5f;

        internal const float TITLE_SCREEN_FLICK_DURATION = 1.5f;
        
        internal const int POWERUP_PLACEMENT_RETRYCOUNT = 100;

        internal const int ACTIVE_TIMEBAR_WIDTH = 640;
        internal const int ACTIVE_TIMEBAR_HEIGHT = 16;
        internal const int ACTIVE_TIMEBAR_ICON_SIZE = 48;
        internal const int PICKUP_TIMEBAR_HEIGHT = 11; //10

        internal const float TIME_POWERUP_DURATION = 5f;
        internal const float TIME_POWERUP_RESPAWN_TIME = 0.2f;
        internal const float TIME_POWERUP_FREQUENCY = 1f;
        internal const float TIME_POWERUP_PICKUP_TIME = 5f;
        internal const int TIME_POWERUP_START_LEVEL = 1;
        internal static readonly Color timePowerupActiveTimeBarColor = new Color(0, 128, 255);
        internal static readonly Color timePowerupPickupTimeBarColor = Color.White;
        internal const float TIME_POWERUP_SLOWDOWN = 0.1f;

        internal const float EXTRALIFE_POWERUP_RESPAWN_TIME = 0.2f; // 3
        internal const float EXTRALIFE_POWERUP_FREQUENCY = 1f;
        internal const float EXTRALIFE_POWERUP_PICKUP_TIME = 5f;
        internal const int EXTRALIFE_POWERUP_START_LEVEL = 1; //7
        internal static readonly Color extralifePowerupPickupTimeBarColor = Color.White;

        internal const float SCALE_DOWN_POWERUP_RESPAWN_TIME = 0.2f; // 2
        internal const float SCALE_DOWN_POWERUP_FREQUENCY = 1f;//0.4
        internal const float SCALE_DOWN_POWERUP_PICKUP_TIME = 5f;
        internal const int SCALE_DOWN_POWERUP_START_LEVEL = 1; //4
        internal static readonly Color scaleDownPowerupPickupTimeBarColor = Color.White;
        internal const float SCALE_DOWN_POWERUP_SCALE = 0.5f;
        internal const float SCALE_DOWN_POWERUP_SCALE_TIME = 2.5f;

        internal const float SCORE_POWERUP_DURATION = 4f;
        internal const float SCORE_POWERUP_RESPAWN_TIME = 0.2f;
        internal const float SCORE_POWERUP_FREQUENCY = 1f;
        internal const float SCORE_POWERUP_PICKUP_TIME = 4f;
        internal const int SCORE_POWERUP_START_LEVEL = 1; //3
        internal static readonly Color scorePowerupActiveTimeBarColor = Color.Orange;
        internal static readonly Color scorePowerupPickupTimeBarColor = Color.White;

        internal const float SHIELD_POWERUP_DURATION = 5f;
        internal const float SHIELD_POWERUP_RESPAWN_TIME = 0.2f;
        internal const float SHIELD_POWERUP_FREQUENCY = 1f;
        internal const float SHIELD_POWERUP_PICKUP_TIME = 5f;
        internal const int SHIELD_POWERUP_START_LEVEL = 1; //5
        internal static readonly Color shieldPowerupActiveTimeBarColor = Color.LightBlue;
        internal static readonly Color shieldPowerupPickupTimeBarColor = Color.White;

        internal const float DESTRUCTION_POWERUP_DURATION = 5f;
        internal const float DESTRUCTION_POWERUP_RESPAWN_TIME = 0.2f;
        internal const float DESTRUCTION_POWERUP_FREQUENCY = 1f;//0.4
        internal const float DESTRUCTION_POWERUP_PICKUP_TIME = 6f;
        internal const int DESTRUCTION_POWERUP_START_LEVEL = 1;//9
        internal static readonly Color destructionPowerupActiveTimeBarColor = Color.Red;
        internal static readonly Color destructionPowerupPickupTimeBarColor = Color.White;
        
        internal const float SPEED_POWERDOWN_DURATION = 10f;
        internal const float SPEED_POWERDOWN_RESPAWN_TIME = 0.2f;
        internal const float SPEED_POWERDOWN_FREQUENCY = 1f;
        internal const float SPEED_POWERDOWN_PICKUP_TIME = CELL_DEATH_TIME_GENERIC;
        internal const int SPEED_POWERDOWN_START_LEVEL = 1; //2
        internal static readonly Color speedPowerdownActiveTimeBarColor = Color.Red;
        internal static readonly List<Color> speedPowerdownPickupTimeBarColors;
        internal const float SPEED_POWERDOWN_SLOWDOWN = 0.4f;


        internal static readonly List<Point> resolutions;
        
        
        static Config()
        {
            speedPowerdownPickupTimeBarColors = new List<Color>();
            speedPowerdownPickupTimeBarColors.Add(Color.Red);
            speedPowerdownPickupTimeBarColors.Add(Color.Yellow);
            speedPowerdownPickupTimeBarColors.Add(Color.Green);

            resolutions = new List<Point>();

            //4:3
            resolutions.Add(new Point(800, 600));
            resolutions.Add(new Point(1024, 768));
            resolutions.Add(new Point(1152, 864));
            resolutions.Add(new Point(1280, 960));

            //16:9
            //resolutions.Add(new Point(1280, 720));
            //resolutions.Add(new Point(800, 640));
            //resolutions.Add(new Point(1024, 820));
            //resolutions.Add(new Point(1152, 920));
            //resolutions.Add(new Point(1280, 1024));
        }
    }
}
