using System.IO;
using System.Text.RegularExpressions;

namespace Mod.Auto.AutoChat
{
    public class Setup : IChatable
    {
        public static string[] inputTextAutoChat = new string[] { "Nhập nội dung muốn autochat", "" };

        public static string[] inputDelayAutoChat = new string[] { "Nhập thời gian delay", "Thời gian >5000(5 giây)" };

        public static int delayAutoChat = 5000;//default 5000ms = 5s
        public static Setup gI { get; } = new Setup();
        public static void loadFile()
        {
            // Kiểm tra nếu tệp tin không tồn tại thì tạo mới và ghi nội dung vào
            if (!File.Exists(Utilities.PathAutoChat))
            {
                using (StreamWriter sw = File.CreateText(Utilities.PathAutoChat))
                {
                    sw.WriteLine("Mod Cộng Đồng");
                    sw.WriteLine("6500");
                }
            }
            // Cập nhật thời gian delay
            Setup.delayAutoChat = int.Parse(File.ReadAllLines(Utilities.PathAutoChat)[1]);
        }
        /// <summary>
        /// Kích hoạt khi người chơi tắt chức năng hoặc tắt game sẽ xóa các dòng auto chat 
        /// </summary>
        // <param name="pattern">Sử dụng biểu thức chính quy tìm các dòng autochat trong history.</param>
        public static void clearStringTrash()
        {
            if (!File.Exists(Utilities.PathChatHistory))
                return;
            // Đọc nội dung file text
            string content = File.ReadAllText(Utilities.PathChatHistory);

            string pattern = @",?\s*\mcd\d{2}\:\s*[^""]*""[^""]*""";

            //Regex.Replace() thay thế các chuỗi tìm được bằng chuỗi rỗng, loại bỏ chúng khỏi chuỗi đầu vào
            string output = Regex.Replace(content, pattern, "");

            // Ghi nội dung vào file output
            File.WriteAllText(Utilities.PathChatHistory, output);
        }
        public void onCancelChat()
        {
            ChatTextField.gI().isShow = false;
            ChatTextField.gI().ResetTF();
        }

        public void onChatFromMe(string text, string to)
        {
            string[] lines = File.ReadAllLines(Utilities.PathAutoChat);

            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) || string.IsNullOrEmpty(text))
            {
                return;
            }
            if (ChatTextField.gI().strChat.Contains(inputTextAutoChat[0]))
            {
                try
                {
                    string newLine = ChatTextField.gI().tfChat.getText();

                    lines[0] = newLine; // chỉnh sửa dòng đầu tiên
                    File.WriteAllLines(Utilities.PathAutoChat, lines);

                    GameCanvas.startOKDlg("Đã lưu nội dung: " + newLine);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xảy ra");
                }
            }
            else if (ChatTextField.gI().strChat.Contains(inputDelayAutoChat[0]))
            {
                try
                {
                    string newContent = ChatTextField.gI().tfChat.getText(); ;
                    lines[1] = newContent; // chỉnh sửa dòng thứ 2

                    File.WriteAllLines(Utilities.PathAutoChat, lines);
                    delayAutoChat = int.Parse(newContent);
                    if (delayAutoChat < 5000)
                        delayAutoChat = 5000;

                    //Dù Interval là Int thì làm tròn hết nhưng mà cứ in ra cho nó chuyên nghiệp
                    GameScr.info1.addInfo("Đã đổi delay thành " + ((float)delayAutoChat / 1000) + " giây", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Nhập sai dữ liệu");
                }
            }
            ChatTextField.gI().ResetTF();
        }
    }
}
