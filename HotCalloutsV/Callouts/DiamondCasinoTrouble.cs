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
using HotCalloutsV.Entities;
using HotCalloutsV.Entities.Bases;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace HotCalloutsV.Callouts
{
    [CalloutInfo("DiamondCasinoTrouble", CalloutProbability.Low)]
    public class DiamondCasinoTrouble : Callout
    {
        Ped suspect;
        Ped security;
        Blip b;
        Blip susB;
        DialogueSubject ds;
        private Vector3 spawnPoint;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = new Vector3(933f, 48f, 81f);

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
            ds = new DialogueSubject(suspect);
            ds.SpeechAble.Add(0, new TextEntire("~b~You~s~: Hi. What are you doing here?"));
            ds.SpeechAble.Add(2, new TextEntire("~r~Suspect~s~: Why I need to tell you what I am doing?"));
            ds.SpeechAble.Add(3, new TextEntire("~b~Security~s~: He's capturing the casino using camera."));
            ds.SpeechAble.Add(4, new PursuitEntire("~r~Suspect~s~: Hell no!"));
            Game.DisplayHelp("Once you arrived, stand close to suspect and press Y.");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
            if(ds.CurrentCount != 4 && Game.LocalPlayer.Character.DistanceTo2D(suspect) <= 3f && Game.IsKeyDown(System.Windows.Forms.Keys.Y))
            {
                ds.Say();
            }
            if(!suspect.Exists() || suspect.IsDeadOrDetained())
            {
                End();
            }
        }

        public override void End()
        {
            PedHelper.DeclareSubjectStatus(suspect);
            if (suspect.Exists())
            {
                Game.LogTrivial("[DiamondCasinoTrouble/HotCallouts] Attmepting to dismiss suspect...");
                if (!Functions.IsPedArrested(suspect)) suspect.Dismiss();
                else Game.LogTrivial("[DiamondCasinoTrouble/HotCallouts] Suspect was not dismissed to prevent them taking over player vehicle.");
            }
            if (security.Exists()) security.Dismiss();
            if (susB.Exists()) susB.Delete();
            if (b.Exists()) b.Delete();
            base.End();
        }
    }
}
