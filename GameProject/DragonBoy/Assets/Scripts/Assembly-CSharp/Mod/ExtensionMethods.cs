using System;
using Mod.CustomPanel;
using Mod.R;
using UnityEngine;

#if UNITY_EDITOR
using Mono.Cecil.Cil;
#endif

namespace Mod
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Mô phỏng <see cref="Panel.setType(int)"/> mà không mở lại panel
        /// </summary>
        internal static void EmulateSetTypePanel(this Panel panel, int position)
        {
            panel.typeShop = -1;
            panel.W = Panel.WIDTH_PANEL;
            panel.H = GameCanvas.h;
            panel.X = 0;
            panel.Y = 0;
            panel.ITEM_HEIGHT = 24;
            panel.position = position;
            if (position == 0)
            {
                panel.xScroll = 2;
                panel.yScroll = 80;
                panel.wScroll = panel.W - 4;
                panel.hScroll = panel.H - 96;
                //panel.cmx = panel.wScroll;
                panel.cmx = panel.cmtoX = 0;
                panel.X = 0;
            }
            else if (position == 1)
            {
                panel.wScroll = panel.W - 4;
                panel.xScroll = GameCanvas.w - panel.wScroll;
                panel.yScroll = 80;
                panel.hScroll = panel.H - 96;
                panel.X = panel.xScroll - 2;
                //panel.cmx = -(GameCanvas.w + panel.W);
                panel.cmx = panel.cmtoX = GameCanvas.w - panel.W;
            }
            panel.TAB_W = panel.W / 5 - 1;
            if (panel.currentTabName.Length < 5)
                panel.TAB_W += 5;
            panel.startTabPos = panel.xScroll + panel.wScroll / 2 - panel.currentTabName.Length * panel.TAB_W / 2;
            panel.cmyLast = new int[panel.currentTabName.Length];
            int[] lastSelect = new int[panel.currentTabName.Length];
            for (int i = 0; i < panel.currentTabName.Length; i++)
                lastSelect[i] = GameCanvas.isTouch ? (-1) : 0;
            panel.lastSelect = lastSelect;
            panel.scroll = null;
            panel.lastTabIndex[CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU] = panel.currentTabIndex;
        }

        /// <summary>
        /// Lấy thông tin đầy đủ (gồm tên, chi tiết, level, ...) của <paramref name="item"/>
        /// </summary>
        /// <param name="item">Item cần lấy tên</param>
        /// <returns></returns>
        internal static string GetFullInfo(this Item item)
        {
            string text = item.template.name;
            if (item.itemOption != null)
                for (int i = 0; i < item.itemOption.Length; i++)
                    if (item.itemOption[i].optionTemplate.id == 72)
                    {
                        text = text + " [+" + item.itemOption[i].param.ToString() + "]";
                        break;
                    }
            if (item.itemOption != null)
                for (int j = 0; j < item.itemOption.Length; j++)
                    if (item.itemOption[j].optionTemplate.name.StartsWith("$"))
                    {
                        string optionColor = item.itemOption[j].getOptiongColor();
                        if (item.itemOption[j].param == 1)
                            text = text + "\n" + optionColor;
                        if (item.itemOption[j].param == 0)
                            text = text + "\n" + optionColor;
                    }
                    else
                    {
                        string optionString = item.itemOption[j].getOptionString();
                        if (!optionString.Equals(string.Empty) && item.itemOption[j].optionTemplate.id != 72)
                            text = text + "\n" + optionString;
                    }
            if (item.template.strRequire > 1)
                text += "\n" + mResources.pow_request + ": " + item.template.strRequire.ToString();
            return text + "\n" + item.template.description;
        }

        internal static T getValueProperty<T>(this object obj, string name)
        {
            return (T)obj.GetType().GetProperty(name).GetValue(obj, null);
        }

        /// <summary>
        /// Khôi phục trạng thái mặc định của <paramref name="tf"/>
        /// </summary>
        /// <param name="tf">ChatTextField cần khôi phục</param>
        internal static void ResetTF(this ChatTextField tf)
        {
            tf.strChat = "Chat";
            tf.tfChat.name = "chat";
            tf.to = "";
            tf.tfChat.setIputType(TField.INPUT_TYPE_ANY);
            tf.isShow = false;
        }

        internal static void ResetSize(this GameCanvas gameCanvas)
        {
            GameCanvas.w = MotherCanvas.instance.getWidthz();
            GameCanvas.h = MotherCanvas.instance.getHeightz();
            GameCanvas.hw = GameCanvas.w / 2;
            GameCanvas.hh = GameCanvas.h / 2;
            GameCanvas.wd3 = GameCanvas.w / 3;
            GameCanvas.hd3 = GameCanvas.h / 3;
            GameCanvas.w2d3 = 2 * GameCanvas.w / 3;
            GameCanvas.h2d3 = 2 * GameCanvas.h / 3;
            GameCanvas.w3d4 = 3 * GameCanvas.w / 4;
            GameCanvas.h3d4 = 3 * GameCanvas.h / 4;
            GameCanvas.wd6 = GameCanvas.w / 6;
            GameCanvas.hd6 = GameCanvas.h / 6;
            GameCanvas.isTouch = true;
            if (GameCanvas.w >= 240)
                GameCanvas.isTouchControl = true;
            if (GameCanvas.w < 320)
            {
                GameCanvas.isTouchControlSmallScreen = true;
                GameCanvas.isTouchControlLargeScreen = false;
            }
            if (GameCanvas.w >= 320)
            {
                GameCanvas.isTouchControlSmallScreen = false;
                GameCanvas.isTouchControlLargeScreen = true;
            }
            if (GameCanvas.h <= 160)
            {
                Paint.hTab = 15;
                mScreen.cmdH = 17;
            }
            GameScr.d = ((GameCanvas.w <= GameCanvas.h) ? GameCanvas.h : GameCanvas.w) + 20;
            Panel.WIDTH_PANEL = 176;
            if (Panel.WIDTH_PANEL > GameCanvas.w)
                Panel.WIDTH_PANEL = GameCanvas.w;
            Utils.ResetTextField(GameCanvas.panel?.chatTField);
            Utils.ResetTextField(GameCanvas.panel2?.chatTField);
        }

        internal static void SetGamePadZone(this GamePad gamePad)
        {
            gamePad.isSmallGamePad = GameCanvas.w < 320;
            gamePad.isMediumGamePad = GameCanvas.w >= 320 && GameCanvas.w <= 400;
            gamePad.isLargeGamePad = GameCanvas.w > 400;
            gamePad.xZone = 0;
            if (!gamePad.isLargeGamePad)
            {
                gamePad.wZone = GameCanvas.hw / 3 * 2;
                gamePad.yZone = GameCanvas.h / 2;
                gamePad.hZone = GameCanvas.h - 80;
            }
            else
            {
                gamePad.wZone = GameCanvas.hw / 3 * 1;
                gamePad.yZone = GameCanvas.hh / 2;
                gamePad.hZone = GameCanvas.h;
            }
        }

        internal static bool IsFromMyClan(this Char ch)
        {
            if (Char.myCharz().clan == null)
                return false;
            if (ch.charID == Char.myCharz().charID)
                return true;
            else if (ch.charID == -Char.myCharz().charID)
                return true;
            if (ch.IsPet())
            {
                ch = GameScr.findCharInMap(-ch.charID);
                if (ch == null)
                    return false;
            }
            return ch.clanID == Char.myCharz().clan.ID;
        }

        internal static int GetTimeHold(this Char ch)
        {
            int num = 35;
            try
            {
                if (!ch.me)
                {
                    num = 35;
                    if (ch.charEffectTime.isTiedByMe && Char.myCharz().cgender == 2)
                        num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5;
                }
                else if (Char.myCharz().cgender == 2)
                    num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5;
            }
            catch
            {
                num = 35;
            }
            return num;
        }

        internal static int GetTimeMonkey(this Char ch)
        {
            int num = 60;
            try
            {
                if (!ch.me)
                {
                    switch (ch.head)
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
                    num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[3]).point * 10 + 50;
            }
            catch
            {
                num = 121;
            }
            return num;
        }

        internal static int GetTimeShield(this Char ch)
        {
            int num;
            try
            {
                if (!ch.me)
                    num = 45;
                else
                    num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[7]).point * 5 + 10;
            }
            catch
            {
                num = 45;
            }
            return num;
        }

        internal static int GetTimeMobMe(this Char ch)
        {
            int num = 60;
            try
            {
                if (!ch.me)
                {
                    switch (ch.mobMe.getTemplate().mobTemplateId)
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
                    num = (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]).point - 1) * 35 + 60;
            }
            catch
            {
                num = 270;
            }
            return num;
        }

        internal static int GetTimeHypnotize(this Char ch)
        {
            int num = 12;
            try
            {
                if (!ch.me)
                {
                    num = 12;
                    if (ch.charEffectTime.isHypnotizedByMe)
                        num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 5;
                }
                else if (Char.myCharz().cgender == 0) 
                    num = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 5;
            }
            catch
            {
                num = 12;
            }
            return num;
        }

        internal static int GetTimeStone(this Char ch) => 5;

        internal static int GetTimeWhistle(this Char ch) => 31;

        internal static int GetTimeChocolate(this Char ch) => 31;

        internal static int GetTimeSelfExplode(this Char ch) => 3;

        internal static int GetTimeQCKK(this Char ch) => 3;

        internal static int GetTimeBlind(this Char ch)
        {
            if (ch.me && ch.cgender == 0)
            {
                int result = ch.getSkill(ch.nClass.skillTemplates[2]).point;
                if (Utils.isMeWearingTXHSet())
                    result *= 2;
                return result - 1;
            }
            return ch.freezSeconds - 1;
        }

        internal static string GetNameWithoutClanTag(this Char ch, bool enableRichText = false)
        {
            string name = ch.cName.Remove(0, ch.cName.IndexOf(']') + 1).TrimStart(' ', '#', '$');
            if (enableRichText)
            {
                if (ch.IsPet())
                    name = $"<color=cyan>{name}</color>";
                else if (ch.IsBoss())
                    name = $"<color=red><size={7 * mGraphics.zoomLevel}>{name}</size></color>";
                else
                    name = $"<color=yellow>{name}</color>";
            }
            return name;
        }

        internal static string GetClanTag(this Char ch) => ch.cName.Substring(0, ch.cName.IndexOf(']') + 1);

        internal static bool IsNormalChar(this Char ch, bool isIncludeBoss = false, bool isIncludePet = false)
        {
            bool result = !string.IsNullOrEmpty(ch.cName) && ch.cName != LocalizedString.arbitration;
            if (!string.IsNullOrEmpty(ch.cName))
            {
                if (!isIncludeBoss)
                    result = result && !char.IsUpper(GetNameWithoutClanTag(ch)[0]);
                if (!isIncludePet)
                    result = result && !ch.IsPet();
            }
            return result;
        }

        internal static bool IsBoss(this Char ch) => !ch.IsPet() && ch.cName != LocalizedString.arbitration && char.IsUpper(GetNameWithoutClanTag(ch)[0]);

        internal static bool IsPet(this Char ch) => ch.isPet || ch.isMiniPet || ch.cName.StartsWith("#") || ch.cName.StartsWith("$");

        internal static Char ClosestChar(this Char ch, int maxDistance, bool isNormalCharOnly)
        {
            int smallestDistance = int.MaxValue;
            Char result = null;
            if (GameScr.vCharInMap.size() <= 0)
                return null;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char c = (Char)GameScr.vCharInMap.elementAt(i);
                if (c == ch)
                    continue;
                if (isNormalCharOnly && !c.IsNormalChar(false, false))
                    continue;
                int distance = Res.distance(ch.cx, ch.cy, c.cx, c.cy);
                if (!c.me && distance < smallestDistance)
                {
                    smallestDistance = distance;
                    result = c;
                }
            }
            if (result != null && Res.distance(ch.cx, ch.cy, result.cx, result.cy) > maxDistance)
                result = null;
            return result;
        }

        internal static string GetGender(this Char ch, bool enableRichText = false)
        {
            if (enableRichText)
            {
                if (ch.cgender == 0)
                    return "<color=#0080ffff>TĐ</color>";
                else if (ch.cgender == 1) 
                    return "<color=#00c000ff>NM</color>";
                else if (ch.cgender == 2)
                    return "<color=#ffff80ff>XD</color>";
                else
                    return "<color=magenta>BĐ</color>";
            }
            if (ch.cgender == 0)
                return "TĐ";
            else if (ch.cgender == 1)
                return "NM";
            else if (ch.cgender == 2)
                return "XD";
            else
                return "BĐ";
        }

        internal static Color GetFlagColor(this Char ch)
        {
            switch (ch.cFlag)
            {
                case 1:
                    return Color.cyan;
                case 2:
                    return Color.red;
                case 3:
                    return new Color(0.56f, 0.19f, 0.77f);
                case 4:
                    return Color.yellow;
                case 5:
                    return Color.green;
                case 6:
                    return Color.magenta;
                case 7:
                    return new Color(1f, 0.5f, 0f);
                case 8:
                    return new Color(0.18f, 0.18f, 0.18f);
                case 9:
                    return Color.blue;
                case 10:
                    return Color.red;
                case 11:
                    return Color.blue;
                case 12:
                    return Color.white;
                case 13:
                    return Color.black;
                default:
                    return Color.clear;
            }
        }

        //public static int getSuicideRange(this Char ch)
        //{
        //    int result = 880;
        //    if (ch.me && ch.cgender == 2)
        //    {
        //        Skill skill5 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]);
        //        if (skill5 == null)
        //            return 0;
        //        result = 340 * (skill5.point - 1) / 3 + 200;
        //    }
        //    return result;
        //}

        internal static bool CanUse(this Skill skill)
        {
            var now = mSystem.currentTimeMillis();
            var isOutOfCooldown = now - skill.lastTimeUseThisSkill > skill.coolDown;
            return isOutOfCooldown && HasManaToUseSkill(skill);
        }

        internal static bool HasManaToUseSkill(this Skill skill)
        {
            switch (skill.template.manaUseType)
            {
                case 1:
                    return Char.myCharz().cMP >= Char.myCharz().cMPFull * (skill.manaUse / 100);
                default:
                    return false;
            }
        }

        internal static bool IsCharDead(this Char ch) => ch.isDie || ch.cHP <= 0 || ch.statusMe == 14;

        internal static int GetPetId(this Char ch) => -ch.charID;

        internal static ItemOption GetBestItemOption(this Item item)
        {
            ItemOption itemOption = null;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                ItemOption iOption = item.itemOption[i];
                if (iOption.optionTemplate == null)
                    continue;
                if (iOption.optionTemplate.id >= 127 && iOption.optionTemplate.id <= 135)   //Set kích hoạt
                {
                    itemOption = iOption;
                    break;
                }
                else if (iOption.optionTemplate.id <= 36 && iOption.optionTemplate.id >= 34)    //tinh ấn/nguyệt ấn/nhật ấn
                    itemOption = iOption;
                else if (itemOption == null || itemOption.optionTemplate == null || itemOption.optionTemplate.id > 36 || itemOption.optionTemplate.id < 34)
                {
                    if (iOption.optionTemplate.id == 72)   //cấp #
                        itemOption = iOption;
                    else if ((itemOption == null || itemOption.optionTemplate == null || (itemOption != null && itemOption.optionTemplate != null && itemOption.optionTemplate.id == 72 && itemOption.param < (int)Math.Ceiling((double)iOption.param / 2))) && iOption.optionTemplate.id == 107) //đồ sao
                        itemOption = iOption;
                }
            }
            return itemOption;
        }

        internal static string GetName(this ScaleMode scaleMode)
        {
            switch (scaleMode)
            {
                case ScaleMode.StretchToFill:
                    return Strings.scaleModeStretchToFill;
                case ScaleMode.ScaleAndCrop:
                    return Strings.scaleModeScaleAndCrop;
                case ScaleMode.ScaleToFit:
                    return Strings.scaleModeScaleToFit;
                default:
                    return "Unknown";
            }
        }

        internal static bool IsNRD(this ItemMap item) => item.template.id >= 372 && item.template.id <= 378;

