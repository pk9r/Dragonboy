using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Assets.src.e;
using Mod.Graphics;
using Mod.R;
using UnityEngine;

namespace Mod.AccountManager
{
    internal class InGameAccountManager : mScreen
    {
        internal class ActionListener : IActionListener
        {
            public void perform(int id, object obj)
            {
                if (id == 999)
                    GameCanvas.serverScreen.switchToMe();
                else
                    gI().switchToMe();
            }
        }

        static Texture2D earthOverlay = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(earthOverlay));
        static Texture2D namekOverlay = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(namekOverlay));
        static Texture2D saiyanOverlay = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(saiyanOverlay));

        static Command back = new Command("", new ActionListener(), 999, null);

        static bool isViewingAccountInfo = true;
        static float offsetViewAccountInfo;
        static float offsetScrollAccounts;
        static float offsetScrollAccountsTo;

        static int selectedAccountIndex = -1;
        static int scrollValue = 80;

        static readonly int ACCOUNT_HEIGHT = 90;

        static List<Account> accounts = new List<Account>();

        static InGameAccountManager instance;
        internal static InGameAccountManager gI()
        {
            if (instance == null)
                instance = new InGameAccountManager();
            return instance;
        }
    }
}