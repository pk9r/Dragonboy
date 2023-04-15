using System.Reflection;
using UnityEngine;

namespace Mod
{
    public static class CharExtensions
    {
        public static int getTimeHold(this Char @char)
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

        public static int getTimeMonkey(this Char @char)
        {
            int num = 60;
            try
            {
                if (!@char.me)
                {
                    switch (@char.head)
                    {
                        case 192:
                            num = 60;   //lv 1
                            break;
                        case 195:
                            num = 70;   //lv 2
                            break;
                        case 196:
                            num = 80;   //lv 3
                            break;
                        case 199:
                            num = 90;   //lv 4
                            break;
                        case 197:
                            num = 100;  //lv 5
                            break;
                        case 200:
                            num = 110;  //lv 6
                            break;
                        case 198:
                            num = 120;  //lv 7
                            break;
                    }
                }
                else if (Char.myCharz().cgender == 2)
                {
                    num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[3]).point * 10 + 50;
                }
            }
            catch
            {
                num = 121;
            }
            return num;
        }

        public static int getTimeShield(this Char @char)
        {
            int num;
            try
            {
                if (!@char.me)
                {
                    num = 45;
                }
                else
                {
                    num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[7]).point * 5 + 10;
                }
            }
            catch
            {
                num = 45;
            }
            return num;
        }

        public static int getTimeMobMe(this Char @char)
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

        public static int getTimeHypnotize(this Char @char)
        {
            int num = 12;
            try
            {
                if (!@char.me)
                {
                    num = 12;
                    if (@char.charEffectTime.isHypnotizedByMe) num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 5;
                }
                else if (Char.myCharz().cgender == 0) num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 5;
            }
            catch
            {
                num = 12;
            }
            return num;
        }

        public static int getTimeStone(this Char @char)
        {
            return 5;
        }

        public static int getTimeHuytSao(this Char @char)
        {
            return 31;
        }

        public static int getTimeChocolate(this Char @char)
        {
            return 31;
        }

        public static string getNameWithoutClanTag(this Char @char, bool enableRichText = false)
        {
            string name = @char.cName.Remove(0, @char.cName.IndexOf(']') + 1).TrimStart(' ', '#', '$');
            if (enableRichText)
            {
                if (@char.isPet())
                    name = $"<color=cyan>{name}</color>";
                else if (@char.isBoss())
                    name = $"<color=red><size={7 * mGraphics.zoomLevel}>{name}</size></color>";
                else 
                    name = $"<color=yellow>{name}</color>";
            }
            return name;
        }

        public static string getClanTag(this Char @char)
        {
            return @char.cName.Substring(0, @char.cName.IndexOf(']') + 1) + ' ';
        }

        public static bool isNormalChar(this Char @char, bool isIncludeBoss = false, bool isIncludePet = false)
        {
            bool result = !string.IsNullOrEmpty(@char.cName) && @char.cName != "Trọng tài";
            if (!string.IsNullOrEmpty(@char.cName))
            {
                if (!isIncludeBoss) result = result && !char.IsUpper(getNameWithoutClanTag(@char)[0]);
                if (!isIncludePet) result = result && !@char.isPet();
            }
            return result;
        }

        public static bool isBoss(this Char @char)
        {
            return !@char.isPet() && @char.cName != "Trọng tài" && char.IsUpper(getNameWithoutClanTag(@char)[0]);
        }

        public static bool isPet(this Char @char)
        {
            return @char.IsPet() || @char.IsMiniPet() || @char.cName.StartsWith("#") || @char.cName.StartsWith("$"); 
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

        public static string getGender(this Char @char, bool enableRichText = false)
        {
            if (enableRichText)
            {
                if (@char.cgender == 0) return "<color=#0080ffff>TĐ</color>";
                else if (@char.cgender == 1) return "<color=#00c000ff>NM</color>";
                else if (@char.cgender == 2) return "<color=#ffff80ff>XD</color>";
                else return "<color=red>B</color><color=purple>Đ</color>";
            }
            if (@char.cgender == 0) return "TĐ";
            else if (@char.cgender == 1) return "NM";
            else if (@char.cgender == 2) return "XD";
            else return "BĐ";
        }

        public static Color getFlagColor(this Char @char)
        {
            switch (@char.cFlag)
            {
                case 1: return Color.cyan;
                case 2: return Color.red;
                case 3: return new Color(0.56f, 0.19f, 0.77f);
                case 4: return Color.yellow;
                case 5: return Color.green;
                case 6: return Color.magenta;
                case 7: return new Color(1f, 0.5f, 0f);
                case 8: return new Color(0.18f, 0.18f, 0.18f);
                case 9: return Color.blue;
                case 10: return Color.red;
                case 11: return Color.blue;
                case 12: return Color.white;
                case 13: return Color.black;
                default: return Color.clear;
            }
        }

        public static int getSuicideRange(this Char @char)
        {
            int result = 880;
            if (@char.me && @char.cgender == 2)
            {
                Skill skill5 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]);
                if (skill5 == null)
                    return 0;
                result = 340 * (skill5.point - 1) / 3 + 200;
            }
            return result;
        }

        public static Char findCharInMap(string name)
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                if (@char.getNameWithoutClanTag() == name)
                    return @char;
            }
            return null;
        }
    }
}