using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using RAGENativeUI.Elements;
using StopThePed.API;

namespace HotCalloutsV.Common
{
    internal static class CommonHelper
    {
        internal static BigMessageThread Message = new BigMessageThread(true);
        public static void DeclareUnexceptedEnd(string callMessage)
        {
            Message.MessageInstance.ShowSimpleShard("Oops!", $"Callout {callMessage} ended unexceptedly,");
        }
        public static void SetInsurance(this Vehicle vehicle, STPVehicleStatus status)
        {
            if(Integreate.StopThePed)
            {
                Functions.setVehicleInsuranceStatus(vehicle, status);
            }
        }

        public static void SetRegistration(this Vehicle vehicle, STPVehicleStatus status)
        {
            if(Integreate.StopThePed)
            {
                Functions.setVehicleRegistrationStatus(vehicle, status);
            }
        }
    }
}
