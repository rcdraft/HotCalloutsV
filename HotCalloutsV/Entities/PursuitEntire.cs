// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using HotCalloutsV.Entities.Interfaces;
using LSPD_First_Response.Mod.API;
using Rage;
using System;

namespace HotCalloutsV.Entities
{
    public class PursuitEntire : ChatEntire
    {
        public string Context { get; private set; }

        public override bool IsFunctional => true;

        public override void Function() => throw new NotImplementedException();

        public override void Function(Ped p) => _ = Function(true, p);

        public LHandle Function(bool activeForPlayer, Ped p)
        {
            Game.DisplaySubtitle(Context);
            LHandle result = Functions.CreatePursuit();
            Functions.AddPedToPursuit(result, p);
            Functions.SetPursuitIsActiveForPlayer(result, activeForPlayer);
            return result;
        }

        public PursuitEntire(string text)
        {
            Context = text;
        }
    }
}
