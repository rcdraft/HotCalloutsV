using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HotCalloutsV.Common
{
    internal static class PedHelper
    {
        public static uint DeclareSubjectStatus(Ped ped)
        {
            string status;
            if(!ped.Exists())
            {
                status = "<font color=\"red\">escaped</font>";
            }
            else if(ped.IsDead)
            {
                status = "<font color=\"lime\">down</font>";
            }
            else if(Functions.IsPedArrested(ped))
            {
                status = "<font color=\"lime\">in custody</font>";
            }
            else
            {
                status = "<font color=\"blueviolet\">innocent</font>";
            }
            return ScannerHelper.DisplayDispatchDialogue("Dispatch", "Code 4, suspect is " + status);
        }
        
        public static uint DecalreSubjectInformation(Ped ped)
        {
            Persona p = Functions.GetPersonaForPed(ped);
            string license;
            switch(p.ELicenseState)
            {
                case ELicenseState.Valid:
                    license = "Valid";
                    break;
                default:
                case ELicenseState.None:
                case ELicenseState.Unlicensed:
                    license = "None";
                    break;
                case ELicenseState.Suspended:
                    license = "Suspended";
                    break;
                case ELicenseState.Expired:
                    license = "Expired";
                    break;
            }
            string wanted;
            if(p.Wanted)
            {
                wanted = "Suspect is <font color=\"red\">wanted</font>";
            }
            else
            {
                wanted = "has <font color=\"limegreen\">no warrants</font>";
            }
            return ScannerHelper.DisplayDispatchDialogue("Dispatch", $"Persona information: <br/> - License {license}<br/> - {wanted}.");
        }
    }
}
