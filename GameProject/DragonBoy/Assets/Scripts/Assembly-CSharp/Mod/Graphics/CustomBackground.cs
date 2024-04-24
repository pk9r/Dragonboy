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
#if UNITY_EDITOR_WIN || UNITY_STANDALONE
using SFB;
#endif

namespace Mod.Graphics
{
    internal class CustomBackground : IChatable
    {
        internal static bool isEnabled;

        internal static Dictionary<string, IImage> customBgs = new Dictionary<string, IImage>();
        static ScaleMode _defaultScaleMode = ScaleMode.StretchToFill;
        internal static ScaleMode DefaultScaleMode
        {
            get => _defaultScaleMode;
            set
            {
                _defaultScaleMode = value;
                if (_defaultScaleMode > ScaleMode.ScaleToFit)
                    _defaultScaleMode = 0;
                foreach (var customBg in customBgs.Where(cBG => !overrideScaleMode.ContainsKey(cBG.Key)))
                    customBg.Value.ScaleMode = _defaultScaleMode;

            }
        }
        internal static Dictionary<string, ScaleMode> overrideScaleMode = new Dictionary<string, ScaleMode>();

        internal static int intervalChangeBg = 30000;
        static int bgIndex;
        static bool isAllBgsLoaded;
        static long lastTimeChangedBg;
        static bool isChangeBg = true;
        static float speed = 1f;
        static CustomBackground instance = new CustomBackground();