#if UNITY_EDITOR
        internal static bool IsLdcI4(this Instruction instruction) => instruction.OpCode.Code - 21 <= Code.Stloc_1;

        internal static bool IsLdloc(this Instruction instruction) => instruction.OpCode == OpCodes.Ldloc || instruction.OpCode == OpCodes.Ldloc_0 || instruction.OpCode == OpCodes.Ldloc_1 || instruction.OpCode == OpCodes.Ldloc_2 || instruction.OpCode == OpCodes.Ldloc_3 || instruction.OpCode == OpCodes.Ldloc_S;

        internal static int GetLdcI4Value(this Instruction instruction)
        {
            return instruction.OpCode.Code switch
            {
                Code.Ldc_I4_M1 => -1,
                Code.Ldc_I4_0 => 0,
                Code.Ldc_I4_1 => 1,
                Code.Ldc_I4_2 => 2,
                Code.Ldc_I4_3 => 3,
                Code.Ldc_I4_4 => 4,
                Code.Ldc_I4_5 => 5,
                Code.Ldc_I4_6 => 6,
                Code.Ldc_I4_7 => 7,
                Code.Ldc_I4_8 => 8,
                Code.Ldc_I4_S => (sbyte)instruction.Operand,
                Code.Ldc_I4 => (int)instruction.Operand,
                _ => throw new InvalidOperationException($"Not a ldc.i4 instruction: {instruction}"),
            };
        }

        internal static int GetLdlocIndex(this Instruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Ldloc_0:
                    return 0;
                case Code.Ldloc_1:
                    return 1;
                case Code.Ldloc_2:
                    return 2;
                case Code.Ldloc_3:
                    return 3;
                case Code.Ldloc_S:
                case Code.Ldloc:
                    return ((VariableReference)instruction.Operand).Index;
                default:
                    throw new InvalidOperationException($"Not a ldloc instruction: {instruction}");
            }
        }
#endif
    }
}
