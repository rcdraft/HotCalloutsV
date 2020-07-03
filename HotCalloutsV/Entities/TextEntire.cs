// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using HotCalloutsV.Entities.Interfaces;
using Rage;

namespace HotCalloutsV.Entities
{
    public class TextEntire : ChatEntire
    {
        public string Context { get; private set; }

        public override bool IsFunctional { get => false; }

        public override void Function()
        {
            Game.DisplaySubtitle(Context);
        }

        public override void Function(Ped p)
        {
            Function();
        }

        public TextEntire(string text)
        {
            Context = text;
        }

        public static implicit operator TextEntire(string text)
        {
            return new TextEntire(text);
        }
    }
}
