using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
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
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Initializing Instance > Dangerous Driver");
            spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawn, 30f);
            AddMinimumDistanceCheck(20f, spawn);

            Game.LogTrivial("[Dangerous Driver/HotCallouts] Initialized Instance > Dangerous Driver");
            CalloutMessage = "Dangerous Driving";
            CalloutPosition = spawn;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_DANGEROUS_DRIVING IN_OR_ON_POSITION", spawn);
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Displayed Instance > Dangerous Driver");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Accepted Instance > Dangerous Driver");
            suspectCar = new Vehicle(spawn);
            suspectCar.IsPersistent = true;

            Game.LogTrivial("[Dangerous Driver/HotCallouts] Spawned suspectCar (" + suspectCar.Model.Name + ") and set to Persistent");

            suspect = suspectCar.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            Game.LogTrivial($"[Dangerous Driver/HotCallouts] Spawned suspect ({suspect.Model.Name}) and set to Persistent & Block Events");

            blip = suspect.AttachBlip();
            blip.IsFriendly = false;
            blip.IsRouteEnabled = true;
            blip.Name = "Reckless Driver";
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Spawned blip and renamed to Reckless Driver");

            situations = MathHelper.GetRandomInteger(0, 2);
            string message;
            string audioMessage;
            switch(situations)
            {
                default:
                case 0:
                    suspect.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.Emergency);
                    message = "driving all over the road, but with normal speed";
                    audioMessage = "CRIME_RECKLESS_DRIVER";
                    break;
                case 1:
                    suspect.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
                    message = "driving all over the road and overspeed";
                    audioMessage = "CRIME_SPEEDING_FELONY";
                    break;
                case 2:
                    suspect.Tasks.CruiseWithVehicle(45f, VehicleDrivingFlags.Normal);
                    message = "overspeeding";
                    audioMessage = "CRIME_SPEEDING_FELONY";
                    break;
            }
            ScannerHelper.ReportEvent(audioMessage);
            ScannerHelper.DisplayDispatchNote("We received a 911 report of a vehicle " + message + ". Respond with Code 3.");
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Done > Dangerous Driver");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
            if(!pursuited && Functions.IsPedInPursuit(suspect))
            {
                pursuited = true;
                Game.LogTrivial("[Dangerous Driver/HotCallouts] Fleeing > suspect");
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "suspect fleeing.");
                currentPursuit = Functions.GetActivePursuit();
            }

            if(pursuited && !Functions.IsPursuitStillRunning(currentPursuit))
            {
                pursuited = false;
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "The pursuit has ~g~concluded~s~.");
            }

            if(Game.IsKeyDown(Keys.End))
            {
                Game.LogTrivial("[Dangerous Driver/HotCallouts] End > Dangerous Driving");
                ScannerHelper.ReportNormalCode4("Dangerous Driving");
                End();
            }
        }

        public override void End()
        {
            base.End();
            Game.LogTrivial("[Dangerous Driver/HotCallouts] Ending Instance");

            if (suspect.Exists()) suspect.Dismiss();
            if (suspectCar.Exists()) suspectCar.Dismiss();
            if (blip.Exists()) blip.Delete();

            Game.LogTrivial("[Dangerous Driver/HotCallouts] Peds dismissed");
        }
    }
}
