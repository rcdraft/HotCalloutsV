// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using HotCalloutsV.Common;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Drawing;
using System.Windows.Forms;
using Functions = LSPD_First_Response.Mod.API.Functions;

namespace HotCalloutsV.Callouts
{
    public class DocumentLack : Callout
    {
        private Vector3 spawn;
        private EDocumentLackSituation situation;

        private Ped suspect;
        private Vehicle suspectCar;
        private Blip blip;
        private bool approach;
        private bool start;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawn, 30f);
            AddMinimumDistanceCheck(20f, spawn);

            situation = (EDocumentLackSituation)MathHelper.GetRandomInteger(0, 1);
            CalloutMessage = situation == EDocumentLackSituation.Insurance ? "Uninsured Vehicle" : "Unregistered Vehicle";
            CalloutPosition = spawn;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_DANGEROUS_DRIVING IN_OR_ON_POSITION", spawn);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            start = true;
            try
            {
                Game.LogTrivial("DocumentLack: Creating Entity");
                suspectCar = new Vehicle(spawn);
                suspectCar.IsPersistent = true;
                suspect = suspectCar.CreateRandomDriver();
                suspect.IsPersistent = true;
                Game.LogTrivial("DocumentLack: Creating Blip");
                blip = suspect.AttachBlip();
                blip.RouteColor = Color.Red;
                blip.IsRouteEnabled = true;
                Game.LogTrivial("DocumentLack: Entity Success, dentermine Situations");
                string message;
                switch (situation)
                {
                    case EDocumentLackSituation.Insurance:
                        Game.LogTrivial("INSURANCE determined");
                        if (Integreate.StopThePed) suspectCar.SetInsurance((HCVehicleStatus)MathHelper.GetRandomInteger(0, 1));
                        Game.LogTrivial("INSURANCE set");
                        message = "The target vehicle has been reported as Uninsured or it's insurance has expired.";
                        break;

                    default:
                    case EDocumentLackSituation.Registration:
                        Game.LogTrivial("REGISTRATION determined");
                        if (Integreate.StopThePed) suspectCar.SetRegistration((HCVehicleStatus)MathHelper.GetRandomInteger(0, 1));
                        Game.LogTrivial("REGISTRATION set");
                        message = "The target vehicle has been reported as No registiration or it's expired.";
                        break;
                }
                Game.LogTrivial("DocumentLack: Situations determined");
                ScannerHelper.DisplayDispatchNote(message);
                Game.LogTrivial("Checking for Stop The Ped");
                if (!Integreate.StopThePed)
                {
                    Game.DisplayHelp("This callout works best if you install ~y~StopThePed~s~ by BejoIlo.");
                }
                Game.LogTrivial("Prepar done");
                Game.DisplaySubtitle("Approach the ~r~suspect~s~.");
                return base.OnCalloutAccepted();
            }
            catch(Exception ex)
            {
                Game.LogExtremelyVerbose("HotCallout Debug: Failed on DocumentLack");
                Game.LogExtremelyVerbose("HotCallout Debug: " + ex.GetType().Name);
                Game.LogExtremelyVerbose("HotCallout Debug: " + ex.Message);
                Game.LogExtremelyVerbose(ex.StackTrace);
                Game.DisplayHelp($"The \"{CalloutMessage}\" callout encounterd fatal error and must exit.");
                return false;
            }
        }

        public override void Process()
        {
            if (!approach && Game.LocalPlayer.Character.Position.DistanceTo2D(suspect.Position) <= 10f)
            {
                approach = true;
                Game.LogTrivialDebug("HotCallout Debug: DistanceTo2D cycle has <= 10f");
                Game.DisplaySubtitle("Pull the ~r~suspect~s~ over.");
                ScannerHelper.DisplayDispatchDialogue("You", "I have the suspect in sight.");
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "10-4. Perform traffic stop.");
            }
            if (approach && Game.IsKeyDown(Keys.End))
            {
                Game.LogTrivialDebug("HotCallout Debug: User End");
                End();
            }
            base.Process();
        }

        public override void End()
        {
            if(!approach && start)
            {
                Game.LogTrivial("HotCallout Warning: The End does not seems like user has opreated because user dident even approach the suspect.");
                Game.LogTrivial("HotCallout Warning: This normally triggered by LSPDFR becuase error on methods or ended by external plugin.");
                CommonHelper.DeclareUnexceptedEnd(CalloutMessage);
            }
            else if(start)
            {
                ScannerHelper.DisplayDispatchDialogue("Dispatch", $"Officer reporting, we're code 4 on <b>{CalloutMessage}</b>.");
            }
            
            if (suspect.Exists()) suspect.Dismiss();
            if (suspectCar.Exists()) suspectCar.Dismiss();
            base.End();
        }
    }
}