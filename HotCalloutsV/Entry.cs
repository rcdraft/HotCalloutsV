using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HotCalloutsV.Common;
using LSPD_First_Response.Mod.API;
using Newtonsoft.Json;
using Rage;

namespace HotCalloutsV
{
    public class Entry : Plugin
    {
        private bool covid;
        private bool first;

        public override void Finally()
        {
            Game.LogTrivial("HotCallouts has been told to shutdown.");
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
            Game.LogTrivial("[HotCallouts] Initialized basic components");
            Game.LogTrivial("[HotCallouts] Initializing Configuration");
            if (!File.Exists("HotCallouts.json"))
            {
                ConfigStruct initial = new ConfigStruct();
                initial.firstRun = true;
                initial.useCovid19Specific = true;
                File.WriteAllText("HotCallouts.json", JsonConvert.SerializeObject(initial));
            }
            Game.LogTrivial("[HotCallouts] [OK] Initialized > Configuration");
            ConfigStruct cfgStruct = JsonConvert.DeserializeObject<ConfigStruct>(File.ReadAllText("HotCallouts.json"));
            covid = cfgStruct.useCovid19Specific;
            first = cfgStruct.firstRun;
            Game.LogTrivial("[HotCallouts] Initializing Integreate Features");
#if DEBUG
            Game.LogTrivial($"[HotCallouts] Warning: This build ({Assembly.GetExecutingAssembly().GetName().Version}) is bulit with Debug build options.");
            Game.LogTrivial("[HotCallouts] This usually means it's contains untested codes and not production ready.");
            Game.LogTrivial("[HotCallouts] USE AT YOUR OWN RISK.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "HotCallouts", "build " + Assembly.GetExecutingAssembly().GetName().Version + "(debug)", "This is a ~r~<b>debug</b>~s~ build!");
#endif
            AppDomain.CurrentDomain.AssemblyResolve += Common.Integreate.Resolve;
            Game.LogTrivial("[Integreate/HotCallouts] Wait 3 millseconds to get all plugins to load");
            GameFiber.Wait(3);
            Game.LogTrivial("[Integreate/HotCallouts] Initializing Integerate Manager");
            Integreate.Initialize();
            Game.LogTrivial("[HotCallouts] [OK] Initialized > Integreate Features");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "HotCallouts", "build " + Assembly.GetExecutingAssembly().GetName().Version, "~g~loaded successfully!~s~");
        }

        static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.DiamondCasinoTrouble));
            Functions.RegisterCallout(typeof(Callouts.CarThief));
            Functions.RegisterCallout(typeof(Callouts.DangerousDriver));
            Functions.RegisterCallout(typeof(Callouts.EscapingPrisoner));
            // Functions.RegisterCallout(typeof(Callouts.DocumentLack));
        }
    }
}
