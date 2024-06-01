#if false
using System;
using UnityEngine;

namespace Mod.AccountManager
{
    internal class MyButton : Command
    {
        internal static Texture2D btnLeft, btnCenter, btnRight;
        internal static Texture2D btnFocusLeft, btnFocusCenter, btnFocusRight;

        static Font font = mFont.tahoma_7.myFont;

        internal Texture2D icon;
        internal Anchor iconAnchor;
        internal Anchor textAnchor;

        static MyButton()
        {
            btnLeft = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(btnLeft));
            btnCenter = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(btnCenter));
            btnRight = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(btnRight));
            btnFocusLeft = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(btnFocusLeft));
            btnFocusCenter = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(btnFocusCenter));
            btnFocusRight = Resources.Load<Texture2D>("InGameAccountManager/img/" + nameof(btnFocusRight));
        }

        internal MyButton(string caption, IActionListener actionListener, int action, object p) : base(caption, actionListener, action, p) { }
        internal MyButton(string caption, IActionListener actionListener, int action, object p, int x, int y) : base(caption, actionListener, action, p, x, y) { }

        internal new void paint(mGraphics g)
        {
            if (img != null)
                throw new Exception("Use Command instead.");
            float ratio = mGraphics.zoomLevel / 4f;
            h = (int)(btnCenter.height * ratio / mGraphics.zoomLevel);
            paintLongImage(x, y, w);
            int iconWidth = icon != null ? icon.width : 0;
            int paddingLeft = 7;
            int paddingRight = 7;
            if (icon != null)
            {
                switch (iconAnchor)
                {
                    case Anchor.Left:
                        GUI.DrawTexture(new Rect((x + paddingLeft) * mGraphics.zoomLevel, y * mGraphics.zoomLevel + (h * mGraphics.zoomLevel - icon.height * ratio) / 2, iconWidth * ratio, icon.height * ratio), icon);
                        paddingLeft += (int)(iconWidth * ratio / mGraphics.zoomLevel + paddingLeft);
                        break;
                    case Anchor.Center:
                        GUI.DrawTexture(new Rect(x * mGraphics.zoomLevel, y * mGraphics.zoomLevel + (h * mGraphics.zoomLevel - icon.height * ratio) / 2, w * mGraphics.zoomLevel, icon.height * ratio), icon, ScaleMode.ScaleToFit);
                        break;
                    case Anchor.Right:
                        GUI.DrawTexture(new Rect((x + w - paddingRight) * mGraphics.zoomLevel - iconWidth * ratio, y * mGraphics.zoomLevel + (h * mGraphics.zoomLevel - icon.height * ratio) / 2, iconWidth * ratio, icon.height * ratio), icon);
                        paddingRight += (int)(iconWidth * ratio / mGraphics.zoomLevel + paddingLeft);
                        break;
                }
            }
            switch (textAnchor)
            {
                case Anchor.Left:
                    GUI.Label(new Rect((x + paddingLeft) * mGraphics.zoomLevel, y * mGraphics.zoomLevel, (w - paddingLeft - paddingRight) * mGraphics.zoomLevel, h * mGraphics.zoomLevel), caption, new GUIStyle(GUI.skin.label)
                    {
                        font = font,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleLeft,
                        fontSize = (int)(48 * (mGraphics.zoomLevel / 4f)),
                        normal = new GUIStyleState { textColor = new Color(0.15f, 0.28f, 0.38f) }
                    });
                    break;
                case Anchor.Center:
                    GUI.Label(new Rect((x + paddingLeft) * mGraphics.zoomLevel, y * mGraphics.zoomLevel, (w - paddingLeft - paddingRight) * mGraphics.zoomLevel, h * mGraphics.zoomLevel), caption, new GUIStyle(GUI.skin.label)
                    {
                        font = font,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = (int)(48 * (mGraphics.zoomLevel / 4f)),
                        normal = new GUIStyleState { textColor = new Color(0.15f, 0.28f, 0.38f) }
                    });
                    break;
                case Anchor.Right:
                    GUI.Label(new Rect((x + paddingLeft) * mGraphics.zoomLevel, y * mGraphics.zoomLevel, (w - paddingLeft - paddingRight) * mGraphics.zoomLevel, h * mGraphics.zoomLevel), caption, new GUIStyle(GUI.skin.label)
                    {
                        font = font,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleRight,
                        fontSize = (int)(48 * (mGraphics.zoomLevel / 4f)),
                        normal = new GUIStyleState { textColor = new Color(0.15f, 0.28f, 0.38f) }
                    });
                    break;
            }

        }

        void paintLongImage(int x, int y, int width)
        {
            Texture2D img0 = isFocus ? btnFocusLeft : btnLeft;
            Texture2D img1 = isFocus ? btnFocusCenter : btnCenter;
            Texture2D img2 = isFocus ? btnFocusRight : btnRight;

            float ratio = mGraphics.zoomLevel / 4f;
            x *= mGraphics.zoomLevel;
            y *= mGraphics.zoomLevel;
            width *= mGraphics.zoomLevel;

            GUI.DrawTexture(new Rect(x, y, (int)(img0.width * ratio), (int)(img0.height * ratio)), img0, ScaleMode.ScaleToFit);
            GUI.DrawTexture(new Rect(x + (int)((img0.width - 2) * ratio), y, (int)(width - (img0.width + img2.width - 4) * ratio), (int)(img1.height * ratio)), img1, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(x + (int)(width - img2.width * ratio), y, (int)(img2.width * ratio), (int)(img2.height * ratio)), img2, ScaleMode.ScaleToFit);
        }
    }
}

#endif