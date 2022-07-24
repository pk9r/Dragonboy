using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Mod;
public static class CharExtensions
{
    //Đéo có cách nào lấy thời gian trói cả :))
    public static int getTimeHold(Char @char)
    {
        int num = 36;
        try
        {
            if (!@char.me)
            {
                num = 121;
            }
            else if (Char.myCharz().cgender == 2)
            {
                num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5 + 1;
            }
        }
        catch
        {
            num = 31;
        }
        return num;
    }

    public static int getTimeMonkey(Char @char)
    {
        int num = 61;
        try
        {
            if (!@char.me)
            {
                switch (@char.head)
                {
                    case 192:
                        num = 61;
                        break;
                    case 195:
                        num = 71;
                        break;
                    case 196:
                        num = 81;
                        break;
                    case 199:
                        num = 91;
                        break;
                    case 197:
                        num = 101;
                        break;
                    case 200:
                        num = 111;
                        break;
                    case 198:
                        num = 121;
                        break;
                }
            }
            else if (Char.myCharz().cgender == 2)
            {
                num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]).point * 10 + 51;
            }
        }
        catch
        {
            num = 121;
        }
        return num;
    }
    //Đéo có cách nào lấy thời gian khiên cả :))
    public static int getTimeShield(Char @char)
    {
        int num;
        try
        {
            if (!@char.me)
            {
                num = 46;
            }
            else
            {
                num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[7]).point * 5 + 11;
            }
        }
        catch
        {
            num = 46;
        }
        return num;
    }

    public static int getTimeMobMe(Char @char)
    {
        int num = 64;
        try
        {
            if (!@char.me)
            {
                switch (@char.mobMe.templateId)
                {
                    case 8:
                        num = 64;
                        break;
                    case 11:
                        num = 99;
                        break;
                    case 32:
                        num = 134;
                        break;
                    case 25:
                        num = 169;
                        break;
                    case 43:
                        num = 204;
                        break;
                    case 49:
                        num = 239;
                        break;
                    case 50:
                        num = 274;
                        break;
                }
            }
            else if (Char.myCharz().cgender == 1)
            {
                num = (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]).point - 1) * 35 + 64;
            }
        }
        catch
        {
            num = 274;
        }
        return num;
    }

    public static string getNameWithoutClanTag(Char @char)
    {
        return @char.cName.Remove(0, @char.cName.IndexOf(']') + 1).Replace(" ", "");
    }

    public static bool isNormalChar(Char @char)
    {
        return !string.IsNullOrEmpty(@char.cName) && !char.IsUpper(getNameWithoutClanTag(@char)[0]) && @char.charID >= 0 && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$");
    }
}
