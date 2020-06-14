using LSPD_First_Response.Mod.API;
using Rage;
using StopThePed.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Functions = LSPD_First_Response.Mod.API.Functions;
using STPFunctions = StopThePed.API.Functions;
using STPEvents = StopThePed.API.Events;

namespace HotCalloutsV.Common
{
    internal class Integreate
    {
        internal static bool StopThePed { get; private set; }
        internal static void Initialize()
        {
            Game.LogTrivial("[Integreate/HotCallouts] Starting Integrate Manager");
            Game.LogTrivial("[Integreate/HotCallouts] Resolving > StopThePed");
            if (File.Exists("plugins//LSPDFR//StopThePed.dll"))
            {
                Game.LogTrivial("[Integreate/HotCallouts] Resolved > StopThePed");
                StopThePed = true;
                STPEvents.pedArrestedEvent += PedHelper.STPArrestPed;
            }
            else
            {
                Game.LogVerbose("[Integreate/HotCallouts] Unresolved > StopThePed");
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
