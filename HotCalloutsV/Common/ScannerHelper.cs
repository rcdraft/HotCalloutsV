using LSPD_First_Response.Mod.API;
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

        public static uint DisplayDispatchNote(string text)
        {
            uint result = Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "San Andreas Central", text);
            return result;
        }

        public static uint ReportNormalCode4(string call)
        {
            Functions.PlayScannerAudio("ATTENTION_ALL_UNITS NO_ADDITIONAL_SUPPORT");
            return DisplayDispatchDialogue("Dispatch", "We're code 4 on ~b~" + call + "~b~.");
        }

        /// <summary>
        /// Direct RandomiseLicensePlate directly copied from Albo1125.Common Project
        /// Thanks, Albo1125
        /// </summary>
        /// <param name="vehicle"></param>
        public static void RandomiseLicencePlate(Vehicle vehicle)
        {
            if (vehicle)
            {
                vehicle.LicensePlate = MathHelper.GetRandomInteger(9).ToString() +
                                       MathHelper.GetRandomInteger(9).ToString() +
                                       Convert.ToChar(MathHelper.GetRandomInteger(0, 25) + 65) +
                                       Convert.ToChar(MathHelper.GetRandomInteger(0, 25) + 65) +
                                       Convert.ToChar(MathHelper.GetRandomInteger(0, 25) + 65) +
                                       MathHelper.GetRandomInteger(9).ToString() +
                                       MathHelper.GetRandomInteger(9).ToString() +
                                       MathHelper.GetRandomInteger(9).ToString();
#if DEBUG
                Game.LogTrivial($"Set {vehicle.Model.Name} license plate to {vehicle.LicensePlate}");
#endif
            }
        }
    }

    
}
