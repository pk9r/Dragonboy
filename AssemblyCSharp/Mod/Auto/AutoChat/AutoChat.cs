using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using System.IO;
namespace Mod.Auto.AutoChat
{
    public class AutoChat : ThreadActionUpdate<AutoChat>
    {
        //Có thể viết tự điều chỉnh time auto mà lười quá
        public override int Interval => 5000;// 5000ms thì chat 1 lần
        protected override void update()
        {
            string filePath = Utilities.PathAutoChat;
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Đọc toàn bộ nội dung tệp tin và bắt đầu chat
                string content = reader.ReadToEnd();
                if (string.IsNullOrEmpty(content))
                {
                    content = "Gà Vãi Lòn";
                }
                Service.gI().chat("ga" + Res.random(10, 100) + ": " + content);
            }            
        }
        [ChatCommand("openachat")]
        public static void showMenu()
        {
            new MenuBuilder()
                .addItem("Auto: ", new(() =>
                {
                    gI.toggle();
                    if (!gI.IsActing)
                        GameScr.info1.addInfo("Tắt tự động chat ", 0);
                }))
                .addItem("Nhập nội dung", new(() =>
                {
                    ChatTextField.gI().strChat = Setup.inputTextAutoChat[0];
                    ChatTextField.gI().tfChat.name = Setup.inputTextAutoChat[1];
                    ChatTextField.gI().startChat2(Setup.gI, string.Empty);
                }))
                .addItem("Kiểm tra nội dung", new(() =>
                {
                    using (StreamReader reader = new StreamReader(Utilities.PathAutoChat))
                    {
                        // Đọc toàn bộ nội dung tệp tin và bắt đầu chat
                        string content = reader.ReadToEnd();
                        GameCanvas.startOKDlg("Nội dung tự động chat:\n" + content);
                    }
                })).start();
        }
    }
}
