// Copyright (C) RelaperCrystal 2019, 2020
// This file is part of HotCallouts for Grand Theft Auto V.

// NO COMPILE OF THIS FILE
// As an result, no class can be created.
// I am still working on how to get user input.
// It can be used as call onscreen keyboard.

/*using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotCalloutsV.Entities
{
    public static class MenuFiber
    {
        private static MenuPool pool;
        private static UIMenu menu;
        private static UIMenuItem detain;
        private static UIMenuItem arrest;

        public static void CreateLoop()
        {
            pool = new MenuPool();
            menu = new UIMenu("HotCallouts", "Control Menu");
            detain = new UIMenuItem("Create Detain Report", "Creates an detain report.");
            arrest = new UIMenuItem("Create Arrest", "Reports an arrest.");
            menu.AddItem(detain);
            menu.AddItem(arrest);
            menu.RefreshIndex();
            detain.Activated += Detain_Activated;
            arrest.Activated += Arrest_Activated;
            pool.Add(menu);
            while(true)
            {
                pool.ProcessMenus();
                if(Game.IsKeyDown(System.Windows.Forms.Keys.F9) && Game.IsKeyDown(System.Windows.Forms.Keys.LControlKey))
                {
                    if(!pool.IsAnyMenuOpen())
                    {
                        menu.Visible = !menu.Visible;
                    }
                }
                GameFiber.Yield();
            }
        }

        private static void Arrest_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            
        }

        private static void Detain_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            
        }
    }
}
*/