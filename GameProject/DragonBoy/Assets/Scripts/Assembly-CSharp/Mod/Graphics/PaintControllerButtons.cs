using System;
using InputMap;
using InputMap.Icons;
using Mod.ModMenu;
using UnityEngine;

namespace Mod.Graphics
{
    internal static class PaintControllerButtons
    {
        static readonly int HCENTER = 1;
        static readonly int VCENTER = 2;
        static readonly int LEFT = 8;
        static readonly int TOP = 32;

        internal static void PaintByImage(Image image, int x, int y, int anchor)
        {
            x *= mGraphics.zoomLevel;
            y *= mGraphics.zoomLevel;
            if (InputDeviceDetector.IsXboxController())
            {
                GetStartingPoint(image, ref x, ref y, anchor);
                int height = 12 * mGraphics.zoomLevel;
                if (image == GameScr.imgFire0 || image == GameScr.imgFire1)
                {
                    Texture2D texture = XboxControllerIcons.A;
                    int width = GetWidth(texture, height);
                    DrawTexture(x + image.texture.width, y, width, height, texture, LEFT);
                }
                if (image == GameScr.imgFocus || image == GameScr.imgFocus2)
                {
                    Texture2D texture = XboxControllerIcons.B;
                    int width = GetWidth(texture, height);
                    DrawTexture(x + image.texture.width, y, width, height, texture, LEFT);
                }
                if (image == GameScr.imgHP1 || image == GameScr.imgHP2 || image == GameScr.imgHP3 || image == GameScr.imgHP4)
                {
                    Texture2D texture = XboxControllerIcons.X;
                    int width = GetWidth(texture, height);
                    DrawTexture(x + image.texture.width, y, width, height, texture, LEFT);
                }
                if (image == GameScr.imgMenu)
                {
                    Texture2D texture = XboxControllerIcons.LB;
                    int width = GetWidth(texture, height);
                    DrawTexture(x, y + image.texture.height, width, height, texture, 0);
                }
                if (image.texture == ModMenuMain.imgMenu)
                {
                    Texture2D texture = XboxControllerIcons.RB;
                    int width = GetWidth(texture, height);
                    DrawTexture(x + image.texture.width - width, y + image.texture.height, width, height, texture, 0);
                }
            }
            //Other kind of controllers which I don't have
        }

        internal static void PaintSelectedSkill(GameScr gameScr, mGraphics g)
        {
            if (gameScr.mobCapcha != null)
                return;
            if (GameCanvas.currentDialog != null || ChatPopup.currChatPopup != null || GameCanvas.menu.showMenu || gameScr.isPaintPopup() || GameCanvas.panel.isShow || Char.myCharz().taskMaint.taskId == 0 || ChatTextField.gI().isShow || GameCanvas.currentScreen == MoneyCharge.instance)
                return;
            if (Char.myCharz().statusMe == 14)
                return;
            if (!GameScr.isudungCapsun4 && !GameScr.isudungCapsun3)
                return;
            if (!InputDeviceDetector.IsXboxController())
                return;
            Image image = (mScreen.keyTouch != 14) ? GameScr.imgNut : GameScr.imgNutF;
            int x, y, width, anchor;
            int height = 12 * mGraphics.zoomLevel;
            Texture2D texture;

            void ConvXY()
            {
                x += g.translateX;
                y += g.translateY;
                x *= mGraphics.zoomLevel;
                y *= mGraphics.zoomLevel;
            }
            if (GameScr.gamePad.isSmallGamePad)
            {
                if (GameScr.isAnalog != 1)
                {
                    x = GameScr.xHP + 5;
                    y = GameScr.yHP - 6 - 40 + 10;
                    anchor = 0;
                    texture = XboxControllerIcons.Y;
                }
                else
                {
                    x = GameScr.xHP + 20 + 5;
                    y = GameScr.yHP + 20 - 6 - 40 + 10;
                    anchor = HCENTER | VCENTER;
                    texture = XboxControllerIcons.Y;
                }
            }
            else if (GameScr.isAnalog != 1)
            {
                x = GameScr.xHP + 20;
                y = GameScr.yHP + 20 - 6 - 40;
                anchor = HCENTER | VCENTER;
                texture = XboxControllerIcons.Y;
            }
            else
            {
                x = GameScr.xHP + 20 + 5;
                y = GameScr.yHP + 20 - 6 - 40 + 10;
                anchor = HCENTER | VCENTER;
                texture = XboxControllerIcons.Y;
            }

            width = GetWidth(texture, height);
            ConvXY();
            GetStartingPoint(image, ref x, ref y, anchor);
            DrawTexture(x + image.texture.width, y, width, height, texture, LEFT);
            if (GameScr.gamePad.isLargeGamePad)
            {
                Skill[] array;
                if (Main.isPC)
                    array = GameScr.keySkill;
                else if (GameCanvas.isTouch)
                    array = GameScr.onScreenSkill;
                else
                    array = GameScr.keySkill;
                int minXS = int.MaxValue;
                int maxXS = int.MinValue;
                int minYS = int.MaxValue;
                int maxYS = int.MinValue;
                bool endNull = false;
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    if (array[i] == null && !endNull)
                        continue;
                    endNull = true;
                    minXS = Math.Min(minXS, GameScr.xS[i]);
                    maxXS = Math.Max(maxXS, GameScr.xS[i]);
                    minYS = Math.Min(minYS, GameScr.yS[i]);
                    maxYS = Math.Max(maxYS, GameScr.yS[i]);
                }
                minXS += GameScr.xSkill + g.translateX;
                maxXS += GameScr.xSkill + g.translateX + GameScr.imgSkill.getHeight();
                minYS += g.translateY + GameScr.imgSkill.getHeight() / 2;
                maxYS += g.translateY + GameScr.imgSkill.getHeight() / 2;
                minXS *= mGraphics.zoomLevel;
                maxXS *= mGraphics.zoomLevel;
                minYS *= mGraphics.zoomLevel;
                maxYS *= mGraphics.zoomLevel;
                int triggerY = (minYS + maxYS) / 2;
                texture = XboxControllerIcons.LT;
                width = GetWidth(texture, height);
                DrawTexture(minXS - width, triggerY, width, height, texture, VCENTER | HCENTER);
                texture = XboxControllerIcons.RT;
                width = GetWidth(texture, height);
                DrawTexture(maxXS + width, triggerY, width, height, texture, VCENTER | HCENTER);
            }
        }

        static void DrawTexture(int x, int y, int width, int height, Texture2D texture, int anchor)
        {
            if ((anchor & HCENTER) == HCENTER)
                x -= width / 2;
            if ((anchor & LEFT) == LEFT)
                x -= width;
            if ((anchor & VCENTER) == VCENTER)
                y -= height / 2;
            if ((anchor & TOP) == TOP)
                y -= height;
            GUI.DrawTexture(new Rect(x, y, width, height), texture);
        }

        static int GetWidth(Texture2D texture, int height) => height * texture.width / texture.height;

        static void GetStartingPoint(Image image, ref int x, ref int y, int anchor)
        {
            if ((anchor & HCENTER) == HCENTER)
                x -= image.texture.width / 2;
            if ((anchor & LEFT) == LEFT)
                x -= image.texture.width;
            if ((anchor & VCENTER) == VCENTER)
                y -= image.texture.height / 2;
            if ((anchor & TOP) == TOP)
                y -= image.texture.height;
        }
    }
}