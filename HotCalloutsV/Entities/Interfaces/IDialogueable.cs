// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using System.Collections.Generic;

namespace HotCalloutsV.Entities.Interfaces
{
    internal interface IDialogueable
    {
        Dictionary<int, ChatEntire> SpeechAble { get; }
        int CurrentCount { get; set; }
        void Say();
        void Say(int specified);
    }
}
