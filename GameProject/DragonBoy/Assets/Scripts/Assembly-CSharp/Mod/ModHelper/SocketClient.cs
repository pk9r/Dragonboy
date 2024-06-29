using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mod.ModHelper
{
    public class SocketClient : ThreadAction<SocketClient>
    {
        private static readonly string pathLogSocket = Path.Combine(Utils.dataPath, "log_socket_client.txt");
        public int port = -1;
        public bool isConnected = false;

        private Socket sender;

        public void initSender()
        {
            loadPort();
            if (this.port == -1)
            {
                return;
            }

            try
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEP = new IPEndPoint(ipAddress, port);

                sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(remoteEP);

                this.sendMessage(new
                {
                    action = "connected",
                    id = Process.GetCurrentProcess().Id,
                });

                this.sendMessage(new
                {
                    action = "setStatus",
                    Utils.status
                });

                byte[] bytes = new byte[1024];
                JObject msg;

                int bytesRec = sender.Receive(bytes);
                string receive = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                msg = JObject.Parse(receive);

                Utils.username = (string)msg["username"];
                Utils.password = (string)msg["password"];
                Utils.server = (JObject)msg["server"];
                Utils.sizeData = (JObject)msg["sizeData"];

                isConnected = true;

                performAction();
            }
            catch (Exception ex)
            {
                writeLog(ex.ToString());
                return;
            }
        }

        private void onMessage(JObject msg)
        {
            string action = (string)msg["action"];
            switch (action)
            {
                case "test":
                    GameScr.info1.addInfo((string)msg["text"], 0);
                    break;
                case "chat":
                    Service.gI().chat((string)msg["text"]);
                    break;
                case "keyPress":
                    GameMidlet.gameCanvas.keyPressedz((int)msg["keyCode"]);
                    GameEvents.OnKeyPressed((int)msg["keyCode"], true);
                    break;
                case "keyRelease":
                    GameMidlet.gameCanvas.keyReleasedz((int)msg["keyCode"]);
                    GameEvents.OnKeyReleased((int)msg["keyCode"], true);
                    break;
                case "syncKeyPressed":
                    if (Utils.channelSyncKey == (int)msg["channelSyncKey"])
                    {
                        GameMidlet.gameCanvas.keyPressedz((int)msg["keyCode"]);
                        GameEvents.OnKeyPressed((int)msg["keyCode"], true);
                    }
                    break;
                case "syncKeyReleased":
                    if (Utils.channelSyncKey == (int)msg["channelSyncKey"])
                    {
                        GameMidlet.gameCanvas.keyReleasedz((int)msg["keyCode"]);
                        GameEvents.OnKeyReleased((int)msg["keyCode"], true);
                    }
                    break;
                default:
                    writeLog($">> Lost action {action} \n");
                    break;
            }
        }

        public void sendMessage(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            byte[] msg = Encoding.ASCII.GetBytes(json);
            try
            {
                _ = this.sender.Send(msg);
            }
            catch (ObjectDisposedException) { }
        }

        protected override void action()
        {
            if (!isConnected)
                return;

            byte[] bytes = new byte[1024];

            while (true)
            {
                JObject msg;
                try
                {
                    int bytesRec = sender.Receive(bytes);
                    string receive = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    msg = JObject.Parse(receive);
                }
                catch (SocketException)
                {
                    GameCanvas.startOKDlg("Mất kết nối với QLTK");
                    return;
                }
                catch (ObjectDisposedException)
                {
                    GameCanvas.startOKDlg("Mất kết nối với QLTK");
                    return;
                }
                catch (Exception e)
                {
                    writeLog(e.ToString());
                    continue;
                }

                MainThreadDispatcher.Dispatch(() => onMessage(msg));
                //try
                //{
                //    onMessage(msg);
                //}
                //catch (Exception ex)
                //{
                //    writeLog(ex.ToString());
                //}
            }
        }

        private void loadPort()
        {
            var args = Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, "-port") + 1;
            try
            {
                port = int.Parse(args[index]);
            }
            catch (Exception e)
            {
                writeLog(e.ToString());
            }
        }

        /// <summary>
        /// Đóng kết nối socket.
        /// </summary>
        public void close()
        {
            if (sender?.Connected == true)
            {
                sendMessage(new { action = "close-socket" });

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
        }

        /// <summary>
        /// Ghi log cho SocketClient.
        /// </summary>
        /// <param name="log"></param>
        private void writeLog(string log)
        {
            try
            {
                File.AppendAllText(pathLogSocket, log + "\n");
            }
            catch { }
        }
    }
}
