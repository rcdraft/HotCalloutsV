// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using LSPD_First_Response.Mod.API;
using Rage;
using System;

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
            return DisplayDispatchDialogue("Dispatch", "We're ~g~code 4~s~ on ~h~" + call + "~s~.");
        }

        public static void ReportEvent(string message)
        {
            Functions.PlayScannerAudio("ATTENTION_ALL_UNIT CITIZENS_REPORT " + message);
        }

        public static void RandomiseLicencePlate(Vehicle vehicle)
        {
            // This code is part of Albo1125.Common.
            // Albo1125.Common is free software; it is licensed under GNU GPL version 3.
            // Copyright (C) 2015-2019 Albo1125.
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
