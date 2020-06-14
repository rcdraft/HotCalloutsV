using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            AppDomain.CurrentDomain.AssemblyResolve += Common.Integreate.Resolve;
            Game.LogTrivial("[Integreate/HotCallouts] Wait 5 millseconds to get all plugins to load");
            GameFiber.Wait(5);
            Game.LogTrivial("[Integreate/HotCallouts] Initializing Integerate Manager");
            Integreate.Initialize();
            Game.LogTrivial("[HotCallouts] [OK] Initialized > Integreate Features");
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
