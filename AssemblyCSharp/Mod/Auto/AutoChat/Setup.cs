using System.IO;

namespace Mod.Auto.AutoChat
{
    public class Setup : IChatable
    {
        public static string[] inputTextAutoChat = new string[] { "Nhập nội dung muốn autochat", "" };
        public static Setup gI { get; } = new Setup();
        public void onCancelChat()
        {
            //ChatTextField.gI().isShow = false;
            //ChatTextField.gI().ResetTF();
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) || string.IsNullOrEmpty(text))
            {
                return;
            }
            if (ChatTextField.gI().strChat.Contains(inputTextAutoChat[0]))
            {
                try
                {
                    string textChat = ChatTextField.gI().tfChat.getText();
                    using (StreamWriter write = new StreamWriter(Utilities.PathAutoChat))
                    {
                        write.WriteLine(textChat);
                        GameCanvas.startOKDlg("Nội dung được thay đổi thành:\n" + textChat);

                        write.Close();
                    }    
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xảy ra");
                }
            }
            ChatTextField.gI().ResetTF();
        }
    }
}
