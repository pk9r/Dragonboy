using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using Mod.ModHelper;
using System.IO;
using Mod.R;

namespace Mod.Auto.AutoChat
{
    internal class AutoChat : ThreadActionUpdate<AutoChat>
    {
        internal override int Interval => /*Res.random(5000, 10000)*/ Setup.delayAutoChat;

        protected override void update()
        {
            string filePath = Utils.PathAutoChat;
            string content = File.ReadAllLines(filePath)[0];
            Service.gI().chat("mcd" + Res.random(10, 100) + ": " + content);
        }

        [ChatCommand("openachat")]
        internal static void showMenu()
        {
            new MenuBuilder()
                .addItem("Auto chat: \n" + (gI.IsActing ? mResources.ON : mResources.OFF), new MenuAction(() =>
                {
                    gI.toggle();
                    if (!gI.IsActing)
                    {
                        Setup.clearStringTrash();
                        GameScr.info1.addInfo(Strings.autoChatDisabled+ '!', 0);
                    }
                }))
                .addItem(Strings.inputContent, new MenuAction(() =>
                {
                    ChatTextField.gI().strChat = Setup.inputTextAutoChat[0];
                    ChatTextField.gI().tfChat.name = Setup.inputTextAutoChat[1];
                    ChatTextField.gI().startChat2(Setup.gI, string.Empty);
                }))
                .addItem(string.Format(Strings.delaySeconds, (float)Setup.delayAutoChat / 1000), new MenuAction(() =>
                {
                    ChatTextField.gI().strChat = Setup.inputDelayAutoChat[0];
                    ChatTextField.gI().tfChat.name = Setup.inputDelayAutoChat[1];
                    ChatTextField.gI().startChat2(Setup.gI, string.Empty);
                }))
                .addItem(Strings.viewContent, new MenuAction(() =>
                {
                    using StreamReader reader = new StreamReader(Utils.PathAutoChat);
                    // Đọc toàn bộ nội dung tệp tin và in ra
                    string content = reader.ReadToEnd();
                    GameCanvas.startOKDlg(Strings.autoChatContent + ":\n" + content);
                })).start();
        }
    }
}
