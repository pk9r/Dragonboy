using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Mod.Graphics
{
    internal class ScrollableMenuItems<T>
    {
        Action<mGraphics, int, int, int, int, int> paintItemAction;

        List<T> items = new List<T>();
        int x, y, width, height;
        int currentItemIndex = -1;
        int stepScroll = 70;

        int currentOffset = 0;
        int currentOffsetTo = 0;
        int currentStepScroll;
        int lastMouseY = -1;
        int lastOffsetTo;
        int itemHeight = 40;
        bool isPressKey;

        internal Action<mGraphics, int, int, int, int, int> PaintItemAction
        {
            get => paintItemAction;
            set => paintItemAction = value;
        }
        internal int X
        {
            get => x;
            set => x = value;
        }
        internal int Y
        {
            get => y;
            set => y = value;
        }
        internal int Width
        {
            get => width;
            set => width = value;
        }
        internal int Height
        {
            get => height;
            set => height = value;
        }
        internal int ItemHeight
        {
            get => itemHeight;
            set => itemHeight = value;
        }
        internal int CurrentItemIndex
        {
            get => currentItemIndex;
            set => currentItemIndex = value;
        }
        internal int StepScroll
        {
            get => stepScroll;
            set => stepScroll = value;
        }
        internal int CurrentOffset => currentOffset;
        internal List<T> Items => items;
        internal bool AllowSelectNone { get; set; }
        internal Action ItemSelected { get; set; }

        internal ScrollableMenuItems(List<T> values)
        {
            items = values;
            currentStepScroll = stepScroll;
        }

        internal void Reset()
        {
            if (AllowSelectNone || items.Count == 0)
                currentItemIndex = -1;
            else
                currentItemIndex = 0;
            currentOffset = currentOffsetTo = 0;
            currentStepScroll = stepScroll;
            isPressKey = false;
        }

        internal void Update()
        {
            if (currentOffset != currentOffsetTo)
            {
                if (currentOffset < currentOffsetTo)
                {
                    if (currentOffset + currentStepScroll * 2 > currentOffsetTo)
                        currentStepScroll /= 3;
                    if (currentOffset > currentOffsetTo || currentStepScroll == 0)
                    {
                        currentOffset = currentOffsetTo;
                        currentStepScroll = stepScroll;
                    }
                    else
                        currentOffset += currentStepScroll;
                }
                else if (currentOffset > currentOffsetTo)
                {
                    if (currentOffset - currentStepScroll * 2 < currentOffsetTo)
                        currentStepScroll /= 3;
                    if (currentOffset < currentOffsetTo || currentStepScroll == 0)
                    {
                        currentOffset = currentOffsetTo;
                        currentStepScroll = stepScroll;
                    }
                    else
                        currentOffset -= currentStepScroll;
                }
            }
        }

        internal void UpdateKey()
        {
            if (items.Count > (float)height / itemHeight)
            {
                if (GameCanvas.pXYScrollMouse != 0)
                {
                    if (IsPointerIn(x, y, width, height))
                    {
                        currentStepScroll = stepScroll;
                        if (GameCanvas.pXYScrollMouse < 0)
                            currentOffsetTo += stepScroll;
                        else if (GameCanvas.pXYScrollMouse > 0)
                            currentOffsetTo -= stepScroll;
                        isPressKey = false;
                    }
                }
                else
                {
                    if (GameCanvas.isPointerJustDown && lastMouseY == -1 && IsPointerIn(x, y, width, height))
                    {
                        lastMouseY = GameCanvas.pyMouse;
                        currentStepScroll = stepScroll;
                        lastOffsetTo = currentOffsetTo = currentOffset;
                        isPressKey = false;
                    }
                    if (GameCanvas.isPointerMove)
                    {
                        currentOffsetTo = currentOffset = lastOffsetTo - (GameCanvas.pyMouse - lastMouseY);
                        isPressKey = false;
                    }
                }
                if (currentOffsetTo > items.Count * itemHeight - height)
                    currentOffsetTo = items.Count * itemHeight - height;
                if (currentOffsetTo < 0)
                    currentOffsetTo = 0;
            }
            if (GameCanvas.isPointerHoldIn(x, y, width, height))
            {
                if (!GameCanvas.isPointerMove || !GameCanvas.isPointerJustRelease)
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerSelect && (lastMouseY == GameCanvas.pyMouse || lastMouseY == -1))
                    {
                        isPressKey = false;
                        int selectedIndex = (GameCanvas.pyMouse - y + currentOffset) / itemHeight;
                        if (selectedIndex != currentItemIndex && selectedIndex < items.Count)
                            currentItemIndex = selectedIndex;
                        else
                        {
                            if (AllowSelectNone || items.Count == 0)
                                currentItemIndex = -1;
                        }
                        new Thread( () =>
                        {
                            Thread.Sleep(50);
                            ItemSelected?.Invoke();
                        }).Start();
                    }
                    //GameCanvas.clearAllPointerEvent();
                }
            }
            if (GameCanvas.isPointerJustRelease)
                lastMouseY = -1;
            if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
            {
                int minVisibleIndex = currentOffsetTo;
                int maxVisibleIndex = currentOffsetTo + height / itemHeight;
                int upperOffset = itemHeight * (currentItemIndex - 2);
                int lowerOffset = itemHeight * (currentItemIndex - (height / itemHeight) + 2);
                bool isCheckNewOffset = true;
                if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
                {
                    GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] = false;
                    if (currentItemIndex == -1)
                    {
                        currentItemIndex = items.Count - 1;
                        if (items.Count > (float)height / itemHeight)
                            currentOffsetTo = currentOffset = itemHeight * (currentItemIndex - 1);
                        isCheckNewOffset = false;
                    }
                    else
                    {
                        if (currentItemIndex - 1 < 0)
                        {
                            currentItemIndex = items.Count - 1;
                            currentOffsetTo = currentOffset = itemHeight * (currentItemIndex - 1);
                            isCheckNewOffset = false;
                        }
                        else
                            currentItemIndex--;
                    }
                    if (isCheckNewOffset && items.Count > (float)height / itemHeight)
                    {
                        if (!isPressKey)
                        {
                            if (currentOffsetTo < lowerOffset)
                                currentOffsetTo = currentOffset = lowerOffset - itemHeight;
                            else if (currentOffsetTo > upperOffset)
                                currentOffsetTo = currentOffset = upperOffset;
                        }
                        else if (upperOffset < minVisibleIndex)
                            currentOffsetTo = currentOffset = upperOffset;
                    }
                }
                else if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
                {
                    GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = false;
                    if (currentItemIndex == -1)
                    {
                        currentItemIndex = 0;
                        if (items.Count > (float)height / itemHeight)
                            currentOffsetTo = currentOffset = 0;
                        isCheckNewOffset = false;
                    }
                    else
                    {
                        if (currentItemIndex + 1 >= items.Count)
                        {
                            currentItemIndex = 0;
                            currentOffsetTo = currentOffset = 0;
                            isCheckNewOffset = false;
                        }
                        else
                            currentItemIndex++;
                    }
                    if (isCheckNewOffset && items.Count > (float)height / itemHeight)
                    {
                        if (!isPressKey)
                        {
                            if (currentOffsetTo < lowerOffset)
                                currentOffsetTo = currentOffset = lowerOffset;
                            else if (currentOffsetTo > upperOffset)
                                currentOffsetTo = currentOffset = upperOffset + itemHeight * 2;
                        }
                        else if (lowerOffset > maxVisibleIndex)
                            currentOffsetTo = currentOffset = lowerOffset;
                    }
                }
                if (currentOffsetTo > items.Count * itemHeight - height)
                    currentOffsetTo = currentOffset = items.Count * itemHeight - height;
                if (currentOffsetTo < 0)
                    currentOffsetTo = currentOffset = 0;
                isPressKey = true;
            }
            if (GameCanvas.keyPressed[13])
            {
                GameCanvas.keyPressed[13] = false;
                if (AllowSelectNone || items.Count == 0)
                    currentItemIndex = -1;
                isPressKey = false;
            }
            if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
            {
                new Thread(() =>
                {
                    ItemSelected?.Invoke();
                }).Start();
            }
        }

        internal void Paint(mGraphics g)
        {
            g.setColor(0xD3A46F);
            g.fillRect(x, y, width, height);
            g.setColor(Color.black);
            g.drawRect(x - 1, y - 1, width + 1, height + 1);
            g.setClip(x, y, width, height);
            for (int i = Math.Min((currentOffset + height) / itemHeight, items.Count - 1); i >= Math.Max(currentOffset / itemHeight, 0); i--)
            {
                g.setColor(Color.white);
                if (i == currentItemIndex)
                    g.setColor(0xFFF9BD);
                g.fillRect(x, y + i * itemHeight - currentOffset, width, itemHeight);
                g.setColor(new Color(0, 0, 0, .3f));
                g.fillRect(x, y + (i + 1) * itemHeight - currentOffset, width, 1);
                paintItemAction?.Invoke(g, i, x, y + i * itemHeight - currentOffset, width, itemHeight);
            }
            if (currentOffset <= 0)
            {
                g.setColor(new Color(0, 0, 0, .3f));
                g.fillRect(x, y - currentOffset, width, 1);
            }
        }

        static bool IsPointerIn(int x, int y, int w, int h) => GameCanvas.pxMouse >= x && GameCanvas.pxMouse <= x + w && GameCanvas.pyMouse >= y && GameCanvas.pyMouse <= y + h;
    }
}