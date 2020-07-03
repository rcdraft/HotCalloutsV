// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using Rage;
using RAGENativeUI.Elements;
using StopThePed.API;
using Traffic_Policer;

namespace HotCalloutsV.Common
{
    internal static class CommonHelper
    {
        internal static BigMessageThread Message = new BigMessageThread(true);
        public static void DeclareUnexceptedEnd(string callMessage)
        {
            Message.MessageInstance.ShowSimpleShard("Oops!", $"Callout {callMessage} ended unexceptedly,");
        }
        public static void SetInsurance(this Vehicle vehicle, HCVehicleStatus status)
        {
            if (Integreate.StopThePed)
            {
                STPVehicleStatus status2;
                switch (status)
                {
                    case HCVehicleStatus.Valid:
                        status2 = STPVehicleStatus.Valid;
                        break;
                    default:
                    case HCVehicleStatus.None:
                        status2 = STPVehicleStatus.None;
                        break;
                    case HCVehicleStatus.Expired:
                        status2 = STPVehicleStatus.Expired;
                        break;
                }
                Functions.setVehicleInsuranceStatus(vehicle, status2);
            }
            if (Integreate.TrafficPolicer)
            {
                EVehicleDetailsStatus status3;
                switch (status)
                {
                    case HCVehicleStatus.Valid:
                        status3 = EVehicleDetailsStatus.Valid;
                        break;
                    default:
                    case HCVehicleStatus.None:
                        status3 = EVehicleDetailsStatus.None;
                        break;
                    case HCVehicleStatus.Expired:
                        status3 = EVehicleDetailsStatus.Expired;
                        break;
                }
                Traffic_Policer.API.Functions.SetVehicleInsuranceStatus(vehicle, status3);
            }
        }

        public static void SetRegistration(this Vehicle vehicle, HCVehicleStatus status)
        {
            if(Integreate.StopThePed)
            {
                STPVehicleStatus status2;
                switch(status)
                {
                    case HCVehicleStatus.Valid:
                        status2 = STPVehicleStatus.Valid;
                        break;
                    default:
                    case HCVehicleStatus.None:
                        status2 = STPVehicleStatus.None;
                        break;
                    case HCVehicleStatus.Expired:
                        status2 = STPVehicleStatus.Expired;
                        break;
                }
                Functions.setVehicleRegistrationStatus(vehicle, status2);
            }
            if(Integreate.TrafficPolicer)
            {
                Traffic_Policer.EVehicleDetailsStatus status3;
                switch (status)
                {
                    case HCVehicleStatus.Valid:
                        status3 = EVehicleDetailsStatus.Valid;
                        break;
                    default:
                    case HCVehicleStatus.None:
                        status3 = EVehicleDetailsStatus.None;
                        break;
                    case HCVehicleStatus.Expired:
                        status3 = EVehicleDetailsStatus.Expired;
                        break;
                }
                Traffic_Policer.API.Functions.SetVehicleRegistrationStatus(vehicle, status3);
            }
        }
    }
}

namespace HotCalloutsV
{
    public enum HCVehicleStatus
    {
        Valid,
        None,
        Expired
    }
}