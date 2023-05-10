using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using Mod.ModHelper;
using System.IO;

namespace Mod.Auto.AutoChat
{
    public class AutoChat : ThreadActionUpdate<AutoChat>
    {
        public override int Interval => /*Res.random(5000, 10000)*/ Setup.delayAutoChat;
        protected override void update()
        {
            string filePath = Utilities.PathAutoChat;
            string content = File.ReadAllLines(filePath)[0];


            Service.gI().chat("mcd" + Res.random(10, 100) + ": " + content);


        }
        [ChatCommand("openachat")]
        public static void showMenu()
        {
            new MenuBuilder()
                .addItem("Auto", new(() =>
                {
                    gI.toggle();
                    if (!gI.IsActing)
                    {
                        Setup.clearStringTrash();
                        GameScr.info1.addInfo("Tắt tự động chat ", 0);
                    }

                }))
                .addItem("Nhập nội dung", new(() =>
                {
                    ChatTextField.gI().strChat = Setup.inputTextAutoChat[0];
                    ChatTextField.gI().tfChat.name = Setup.inputTextAutoChat[1];
                    ChatTextField.gI().startChat2(Setup.gI, string.Empty);
                }))
                .addItem("Delay:\n" + (float)(Setup.delayAutoChat) / 1000 + " giây", new(() =>
                {
                    ChatTextField.gI().strChat = Setup.inputDelayAutoChat[0];
                    ChatTextField.gI().tfChat.name = Setup.inputDelayAutoChat[1];
                    ChatTextField.gI().startChat2(Setup.gI, string.Empty);
                }))
                .addItem("Kiểm tra nội dung", new(() =>
                {
                    using (StreamReader reader = new StreamReader(Utilities.PathAutoChat))
                    {
                        // Đọc toàn bộ nội dung tệp tin và in ra
                        string content = reader.ReadToEnd();
                        GameCanvas.startOKDlg("Nội dung tự động chat:\n" + content);
                    }
                })).start();
        }
    }
}
