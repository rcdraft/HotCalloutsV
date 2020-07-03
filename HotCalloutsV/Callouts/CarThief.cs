// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using System;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using HotCalloutsV.Common;

namespace HotCalloutsV.Callouts
{
    [CalloutInfo("CarThief", CalloutProbability.High)]
    public class CarThief : Callout
    {
        Ped suspect;
        Vehicle suspectVehicle;
        Vector3 spawnPoint;
        Blip blip;
        // LHandle pursuit;
        bool approach = false;
        private bool inPursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(20f, spawnPoint);

            CalloutMessage = "Car Thief spotted";
            CalloutPosition = spawnPoint;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_PERSON_IN_A_STOLEN_VEHICLE IN_OR_ON_POSITION", spawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectVehicle = new Vehicle(spawnPoint);
            suspectVehicle.IsPersistent = true;

            suspect = suspectVehicle.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            blip = suspect.AttachBlip();
            blip.IsFriendly = false;
            blip.IsRouteEnabled = true;

            suspectVehicle.AlarmTimeLeft = new TimeSpan(0, 0, 30);

            suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);
            Functions.SetPedResistanceChance(suspect, 70f);
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(!approach && Game.LocalPlayer.Character.Position.DistanceTo(suspect) < 30f)
            {
                approach = true;
                Game.DisplayHelp("Perform a traffic stop to target " + suspectVehicle.Model.Name + ".");
            }

            /*
            if(!pursuited && Game.LocalPlayer.Character.Position.DistanceTo(suspect) < 30f)
            {
                pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(pursuit, suspect);
                Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                pursuited = true;
                ScannerHelper.DisplayDispatchDialogue("You", "To dispatch, suspect fleeing.");
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "Affirmtive, suspect plate " + suspectVehicle.LicensePlate.ToUpper() + ", vehicle " + suspectVehicle.Model.Name);
                Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            }

            if(pursuited && !Functions.IsPursuitStillRunning(pursuit))
            {
                PedHelper.DeclareSubjectStatus(suspect);
                End();
            }
            */
            if(!inPursuit && Functions.IsPedInPursuit(suspect))
            {
                inPursuit = true;
                ScannerHelper.DisplayDispatchDialogue("You", "To dispatch, suspect fleeing.");
            }
            if(!suspect.Exists() || suspect.IsDead || Functions.IsPedArrested(suspect))
            {
                PedHelper.DeclareSubjectStatus(suspect);
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (suspect.Exists()) suspect.Dismiss();
            if (suspectVehicle.Exists()) suspectVehicle.Dismiss();
            if (blip.Exists()) blip.Delete();
        }
    }
}
