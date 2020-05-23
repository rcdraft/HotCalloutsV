using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotCalloutsV.Common
{
    internal static class ScannerHelper
    {
        /// <summary>
        /// Send a notification that contains an sender and text. Used to simulate
        /// radio communications.
        /// </summary>
        /// <param name="sender">The sender of the text, for example, Dispatch.</param>
        /// <param name="text">Text itself.</param>
        /// <returns>The handle that <see cref="Game.DisplayNotification(string)"/> returns.</returns>
        public static uint DisplayDispatchDialogue(string sender, string text)
        {
            uint result = Game.DisplayNotification($"<b>{sender}: </b>{text}");
            return result;
        }
    }

    
}
