using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace HotCalloutsV.Callouts
{
    [CalloutInfo("EscapingPrisoner", CalloutProbability.Low)]
    public class EscapingPrisoner : Callout
    {
        Ped suspect;
        Ped prisoner;
        Vehicle suspectCar;
        Vector3 spawn;
        Blip blip;
        private bool pursuited;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawn, 30f);
            AddMinimumDistanceCheck(20f, spawn);

            CalloutMessage = "Escaping Prisoner";
            CalloutPosition = spawn;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_WANTED_FELON_ON_THE_LOOSE IN_OR_ON_POSITION", spawn);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectCar = new Vehicle(spawn);
            suspectCar.IsPersistent = true;
            ScannerHelper.RandomiseLicencePlate(suspectCar);

            suspect = suspectCar.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            prisoner = new Ped("S_M_Y_PRISONER_01", suspect.Position.Around(5f), suspect.Heading);
            prisoner.IsPersistent = true;
            prisoner.WarpIntoVehicle(suspectCar, 0);
            prisoner.AttachBlip();

            blip = suspect.AttachBlip();
            blip.IsFriendly = false;
            blip.IsRouteEnabled = true;
            blip.Name = "Escaping Prisoner";

            suspect.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
            if (!pursuited && Game.LocalPlayer.Character.Position.DistanceTo2D(suspect) <= 10f)
            {
                pursuited = true;
                Game.DisplayHelp("Press End once you have taken all the necessary actions.");
                Game.DisplaySubtitle($"Pull the target~r~{Game.GetLocalizedString(suspectCar.Model.Name.ToUpper())}~s~ over.");
            }

            if(pursuited && Game.IsKeyDown(System.Windows.Forms.Keys.End))
            {
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "We are code 4 on Escaping Prisoner.");
                End();
            }

            /*if(!suspect == null || !suspect.Exists())
            {
                Game.DisplayNotification("<b>Dispatch: </b>Code 4, suspect has escaped.");
                End();
            }
            
            if(!suspect.IsAlive)
            {
                Game.DisplayNotification("<b>Dispatch: </b>Code 4, suspect down.");
                End();
            }
            if (Functions.IsPedArrested(suspect))
            {
                Game.DisplayNotification("<b>Dispatch: </b>Code 4, suspect in custody.");
                End();
            }
            */
        }

        public override void End()
        {
            base.End();

            if (prisoner.Exists()) prisoner.Dismiss();
            if (suspect.Exists()) suspect.Dismiss();
            if (suspectCar.Exists()) suspectCar.Dismiss();
            if (blip.Exists()) blip.Delete();
        }
    }
}