        internal static void ShowMenu()
        {
            new MenuBuilder()
                .setChatPopup(Strings.customBgChatPopup)
                .addItem(customBgs.Count > 0, Strings.customBgOpenBgList, new MenuAction(() => CustomPanelMenu.Show(SetTabCustomBackgroundPanel, DoFireCustomBackgroundListPanel, PaintTabHeader, PaintCustomBackgroundPanel)))
                .addItem(Strings.customBgAddNewBg, new MenuAction(SelectBackgrounds))
                .addItem(customBgs.Count > 0, Strings.customBgRemoveAll, new MenuAction(() =>
                    {
                        foreach (BackgroundVideo backgroundVideo in customBgs.OfType<BackgroundVideo>())
                            backgroundVideo.Stop();
                        customBgs.Clear();
                        GameScr.info1.addInfo(Strings.customBgAllBgRemoved + '!', 0);
                    }))
                .addItem(Strings.customBgAutoChangeBg + ": " + Strings.OnOffStatus(isChangeBg), new MenuAction(() => 
                {
                    isChangeBg = !isChangeBg;
                    lastTimeChangedBg = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo(Strings.customBgAutoChangeBg + ": " + Strings.OnOffStatus(isChangeBg), 0); 
                }))
                .addItem(Strings.customBgDefaultScaleModeTitle + ": " + DefaultScaleMode.GetName(), new MenuAction(() => 
                {
                    DefaultScaleMode++;
                    GameScr.info1.addInfo(Strings.customBgDefaultScaleModeTitle + ": " + DefaultScaleMode.GetName(), 0); 
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
            foreach (BackgroundVideo backgroundVideo in customBgs.Values.OfType<BackgroundVideo>().Where(v => v.isPlaying))
                backgroundVideo.Stop();
        }

        internal static void SelectBackgrounds()
        {
            string[] paths = null;
            new Thread(delegate ()
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE
                ExtensionFilter[] extensions = new[]
                {
                    new ExtensionFilter(Strings.imageVideoFile, "png", "jpg", "jpeg", "gif", "mp4" ),
                    new ExtensionFilter(Strings.allFileTypes, "*" ),
                };
                paths = StandaloneFileBrowser.OpenFilePanel(Strings.customBgSelectBgFiles, "", extensions, true);
#elif UNITY_ANDROID
                paths = EHVN.FileChooser.Open(new string[] { "image/*", "video/mp4" });
#endif
                if (paths.Length == 0)
                    return;
                foreach (string path in paths)
                    customBgs.Add(path, null);
                isAllBgsLoaded = false;
            })
            { IsBackground = true }.Start();
        }

        internal static void Update()
        {
            if (!isAllBgsLoaded)
            {
                List<string> paths = new List<string>(customBgs.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (customBgs[path] != null && customBgs[path].IsLoaded)
                            continue;
                        if (path.EndsWith(".gif"))
                            customBgs[path] = new GifImage(path);
                        else if (path.EndsWith(".mp4"))
                            customBgs[path] = new BackgroundVideo(path);
                        else
                            customBgs[path] = new StaticImage(path);
                        if (overrideScaleMode.ContainsKey(path))
                            customBgs[path].ScaleMode = overrideScaleMode[path];
                        else
                            customBgs[path].ScaleMode = DefaultScaleMode;
                    }
                    catch (FileNotFoundException)
                    {
                        customBgs.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        customBgs.Remove(path);
                    }
                    catch (Exception ex) { Debug.LogException(ex); }
                }
                lastTimeChangedBg = mSystem.currentTimeMillis();
                isAllBgsLoaded = true;
                SaveData();
            }
        }

        internal static void Paint(mGraphics g)
        {
            if (!isEnabled || customBgs.Count <= 0)
                return;
            try
            {
                g.setColor(0);
                g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
                if (bgIndex >= customBgs.Count)
                    bgIndex = 0;
                IImage background = customBgs.ElementAt(bgIndex).Value;
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
                if (isChangeBg)
                {
                    if (mSystem.currentTimeMillis() - lastTimeChangedBg > intervalChangeBg - 2000)
                    {
                        int index = bgIndex + 1;
                        if (index >= customBgs.Count)
                            index = 0;
                        if (customBgs.ElementAt(index).Value is BackgroundVideo backgroundVideo1 && !backgroundVideo1.isPreparing && !backgroundVideo1.IsLoaded)
                            backgroundVideo1.Prepare();
                    }
                    if (mSystem.currentTimeMillis() - lastTimeChangedBg > intervalChangeBg)
                    {
                        lastTimeChangedBg = mSystem.currentTimeMillis();
                        if (background is BackgroundVideo backgroundVideo1 && backgroundVideo1.isPlaying)
                            backgroundVideo1.Stop();
                        bgIndex++;
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
            if (customBgs.Count != GameCanvas.panel.currentListLength)
                return;
            int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
            for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, panel.currentListLength); i++)
            {
                int xScroll = GameCanvas.panel.xScroll;
                int yScroll = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int wScroll = GameCanvas.panel.wScroll;
                int itemHeight = GameCanvas.panel.ITEM_HEIGHT - 1;
                if (bgIndex == i)
                    g.setColor((i != GameCanvas.panel.selected) ? new Color(.5f, 1, 0) : new Color(.375f, .75f, 0));
                else
                    g.setColor((i != GameCanvas.panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                g.fillRect(xScroll, yScroll, wScroll, itemHeight);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(customBgs.ElementAt(i).Key), xScroll + 5, yScroll, 0);
                mFont.tahoma_7_blue.drawString(g, $"{Strings.fullPath}: {customBgs.ElementAt(i).Key}", xScroll + 5, yScroll + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        internal static void PaintTabHeader(Panel panel, mGraphics g) => PaintPanelTemplates.PaintTabHeaderTemplate(panel, g, Strings.customBgList);

        internal static void SetTabCustomBackgroundPanel(Panel panel) => SetTabPanelTemplates.setTabListTemplate(panel, customBgs);

        internal static void DoFireCustomBackgroundListPanel(Panel panel)
        {
            int selected = panel.selected;
            if (selected < 0)
                return;
            KeyValuePair<string, IImage> customBg = customBgs.ElementAt(selected);
            MenuBuilder menuBuilder = new MenuBuilder()
                .addItem(bgIndex != selected, Strings.customBgSwitchToThisBg, new MenuAction(() =>
                {
                    StopAllBackgroundVideo();
                    bgIndex = selected;
                    lastTimeChangedBg = mSystem.currentTimeMillis();
                }))
                .addItem(Strings.delete, new MenuAction(() =>
                {
                    if (customBg.Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
                        videoBackground.Stop();
                    string bgFileName = customBg.Key;
                    customBgs.Remove(customBg.Key);
                    if (selected < bgIndex)
                    {
                        bgIndex--;
                        lastTimeChangedBg = mSystem.currentTimeMillis();
                    }
                    else if (selected == bgIndex && customBgs.Count == bgIndex)
                    {
                        bgIndex = 0;
                        lastTimeChangedBg = mSystem.currentTimeMillis();
                    }
                    GameScr.info1.addInfo(string.Format(Strings.customBgRemovedBg, bgFileName) + '!', 0);
                    SetTabCustomBackgroundPanel(panel);
                    SaveData();
                }))
                .addItem(Strings.customBgScaleMode + ": " + customBg.Value.ScaleMode.GetName(), new MenuAction(() =>
                {
                    if (overrideScaleMode.ContainsKey(customBg.Key))
                        overrideScaleMode[customBg.Key]++;
                    else
                        overrideScaleMode.Add(customBg.Key, customBg.Value.ScaleMode + 1);
                    if (overrideScaleMode[customBg.Key] > ScaleMode.ScaleToFit)
                        overrideScaleMode[customBg.Key] = 0;
                    customBg.Value.ScaleMode = overrideScaleMode[customBg.Key];
                    GameScr.info1.addInfo(Strings.customBgScaleMode + ": " + overrideScaleMode[customBg.Key].GetName(), 0);
                }))
                .setPos(panel.X, (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll);
            if (overrideScaleMode.ContainsKey(customBg.Key))
                menuBuilder.addItem(Strings.customBgResetScaleModeToDefault, new MenuAction(() =>
                {
                    if (overrideScaleMode.ContainsKey(customBg.Key))
                        overrideScaleMode.Remove(customBg.Key);
                    customBg.Value.ScaleMode = DefaultScaleMode;
                }));
            menuBuilder.start();
            string fileName = Path.GetFileName(customBg.Key);
            panel.cp = new ChatPopup();
            panel.cp.isClip = false;
            panel.cp.sayWidth = 180;
            panel.cp.cx = 3 + panel.X;
            if (panel.X != 0)
                panel.cp.cx -= Res.abs(panel.cp.sayWidth - panel.W) + 8;
            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|" + Strings.fullPath + ": " + customBg.Key, panel.cp.sayWidth - 10);
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
                if (Utils.TryLoadDataString("custom_bg_override_scale_modes", out string str))
                {
                    foreach (string item in str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] data = item.Split('|');
                        overrideScaleMode.Add(data[0], Enum.Parse<ScaleMode>(data[1]));
                    }
                }
                foreach (string path in Utils.LoadDataString("custom_bg_paths").Split('|'))
                {
                    if (!string.IsNullOrEmpty(path))
                        customBgs.Add(path, null);
                }
                isAllBgsLoaded = false;
                Utils.TryLoadDataBool("custom_bg_change", out isChangeBg);
                Utils.TryLoadDataInt("custom_bg_index", out bgIndex);
                if (Utils.TryLoadDataInt("custom_bg_default_scale_mode", out int value))
                    DefaultScaleMode = (ScaleMode)value;
                if (bgIndex >= customBgs.Count)
                    bgIndex = 0;
                if (Utils.TryLoadDataFloat("custom_bg_gif_speed", out float value2))
                    speed = Mathf.Clamp(value2, 0, 100);
            }
            catch (Exception)
            { }
        }

        internal static void SaveData()
        {
            string data = string.Join("|", customBgs.Keys.ToArray());
            Utils.SaveData("custom_bg_paths", data);
            Utils.SaveData("custom_bg_change", isChangeBg);
            Utils.SaveData("custom_bg_index", bgIndex);
            Utils.SaveData("custom_bg_gif_speed", speed);
            Utils.SaveData("custom_bg_default_scale_mode", (int)DefaultScaleMode);
            Utils.SaveData("custom_bg_override_scale_modes", string.Join(Environment.NewLine, overrideScaleMode.Select(kVP => kVP.Key + '|' + kVP.Value)));
        }

        internal static void SetState(bool value)
        {
            isEnabled = value;
            if (value)
                return;
            foreach (BackgroundVideo backgroundVideo in customBgs.Values.Where((background) => background is BackgroundVideo))
                backgroundVideo.Stop();
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text))
            {
                onCancelChat();
                return;
            }
            if (ChatTextField.gI().strChat == Strings.customBgInputGifSpeed)
            {
                try
                {
                    float value = float.Parse(text);
                    if (value > 10f || value < 0.1f)
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, 0.1, 10) + '!');
                    else
                    {
                        if (value != speed)
                            speed = value;
                        GameScr.info1.addInfo(string.Format(Strings.valueChanged, Strings.customBgGifSpeed, value) + '!', 0);
                        SaveData();
                    }
                }
                catch (Exception)
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                }
            }
            else if (ChatTextField.gI().strChat == Strings.inputTimeChangeBg)
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 10)
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberMustBeBiggerThanOrEqual, 10) + '!');
                    else
                    {
                        ModMenuMain.GetModMenuItem<ModMenuItemValues>("Set_TimeChangeBg").SelectedValue = value;
                        GameScr.info1.addInfo(string.Format(Strings.valueChanged, Strings.setTimeChangeCustomBgTitle.ToLower(), value) + '!', 0);
                        SaveData();
                    }
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
