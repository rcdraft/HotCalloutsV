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

using System;
using System.Reflection;
using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using Rage;

namespace HotCalloutsV
{
    public class Entry : Plugin
    {
        public override void Finally()
        {
            Game.LogTrivial("HotCallouts has been told to shutdown.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "HotCallouts", "Shutdown", "HotCallouts has been told to shutdown ~h~unexceptedly~s~. This is probably due to ~b~user commanded ~g~reload/unload~s~ or simply ~b~plugin~s~ or ~b~LSPDFR~s~ has ~r~crashed~s~.");
        }

        private void OnDutyChanged(bool onDuty)
        {
            if(onDuty)
            {
                RegisterCallouts();
                
            }
        }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnDutyChanged;

            Game.LogTrivial("[HotCallouts] Initializing Integreate Features");
#if DEBUG
            Game.LogTrivial($"[HotCallouts] Warning: This build ({Assembly.GetExecutingAssembly().GetName().Version}) is bulit with Debug build options.");
            Game.LogTrivial("[HotCallouts] It has features that has not production ready yet and most likely to crash the LSPDFR, the plugin itself, or the whole game.");
            Game.LogTrivial("[HotCallouts] USE AT YOUR OWN RISK.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "HotCallouts", Assembly.GetExecutingAssembly().GetName().Version + "(debug)", "This is a ~r~<b>debug</b>~s~ build!");
#endif
            AppDomain.CurrentDomain.AssemblyResolve += Common.Integreate.Resolve;
            Game.LogTrivial("[Integreate/HotCallouts] Wait 3 millseconds to get all plugins to load");
            GameFiber.Sleep(3);
            Game.LogTrivial("[Integreate/HotCallouts] Initializing Integerate Manager");
            Integreate.Initialize();
            Game.LogTrivial("[HotCallouts] [OK] Initialized > Integreate Features");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "HotCallouts", Assembly.GetExecutingAssembly().GetName().Version.ToString(), "~b~loaded ~g~successfully~s~!");
        }

        static void RegisterCallouts()
        {
#if DEBUG
            Functions.RegisterCallout(typeof(Callouts.DiamondCasinoTrouble));
#endif
            Functions.RegisterCallout(typeof(Callouts.CarThief));
            Functions.RegisterCallout(typeof(Callouts.DangerousDriver));
            Functions.RegisterCallout(typeof(Callouts.EscapingPrisoner));
            // Functions.RegisterCallout(typeof(Callouts.DocumentLack));
        }
    }
}
