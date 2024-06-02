using System;
using System.Collections.Generic;
using System.Linq;
using Mod.Graphics;
using Mod.R;
using Newtonsoft.Json;
using UnityEngine;

namespace Mod.AccountManager
{
    internal class InGameAccountManager : mScreen
    {
        enum CommandType
        {
            CloseAccountManager = 1,
            AddAccount,
            EditAccount,
            ConfirmDeleteAccount,
            DeleteAccount,
            SelectAccountToLogin,
            OpenAccountManager = 7,
            FinishInputAccount,
            CloseInputAccount,
            EditCustomServer,
            FinishEditCustomServer,
            CancelEditCustomServer,
            ToggleHidePassword,
            MoveAccountUp,
            MoveAccountDown,
        }

        internal class ActionListener : IActionListener
        {
            static ActionListener instance;
            internal static ActionListener gI()
            {
                if (instance == null)
                    instance = new ActionListener();
                return instance;
            }

            public void perform(int id, object obj)
            {
                switch ((CommandType)id)
                {
                    case CommandType.OpenAccountManager:
                        InGameAccountManager.gI().switchToMe();
                        break;
                    case CommandType.CloseAccountManager:
                        GameCanvas.serverScreen.switchToMe();
                        break;
                    case CommandType.AddAccount:
                        isAddingAccount = true;
                        tfUser.isFocus = false;
                        tfPass.isFocus = false;
                        tfUser.setText("");
                        tfPass.setText("");
                        tfPass.setIputType(TField.INPUT_TYPE_PASSWORD);
                        toggleHidePassword.img.texture = show;
                        break;
                    case CommandType.EditAccount:
                        isEditingAccount = true;
                        tfUser.isFocus = false;
                        tfPass.isFocus = false;
                        tfUser.setText(accounts[scrollableMenuAccounts.CurrentItemIndex].Username);
                        tfPass.setText(accounts[scrollableMenuAccounts.CurrentItemIndex].Password);
                        tfPass.setIputType(TField.INPUT_TYPE_PASSWORD);
                        toggleHidePassword.img.texture = show;
                        Server server = accounts[scrollableMenuAccounts.CurrentItemIndex].Server;
                        if (server.IsCustomIP())
                        {
                            customServer = server;
                            selectServer.SelectedIndex = selectServer.Items.Count - 1;
                        }
                        else
                            selectServer.SelectedIndex = server.index;
                        break;
                    case CommandType.ConfirmDeleteAccount:
                        GameCanvas.startYesNoDlg(Strings.inGameAccountManagerConfirmDeleteAcc, new Command(mResources.YES, gI(), (int)CommandType.DeleteAccount, null), new Command(mResources.NO, 2001));
                        break;
                    case CommandType.DeleteAccount:
                        accounts.RemoveAt(scrollableMenuAccounts.CurrentItemIndex);
                        if (accounts.Count == 0)
                            scrollableMenuAccounts.CurrentItemIndex = -1;
                        else if (scrollableMenuAccounts.CurrentItemIndex > accounts.Count - 1)
                            scrollableMenuAccounts.CurrentItemIndex = accounts.Count - 1;
                        InfoDlg.hide();
                        GameCanvas.currentDialog = null;
                        SaveDataAccounts();
                        break;
                    case CommandType.SelectAccountToLogin:
                        selectedAccountIndex = scrollableMenuAccounts.CurrentItemIndex;
                        SaveDataAccounts();
                        Rms.saveRMSString("acc", SelectedAccount.Username);
                        Rms.saveRMSString("pass", SelectedAccount.Password);
                        Session_ME.gI().close();
                        Session_ME2.gI().close();
                        GameCanvas.connect();
                        GameCanvas.serverScreen.switchToMe();
                        break;
                    case CommandType.FinishInputAccount:
                        if (string.IsNullOrEmpty(tfUser.getText()))
                        {
                            GameCanvas.startOKDlg(mResources.userBlank);
                            break;
                        }
                        if (string.IsNullOrEmpty(tfPass.getText()))
                        {
                            GameCanvas.startOKDlg(mResources.passwordBlank + '!');
                            break;
                        }
                        if (selectServer.SelectedIndex == -1)
                        {
                            GameCanvas.startOKDlg(Strings.inGameAccountManagerServerBlank + '!');
                            break;
                        }
                        if (isAddingAccount)
                        {
                            Account acc = new Account()
                            {
                                Username = tfUser.getText(),
                                Password = tfPass.getText(),
                                Server = customServer,
                            };
                            if (selectServer.SelectedIndex != selectServer.Items.Count - 1)
                                acc.Server = defaultServers[selectServer.SelectedIndex];
                            if (scrollableMenuAccounts.CurrentItemIndex == -1)
                                accounts.Add(acc);
                            else
                                accounts.Insert(scrollableMenuAccounts.CurrentItemIndex + 1, acc);
                        }
                        else if (isEditingAccount)
                        {
                            Account acc = accounts[scrollableMenuAccounts.CurrentItemIndex];
                            string userName = tfUser.getText();
                            string password = tfPass.getText();
                            if (acc.Server == customServer && acc.Username == userName && acc.Password == password)
                                break;
                            accounts[scrollableMenuAccounts.CurrentItemIndex] = new Account()
                            {
                                Username = userName,
                                Password = password,
                                Server = customServer,
                            };
                        }
                        isAddingAccount = isEditingAccount = false;
                        SaveDataAccounts();
                        break;
                    case CommandType.CloseInputAccount:
                        isAddingAccount = isEditingAccount = false;
                        break;
                    case CommandType.EditCustomServer:
                        if (isEditingAccount && accounts[scrollableMenuAccounts.CurrentItemIndex].Server.IsCustomIP())
                        {
                            tfCustomServerName.setText(accounts[scrollableMenuAccounts.CurrentItemIndex].Server.name);
                            tfCustomServerAddress.setText(accounts[scrollableMenuAccounts.CurrentItemIndex].Server.hostnameOrIPAddress);
                            tfCustomServerPort.setText(accounts[scrollableMenuAccounts.CurrentItemIndex].Server.port.ToString());
                        }
                        isEditingCustomServer = true;
                        break;
                    case CommandType.FinishEditCustomServer:
                        if (string.IsNullOrEmpty(tfCustomServerName.getText()))
                        {
                            GameCanvas.startOKDlg(Strings.inGameAccountManagerServerNameBlank + '!');
                            break;
                        }
                        if (string.IsNullOrEmpty(tfCustomServerAddress.getText()))
                        {
                            GameCanvas.startOKDlg(Strings.inGameAccountManagerServerAddressBlank + '!');
                            break;
                        }
                        if (string.IsNullOrEmpty(tfCustomServerPort.getText()))
                        {
                            GameCanvas.startOKDlg(Strings.inGameAccountManagerServerPortBlank + '!');
                            break;
                        }
                        if (!int.TryParse(tfCustomServerPort.getText(), out int port))
                        {
                            GameCanvas.startOKDlg(Strings.inGameAccountManagerServerPortInvalid + '!');
                            break;
                        }
                        else if (port < ushort.MinValue || port > ushort.MaxValue)
                        {
                            GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, ushort.MinValue, ushort.MaxValue));
                            break;
                        }
                        customServer = new Server(tfCustomServerName.getText(), tfCustomServerAddress.getText(), (ushort)port);
                        isEditingCustomServer = false;
                        goto case CommandType.FinishInputAccount;
                    case CommandType.CancelEditCustomServer:
                        isEditingCustomServer = false;
                        break;
                    case CommandType.ToggleHidePassword:
                        tfPass.setIputType(tfPass.inputType == TField.INPUT_TYPE_PASSWORD ? TField.INPUT_TYPE_ANY : TField.INPUT_TYPE_PASSWORD);
                        toggleHidePassword.img.texture = tfPass.inputType == TField.INPUT_TYPE_PASSWORD ? show : hide;
                        break;
                    case CommandType.MoveAccountUp:
                        if (scrollableMenuAccounts.CurrentItemIndex > 0)
                        {
                            Account acc = accounts[scrollableMenuAccounts.CurrentItemIndex];
                            accounts.RemoveAt(scrollableMenuAccounts.CurrentItemIndex);
                            accounts.Insert(scrollableMenuAccounts.CurrentItemIndex - 1, acc);
                            scrollableMenuAccounts.CurrentItemIndex--;
                            SaveDataAccounts();
                        }
                        break;
                    case CommandType.MoveAccountDown:
                        if (scrollableMenuAccounts.CurrentItemIndex < accounts.Count - 1)
                        {
                            Account acc = accounts[scrollableMenuAccounts.CurrentItemIndex];
                            accounts.RemoveAt(scrollableMenuAccounts.CurrentItemIndex);
                            accounts.Insert(scrollableMenuAccounts.CurrentItemIndex + 1, acc);
                            scrollableMenuAccounts.CurrentItemIndex++;
                            SaveDataAccounts();
                        }
                        break;
                }
            }
        }

        static Texture2D earthOverlay = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(earthOverlay));
        static Texture2D namekOverlay = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(namekOverlay));
        static Texture2D saiyanOverlay = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(saiyanOverlay));
        static Texture2D add = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(add));
        static Texture2D hide = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(hide));
        static Texture2D show = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(show));
        static Texture2D up = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(up));
        static Texture2D down = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(down));
        static Dictionary<int, Texture2D> icons = new Dictionary<int, Texture2D>();

        static Command closeAccountManager = new Command("", ActionListener.gI(), (int)CommandType.CloseAccountManager, null)
        {
            imgFocus = new Image(),
        };
        static Command selectAccountToLogin;
        static Command addAccount;
        static Command editAccount;
        static Command deleteAccount;
        static Command finishInputAccount;
        static Command editCustomServer;
        static Command finishEditCustomServer;
        static Command cancelEditCustomServer;
        static Command closeInputAccount = new Command("", ActionListener.gI(), (int)CommandType.CloseInputAccount, null)
        {
            imgFocus = new Image(),
        };
        static Command cancelInputAccount;
        static Command toggleHidePassword;
        static Command moveAccountUp;
        static Command moveAccountDown;

        static ComboBox selectServer;

        internal static Account SelectedAccount => selectedAccountIndex == -1 ? null : accounts[selectedAccountIndex];
        static List<Account> accounts = new List<Account>();
        static ScrollableMenuItems<Account> scrollableMenuAccounts;

        static TField tfUser;
        static TField tfPass;
        static TField tfCustomServerName;
        static TField tfCustomServerAddress;
        static TField tfCustomServerPort;

        static Server[] defaultServers;
        static Server customServer;

        static int selectedAccountIndex = -1;
        static int currentAccountInfoX;

        static bool isAddingAccount;
        static bool isEditingAccount;
        static bool isEditingCustomServer;

        static readonly int TITLE_HEIGHT = 30;
        static readonly int ACCOUNT_HEIGHT = 34;
        static readonly int X_LIST_ACCOUNTS = 50;
        static readonly int Y_LIST_ACCOUNTS = 30;
        static readonly int DEFAULT_STEP_SCROLL = 70;
        static readonly int WIDTH_ACCOUNT_INFO = 160;
        static readonly int INPUT_ACCOUNT_WIDTH = 200;
        static readonly int INPUT_ACCOUNT_HEIGHT = 180;
        static readonly int SELECT_SERVER_WIDTH = 500;
        static readonly int SELECT_SERVER_HEIGHT = 300;

        //[name]:[address]:[port]:[language]:[typesv]:[isnew],...
        //Super 2:dragon11.teamobi.com:17001:0:1:1,0,0
        static InGameAccountManager instance;
        internal static InGameAccountManager gI()
        {
            if (instance == null)
                instance = new InGameAccountManager();
            return instance;
        }

        public override void paint(mGraphics g)
        {
            g.setColor(0);
            g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
            GameCanvas.paintBGGameScr(g);
            g.reset();
            PaintListAccounts(g);
            PaintCurrentAccountInfo(g);
            base.paint(g);
            if (isEditingCustomServer)
                PaintEditCustomServer(g);
            else if (isAddingAccount || isEditingAccount)
                PaintInputAccount(g);
        }

        public override void switchToMe()
        {
            if (defaultServers == null)
            {
                defaultServers = new Server[ServerListScreen.nameServer.Length];
                for (int i = 0; i < defaultServers.Length; i++)
                    defaultServers[i] = new Server(i);
            }
            isAddingAccount = isEditingAccount = false;
            GetAccountsArea(out int x, out int y, out int width, out int height, true);
            scrollableMenuAccounts.X = x;
            scrollableMenuAccounts.Y = y;
            scrollableMenuAccounts.Width = width;
            scrollableMenuAccounts.Height = height;
            scrollableMenuAccounts.Reset();
            editAccount = new Command(Strings.edit, ActionListener.gI(), (int)CommandType.EditAccount, null)
            {
                w = 50,
            };
            deleteAccount = new Command(Strings.delete, ActionListener.gI(), (int)CommandType.ConfirmDeleteAccount, null)
            {
                w = 50,
            };
            selectAccountToLogin = new Command(Strings.select, ActionListener.gI(), (int)CommandType.SelectAccountToLogin, null)
            {
                w = 50,
            };
            finishInputAccount = new Command(Strings.save, ActionListener.gI(), (int)CommandType.FinishInputAccount, null);
            editCustomServer = new Command(Strings.inGameAccountManagerEditServer, ActionListener.gI(), (int)CommandType.EditCustomServer, null);
            finishEditCustomServer = new Command(Strings.save, ActionListener.gI(), (int)CommandType.FinishEditCustomServer, null);
            cancelEditCustomServer = new Command(mResources.CANCEL, ActionListener.gI(), (int)CommandType.CancelEditCustomServer, null);
            cancelInputAccount = new Command(mResources.CANCEL, ActionListener.gI(), (int)CommandType.CloseInputAccount, null);
            selectServer = new ComboBox(mResources.server, ServerListScreen.nameServer.ToList().Append(Strings.custom).ToList());

            tfUser = new TField
            {
                name = ((mResources.language != 2) ? (mResources.phone + "/") : string.Empty) + mResources.email,
                width = INPUT_ACCOUNT_WIDTH - 10,
                height = ITEM_HEIGHT + 2,
            };
            tfPass = new TField
            {
                name = mResources.password,
                width = INPUT_ACCOUNT_WIDTH - 10,
                height = ITEM_HEIGHT + 2,
            };
            tfUser.setIputType(TField.INPUT_TYPE_ANY);
            tfPass.setIputType(TField.INPUT_TYPE_PASSWORD);

            tfCustomServerName = new TField
            {
                name = Strings.inGameAccountManagerServerName,
                width = SELECT_SERVER_WIDTH - 10,
                height = ITEM_HEIGHT + 2,
            };
            tfCustomServerAddress = new TField
            {
                name = Strings.inGameAccountManagerServerAddress,
                width = SELECT_SERVER_WIDTH - 10,
                height = ITEM_HEIGHT + 2,
            };
            tfCustomServerPort = new TField
            {
                name = Strings.inGameAccountManagerServerPort,
                width = SELECT_SERVER_WIDTH - 10,
                height = ITEM_HEIGHT + 2,
            };
            tfCustomServerName.setIputType(TField.INPUT_TYPE_ANY);
            tfCustomServerAddress.setIputType(TField.INPUT_TYPE_ANY);
            tfCustomServerPort.setIputType(TField.INPUT_TYPE_NUMERIC);

            UpdateSizeAndPos();
            base.switchToMe();
        }

        public override void update()
        {
            GameScr.cmx++;
            if (GameScr.cmx > GameCanvas.w * 3 + 100)
                GameScr.cmx = 100;
            if (isEditingCustomServer)
            {
                tfCustomServerName.update();
                tfCustomServerAddress.update();
                tfCustomServerPort.update();
            }
            else if (isAddingAccount || isEditingAccount)
            {
                tfUser.update();
                tfPass.update();
            }
            scrollableMenuAccounts.Update();
            selectServer.Update();
            GetAccountsArea(out _, out _, out int width, out int height, true);
            scrollableMenuAccounts.Width = width;
            scrollableMenuAccounts.Height = height;
            if (scrollableMenuAccounts.CurrentItemIndex != -1)
            {
                int minX = GameCanvas.w - WIDTH_ACCOUNT_INFO - 40;
                if (currentAccountInfoX > minX)
                {
                    if (currentAccountInfoX - 50 < minX)
                        currentAccountInfoX = minX;
                    else
                        currentAccountInfoX -= 50;
                }
            }
            else
            {
                if (currentAccountInfoX < GameCanvas.w)
                {
                    if (currentAccountInfoX + 50 > GameCanvas.w)
                        currentAccountInfoX = GameCanvas.w;
                    else
                        currentAccountInfoX += 50;
                }
            }
            UpdateButtonsPos();
            base.update();
        }

        public override void updateKey()
        {
            if (isEditingCustomServer)
                UpdateKeyEditCustomServer();
            else if (isAddingAccount || isEditingAccount)
                UpdateKeyInputAccount();
            else
                UpdateKeyMain();
            base.updateKey();
            GameCanvas.clearKeyPressed();
        }

        public override void keyPress(int keyCode)
        {
            if (isEditingCustomServer)
            {
                if (tfCustomServerName.isFocus)
                    tfCustomServerName.keyPressed(keyCode);
                else if (tfCustomServerAddress.isFocus)
                    tfCustomServerAddress.keyPressed(keyCode);
                else if (tfCustomServerPort.isFocus)
                    tfCustomServerPort.keyPressed(keyCode);
            }
            else if (isAddingAccount || isEditingAccount)
            {
                if (tfUser.isFocus)
                    tfUser.keyPressed(keyCode);
                else if (tfPass.isFocus)
                    tfPass.keyPressed(keyCode);
            }
            base.keyPress(keyCode);
        }

        static void LoadDataAccounts()
        {
            if (Utils.TryLoadDataString("account_manager_accounts", out string jsonData))
                accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();
            if (Utils.TryLoadDataInt("account_manager_selected_account_index", out int value))
                selectedAccountIndex = value;
        }

        static void SaveDataAccounts()
        {
            Utils.SaveData("account_manager_accounts", JsonConvert.SerializeObject(accounts));
            Utils.SaveData("account_manager_selected_account_index", selectedAccountIndex);
        }

        static void PaintListAccounts(mGraphics g)
        {
            GetAccountsArea(out int x, out int y, out int width, out int height, false);
            PopUp.paintPopUp(g, x - 10, y - 10, width + 20, TITLE_HEIGHT + 50, 0, true);
            mFont.tahoma_7b_dark.drawString(g, Strings.accounts, x + 10, y, 0);
            addAccount.paint(g);
            if (scrollableMenuAccounts.CurrentItemIndex > -1)
            {
                moveAccountDown.paint(g);
                moveAccountUp.paint(g);
            }
            PopUp.paintPopUp(g, x - 10, y + TITLE_HEIGHT - 10, width + 20, height - TITLE_HEIGHT + 20, 0, true);
            GetAccountsArea(out _, out y, out _, out height, true);
            if (accounts.Count > height / ACCOUNT_HEIGHT)
            {
                g.setColor(0x998978);
                int scrollThumbHeight = height * height / ACCOUNT_HEIGHT / accounts.Count;
                int scrollThumbY = y + Mathf.Clamp(height * scrollableMenuAccounts.CurrentOffset / ACCOUNT_HEIGHT / accounts.Count, 0, height - scrollThumbHeight);
                g.fillRect(x + width + 3, scrollThumbY, 4, scrollThumbHeight);
            }
            g.setColor(0xD3A46F);
            g.fillRect(x, y, width, height);
            g.setColor(Color.black);
            g.drawRect(x - 1, y - 1, width + 1, height + 1);
            g.setClip(x, y, width, height);
            scrollableMenuAccounts.Paint(g);
            g.reset();
            closeAccountManager.paint(g);
        }

        static void PaintCurrentAccountInfo(mGraphics g)
        {
            if (currentAccountInfoX == GameCanvas.w)
                return;
            int y = Y_LIST_ACCOUNTS;
            int height = GameCanvas.h - y * 2;
            PopUp.paintPopUp(g, currentAccountInfoX - 10, y - 10, WIDTH_ACCOUNT_INFO + 20, height + 20, 0, true);
            height -= editAccount.h;
            PopUp.paintPopUp(g, currentAccountInfoX, y, WIDTH_ACCOUNT_INFO, height, 0xffffff, false);
            editAccount.paint(g);
            deleteAccount.paint(g);
            selectAccountToLogin.paint(g);
            g.setClip(currentAccountInfoX, y, WIDTH_ACCOUNT_INFO, height);
            if (scrollableMenuAccounts.CurrentItemIndex == -1)
                return;
            Account account = accounts[scrollableMenuAccounts.CurrentItemIndex];
            EnsureSmallImage(account.Info.Icon);
            int offsetXAccountMainInfo = currentAccountInfoX;
            Texture2D icon = icons[account.Info.Icon];
            if (icon != null)
            {
                float iconWidth = ACCOUNT_HEIGHT * icon.width / (float)icon.height;
                offsetXAccountMainInfo += 65;
                DrawTexture(currentAccountInfoX + 10, y + 10, iconWidth, ACCOUNT_HEIGHT, icon, ScaleMode.ScaleToFit);
                if (account.PetInfo != null)
                {
                    EnsureSmallImage(account.PetInfo.Icon);
                    Texture2D petIcon = icons[account.PetInfo.Icon];
                    if (petIcon != null)
                    {
                        float petIconWidth = ACCOUNT_HEIGHT / 2f * petIcon.width / petIcon.height;
                        offsetXAccountMainInfo -= ACCOUNT_HEIGHT / 2;
                        offsetXAccountMainInfo += 30;
                        DrawTexture(currentAccountInfoX + 47, y + 10 + ACCOUNT_HEIGHT / 2f, petIconWidth, ACCOUNT_HEIGHT / 2f, petIcon, ScaleMode.ScaleToFit);
                    }
                }
            }
            else
                offsetXAccountMainInfo += 20;
            mFont.tahoma_7b_dark.drawString(g, account.Info.Name, offsetXAccountMainInfo, y + 5, 0);
            string serverName = account.Server.IsCustomIP() ? account.Server.name : ServerListScreen.nameServer[account.Server.index];
            string lastTimeLogin = (account.LastTimeLogin != DateTime.MinValue ? (Strings.lastLogin + ": ") : "") + account.GetLastTimeLogin();
            if (mFont.tahoma_7_greySmall.getWidth(lastTimeLogin) + offsetXAccountMainInfo - currentAccountInfoX > WIDTH_ACCOUNT_INFO)
            {
                //g.setColor(new Color(0, 0, 0, .3f));
                //g.fillRect(offsetXAccountMainInfo - 1, y + 19, 75, 1);
                mFont.tahoma_7_greySmall.drawString(g, mResources.server + ' ' + serverName, offsetXAccountMainInfo, y + 15, 0);
                mFont.tahoma_7_greySmall.drawString(g, Strings.lastLogin + ":", offsetXAccountMainInfo, y + 25, 0);
                mFont.tahoma_7_greySmall.drawString(g, account.GetLastTimeLogin(), offsetXAccountMainInfo, y + 35, 0);
            }
            else
            {
                //g.setColor(new Color(0, 0, 0, .3f));
                //g.fillRect(offsetXAccountMainInfo - 1, y + 24, 75, 1);
                mFont.tahoma_7_greySmall.drawString(g, mResources.server + ' ' + serverName, offsetXAccountMainInfo, y + 15, 0);
                mFont.tahoma_7_greySmall.drawString(g, lastTimeLogin, offsetXAccountMainInfo, y + 25, 0);
            }

            g.setColor(new Color(0, 0, 0, .3f));
            g.fillRect(currentAccountInfoX + 20, y + 50, WIDTH_ACCOUNT_INFO - 40, 1);
            mFont.tahoma_7b_dark.drawString(g, Strings.info, currentAccountInfoX + WIDTH_ACCOUNT_INFO / 2 - mFont.tahoma_7b_dark.getWidth(Strings.info) / 2, y + 55, mFont.LEFT);
            int offsetY = 65;
            int offsetX = currentAccountInfoX;
            if (account.PetInfo != null)
            {
                offsetX += 10;
                int center = currentAccountInfoX + WIDTH_ACCOUNT_INFO / 2;
                mFont.tahoma_7b_focus.drawString(g, Strings.master, currentAccountInfoX + WIDTH_ACCOUNT_INFO / 4 - mFont.tahoma_7b_focus.getWidth(Strings.master) / 2, y + offsetY, 0);
                mFont.tahoma_7b_focus.drawString(g, mResources.pet, currentAccountInfoX + WIDTH_ACCOUNT_INFO * 3 / 4 - mFont.tahoma_7b_focus.getWidth(mResources.pet) / 2, y + offsetY, 0);
                offsetY += 5;
                g.fillRect(center, y + offsetY, 1, 58);
                offsetY += 7;
                mFont.tahoma_7_greySmall.drawString(g, "CharID: " + account.Info.CharID, currentAccountInfoX + 10, y + offsetY, 0);
                mFont.tahoma_7_greySmall.drawString(g, Strings.name + ": " + account.PetInfo.Name, currentAccountInfoX + 10 + WIDTH_ACCOUNT_INFO / 2, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, mResources.HP + ": " + Utils.FormatWithSIPrefix(account.Info.MaxHP), currentAccountInfoX + 10, y + offsetY, 0);
                mFont.tahoma_7_greySmall.drawString(g, mResources.HP + ": " + Utils.FormatWithSIPrefix(account.PetInfo.MaxHP), currentAccountInfoX + 10 + WIDTH_ACCOUNT_INFO / 2, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, mResources.KI + ": " + Utils.FormatWithSIPrefix(account.Info.MaxMP), currentAccountInfoX + 10, y + offsetY, 0);
                mFont.tahoma_7_greySmall.drawString(g, mResources.KI + ": " + Utils.FormatWithSIPrefix(account.PetInfo.MaxMP), currentAccountInfoX + 10 + WIDTH_ACCOUNT_INFO / 2, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, mResources.power + ": " + Utils.FormatWithSIPrefix(account.Info.EXP), currentAccountInfoX + 10, y + offsetY, 0);
                mFont.tahoma_7_greySmall.drawString(g, mResources.power + ": " + Utils.FormatWithSIPrefix(account.PetInfo.EXP), currentAccountInfoX + 10 + WIDTH_ACCOUNT_INFO / 2, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, Strings.gender + ": " + account.Info.GetGender(), currentAccountInfoX + 10, y + offsetY, 0);
                mFont.tahoma_7_greySmall.drawString(g, Strings.gender + ": " + account.PetInfo.GetGender(), currentAccountInfoX + 10 + WIDTH_ACCOUNT_INFO / 2, y + offsetY, 0);
                offsetY += 10;
            }
            else
            {
                offsetY += 2;
                offsetX += 20;
                mFont.tahoma_7_greySmall.drawString(g, "CharID: " + account.Info.CharID, currentAccountInfoX + 20, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, mResources.HP + ": " + Utils.FormatWithSIPrefix(account.Info.MaxHP), currentAccountInfoX + 20, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, mResources.KI + ": " + Utils.FormatWithSIPrefix(account.Info.MaxMP), currentAccountInfoX + 20, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, mResources.power + ": " + Utils.FormatWithSIPrefix(account.Info.EXP), currentAccountInfoX + 20, y + offsetY, 0);
                offsetY += 10;
                mFont.tahoma_7_greySmall.drawString(g, Strings.gender + ": " + account.Info.GetGender(), currentAccountInfoX + 20, y + offsetY, 0);
                offsetY += 10;
            }
            offsetY += 5;

            g.drawImage(Panel.imgXu, offsetX, y + offsetY);
            offsetX += Panel.imgXu.getWidth() + 5;
            mFont.tahoma_7_greySmall.drawString(g, Utils.FormatWithSIPrefix(account.Gold), offsetX, y + offsetY, 0);
            offsetX += mFont.tahoma_7_greySmall.getWidth(Utils.FormatWithSIPrefix(account.Gold)) + 5;
            g.drawImage(Panel.imgLuong, offsetX, y + offsetY);
            offsetX += Panel.imgLuong.getWidth() + 5;
            mFont.tahoma_7_greySmall.drawString(g, Utils.FormatWithSIPrefix(account.Gem), offsetX, y + offsetY, 0);
            offsetX += mFont.tahoma_7_greySmall.getWidth(Utils.FormatWithSIPrefix(account.Gem)) + 5;
            g.drawImage(Panel.imgLuongKhoa, offsetX, y + offsetY);
            offsetX += Panel.imgLuongKhoa.getWidth() + 5;
            mFont.tahoma_7_greySmall.drawString(g, Utils.FormatWithSIPrefix(account.Ruby), offsetX, y + offsetY, 0);
            offsetY += 15;
            g.fillRect(currentAccountInfoX + 20, y + offsetY, WIDTH_ACCOUNT_INFO - 40, 1);

        }

        static void PaintInputAccount(mGraphics g)
        {
            g.setColor(new Color(0, 0, 0, .5f));
            g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
            GetInputAccountArea(out int x, out int y, out int width, out int height, false);
            PopUp.paintPopUp(g, x, y, width, TITLE_HEIGHT + 10, -1, true);
            if (isAddingAccount)
                mFont.tahoma_7b_dark.drawString(g, Strings.inGameAccountManagerAddAccount, x + width / 2, y + 10, mFont.CENTER);
            else if (isEditingAccount)
                mFont.tahoma_7b_dark.drawString(g, Strings.inGameAccountManagerEditAccount, x + width / 2, y + 10, mFont.CENTER);
            closeInputAccount.paint(g);
            toggleHidePassword.paint(g);
            GetInputAccountArea(out _, out y, out _, out height, true);
            PopUp.paintPopUp(g, x, y, width, height, -1, true);
            tfUser.paint(g);
            tfPass.paint(g);
            g.reset();
            bool shouldEditServer = !selectServer.IsShowingListItems && selectServer.SelectedIndex == selectServer.Items.Count - 1;
            if (shouldEditServer)
                editCustomServer.paint(g);
            else
                finishInputAccount.paint(g);
            cancelInputAccount.paint(g);
            selectServer.Paint(g);
            g.reset();
        }

        static void PaintEditCustomServer(mGraphics g)
        {
            g.setColor(new Color(0, 0, 0, .75f));
            g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
            GetInputAccountArea(out int x, out int y, out int width, out int height, false);
            PopUp.paintPopUp(g, x, y, width, height, -1, true);
            mFont.tahoma_7b_dark.drawString(g, Strings.inGameAccountManagerEditServer, x + width / 2, y + 10, mFont.CENTER);
            GetInputAccountArea(out _, out y, out _, out height, true);
            PopUp.paintPopUp(g, x, y, width, height, -1, true);
            tfCustomServerName.paint(g);
            tfCustomServerAddress.paint(g);
            tfCustomServerPort.paint(g);
            g.reset();
            finishEditCustomServer.paint(g);
            cancelEditCustomServer.paint(g);
        }

        static void UpdateKeyInputAccount()
        {
            selectServer.UpdateKey();
            if (selectServer.IsShowingListItems)
                return;
            if (closeInputAccount.isPointerPressInside())
                closeInputAccount.performAction();
            if (toggleHidePassword.isPointerPressInside())
                toggleHidePassword.performAction();
            bool shouldEditServer = !selectServer.IsShowingListItems && selectServer.SelectedIndex == selectServer.Items.Count - 1;
            if (shouldEditServer)
            {
                if (editCustomServer.isPointerPressInside())
                    editCustomServer.performAction();
            }
            else
            {
                if (finishInputAccount.isPointerPressInside())
                    finishInputAccount.performAction();
            }
            if (cancelInputAccount.isPointerPressInside())
                cancelInputAccount.performAction();
            if (GameCanvas.keyPressed[16])
            {
                GameCanvas.clearKeyPressed();
                if (tfUser.isFocus)
                {
                    tfUser.isFocus = false;
                    tfPass.isFocus = true;
                }
                else if (tfPass.isFocus)
                {
                    tfPass.isFocus = false;
                    selectServer.IsFocus = true;
                }
                else if (selectServer.IsFocus)
                    selectServer.IsFocus = false;
                else
                    tfUser.isFocus = true;
            }
        }

        static void UpdateKeyEditCustomServer()
        {
            if (finishEditCustomServer.isPointerPressInside())
                finishEditCustomServer.performAction();
            if (cancelEditCustomServer.isPointerPressInside())
                cancelEditCustomServer.performAction();
            if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] || GameCanvas.keyPressed[16])
            {
                GameCanvas.clearKeyPressed();
                if (tfCustomServerName.isFocus)
                {
                    tfCustomServerName.isFocus = false;
                    tfCustomServerAddress.isFocus = true;
                }
                else if (tfCustomServerAddress.isFocus)
                {
                    tfCustomServerAddress.isFocus = false;
                    tfCustomServerPort.isFocus = true;
                }
                else if (tfCustomServerPort.isFocus)
                    tfCustomServerPort.isFocus = false;
                else
                    tfCustomServerName.isFocus = true;
            }
        }

        static void UpdateKeyMain()
        {
            GetAccountsArea(out int x, out int y, out int width, out int height, true);
            scrollableMenuAccounts.UpdateKey();
            if (closeAccountManager.isPointerPressInside())
                closeAccountManager.performAction();
            if (editAccount.isPointerPressInside())
                editAccount.performAction();
            if (deleteAccount.isPointerPressInside())
                deleteAccount.performAction();
            if (selectAccountToLogin.isPointerPressInside())
                selectAccountToLogin.performAction();
            if (addAccount.isPointerPressInside())
                addAccount.performAction();
            if (scrollableMenuAccounts.CurrentItemIndex > -1)
            {
                if (moveAccountUp.isPointerPressInside())
                    moveAccountUp.performAction();
                if (moveAccountDown.isPointerPressInside())
                    moveAccountDown.performAction();
            }

            if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
            {
                if (scrollableMenuAccounts.CurrentItemIndex != -1)
                    selectAccountToLogin.performAction();
            }
        }

        internal static void UpdateSizeAndPos()
        {
            if (scrollableMenuAccounts.CurrentItemIndex != -1)
                currentAccountInfoX = GameCanvas.w - WIDTH_ACCOUNT_INFO - 40;
            else
                currentAccountInfoX = GameCanvas.w;
            UpdateButtonsPos();
            GetInputAccountArea(out int x, out int y, out int width, out int height, true);
            tfUser.x = tfPass.x = selectServer.X = tfCustomServerName.x = tfCustomServerAddress.x = tfCustomServerPort.x = x + 10;
            tfUser.y = tfCustomServerName.y = y + 15;
            tfPass.y = tfCustomServerAddress.y = tfUser.y + ITEM_HEIGHT + 15;
            selectServer.Y = tfCustomServerPort.y = tfPass.y + ITEM_HEIGHT + 15;
            tfUser.width = tfPass.width = selectServer.Width = tfCustomServerName.width = tfCustomServerAddress.width = tfCustomServerPort.width = width - 20;
            GetAccountsArea(out _, out _, out width, out height, true);
            scrollableMenuAccounts.Width = width;
            scrollableMenuAccounts.Height = height;
        }

        static void UpdateButtonsPos()
        {
            closeAccountManager.x = GameCanvas.w - 63;
            closeAccountManager.y = addAccount.y = moveAccountDown.y = moveAccountUp.y = 25;
            int currentAccountInfoWidth = GameCanvas.w - currentAccountInfoX;
            if (currentAccountInfoWidth > 0)
                closeAccountManager.x -= currentAccountInfoWidth - 20;
            addAccount.x = closeAccountManager.x - addAccount.w - 5;
            moveAccountDown.x = addAccount.x - moveAccountUp.w - 5;
            moveAccountUp.x = moveAccountDown.x - moveAccountUp.w - 5;

            editAccount.y = deleteAccount.y = selectAccountToLogin.y = GameCanvas.h - 50;
            editAccount.x = currentAccountInfoX;
            deleteAccount.x = currentAccountInfoX + (WIDTH_ACCOUNT_INFO - deleteAccount.w) / 2;
            selectAccountToLogin.x = currentAccountInfoX + WIDTH_ACCOUNT_INFO - selectAccountToLogin.w;

            GetInputAccountArea(out int x, out int y, out int width, out int height, false);
            closeInputAccount.y = toggleHidePassword.y = y + 5;
            closeInputAccount.x = x + width - closeInputAccount.img.getWidth() - 5;
            toggleHidePassword.x = closeInputAccount.x - toggleHidePassword.img.getWidth() - 5;

            finishInputAccount.x = editCustomServer.x = finishEditCustomServer.x = x + 10;
            cancelInputAccount.x = cancelEditCustomServer.x = x + width - cancelInputAccount.w - 10;
            finishInputAccount.y = cancelInputAccount.y = editCustomServer.y = finishEditCustomServer.y = cancelEditCustomServer.y = y + height - finishInputAccount.h - 10;
            finishInputAccount.w = editCustomServer.w = cancelInputAccount.w = finishEditCustomServer.w = cancelEditCustomServer.w = (width - 30) / 2;
        }

        internal static void OnStart()
        {
            closeAccountManager.img = closeInputAccount.img = GameCanvas.loadImage("/mainImage/myTexture2dbtX.png");
            add = CustomGraphics.Resize(add, add.width * mGraphics.zoomLevel / 4, add.height * mGraphics.zoomLevel / 4);
            hide = CustomGraphics.Resize(hide, hide.width * mGraphics.zoomLevel / 4, hide.height * mGraphics.zoomLevel / 4);
            show = CustomGraphics.Resize(show, show.width * mGraphics.zoomLevel / 4, show.height * mGraphics.zoomLevel / 4);
            up = CustomGraphics.Resize(up, up.width * mGraphics.zoomLevel / 4, up.height * mGraphics.zoomLevel / 4);
            down = CustomGraphics.Resize(down, down.width * mGraphics.zoomLevel / 4, down.height * mGraphics.zoomLevel / 4);
            LoadDataAccounts();
            scrollableMenuAccounts = new ScrollableMenuItems<Account>(accounts)
            {
                PaintItemAction = PaintAccount,
                CurrentItemIndex = -1,
                ItemHeight = ACCOUNT_HEIGHT,
                StepScroll = DEFAULT_STEP_SCROLL,
            };
            addAccount = new Command("", ActionListener.gI(), (int)CommandType.AddAccount, null)
            {
                img = Image.createImage(add.width, add.height),
                imgFocus = new Image(),
                w = add.width / mGraphics.zoomLevel,
                h = add.height / mGraphics.zoomLevel
            };
            addAccount.img.texture = add;
            toggleHidePassword = new Command("", ActionListener.gI(), (int)CommandType.ToggleHidePassword, null)
            {
                img = Image.createImage(show.width, show.height),
                imgFocus = new Image(),
                w = show.width / mGraphics.zoomLevel,
                h = show.height / mGraphics.zoomLevel
            };
            toggleHidePassword.img.texture = show;
            moveAccountUp = new Command("", ActionListener.gI(), (int)CommandType.MoveAccountUp, null)
            {
                img = Image.createImage(up.width, up.height),
                imgFocus = new Image(),
                w = up.width / mGraphics.zoomLevel,
                h = up.height / mGraphics.zoomLevel
            };
            moveAccountUp.img.texture = up;
            moveAccountDown = new Command("", ActionListener.gI(), (int)CommandType.MoveAccountDown, null)
            {
                img = Image.createImage(down.width, down.height),
                imgFocus = new Image(),
                w = down.width / mGraphics.zoomLevel,
                h = down.height / mGraphics.zoomLevel
            };
            moveAccountDown.img.texture = down;
        }

        internal static void OnCloseAndPause()
        {
            SaveDataAccounts();
        }

        static void PaintAccount(mGraphics g, int i, int x, int y, int width, int height)
        {
            Account account = accounts[i];
            EnsureSmallImage(account.Info.Icon);
            Texture2D icon = icons[account.Info.Icon];
            if (icon != null)
            {
                float iconWidth = (height - 2) * icon.width / (float)icon.height;
                BeginGroup(scrollableMenuAccounts.X, scrollableMenuAccounts.Y, scrollableMenuAccounts.Width, scrollableMenuAccounts.Height);
                DrawTexture(10, i * height + 2 - scrollableMenuAccounts.CurrentOffset, iconWidth, height - 2, icon, ScaleMode.ScaleToFit);
                GUI.EndGroup();
            }
            if (account.PetInfo != null)
            {
                EnsureSmallImage(account.PetInfo.Icon);
                Texture2D petIcon = icons[account.PetInfo.Icon];
                if (petIcon != null)
                {
                    float iconWidth = height / 2f * petIcon.width / petIcon.height;
                    BeginGroup(scrollableMenuAccounts.X, scrollableMenuAccounts.Y, scrollableMenuAccounts.Width, scrollableMenuAccounts.Height);
                    DrawTexture(45, i * height - scrollableMenuAccounts.CurrentOffset + height / 2f, iconWidth, height / 2f, petIcon, ScaleMode.ScaleToFit);
                    GUI.EndGroup();
                }
            }
            Texture2D overlay = GetOverlay(account.Info.Gender);
            if (overlay != null)
            {
                float overlayWidth = (height - 1) * overlay.width / (float)overlay.height;
                BeginGroup(scrollableMenuAccounts.X, scrollableMenuAccounts.Y, scrollableMenuAccounts.Width, scrollableMenuAccounts.Height);
                DrawTexture(width - overlayWidth, i * height - scrollableMenuAccounts.CurrentOffset + 1, overlayWidth, height - 1, overlay, ScaleMode.ScaleToFit);
                GUI.EndGroup();
            }
            mFont.tahoma_7b_dark.drawString(g, account.Info.Name, x + 80, y + 2, 0);

            string serverName = account.Server.IsCustomIP() ? account.Server.name : ServerListScreen.nameServer[account.Server.index];
            mFont.tahoma_7_greySmall.drawString(g, account.GetLastTimeLogin(), x + 80, y + height - 12, 0);
            mFont.tahoma_7_greySmall.drawString(g, mResources.server + ' ' + serverName, x + 80, y + height - 22, 0);
            //g.setColor(new Color(0, 0, 0, .3f));
            //g.fillRect(x + 79, y + height - 25, 75, 1);
        }

        static void EnsureSmallImage(int id)
        {
            Texture2D texture = null;
            if (!icons.ContainsKey(id) || icons[id] == null)
            {
                texture = Resources.Load($"{Main.res}/x{mGraphics.zoomLevel}/SmallImage/Small{id}") as Texture2D;
                if (texture == null)
                {
                    sbyte[] data = Rms.loadRMS(mGraphics.zoomLevel + "Small" + id);
                    if (data != null)
                    {
                        texture = new Texture2D(1, 1);
                        texture.LoadImage(ArrayCast.cast(data));
                    }
                }
                if (texture != null)
                {
                    texture.filterMode = FilterMode.Bilinear;
                    texture.wrapMode = TextureWrapMode.Clamp;
                    texture.anisoLevel = 1;
                }
            }
            if (!icons.ContainsKey(id))
                icons.Add(id, texture);
            else if (icons[id] == null)
                icons[id] = texture;
        }

        static Texture2D GetOverlay(sbyte gender)
        {
            switch (gender)
            {
                case 0:
                    return earthOverlay;
                case 1:
                    return namekOverlay;
                case 2:
                    return saiyanOverlay;
                default:
                    return null;
            }
        }

        static void GetAccountsArea(out int x, out int y, out int width, out int height, bool withoutTitle)
        {
            x = X_LIST_ACCOUNTS;
            y = Y_LIST_ACCOUNTS;
            width = GameCanvas.w - x * 2;
            height = GameCanvas.h - y * 2;
            if (withoutTitle)
            {
                y += TITLE_HEIGHT;
                height -= TITLE_HEIGHT;
            }
            int currentAccountInfoWidth = GameCanvas.w - currentAccountInfoX;
            if (currentAccountInfoWidth > 0)
                width -= currentAccountInfoWidth - 20;
        }

        static void GetInputAccountArea(out int x, out int y, out int width, out int height, bool withoutTitle)
        {
            x = GameCanvas.w / 2 - INPUT_ACCOUNT_WIDTH / 2;
            y = GameCanvas.h / 2 - INPUT_ACCOUNT_HEIGHT / 2;
            width = INPUT_ACCOUNT_WIDTH;
            height = INPUT_ACCOUNT_HEIGHT;
            if (withoutTitle)
            {
                y += TITLE_HEIGHT;
                height -= TITLE_HEIGHT;
            }
        }

        static bool IsPointerIn(int x, int y, int w, int h) => GameCanvas.pxMouse >= x && GameCanvas.pxMouse <= x + w && GameCanvas.pyMouse >= y && GameCanvas.pyMouse <= y + h;

        static void BeginGroup(int x, int y, int w, int h) => GUI.BeginGroup(new Rect(x * mGraphics.zoomLevel, y * mGraphics.zoomLevel, w * mGraphics.zoomLevel, h * mGraphics.zoomLevel));

        static void DrawTexture(float x, float y, float w, float h, Texture texture, ScaleMode scaleMode = ScaleMode.StretchToFill) => GUI.DrawTexture(new Rect(x * mGraphics.zoomLevel, y * mGraphics.zoomLevel, w * mGraphics.zoomLevel, h * mGraphics.zoomLevel), texture, scaleMode);
    }
}