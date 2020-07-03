// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

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
