using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Mod.Graphics
{
    internal class ComboBox
    {
        enum ComboBoxAction
        {
            ShowListItems = 1,
        }

        class ActionListener : IActionListener
        {
            public void perform(int idAction, object p)
            {
                ComboBox comboBox = (ComboBox)p;
                switch ((ComboBoxAction)idAction)
                {
                    case ComboBoxAction.ShowListItems:
                        comboBox.isShowingListItems = !comboBox.isShowingListItems;
                        break;
                }
            }
        }

        List<string> items = new List<string>();
        Image imgTf;
        bool isShowingListItems;
        int x, y, w, h;
        int offset;
        ScrollableMenuItems<string> scrollableMenuItems;

        internal int SelectedIndex
        {
            get => scrollableMenuItems.CurrentItemIndex;
            set => scrollableMenuItems.CurrentItemIndex = value;
        }
        internal bool IsShowingListItems => isShowingListItems;
        internal bool IsFocus { get; set; }
        internal string Hint { get; set; } = "";
        internal List<string> Items => items;
        internal int ListItemsWidth
        {
            get => scrollableMenuItems.Width;
            set => scrollableMenuItems.Width = value;
        }
        internal int ListItemsHeight
        {
            get => scrollableMenuItems.Height;
            set => scrollableMenuItems.Height = value;
        }
        internal int X
        {
            get => x;
            set
            {
                x = value;
                UpdateScrollableMenuItemsPos();
            }
        }
        internal int Y
        {
            get => y;
            set
            {
                y = value;
                UpdateScrollableMenuItemsPos();
            }
        }
        internal int Width
        {
            get => w;
            set
            {
                w = value;
                UpdateScrollableMenuItemsPos();
            }
        }

        internal static readonly int TEXT_GAP_X = 4;

        static GUIStyle style;

        static Image imgExpand;
        static Image imgCollapse;

        Command expandListItems;

        static ComboBox()
        {
        }

        internal ComboBox(string hint, List<string> items)
        {
            this.items = items;
            Hint = hint;
            if (imgTf == null)
                imgTf = GameCanvas.loadImage("/mainImage/myTexture2dtf.png");
            h = mScreen.ITEM_HEIGHT + 2;
            expandListItems = new Command("", new ActionListener(), (int)ComboBoxAction.ShowListItems, this)
            {
                imgFocus = new Image()
            };
            scrollableMenuItems = new ScrollableMenuItems<string>(items)
            {
                ItemHeight = h,
                Height = h * 4,
                PaintItemAction = PaintItem,
                StepScroll = h * 2
            };
            SelectedIndex = 0;
        }

        static void InitGraphics()
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperLeft,
                    wordWrap = false,
                };
                Font font = (Font)Resources.Load($"FontSys/x{mGraphics.zoomLevel}/barmeneb");
                style.font = font;
                style.normal.textColor = style.hover.textColor = new Color(.33f, .16f, .02f);
                style.fontSize = 6 * mGraphics.zoomLevel;
                //style.fontStyle = styleUnfocus.fontStyle = FontStyle.Bold;
            }
            if (imgExpand == null)
            {
                imgExpand = Image.createImage("ComboBox/imgExpand");
                imgExpand.texture = CustomGraphics.Resize(imgExpand.texture, imgExpand.texture.width * mGraphics.zoomLevel / 4, imgExpand.texture.height * mGraphics.zoomLevel / 4);
                imgExpand.w = imgExpand.texture.width;
                imgExpand.h = imgExpand.texture.height;
                Image.setTextureQuality(imgExpand.texture);
            }
            if (imgCollapse == null)
            {
                imgCollapse = Image.createImage("ComboBox/imgCollapse");
                imgCollapse.texture = CustomGraphics.Resize(imgCollapse.texture, imgCollapse.texture.width * mGraphics.zoomLevel / 4, imgCollapse.texture.height * mGraphics.zoomLevel / 4);
                imgCollapse.w = imgCollapse.texture.width;
                imgCollapse.h = imgCollapse.texture.height;
                Image.setTextureQuality(imgCollapse.texture);
            }
        }

        void UpdateScrollableMenuItemsPos()
        {
            scrollableMenuItems.X = x + 5;
            scrollableMenuItems.Y = y + h + 5;
            scrollableMenuItems.Width = Width - 10;
        }

        void PaintItem(mGraphics g, int i, int x, int y, int w, int h)
        {
            mFont.tahoma_7b_dark.drawString(g, items[i], x + 3, y + 3, 0);
        }

        internal void Paint(mGraphics g)
        {
            InitGraphics();
            if (expandListItems.img == null)
                expandListItems.img = imgExpand;
            expandListItems.x = x + w - expandListItems.img.getWidth() - 6;
            expandListItems.y = y + h - expandListItems.img.getHeight();
            g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
            int xText = TEXT_GAP_X + x + 3;
            int yText = y + (h - mFont.tahoma_8b.getHeight()) / 2 + 2;
            int wText = w - 7;
            g.setColor(0);
            if (IsFocus)
            {
                g.drawRegion(imgTf, 0, 81, 29, 27, 0, x, y - 1, 0);
                g.drawRegion(imgTf, 0, 135, 29, 27, 0, x + w - 29, y - 1, 0);
                g.drawRegion(imgTf, 0, 108, 29, 27, 0, x + w - 58, y - 1, 0);
                for (int i = 0; i < (w - 58) / 29; i++)
                    g.drawRegion(imgTf, 0, 108, 29, 27, 0, x + 29 + i * 29, y - 1, 0);
                wText -= x + w - expandListItems.x;
            }
            else
            {
                g.drawRegion(imgTf, 0, 0, 29, 27, 0, x, y - 1, 0);
                g.drawRegion(imgTf, 0, 54, 29, 27, 0, x + w - 29, y - 1, 0);
                g.drawRegion(imgTf, 0, 27, 29, 27, 0, x + w - 58, y - 1, 0);
                for (int j = 0; j < (w - 58) / 29; j++)
                    g.drawRegion(imgTf, 0, 27, 29, 27, 0, x + 29 + j * 29, y - 1, 0);
            }

            if (IsFocus)
                expandListItems.paint(g);

            g.setClip(xText - 3, y, wText, h + 5);

            string value = "";
            if (SelectedIndex > -1 && SelectedIndex < items.Count)
                value = items[SelectedIndex];
            if (!string.IsNullOrEmpty(value))
                mFont.tahoma_8b.drawString(g, value, xText, yText + 3, 0);
            if (!string.IsNullOrEmpty(Hint))
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (IsFocus)
                        mFont.tahoma_7b_focus.drawString(g, Hint, xText, yText, 0);
                    else
                        mFont.tahoma_7b_unfocus.drawString(g, Hint, xText, yText, 0);
                }
                else
                    g.drawString(Hint, xText - 1, yText - 4, style);
            }
            g.setClip(scrollableMenuItems.X - 1, scrollableMenuItems.Y - 1, scrollableMenuItems.Width + 2, scrollableMenuItems.Height + 2);
            if (isShowingListItems)
            {
                if (expandListItems.img != imgCollapse)
                    expandListItems.img = imgCollapse;
                scrollableMenuItems.Paint(g);
            }
            else
            {
                if (expandListItems.img != imgExpand)
                    expandListItems.img = imgExpand;
            }
        }

        internal void Update()
        {
            scrollableMenuItems.Update();
        }

        internal void UpdateKey()
        {
            if (!IsFocus && GameCanvas.isPointerHoldIn(x, y, w, h) && GameCanvas.isPointerJustRelease)
            {
                IsFocus = true;
                return;
            }
            if (isShowingListItems && GameCanvas.isPointerHoldIn(0, 0, GameCanvas.w, GameCanvas.h) && !GameCanvas.isPointerHoldIn(scrollableMenuItems.X, scrollableMenuItems.Y, scrollableMenuItems.Width, scrollableMenuItems.Height) && GameCanvas.isPointerJustRelease)
            {
                isShowingListItems = false;
                GameCanvas.clearAllPointerEvent();
                return;
            }
            if (!isShowingListItems || !GameCanvas.isPointerHoldIn(scrollableMenuItems.X, scrollableMenuItems.Y, scrollableMenuItems.Width, scrollableMenuItems.Height))
                if (IsFocus && GameCanvas.isPointerHoldIn(0, 0, GameCanvas.w, GameCanvas.h) && !GameCanvas.isPointerHoldIn(x, y, w, h) && GameCanvas.isPointerJustRelease)
                {
                    IsFocus = false;
                    return;
                }
                else if (expandListItems.isPointerPressInside())
                    expandListItems.performAction();
            if (isShowingListItems)
                scrollableMenuItems.UpdateKey();
        }

        internal string GetSelectedValue() => items[SelectedIndex];
    }
}