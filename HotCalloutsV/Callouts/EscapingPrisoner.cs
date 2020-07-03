// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

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
        private LHandle pursuit;

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
				blip.Delete();
                pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(pursuit, suspect);
                Functions.AddPedToPursuit(pursuit, prisoner);
                Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit);
                Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
				ScannerHelper.DisplayDispatchDialogue("Dispatch", "Suspect fleeing. Sending backup and air unit.");
			}

            if(pursuited && pursuit != null && !Functions.IsPursuitStillRunning(pursuit))
            {
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "We are code 4 on Escaping Prisoner.");
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (prisoner.Exists()) prisoner.Dismiss();
            if (suspect.Exists()) suspect.Dismiss();
            if (suspectCar.Exists()) suspectCar.Dismiss();
        }
    }
}
