using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mod.ModHelper.Menu;
using Mod.R;
using Vietpad.InputMethod;

namespace Mod
{
    internal class VietnameseInput
    {
        class VietnameseInputChatable : IChatable
        {
            public void onChatFromMe(string text, string to)
            {
                GameScr.info1.addInfo(text, 0);
                onCancelChat();
            }

            public void onCancelChat() => ChatTextField.gI().ResetTF();
        }

        static VietnameseInput()
        {
            VietKeyHandler.VietModeEnabled = false;
            VietKeyHandler.SmartMark = true;
        }

        internal static void ShowMenu()
        {
            MenuBuilder menuBuilder = new MenuBuilder()
               .setChatPopup(Strings.vnInputEnable + ": " + Strings.OnOffStatus(VietKeyHandler.VietModeEnabled) + Environment.NewLine + Strings.vnInputInputMethod + ": " + Enum.GetName(typeof(InputMethods), VietKeyHandler.InputMethod) + Environment.NewLine + Strings.vnInputDiacritics + ": " + (VietKeyHandler.DiacriticsPosClassic ? "òa, úy" : "oà, uý") + Environment.NewLine + Strings.vnInputConsumeRepeatKey + ": " + Strings.OnOffStatus(VietKeyHandler.ConsumeRepeatKey))
               .addItem(Strings.vnInputEnable + ": " + Strings.OnOffStatus(VietKeyHandler.VietModeEnabled), new MenuAction(() =>
               {
                   VietKeyHandler.VietModeEnabled = !VietKeyHandler.VietModeEnabled;
                   GameScr.info1.addInfo(Strings.vnInputEnable + ": " + Strings.OnOffStatus(VietKeyHandler.VietModeEnabled), 0);
               }))
               .addItem(Strings.vnInputInputMethod + ": " + Enum.GetName(typeof(InputMethods), VietKeyHandler.InputMethod), new MenuAction(() =>
               {
                   VietKeyHandler.InputMethod++;
                   if (VietKeyHandler.InputMethod > InputMethods.Auto)
                          VietKeyHandler.InputMethod = InputMethods.Telex;
                   GameScr.info1.addInfo(Strings.vnInputInputMethod + ": " + Enum.GetName(typeof(InputMethods), VietKeyHandler.InputMethod), 0);
               }))
               .addItem(Strings.vnInputDiacritics + ": " + (VietKeyHandler.DiacriticsPosClassic ? "òa, úy" : "oà, uý"), new MenuAction(() =>
               {
                   VietKeyHandler.DiacriticsPosClassic = !VietKeyHandler.DiacriticsPosClassic;
                     GameScr.info1.addInfo(Strings.vnInputDiacritics + ": " + (VietKeyHandler.DiacriticsPosClassic ? "òa, úy" : "oà, uý"), 0);
               }))
               .addItem(Strings.vnInputConsumeRepeatKey + ": " + Strings.OnOffStatus(VietKeyHandler.ConsumeRepeatKey), new MenuAction(() =>
               {
                   VietKeyHandler.ConsumeRepeatKey = !VietKeyHandler.ConsumeRepeatKey;
                     GameScr.info1.addInfo(Strings.vnInputConsumeRepeatKey + ": " + Strings.OnOffStatus(VietKeyHandler.ConsumeRepeatKey), 0);
               }))
               .addItem("Test", new MenuAction(() =>
               {
                   ChatTextField.gI().strChat = "Test";
                   ChatTextField.gI().tfChat.name = "Test";
                   ChatTextField.gI().startChat2(new VietnameseInputChatable(), string.Empty);
               }));
            menuBuilder.start();
        }

        internal static void LoadData()
        {
            if (Utils.TryLoadDataBool("vn_input_enabled", out bool value))
                VietKeyHandler.VietModeEnabled = value;
            if (Utils.TryLoadDataInt("vn_input_input_method", out int value2))
                VietKeyHandler.InputMethod = (InputMethods)value2;
            if (Utils.TryLoadDataBool("vn_input_diacritics", out bool value3))
                VietKeyHandler.DiacriticsPosClassic = value3;
            if (Utils.TryLoadDataBool("vn_input_consume_repeat_key", out bool value4))
                VietKeyHandler.ConsumeRepeatKey = value4;
        }

        internal static void SaveData()
        {
            Utils.SaveData("vn_input_enabled", VietKeyHandler.VietModeEnabled);
            Utils.SaveData("vn_input_input_method", (int)VietKeyHandler.InputMethod);
            Utils.SaveData("vn_input_diacritics", VietKeyHandler.DiacriticsPosClassic);
            Utils.SaveData("vn_input_consume_repeat_key", VietKeyHandler.ConsumeRepeatKey);
        }

        internal static bool ToVietnamese(string str, out string result, ref int caretPos, int inputType)
        {
            result = "";
            if (!VietKeyHandler.VietModeEnabled)
                return false;
            if (inputType != TField.INPUT_TYPE_ANY || str.StartsWith("/"))
                return false;
            result = VietKeyHandler.HandleTextInput(str, caretPos - 1);
            if (result != str)
            {
                caretPos -= str.Length - result.Length;
                return true;
            }
            return false;
        }

    }
}
