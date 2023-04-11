using Mod.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

namespace Mod.CustomGroupBox
{
    [Obsolete("Cần fix thêm")]
    public class GroupBox
    {
        #region Properties
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int AbsoluteWidth
        {
            get => m_AbsoluteWidth;
            set
            {
                m_AbsoluteWidth = value;
                Width = value;
            }
        }

        public int AbsoluteHeight
        {
            get => m_AbsoluteHeight;
            set
            {
                m_AbsoluteHeight = value;
                Height = value;
            }
        }

        public bool HasBorder { get; set; }

        public bool Collapsible { get; set; }

        public string Title
        {
            get => m_title;
            set
            {
                onTitleValueChanged(value);
                m_title = value;
            }
        }

        public GUIStyle TitleStyle { get; set; }

        public TextAnchor TitleAnchor
        {
            get => _titleAnchor;
            set
            {
                if (value > TextAnchor.UpperRight)
                    throw new ArgumentException("TextAnchor must be upper!");
                _titleAnchor = value;
            }
        }

        public Color BackColor { get; set; } = Color.clear;

        public Color BorderColor { get; set; } = new Color(1f, 1f, 0);
        #endregion

        TextAnchor _titleAnchor;

        public StateGroupBox CurrentState { get; private set; } = StateGroupBox.Showed;

        float angleRotation;
        int m_AbsoluteWidth;
        int m_AbsoluteHeight;
        int titleWidth;
        int titleHeight;
        string m_title;
        bool m_isSetTitleWithAndHeight;

        public GroupBox(string title = "", int x = 50, int y = 50, int width = 100, int height = 50)
        {
            Title = title;
            X = x;
            Y = y;
            AbsoluteWidth = Width = width;
            AbsoluteHeight = Height = height;
        }

