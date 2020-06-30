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
using Rage;
using System;
using System.Collections.Generic;

namespace HotCalloutsV.Entities.Bases
{
    public class DialogueSubject : IDialogueable
    {
        public DialogueSubject(Ped f)
        {
            Functional = f;
            SpeechAble = new Dictionary<int, ChatEntire>();
        }

        public Dictionary<int, ChatEntire> SpeechAble { get; set; }
        public Ped Functional { get; private set; }

        public int CurrentCount { get; set; } = 0;

        public virtual void Say()
        {
            if(CurrentCount <= SpeechAble.Count)
            {
                ChatEntire ce;
                bool success = SpeechAble.TryGetValue(CurrentCount, out ce);
                if(!success)
                {
                    Functional.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                    Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "HotCallouts", "Dialogue Failed", "The dialog has failed.");
                    Game.LogTrivial("ERROR: Dialogue Failed: key " + CurrentCount);
                    Game.LogTrivial("ERROR: Remember, this is totally code error");
                }
                SpeechAble[CurrentCount].Function(Functional);
                CurrentCount++;
            }
        }

        public void Say(int specified)
        {
            if (specified <= SpeechAble.Count)
            {
                SpeechAble[specified].Function(Functional);
            }
            else
            {
                throw new ArgumentException("The specified dialogue does not exist.", "specified");
            }
        }
    }
}
