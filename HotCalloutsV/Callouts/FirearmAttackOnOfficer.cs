// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System.Drawing;

namespace HotCalloutsV.Callouts
{
    [CalloutInfo("FirearmAttackOnOfficer", CalloutProbability.Low)]
    public class FirearmAttackOnOfficer : Callout
    {
        private Vector3 spawnPoint;
        private Ped officer;
        private Ped suspect;
        private Blip suspectBlip;
        private bool dead;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(20f, spawnPoint);

            CalloutMessage = "Shots fired at an Officer";
            CalloutPosition = spawnPoint;

            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS WE_HAVE CRIME_SHOTS_FIRED_AT_OFFICER IN_OR_ON_POSITION", spawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspect = new Ped(spawnPoint);
            suspect.BlockPermanentEvents = true;
            suspect.IsPersistent = true;
            officer = new Ped("s_m_y_cop_01", suspect.Position.Around(5f), suspect.Heading);
            officer.BlockPermanentEvents = true;
            officer.IsPersistent = true;
            Functions.SetPedAsCop(officer);
            officer.Inventory.GiveNewWeapon(WeaponHash.Pistol, short.MaxValue, true);
            officer.Tasks.FightAgainst(suspect);
            suspect.Inventory.GiveNewWeapon(WeaponHash.Pistol, short.MaxValue, true);
            suspect.Tasks.FightAgainst(officer);
            suspectBlip = suspect.AttachBlip();
            suspectBlip.Sprite = BlipSprite.Enemy;
            suspectBlip.IsFriendly = false;
            suspectBlip.IsRouteEnabled = true;
            suspectBlip.Color = Color.Red;
            ScannerHelper.DisplayDispatchNote("There's a suspect shooting at an officer. Quick, before anything goes wrong.");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if(!dead && officer.IsDead)
            {
                dead = true;
                ScannerHelper.DisplayDispatchDialogue("Dispatch", "Officer down. ~r~Respond with code 99~s~.");
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS WE_HAVE CRIME_OFFICER_DOWN IN_OR_ON_POSITION", officer.Position);
            }
            if(!suspect.Exists() || suspect.IsDeadOrDetained())
            {
                End();
            }
        }

        public override void End()
        {
            base.End();
            if (suspect.Exists() && !Functions.IsPedArrested(suspect)) suspect.Dismiss();
            if (officer.Exists()) officer.Dismiss();
        }
    }
}
