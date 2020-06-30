// Copyright (C) RelaperCrystal 2019, 2020
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
