using Mod.CustomPanel;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using UnityEngine;
using Mod.R;
#if UNITY_EDITOR || UNITY_STANDALONE
using SFB;
#elif UNITY_ANDROID
using EHVN;
#endif

namespace Mod.Graphics
{
    internal class CustomBackground : IChatable
    {
        internal static bool isEnabled;

        internal static Dictionary<string, IImage> backgroundWallpapers = new Dictionary<string, IImage>();

        internal static int intervalChangeBackgroundWallpaper = 30000;
        static int backgroundIndex;
        static bool isAllWallpaperLoaded;
        static long lastTimeChangedWallpaper;
        static bool isChangeWallpaper = true;
        static float speed = 1f;
        static CustomBackground instance = new CustomBackground();

        internal static void ShowMenu()
        {
            new MenuBuilder()
                .setChatPopup(Strings.customBgChatPopup)
                .addItem(backgroundWallpapers.Count > 0, Strings.customBgOpenBgList, new MenuAction(() => CustomPanelMenu.show(SetTabCustomBackgroundPanel, DoFireCustomBackgroundListPanel, PaintTabHeader, PaintCustomBackgroundPanel)))
                .addItem(Strings.customBgAddNewBg, new MenuAction(SelectBackgrounds))
                .addItem(backgroundWallpapers.Count > 0, Strings.customBgRemoveAll, new MenuAction(() =>
                    {
                        foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.OfType<BackgroundVideo>())
                            backgroundVideo.Stop();
                        backgroundWallpapers.Clear();
                        GameScr.info1.addInfo(Strings.customBgAllBgRemoved + '!', 0);
                    }))
                .addItem(Strings.customBgAutoChangeBg + ": " + Strings.OnOffStatus(isChangeWallpaper), new MenuAction(() => 
                {
                    isChangeWallpaper = !isChangeWallpaper;
                    lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo(Strings.customBgAutoChangeBg + ": " + Strings.OnOffStatus(isChangeWallpaper), 0); 
                }))
                .addItem(Strings.customBgSetTimeChange, new MenuAction(() =>
                { 
                    ChatTextField.gI().strChat = Strings.inputTimeChangeBg;
                    ChatTextField.gI().tfChat.name = Strings.inputTimeChangeBgHint; 
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(instance, string.Empty); 
                }))
                .addItem(Strings.customBgChangeGifSpeed, new MenuAction(() => 
                { 
                    ChatTextField.gI().strChat = Strings.customBgInputGifSpeed;
                    ChatTextField.gI().tfChat.name = Strings.speed; 
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    ChatTextField.gI().startChat2(instance, string.Empty); 
                    ChatTextField.gI().tfChat.setText(speed.ToString()); 
                }))
                .start();
        }

