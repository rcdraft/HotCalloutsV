using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using Rage.Native;
using StopThePed.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Functions = LSPD_First_Response.Mod.API.Functions;

namespace HotCalloutsV.Common
{
    internal static class PedHelper
    {
        internal static List<Ped> ArrestedBySTP = new List<Ped>();
        public static bool IsDeadOrDetained(this Ped ped) 
        {
            if (Functions.IsPedArrested(ped)) return true;
            return Integreate.StopThePed && ped.IsDead && ArrestedBySTP.Contains(ped);
        }

        public static uint DeclareSubjectStatus(Ped ped)
        {
            string status;
            string radioStatus;
            if(!ped.Exists())
            {
                status = "<font color=\"red\">escaped</font>";
                radioStatus = "SUSPECT_LAST_SEEN IN_OR_ON_POSITION ATTEMPT_FIND";
            }
            else if(Integreate.StopThePed && ped.IsDead && ArrestedBySTP.Contains(ped))
            {
                status = "<font color=\"lime\">in custody</font>";
                radioStatus = "SUSPECT_APPREHENDED";
            }
            else if(ped.IsDead)
            {
                status = "<font color=\"lime\">down</font>";
                radioStatus = "PASSIFIED";
            }
            else if(Functions.IsPedArrested(ped))
            {
                status = "<font color=\"lime\">in custody</font>";
                radioStatus = "SUSPECT_APPREHENDED";
            }
            else
            {
                status = "<font color=\"blueviolet\">innocent</font>";
                radioStatus = "";
            }
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS " + radioStatus + " NO_ADDITIONAL_SUPPORT", Game.LocalPlayer.Character.Position);
            return ScannerHelper.DisplayDispatchDialogue("Dispatch", "Code 4, suspect is " + status);
        }

        internal static void STPArrestPed(Ped ped)
        {
            if(ped.Exists() && ped.IsPersistent)
            {
                ArrestedBySTP.Add(ped);
            }
        }

        public static Vector3 GetNextPositionOnPavement(Vector3 position, bool onGround = true)
        {
                Vector3 result;
                bool success = NativeFunction.Natives.GET_SAFE_COORD_FOR_PED(position.X, position.Y, position.Z, onGround, out result, 0);
            if(success)
            {
                return result;
            }
            else
            {
                throw new Rage.Exceptions.InvalidHandleableException("Error when trying to access. Success was none.");
            }
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
