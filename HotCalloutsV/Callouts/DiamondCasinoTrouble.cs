using HotCalloutsV.Common;
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
    [CalloutInfo("DiamondCasinoTrouble", CalloutProbability.Low)]
    public class DiamondCasinoTrouble : Callout
    {
        Ped suspect;
        Ped security;
        Blip b;
        Blip susB;
        private Vector3 spawnPoint;
        private bool approach;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = new Vector3(92f, 48f, 81f);

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(20f, spawnPoint);

            CalloutMessage = "Troublemaker at Diamond Casino";
            CalloutPosition = spawnPoint;

            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_CIVIL_DISTURBANCE DISPATCH_REQUEST_BACKUP CODE_2_HOT IN_OR_ON_POSITION", spawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            security = new Ped("s_m_m_security_01", new Vector3(933.0486f, 43.07027f, 81.09576f), 63f);
            security.IsPersistent = true;
            suspect = new Ped(new Vector3(935.1854f, 46.54966f, 81.09575f), 153f);
            suspect.IsPersistent = true;
            b = security.AttachBlip();
            b.IsFriendly = true;
            b.Sprite = BlipSprite.Friend;
            susB = suspect.AttachBlip();
            susB.IsFriendly = false;
            susB.Sprite = BlipSprite.Enemy;
            susB.IsRouteEnabled = true;
            Game.DisplayHelp("Once you arrived, stand close to suspect and press Y.");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
            if(!approach && Game.LocalPlayer.Character.DistanceTo2D(suspect) <= 3f && Game.IsKeyDown(System.Windows.Forms.Keys.Y))
            {
                approach = true;
                suspect.PlayAmbientSpeech("GENERIC_INSULT_HIGH");
                Functions.SetPedAsStopped(suspect, false);
                LHandle pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(pursuit, suspect);
                Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS OFFICERS_REPORT CRIME_RESIST_ARREST REQUEST_BACKUP", suspect.Position);
            }
            if(!suspect.Exists() || suspect.IsDead || Functions.IsPedArrested(suspect))
            {
                End();
            }
        }

        public override void End()
        {
            PedHelper.DeclareSubjectStatus(suspect);
            if (suspect.Exists()) suspect.Dismiss();
            if (security.Exists()) security.Dismiss();
            if (susB.Exists()) susB.Delete();
            if (b.Exists()) b.Delete();
            base.End();
        }
    }
}
