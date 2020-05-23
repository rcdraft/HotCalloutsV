using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotCalloutsV.Callouts
{
    [CalloutInfo("FirearmAttackOnOfficer", CalloutProbability.Low)]
    public class FirearmAttackOnOfficer : Callout
    {
        private Vector3 spawnPoint;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(20f, spawnPoint);

            CalloutMessage = "Car Thief spotted";
            CalloutPosition = spawnPoint;

            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS WE_HAVE CRIME_SHOTS_FIRED_AT_OFFICER IN_OR_ON_POSITION", spawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
    }
}
