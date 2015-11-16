#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace CHESS
{
    static class Program
    {
        private static ChessGame game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new ChessGame();
            game.Run();
        }
    }
}
