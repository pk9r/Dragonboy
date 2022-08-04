using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using System.Reflection;

namespace Mod;
public static class CharExtensions
{
    public static int getTimeHold(Char @char)
    {
        
        int num = 35;
        try
        {
            if (!@char.me)
            {
                num = 35;
                if (@char.charEffectTime.isTiedByMe && Char.myCharz().cgender == 2) num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5;
            }
            else if (Char.myCharz().cgender == 2) num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5;
        }
        catch
        {
            num = 35;
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

    public static int getTimeShield(Char @char)
    {
        int num;
        try
        {
            if (!@char.me)
            {
                num = 47;
            }
            else
            {
                num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[7]).point * 5 + 12;
            }
        }
        catch
        {
            num = 47;
        }
        return num;
    }

    public static int getTimeMobMe(Char @char)
    {
        int num = 60;
        try
        {
            if (!@char.me)
            {
                switch (@char.mobMe.templateId)
                {
                    case 8:
                        num = 60;
                        break;
                    case 11:
                        num = 95;
                        break;
                    case 32:
                        num = 130;
                        break;
                    case 25:
                        num = 165;
                        break;
                    case 43:
                        num = 200;
                        break;
                    case 49:
                        num = 235;
                        break;
                    case 50:
                        num = 270;
                        break;
                }
            }
            else if (Char.myCharz().cgender == 1)
            {
                num = (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]).point - 1) * 35 + 60;
            }
        }
        catch
        {
            num = 270;
        }
        return num;
    }

    public static int getTimeHypnotize(Char @char)
    {
        int num = 13;
        try
        {
            if (!@char.me)
            {
                num = 13;
                if (@char.charEffectTime.isHypnotizedByMe) num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 6;
            }
            else if (Char.myCharz().cgender == 0) num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 6;
        }
        catch
        {
            num = 13;
        }
        return num;
    }

    public static int getTimeStone(Char @char)
    {
        return 3;
    }

    public static string getNameWithoutClanTag(Char @char)
    {
        return @char.cName.Remove(0, @char.cName.IndexOf(']') + 1).Replace(" ", "");
    }

    public static bool isNormalChar(Char @char, bool isIncludeBoss, bool isIncludePet)
    {
        bool result = !string.IsNullOrEmpty(@char.cName) && @char.cName != "Trọng tài";
        if (!string.IsNullOrEmpty(@char.cName))
        {
            bool isPet = (bool)typeof(Char).GetField("isPet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@char);
            bool isMiniPet = (bool)typeof(Char).GetField("isMiniPet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@char);
            if (!isIncludeBoss) result = result && !char.IsUpper(getNameWithoutClanTag(@char)[0]);
            if (!isIncludePet) result = result && !isPet && !isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$");
        }
        return result;
    }

    public static bool isBoss(Char @char)
    {
        bool isPet = (bool)typeof(Char).GetField("isPet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@char);
        bool isMiniPet = (bool)typeof(Char).GetField("isMiniPet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@char);
        return !isPet && !isMiniPet && @char.cName != "Trọng tài" && char.IsUpper(getNameWithoutClanTag(@char)[0]);
    }

    public static bool isPet(Char @char)
    {
        bool isPet = (bool)typeof(Char).GetField("isPet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@char);
        bool isMiniPet = (bool)typeof(Char).GetField("isMiniPet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@char);
        return isPet || isMiniPet || @char.cName.StartsWith("#") || @char.cName.StartsWith("$");
    }

    public static Char ClosestChar(int maxDistance, bool isNormalCharOnly)
    {
        int smallestDistance = 9999;
        Char result = null;
        if (GameScr.vCharInMap.size() <= 0) return null;
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char c = (Char)GameScr.vCharInMap.elementAt(i);
            if (isNormalCharOnly && !isNormalChar(c, false, false)) continue;
            int distance = Res.distance(Char.myCharz().cx, Char.myCharz().cy, c.cx, c.cy);
            if (!c.me && distance < smallestDistance)
            {
                smallestDistance = distance;
                result = c;
            }
        }
        if (result != null && Res.distance(Char.myCharz().cx, Char.myCharz().cy, result.cx, result.cy) > maxDistance) result = null;
        return result;
    }

    public static string getGender(Char @char)
    {
        if (@char.cgender == 0) return "TĐ";
        else if (@char.cgender == 1) return "NM";
        else if (@char.cgender == 2) return "XD";
        else return "BĐ";
    }

    public static Color getFlagColor(Char @char)
    {
        return @char.cFlag switch
        {
            1 => Color.cyan,
            2 => Color.red,
            3 => new Color(0.56f, 0.19f, 0.77f),
            4 => Color.yellow,
            5 => Color.green,
            6 => Color.magenta,
            7 => new Color(1f, 0.5f, 0),
            8 => new Color(0.18f, 0.18f, 0.18f),
            9 => Color.blue,
            10 => Color.red,
            11 => Color.blue,
            12 => Color.white,
            13 => Color.black,
            _ => Color.clear,
        };
    }
}