        public void Paint(mGraphics g)
        {
            if (TitleStyle == null)
                TitleStyle = GUI.skin.label;
            if (!m_isSetTitleWithAndHeight)
            {
                m_isSetTitleWithAndHeight = true;
                titleWidth = string.IsNullOrEmpty(Title) ? 0 : Utilities.getWidth(TitleStyle, Title) + (_titleAnchor == TextAnchor.UpperCenter || _titleAnchor == TextAnchor.LowerCenter ? 2 : 7);
                titleHeight = string.IsNullOrEmpty(Title) ? 0 : Utilities.getHeight(TitleStyle, Title);
            }
            UpdateTouch();
            int translateX = g.getTranslateX();
            int translateY = g.getTranslateY();
            if (TitleStyle.alignment != TextAnchor.UpperLeft)
                TitleStyle.alignment = TextAnchor.UpperLeft;
            g.reset();
            g.setClip(X, Y, Width, Height);
            g.translate(X, Y);
            g.setColor(BackColor);
            g.fillRect(0, 0, Width, Height);
            if ((CurrentState & StateGroupBox.Collapsing) != 0)
            {
                if ((CurrentState & StateGroupBox.Showed) != 0)
                {
                    if (angleRotation > -90f)
                        angleRotation -= 90 / 10;
                    if (angleRotation == -90f && Height == titleHeight)
                        CurrentState = CurrentState ^ StateGroupBox.Showed | StateGroupBox.Collapsed;
                    Height -= AbsoluteHeight / 10;
                    if (Height < titleHeight)
                        Height = titleHeight;
                }
                else if ((CurrentState & StateGroupBox.Collapsed) != 0)
                {
                    Width -= AbsoluteWidth / 10;
                    int addedWidth = 1;
                    if (_titleAnchor == TextAnchor.UpperCenter)
                    {
                        addedWidth = 8;
                        X += AbsoluteWidth / 20;
                    }
                    else if (_titleAnchor == TextAnchor.UpperRight)
                    {
                        addedWidth = 4;
                        X += AbsoluteWidth / 10;
                    }
                    if (Width <= addedWidth + titleWidth) 
                    {
                        Width = addedWidth + titleWidth;
                        if (_titleAnchor == TextAnchor.UpperCenter)
                            X -= AbsoluteWidth / 20;
                        CurrentState = StateGroupBox.Hided;
                    }
                }
            }
            else if ((CurrentState & StateGroupBox.Expanding) != 0) 
            {
                if ((CurrentState & StateGroupBox.Hided) != 0)
                {
                    Width += AbsoluteWidth / 10;
                    if (_titleAnchor == TextAnchor.UpperCenter)
                        X -= AbsoluteWidth / 20;
                    else if (_titleAnchor == TextAnchor.UpperRight)
                        X -= AbsoluteWidth / 10;
                    if (Width >= AbsoluteWidth)
                    {
                        Width = AbsoluteWidth;
                        if (_titleAnchor == TextAnchor.UpperCenter)
                            X += AbsoluteWidth / 20;
                        CurrentState = CurrentState ^ StateGroupBox.Hided | StateGroupBox.Collapsed;
                    }
                }
                else if ((CurrentState & StateGroupBox.Collapsed) != 0)
                {
                    if (angleRotation < 0)
                        angleRotation += 90 / 10;
                    if (angleRotation == 0 && Height == AbsoluteHeight)
                        CurrentState = StateGroupBox.Showed;
                    Height += AbsoluteHeight / 10;
                    if (Height > AbsoluteHeight)
                        Height = AbsoluteHeight;

                }
            }
            if (CurrentState != StateGroupBox.Hided)
            {
                if (_titleAnchor == TextAnchor.UpperLeft)
                    g.drawString(Title, 7, 0, TitleStyle);
                else if (_titleAnchor == TextAnchor.UpperCenter)
                    g.drawString(Title, Width / 2 - titleWidth / 2, 0, TitleStyle);
                else if (_titleAnchor == TextAnchor.UpperRight)
                    g.drawString(Title, Width - titleWidth + 1, 0, TitleStyle);
                if (HasBorder)
                {
                    g.setColor(BorderColor);
                    if (_titleAnchor == TextAnchor.UpperLeft)
                    {
                        g.fillRect(0, titleHeight / 2, 5, 1); //top left
                        if (Collapsible)
                        {
                            if (titleWidth < Width)
                                g.fillRect(titleWidth + 2, titleHeight / 2, Width - titleWidth + 2 - 19, 1); //top center
                            CustomGraphics.DrawAPartOfImage(Mob.imgHP, X + Width - 14, Y + titleHeight / 4, 9, 6, 0, 0, angleRotation);
                            g.fillRect(Width - 4, titleHeight / 2, 4, 1); //top right
                        }
                        else if (titleWidth < Width)
                            g.fillRect(titleWidth + 1, titleHeight / 2, Width - titleWidth + 3, 1); //top right
                        if (Height > titleHeight && Width > titleWidth)
                        {
                            g.fillRect(0, titleHeight / 2, 1, Height - titleHeight / 2);  //left
                            g.fillRect(0, Height - 1, Width, 1); //bottom
                            g.fillRect(0 + Width - 1, titleWidth < Width ? titleHeight / 2 : titleHeight, 1, Height - (titleWidth < Width ? titleHeight / 2 : titleHeight));    //right
                        }
                    }
                    else if (_titleAnchor == TextAnchor.UpperCenter)
                    {
                        if (Collapsible)
                        {
                            g.fillRect(0, titleHeight / 2, 5, 1); //top left
                            CustomGraphics.DrawAPartOfImage(Mob.imgHP, X + 6, Y + titleHeight / 4, 9, 6, 0, 0, angleRotation);
                            if (titleWidth < Width)
                                g.fillRect(16, titleHeight / 2, Width / 2 - titleWidth / 2 - 18, 1); //top center    
                        }
                        else if (titleWidth < Width)
                            g.fillRect(0, titleHeight / 2, Width / 2 - titleWidth / 2 - 2, 1); //top left
                        if (titleWidth < Width)
                            g.fillRect(Width / 2 + titleWidth / 2 - 2, titleHeight / 2, Width / 2 - titleWidth / 2 + 2, 1); //top right
                        if (Height > titleHeight && Width > titleWidth)
                        {
                            g.fillRect(0, titleHeight / 2, 1, Height - titleHeight / 2);  //left
                            g.fillRect(0, Height - 1, Width, 1); //bottom
                            g.fillRect(0 + Width - 1, titleWidth < Width ? titleHeight / 2 : titleHeight, 1, Height - (titleWidth < Width ? titleHeight / 2 : titleHeight));    //right
                        }
                    }
                    else if (_titleAnchor == TextAnchor.UpperRight)
                    {
                        g.fillRect(Width - 5, titleHeight / 2, 5, 1); //top right
                        if (Collapsible)
                        {
                            if (titleWidth < Width)
                                g.fillRect(16, titleHeight / 2, Width - titleWidth + 2 - 20, 1); //top center
                            CustomGraphics.DrawAPartOfImage(Mob.imgHP, X + 6, Y + titleHeight / 4, 9, 6, 0, 0, angleRotation);
                            g.fillRect(0, titleHeight / 2, 5, 1); //top left
                        }
                        else if (titleWidth < Width)
                                g.fillRect(0, titleHeight / 2, Width - titleWidth - 2, 1); //top left
                        if (Height > titleHeight && Width > titleWidth)
                        {
                            g.fillRect(0, titleHeight / 2, 1, Height - titleHeight / 2);  //left
                            g.fillRect(0, Height - 1, Width, 1); //bottom
                            g.fillRect(0 + Width - 1, titleWidth < Width ? titleHeight / 2 : titleHeight, 1, Height - (titleWidth < Width ? titleHeight / 2 : titleHeight));    //right
                        }
                    }
                }
            }
            else if(Collapsible)
            {
                if (_titleAnchor == TextAnchor.UpperLeft)
                {
                    g.drawString(Title, 2, 0, TitleStyle);
                    CustomGraphics.DrawAPartOfImage(Mob.imgHP, X + titleWidth - 8, Y + titleHeight / 4, 9, 6, 0, 0, angleRotation);
                }
                else if (_titleAnchor == TextAnchor.UpperCenter || _titleAnchor == TextAnchor.UpperRight)
                {
                    g.drawString(Title, 10, 0, TitleStyle);
                    CustomGraphics.DrawAPartOfImage(Mob.imgHP, X + 1, Y + titleHeight / 4, 9, 6, 0, 0, angleRotation);
                }
            }
            g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
            g.translate(translateX, translateY);
        }

