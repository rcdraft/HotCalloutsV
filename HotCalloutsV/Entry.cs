using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;

namespace HotCalloutsV
{
    public class Entry : Plugin
    {
        public override void Finally()
        {
           

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
            Game.LogTrivial("HotCallouts loaded.");
        }


        // not implemented
        static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.CarThief));
            Functions.RegisterCallout(typeof(Callouts.DangerousDriver));
        }
    }
}
