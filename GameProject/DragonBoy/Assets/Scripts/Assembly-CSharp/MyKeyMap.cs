using System.Collections;
using UnityEngine;

public class MyKeyMap
{
	private static Hashtable h;

	static MyKeyMap()
	{
        h = new Hashtable()
        {
            { KeyCode.UpArrow, -1 },
            { KeyCode.DownArrow, -2 },
            { KeyCode.LeftArrow, -3 },
            { KeyCode.RightArrow, -4 },
            { KeyCode.Return, -5 },
            { KeyCode.Backspace, -8 },
            { KeyCode.F1, -21 },
            { KeyCode.F2, -22 },
            { KeyCode.F3, -23 },
            { KeyCode.Tab, -26 },
            { KeyCode.Escape, -30 },

            { KeyCode.F4, 0 },
            { KeyCode.F5, 0 },
            { KeyCode.F6, 0 },
            { KeyCode.F7, 0 },
            { KeyCode.F8, 0 },
            { KeyCode.F9, 0 },
            { KeyCode.F10, 0 },
            { KeyCode.F11, 0 },
            { KeyCode.F12, 0 },
            { KeyCode.F13, 0 },
            { KeyCode.F14, 0 },
            { KeyCode.F15, 0 },
            { KeyCode.LeftShift, 0 },
            { KeyCode.RightShift, 0 },
            { KeyCode.LeftAlt, 0 },
            { KeyCode.RightAlt, 0 },
            { KeyCode.AltGr, 0 },
            { KeyCode.LeftControl, 0 },
            { KeyCode.RightControl, 0 },
            { KeyCode.LeftMeta, 0 },
            { KeyCode.RightMeta, 0 },
            { KeyCode.Numlock, 0 },
            { KeyCode.PageUp, 0 },
            { KeyCode.PageDown, 0 },
            { KeyCode.Insert, 0 },
            { KeyCode.Delete, 0 },
            { KeyCode.Pause, 0 },
            { KeyCode.Break, 0 },
            { KeyCode.Print, 0 },
            { KeyCode.SysReq, 0 },
            { KeyCode.Home, 0 },
            { KeyCode.End, 0 },
            { KeyCode.Clear, 0 },
            { KeyCode.CapsLock, 0 },
            { KeyCode.Help, 0 },
            { KeyCode.Menu, 0 },
        };
	}

	public static int map(KeyCode k)
	{
		object obj = h[k];
        if (obj == null)
        {
            int num = (int)k;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Event.current.capsLock)
            {
                if (num >= 97 && num <= 122)
                    num -= 32;
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                switch (k)
                {
                    case KeyCode.BackQuote:
                        num = '~';
                        break;
                    case KeyCode.Alpha1:
                        num = (int)KeyCode.Exclaim;
                        break;
                    case KeyCode.Alpha2:
                        num = (int)KeyCode.At;
                        break;
                    case KeyCode.Alpha3:
                        num = (int)KeyCode.Hash;
                        break;
                    case KeyCode.Alpha4:
                        num = (int)KeyCode.Dollar;
                        break;
                    case KeyCode.Alpha5:
                        num = '%';
                        break;
                    case KeyCode.Alpha6:
                        num = (int)KeyCode.Caret;
                        break;
                    case KeyCode.Alpha7:
                        num = (int)KeyCode.Ampersand;
                        break;
                    case KeyCode.Alpha8:
                        num = (int)KeyCode.Asterisk;
                        break;
                    case KeyCode.Alpha9:
                        num = (int)KeyCode.LeftParen;
                        break;
                    case KeyCode.Alpha0:
                        num = (int)KeyCode.RightParen;
                        break;
                    case KeyCode.Minus:
                        num = (int)KeyCode.Underscore;
                        break;
                    case KeyCode.Equals:
                        num = (int)KeyCode.Plus;
                        break;
                    case KeyCode.LeftBracket:
                        num = '{';
                        break;
                    case KeyCode.RightBracket:
                        num = '}';
                        break;
                    case KeyCode.Backslash:
                        num = '|';
                        break;
                    case KeyCode.Semicolon:
                        num = (int)KeyCode.Colon;
                        break;
                    case KeyCode.Quote:
                        num = (int)KeyCode.DoubleQuote;
                        break;
                    case KeyCode.Comma:
                        num = (int)KeyCode.Less;
                        break;
                    case KeyCode.Period:
                        num = (int)KeyCode.Greater;
                        break;
                    case KeyCode.Slash:
                        num = (int)KeyCode.Question;
                        break;
                }
            }
            return num;
        }
        return (int)obj;
	}
}
