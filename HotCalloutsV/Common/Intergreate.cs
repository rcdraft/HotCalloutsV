﻿// Copyright (C) RelaperCrystal 2019, 2020
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

using Rage;
using System;
using System.IO;
using System.Reflection;
using Functions = LSPD_First_Response.Mod.API.Functions;
using STPEvents = StopThePed.API.Events;

namespace HotCalloutsV.Common
{

    internal class Integreate
    {
        internal static bool TrafficPolicer { get; private set; }
        internal static bool LSPDFRPlus { get; private set; }
        internal static bool StopThePed { get; private set; }
        internal static void Initialize()
        {
            Game.LogTrivial("[Integreate/HotCallouts] Starting Integrate Manager");
            Game.LogTrivial("[Integreate/HotCallouts] Resolving > StopThePed");
            if (File.Exists(@"plugins\LSPDFR\StopThePed.dll"))
            {
                Game.LogTrivial("[Integreate/HotCallouts] Resolved > StopThePed");
                StopThePed = true;
                STPEvents.pedArrestedEvent += PedHelper.STPArrestPed;
            }
            else
            {
                Game.LogVerbose("[Integreate/HotCallouts] Unresolved > StopThePed");
            }
            Game.LogTrivial("[Integreate/HotCallouts] Resolving > LSPDFR+");
            if (File.Exists(@"plugins\LSPDFR\LSPDFR+.dll"))
            {
                Game.LogTrivial("[Integreate/HotCallouts] Resolved > LSPDFR+");
                LSPDFRPlus = true;
            }
            else
            {
                Game.LogTrivial("[Integreate/HotCallouts] Unresolved > LSPDFR+");
            }
            Game.LogTrivial("[Integreate/HotCallouts] Resolving > Traffic Policer");
            if(File.Exists(@"plugins\LSPDFR\Traffic Policer.dll"))
            {
                Game.LogTrivial("[Integreate/HotCallouts] Resolved > Traffic Policer");
                TrafficPolicer = true;
            }
            else
            {
                Game.LogTrivial("[Integreate/HotCallouts] Unresolved > Traffic Policer");
            }
            Game.LogTrivial("[Integreate/HotCallouts] Components Resolved");
    }

        internal static bool IsPluginRunning(string plugin, Version minimal = null)
        {
            foreach(Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if(an.Name.ToLower() == plugin.ToLower())
                {
                    if(minimal == null || an.Version.CompareTo(minimal) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static Assembly Resolve(object sender, ResolveEventArgs args)
        {
            foreach(Assembly assembly in Functions.GetAllUserPlugins())
            {
                if(args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }
    }
}
