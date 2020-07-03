// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

namespace HotCalloutsV.Entities
{
    public static class MenuFiber
    {
        private static MenuPool pool;
        private static UIMenu menu;
        private static UIMenuItem itemEndCall;
        private static UIMenuItem arrest;

        public static void CreateLoop()
        {
            pool = new MenuPool();
            menu = new UIMenu("HotCallouts", "Interaction Menu");
            itemEndCall = new UIMenuItem("End Call", "Ends current callout. All units will disengage and suspects will go away.");
            menu.AddItem(itemEndCall);
            menu.RefreshIndex();
            itemEndCall.Activated += Detain_Activated;
            pool.Add(menu);
            while(true)
            {
                pool.ProcessMenus();
                if(Game.IsKeyDown(System.Windows.Forms.Keys.Y) && Game.IsKeyDown(System.Windows.Forms.Keys.LControlKey))
                {
                    itemEndCall.Enabled = Functions.IsCalloutRunning();
                    if (!pool.IsAnyMenuOpen())
                    {
                        menu.Visible = !menu.Visible;
                    }
                }
                GameFiber.Yield();
            }
        }

        private static void Detain_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            if(Functions.IsCalloutRunning())
            {
                Game.LogTrivial("Player is ending " + Functions.GetCalloutName(Functions.GetCurrentCallout()) + "by force.");
                Functions.StopCurrentCallout();
                itemEndCall.Enabled = false;
            }
        }
    }
}