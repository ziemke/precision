using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Precision.Classes
{
    internal class Audio
    {
        #region Fields
        static AudioEngine audioEngine;
        static WaveBank waveBank;
        static SoundBank soundBank;

        internal enum Cue { Game_BgLoop, Menu_UpDown, Menu_LeftRight, Powerup_Time, Enemy_Kill, Game_Pickup, Game_Click};
        #endregion

        #region Constructors
        static Audio()
        { 
           
        }
        #endregion

        #region Methods
        #region Initialize
        internal static void Initialize()
        {
            audioEngine = new AudioEngine(@"Content\Sounds\precision_sounds.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Sounds\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Sounds\Sound Bank.xsb");
        }
        #endregion

        #region Update
        internal static void Update()
        {
            audioEngine.Update();
        }
        #endregion

        #region Play (Cue)
        /// <summary>
        /// Plays a cue by enum
        /// </summary>
        /// <param name="cue">enum</param>
        internal static void Play(Cue cue)
        {
            soundBank.PlayCue(cue.ToString());
        }
	    #endregion

        #region Play (String)
        internal static void Play(string cueName)
        {
            soundBank.PlayCue(cueName);
        }
        #endregion

        #region Stop (Cue)
        /// <summary>
        /// Stops a cue by enum
        /// </summary>
        /// <param name="cue">enum</param>
        internal static void Stop(Cue cue)
        {
            soundBank.GetCue(cue.ToString()).Stop(AudioStopOptions.Immediate);
        }
        #endregion

        #region Stop (String)
        internal static void Stop(string cueName)
        {
            soundBank.GetCue(cueName).Stop(AudioStopOptions.Immediate);
        }
        #endregion

        #region Stop Sounds
        internal static void StopSounds()
        {
            audioEngine.GetCategory("Sounds").Stop(AudioStopOptions.Immediate);
        }
        #endregion

        #region Stop Category
        internal static void StopCategory(string category)
        {
            audioEngine.GetCategory(category).Stop(AudioStopOptions.Immediate);
        }
        #endregion

        #region Stop Music
        internal static void StopMusic()
        {
            audioEngine.GetCategory("Music").Stop(AudioStopOptions.Immediate);
        }
        #endregion
        #endregion
    }
}
