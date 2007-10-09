using System;

namespace Precision
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PrecisionGame game = new PrecisionGame())
            {
                game.Run();
            }
        }
    }
}