        void onTitleValueChanged(string value)
        {
            if (TitleStyle == null)
                return;
            titleWidth = string.IsNullOrEmpty(value) ? 0 : Utilities.getWidth(TitleStyle, value) + (_titleAnchor == TextAnchor.UpperCenter || _titleAnchor == TextAnchor.LowerCenter ? 2 : 7);
            titleHeight = string.IsNullOrEmpty(value) ? 0 : Utilities.getHeight(TitleStyle, value);
        }

        void UpdateTouch()
        {
            if (!Collapsible)
                return;
            if (GameScr.gI().isNotPaintTouchControl())
                return;
            if (GameCanvas.panel.isShow || ChatPopup.serverChatPopUp != null)
                return;
            if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                return;
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            if (_titleAnchor == TextAnchor.UpperLeft)
            {
                x = X + Width - 14;
                y = Y + titleHeight / 4;
                w = h = 9;
            }
            else if (_titleAnchor == TextAnchor.UpperCenter || _titleAnchor == TextAnchor.UpperRight)
            {
                x = X + 2;
                y = Y + titleHeight / 4;
                w = h = 9;
            }
            if (GameCanvas.isPointer(X, Y, Width, titleHeight))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick && GameCanvas.px >= x && GameCanvas.px <= x + w && GameCanvas.py >= y && GameCanvas.py <= y + h)
                        ButtonClick();
                GameCanvas.clearAllPointerEvent();
            }
        }

        public void ButtonClick()
        {
            if (CurrentState == StateGroupBox.Showed)
            {
                if ((CurrentState | StateGroupBox.Expanding) == CurrentState)   //Collapsed | expanding
                    CurrentState ^= StateGroupBox.Expanding;
                CurrentState |= StateGroupBox.Collapsing;
            }
            else if (CurrentState == StateGroupBox.Hided)
            {
                if ((CurrentState | StateGroupBox.Collapsing) == CurrentState)   //Collapsed | collapsing
                    CurrentState ^= StateGroupBox.Collapsing;
                CurrentState |= StateGroupBox.Expanding;
            }
        }
    }
}
