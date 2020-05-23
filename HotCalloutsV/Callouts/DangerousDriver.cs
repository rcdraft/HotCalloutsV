using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;

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

        public override bool OnBeforeCalloutDisplayed()
        {
            spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawn, 30f);
            AddMinimumDistanceCheck(20f, spawn);

            CalloutMessage = "Dangerous Driver";
            CalloutPosition = spawn;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_DANGEROUS_DRIVING IN_OR_ON_POSITION", spawn);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectCar = new Vehicle(spawn);
            suspectCar.IsPersistent = true;

            suspect = suspectCar.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            blip = suspect.AttachBlip();
            blip.IsFriendly = false;
            blip.IsRouteEnabled = true;
            blip.Name = "Reckless Driver";

            suspect.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.Emergency);

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
            if(!pursuited && Functions.IsPedInPursuit(suspect))
            {
                pursuited = true;
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "suspect fleeing.");
            }

            if(!suspect.Exists() || suspect.IsDead || Functions.IsPedArrested(suspect))
            {
                PedHelper.DeclareSubjectStatus(suspect);
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

            if (suspect.Exists()) suspect.Dismiss();
            if (suspectCar.Exists()) suspectCar.Dismiss();
            if (blip.Exists()) blip.Delete();
        }
    }
}
