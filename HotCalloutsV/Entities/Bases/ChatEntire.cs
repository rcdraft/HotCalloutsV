// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using Rage;

namespace HotCalloutsV.Entities.Interfaces
{
    public abstract class ChatEntire
    {
        string Context { get; }
        public abstract bool IsFunctional { get; }
        public virtual void Function()
        {
            if (!string.IsNullOrEmpty(Context)) Game.DisplaySubtitle(Context);
        }

        public virtual void Function(Ped p)
        {
            Function();
        }
        
    }
}
