using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Mod.ModHelper
{
    public class SocketClient : ThreadAction
    {
        #region singleton
        public static SocketClient gI { get; } = new();

        static SocketClient() { }

        private SocketClient() { }
        #endregion

        private const string pathLogSocket = "ModData\\log_socket_client.txt";
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

                byte[] bytes = new byte[1024];
                JsonData msg;

                int bytesRec = sender.Receive(bytes);
                string receive = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                msg = JsonMapper.ToObject(receive);

                Utilities.username = (string)msg["username"];
                Utilities.password = (string)msg["password"];
                Utilities.server = msg["server"];
                Utilities.sizeData = msg["sizeData"];

                isConnected = true;

                performAction();
            }
            catch (Exception ex)
            {
                writeLog(ex.ToString());
                return;
            }
        }

        private void onMessage(JsonData msg)
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
                    GameMidlet.gameCanvas.keyPressedz((int)msg["keyCode"], isFromSync: true);
                    break;
                case "keyRelease":
                    GameMidlet.gameCanvas.keyReleasedz((int)msg["keyCode"], isFromAsync: true);
                    break;
                case "syncKeyPressed":
                    if (Utilities.channelSyncKey == (int)msg["channelSyncKey"])
                    {
                        GameMidlet.gameCanvas.keyPressedz((int)msg["keyCode"], isFromSync: true);
                    }
                    break;
                case "syncKeyReleased":
                    if (Utilities.channelSyncKey == (int)msg["channelSyncKey"])
                    {
                        GameMidlet.gameCanvas.keyReleasedz((int)msg["keyCode"], isFromAsync: true);
                    }
                    break;
                default:
                    writeLog($">> Lost action {action} \n");
                    break;
            }
        }

        public void setStatus(string status)
        {
            this.sendMessage(new
            {
                action = "setStatus",
                status
            });
        }

        public void sendMessage(object obj)
        {
            var json = JsonMapper.ToJson(obj);
            byte[] msg = Encoding.ASCII.GetBytes(json);
            _ = this.sender.Send(msg);
        }

        protected override void action()
        {
            if (!isConnected)
                return;

            byte[] bytes = new byte[1024];

            while (true)
            {
                JsonData msg;
                try
                {
                    int bytesRec = sender.Receive(bytes);
                    string receive = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    msg = JsonMapper.ToObject(receive);
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

                MainThreadDispatcher.dispatcher(() => onMessage(msg));
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
