// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

// HotCallouts for Grand Theft Auto V (or HotCalloutsV)
// is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// HotCalloutsV is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with HotCalloutsV.  If not, see <https://www.gnu.org/licenses/>. 

using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.IO;
using System.Windows.Forms;

namespace HotCalloutsV.Callouts
{
    [CalloutInfo("DangerousDriver", CalloutProbability.Medium)]
    public class DangerousDriver : Callout
    {
        Ped suspect;
        Vehicle suspectCar;
        Vector3 spawn;
        Blip blip;
        private bool pursuited;
        private int situations;
        private LHandle currentPursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            /* Archival
             * 2020/6/30 09:26 - Add line number logging to locate error
             * 
             */
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Initializing Instance > Dangerous Driver");
            Game.LogTrivialDebug("LINE NUMBER: 25 / Used with source repository if pushed.");
            spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawn, 30f);
            AddMinimumDistanceCheck(20f, spawn);

            Game.LogTrivial("[Dangerous Driver/HotCallouts] Initialized Instance > Dangerous Driver");
            Game.LogTrivialDebug("LINE NUMBER: 32 / Used with source repository if pushed.");
            CalloutMessage = "Dangerous Driving";
            CalloutPosition = spawn;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_DANGEROUS_DRIVING IN_OR_ON_POSITION", spawn);
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Displayed Instance > Dangerous Driver");
            Game.LogTrivialDebug("LINE NUMBER: 38 / Used with source repository if pushed.");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            /* Archival
             * 2020/6/30 09:23 - Add logging to flags
             * 2020/6/30 09:28 - Add line number logging to locate error
             */
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Accepted Instance > Dangerous Driver");
            Game.LogTrivialDebug("LINE NUMBER: 52 / Used with source repository if pushed.");
            suspectCar = new Vehicle(spawn);
            suspectCar.IsPersistent = true;

            Game.LogTrivial("[Dangerous Driver/HotCallouts] Spawned suspectCar (" + suspectCar.Model.Name + ") and set to Persistent");
            Game.LogTrivialDebug("LINE NUMBER: 56 / Used with source repository if pushed.");

            suspect = suspectCar.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            Game.LogTrivial($"[Dangerous Driver/HotCallouts] Spawned suspect ({suspect.Model.Name}) and set to Persistent & Block Events");
            Game.LogTrivialDebug("LINE NUMBER: 63 / Used with source repository if pushed.");

            blip = suspect.AttachBlip();
            blip.IsFriendly = false;
            blip.IsRouteEnabled = true;
            blip.Name = "Reckless Driver";
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Spawned blip and renamed to Reckless Driver");
            Game.LogTrivialDebug("LINE NUMBER: 70 / Used with source repository if pushed.");

            situations = MathHelper.GetRandomInteger(0, 3);
            string message;
            string audioMessage;
            switch(situations)
            {
                default:
                case 0:
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] Flag 0: Emergency with 15f speed");
                    suspect.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.Emergency);
                    message = "driving all over the road, but with normal speed";
                    audioMessage = "CRIME_RECKLESS_DRIVER";
                    break;
                case 1:
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] Flag 1: Emergency with 30f speed");
                    suspect.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
                    message = "driving all over the road and overspeed";
                    audioMessage = "CRIME_SPEEDING_FELONY";
                    break;
                case 2:
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] Flag 2: Normal with 45f speed");
                    suspect.Tasks.CruiseWithVehicle(45f, VehicleDrivingFlags.Normal);
                    message = "overspeeding";
                    audioMessage = "CRIME_SPEEDING_FELONY";
                    break;
                case 3:
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] Flag 3: Alcohol intoxication");
                    if (Integreate.TrafficPolicer)
                    {
                        Game.LogTrivial("[Dangerous Driver/HotCallouts] Flag 3 Created: Traffic Policer Exists");
                        suspect.Tasks.CruiseWithVehicle(50f, VehicleDrivingFlags.Emergency);
                        Traffic_Policer.API.Functions.SetPedAlcoholLevel(suspect, Traffic_Policer.Impairment_Tests.AlcoholLevels.OverLimit);
                        audioMessage = "CRIME_RECKLESS_DRIVER";
                        message = "driving under the influence of alcohol";
                        break;
                    }
                    else
                    {
                        Game.LogTrivial("[Dangerous Driver/HotCallouts] Flag 3 Aborted: Traffic Policer does not exists");
                        Game.LogTrivial("[Dangerous Driver/HotCallouts] Creating Flag 2");
                        goto case 2;
                    }
            }
            ScannerHelper.ReportEvent(audioMessage);
            ScannerHelper.DisplayDispatchNote("We received a 911 report of a vehicle " + message + ". Respond with Code 3.");
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Done > Dangerous Driver");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            /* Archival
             * 2020/6/29 20:48 - Fix "InvalidHandleableException" causing whole LSPDFR to crash.
             *                   Add crash prevention system.
             */
            
            base.Process();
            try
            {
                if (!suspect.Exists() && !suspectCar.Exists())
                {
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] Forced ending Dangerous Driver because suspect or suspect's car does not exist.");
                    ScannerHelper.ReportNormalCode4("Dangerous Driving");
                    End();
                    return;
                }

                if (!pursuited && Functions.IsPedInPursuit(suspect))
                {
                    pursuited = true;
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] Fleeing > suspect");
                    ScannerHelper.DisplayDispatchDialogue("Dispatch", "suspect fleeing.");
                    currentPursuit = Functions.GetActivePursuit();
                }

                if (!(!pursuited || currentPursuit == null || !Functions.IsPursuitStillRunning(currentPursuit)))
                {
                    pursuited = false;
                    ScannerHelper.DisplayDispatchDialogue("Dispatch", "The pursuit has ~g~concluded~s~.");
                }

                if (Game.IsKeyDown(Keys.End))
                {
                    Game.LogTrivial("[Dangerous Driver/HotCallouts] End > Dangerous Driving");
                    ScannerHelper.ReportNormalCode4("Dangerous Driving");
                    End();
                }
            }
            catch(Exception ex)
            {
                End();
                Game.DisplayNotification("The callout <b>Dangerous Driver</b> was encountered error and must exit.");
                Game.DisplayNotification("Check log file for more details.");
                Game.LogTrivial("HotCallouts: exception: " + ex.GetType().Name);
                Game.LogTrivial("HotCallouts: message: " + ex.Message);
                Game.LogTrivial("HotCallouts: trace: \r\n" + ex.StackTrace);
            }
        }

        public override void End()
        {
            base.End();
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Ending Instance");

            if (suspect.Exists() && !Functions.IsPedArrested(suspect)) suspect.Dismiss();
            if (suspectCar.Exists()) suspectCar.Dismiss();
            if (blip.Exists()) blip.Delete();

            Game.LogTrivial("[Dangerous Driver/HotCallouts] Peds dismissed");
        }
    }
}
