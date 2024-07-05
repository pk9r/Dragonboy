using UnityEngine;

namespace InputMap.Icons
{
    internal static class XboxControllerIcons
    {
        internal static Texture2D A;
        internal static Texture2D B;
        internal static Texture2D X;
        internal static Texture2D Y;
        internal static Texture2D LB;
        internal static Texture2D RB;
        internal static Texture2D LT;
        internal static Texture2D RT;
        internal static Texture2D Menu;
        internal static Texture2D View;
        internal static Texture2D Left;
        internal static Texture2D Right;
        internal static Texture2D Up;
        internal static Texture2D Down;
        internal static Texture2D LeftStick;
        internal static Texture2D RightStick;
        internal static Texture2D LeftStickButton;
        internal static Texture2D RightStickButton;
        internal static Texture2D DPad;
        internal static Texture2D DPadUp;
        internal static Texture2D DPadDown;
        internal static Texture2D DPadLeft;
        internal static Texture2D DPadRight;

        internal static void Initialize()
        {
            A = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(A) + "_dark");
            B = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(B) + "_dark");
            X = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(X) + "_dark");
            Y = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(Y) + "_dark");
            LB = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(LB) + "_dark");
            RB = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(RB) + "_dark");
            LT = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(LT) + "_dark");
            RT = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(RT) + "_dark");
            Menu = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(Menu) + "_dark");
            View = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(View) + "_dark");
            Left = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(Left) + "_dark");
            Right = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(Right) + "_dark");
            Up = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(Up) + "_dark");
            Down = Resources.Load<Texture2D>("Controller/Xbox/" + nameof(Down) + "_dark");
            DPadUp = Resources.Load<Texture2D>("Controller/Xbox/Digipad_dark");
            DPadUp = Resources.Load<Texture2D>("Controller/Xbox/Digipad_up_dark");
            DPadDown = Resources.Load<Texture2D>("Controller/Xbox/Digipad_down_dark");
            DPadLeft = Resources.Load<Texture2D>("Controller/Xbox/Digipad_left_dark");
            DPadRight = Resources.Load<Texture2D>("Controller/Xbox/Digipad_right_dark");
            LeftStick = Resources.Load<Texture2D>("Controller/Xbox/L_dark");
            RightStick = Resources.Load<Texture2D>("Controller/Xbox/R_dark");
            LeftStickButton = Resources.Load<Texture2D>("Controller/Xbox/L_light");
            RightStickButton = Resources.Load<Texture2D>("Controller/Xbox/R_light");
        }
    }
}