        internal static void StopAllBackgroundVideo()
        {
            foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.Values.OfType<BackgroundVideo>().Where(v => v.isPlaying))
                backgroundVideo.Stop();
        }

        internal static void SelectBackgrounds()
        {
            string[] paths = null;
            new Thread(delegate ()
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                ExtensionFilter[] extensions = new[]
                {
                    new ExtensionFilter(Strings.imageVideoFile, "png", "jpg", "jpeg", "gif", "mp4" ),
                    new ExtensionFilter(Strings.allFileTypes, "*" ),
                };
                paths = StandaloneFileBrowser.OpenFilePanel(Strings.customBgSelectBgFiles, "", extensions, true);
#elif UNITY_ANDROID
                paths = FileChooser.Open(new string[] { "image/*", "video/mp4" });
#endif
                if (paths.Length == 0)
                    return;
                foreach (string path in paths)
                    backgroundWallpapers.Add(path, null);
                isAllWallpaperLoaded = false;
            })
            { IsBackground = true }.Start();
        }

        internal static void Update()
        {
            if (!isAllWallpaperLoaded)
            {
                List<string> paths = new List<string>(backgroundWallpapers.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (backgroundWallpapers[path] != null && backgroundWallpapers[path].IsLoaded)
                            continue;
                        if (path.EndsWith(".gif"))
                            backgroundWallpapers[path] = new GifImage(path);
                        else if (path.EndsWith(".mp4"))
                            backgroundWallpapers[path] = new BackgroundVideo(path);
                        else
                            backgroundWallpapers[path] = new StaticImage(path);
                    }
                    catch (FileNotFoundException)
                    {
                        backgroundWallpapers.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        backgroundWallpapers.Remove(path);
                    }
                    catch (Exception ex) { Debug.LogException(ex); }
                }
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                isAllWallpaperLoaded = true;
                SaveData();
            }
        }

        internal static void Paint(mGraphics g)
        {
            if (!isEnabled || backgroundWallpapers.Count <= 0)
                return;
            try
            {
                g.setColor(0);
                g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
                if (backgroundIndex >= backgroundWallpapers.Count)
                    backgroundIndex = 0;
                IImage background = backgroundWallpapers.ElementAt(backgroundIndex).Value;
                if (background == null)
                    return;
                if (background is BackgroundVideo backgroundVideo && !backgroundVideo.isPlaying)
                {
                    if (!backgroundVideo.IsLoaded && !backgroundVideo.isPreparing)
                        backgroundVideo.Prepare();
                    backgroundVideo.Play();
                }
                if (background is GifImage gif && gif.speed != speed)
                    gif.speed = speed;
                background.Paint(g, 0, 0);
                if (isChangeWallpaper)
                {
                    if (mSystem.currentTimeMillis() - lastTimeChangedWallpaper > intervalChangeBackgroundWallpaper - 2000)
                    {
                        int index = backgroundIndex + 1;
                        if (index >= backgroundWallpapers.Count)
                            index = 0;
                        if (backgroundWallpapers.ElementAt(index).Value is BackgroundVideo backgroundVideo1 && !backgroundVideo1.isPreparing && !backgroundVideo1.IsLoaded)
                            backgroundVideo1.Prepare();
                    }
                    if (mSystem.currentTimeMillis() - lastTimeChangedWallpaper > intervalChangeBackgroundWallpaper)
                    {
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                        if (background is BackgroundVideo backgroundVideo1 && backgroundVideo1.isPlaying)
                            backgroundVideo1.Stop();
                        backgroundIndex++;
                    }
                }
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        internal static void PaintCustomBackgroundPanel(Panel panel, mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (backgroundWallpapers.Count != GameCanvas.panel.currentListLength)
                return;
            int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
            for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, panel.currentListLength); i++)
            {
                int xScroll = GameCanvas.panel.xScroll;
                int yScroll = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int wScroll = GameCanvas.panel.wScroll;
                int itemHeight = GameCanvas.panel.ITEM_HEIGHT - 1;
                if (backgroundIndex == i)
                    g.setColor((i != GameCanvas.panel.selected) ? new Color(.5f, 1, 0) : new Color(.375f, .75f, 0));
                else
                    g.setColor((i != GameCanvas.panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                g.fillRect(xScroll, yScroll, wScroll, itemHeight);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(backgroundWallpapers.ElementAt(i).Key), xScroll + 5, yScroll, 0);
                mFont.tahoma_7_blue.drawString(g, $"{Strings.fullPath}: {backgroundWallpapers.ElementAt(i).Key}", xScroll + 5, yScroll + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        internal static void PaintTabHeader(Panel panel, mGraphics g) => PaintPanelTemplates.paintTabHeaderTemplate(panel, g, Strings.customBgList);

        internal static void SetTabCustomBackgroundPanel(Panel panel) => SetTabPanelTemplates.setTabListTemplate(panel, backgroundWallpapers);

        internal static void DoFireCustomBackgroundListPanel(Panel panel)
        {
            int selected = panel.selected;
            if (selected < 0)
                return;
            string fileName = Path.GetFileName(backgroundWallpapers.ElementAt(selected).Key);

            new MenuBuilder()
                .addItem(backgroundIndex != selected, Strings.customBgSwitchToThisBg, new MenuAction(() =>
                {
                    StopAllBackgroundVideo();
                    backgroundIndex = selected;
                    lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                }))
                .addItem(Strings.delete, new MenuAction(() =>
                {
                    if (backgroundWallpapers.ElementAt(selected).Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
                        videoBackground.Stop();
                    string bgFileName = backgroundWallpapers.ElementAt(selected).Key;
                    backgroundWallpapers.Remove(backgroundWallpapers.ElementAt(selected).Key);
                    if (selected < backgroundIndex)
                    {
                        backgroundIndex--;
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    }
                    else if (selected == backgroundIndex && backgroundWallpapers.Count == backgroundIndex)
                    {
                        backgroundIndex = 0;
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    }
                    GameScr.info1.addInfo(string.Format(Strings.customBgRemovedBg, bgFileName) + '!', 0); SetTabCustomBackgroundPanel(panel); SaveData();
                }))
                .setPos(panel.X, (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();

            panel.cp = new ChatPopup();
            panel.cp.isClip = false;
            panel.cp.sayWidth = 180;
            panel.cp.cx = 3 + panel.X;
            if (panel.X != 0)
                panel.cp.cx -= Res.abs(panel.cp.sayWidth - panel.W) + 8;
            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|" + Strings.fullPath + ": " + backgroundWallpapers.ElementAt(selected).Key, panel.cp.sayWidth - 10);
            panel.cp.delay = 10000000;
            panel.cp.c = null;
            panel.cp.sayRun = 7;
            panel.cp.ch = 15 - panel.cp.sayRun + panel.cp.says.Length * 12 + 10;
            if (panel.cp.ch > GameCanvas.h - 80)
            {
                panel.cp.ch = GameCanvas.h - 80;
                panel.cp.lim = panel.cp.says.Length * 12 - panel.cp.ch + 17;
                if (panel.cp.lim < 0)
                    panel.cp.lim = 0;
                ChatPopup.cmyText = 0;
                panel.cp.isClip = true;
            }
            panel.cp.cy = GameCanvas.menu.menuY - panel.cp.ch;
            while (panel.cp.cy < 10)
            {
                panel.cp.cy++;
                GameCanvas.menu.menuY++;
            }
            panel.cp.mH = 0;
            panel.cp.strY = 10;
        }

        internal static void LoadData()
        {
            try
            {
                foreach (string path in Utils.LoadDataString("custombackgroundpath").Split('|'))
                {
                    if (!string.IsNullOrEmpty(path))
                        backgroundWallpapers.Add(path, null);
                }
                isAllWallpaperLoaded = false;
                Utils.TryLoadDataBool("ischangewallpaper", out isChangeWallpaper);
                Utils.TryLoadDataInt("backgroundindex", out backgroundIndex);
                if (backgroundIndex >= backgroundWallpapers.Count)
                    backgroundIndex = 0;
                if (Utils.TryLoadDataFloat("gifbackgroundspeed", out float gifbackgroundspeed))
                    speed = Mathf.Clamp(gifbackgroundspeed, 0, 100);
            }
            catch (Exception)
            { }
        }

        internal static void SaveData()
        {
            string data = string.Join("|", backgroundWallpapers.Keys.ToArray());
            Utils.SaveData("custombackgroundpath", data);
            Utils.SaveData("ischangewallpaper", isChangeWallpaper);
            Utils.SaveData("backgroundindex", backgroundIndex);
            Utils.SaveData("gifbackgroundspeed", speed);
        }

        internal static void SetState(bool value)
        {
            isEnabled = value;
            if (value)
                return;
            foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.Values.Where((background) => background is BackgroundVideo))
                backgroundVideo.Stop();
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (to == Strings.customBgInputGifSpeed)
            {
                try
                {
                    float value = float.Parse(text);
                    if (value > 10f || value < 0.1f)
                    {
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, 0.1, 10) + '!');
                        return;
                    }
                    if (value == speed)
                        return;
                    speed = value;
                    GameScr.info1.addInfo(string.Format(Strings.valueChanged, Strings.customBgGifSpeed, value) + '!', 0);
                    SaveData();
                }
                catch (Exception)
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                }
            }
            else if (to == Strings.inputTimeChangeBg)
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 10)
                    {
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberMustBeBiggerThanOrEqual, 10) + '!');
                        return;
                    }
                    ModMenuMain.GetModMenuItem<ModMenuItemValues>("Set_TimeChangeBg").SelectedValue = value;
                    GameScr.info1.addInfo(string.Format(Strings.valueChanged, Strings.setTimeChangeCustomBgTitle.ToLower(), value) + '!', 0);
                    SaveData();
                    ChatTextField.gI().ResetTF();
                }
                catch (Exception)
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                }
            }
            onCancelChat();
        }

        public void onCancelChat() => ChatTextField.gI().ResetTF();
    }
}
