using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Session_ME2 : ISession
{
	public class Sender
	{
		public List<Message> sendingMessage;

		public Sender()
		{
			sendingMessage = new List<Message>();
		}

		public void AddMessage(Message message)
		{
			sendingMessage.Add(message);
		}

		public void run()
		{
			while (connected)
			{
				try
				{
					if (getKeyComplete)
					{
						while (sendingMessage.Count > 0)
						{
							doSendMessage(sendingMessage[0]);
							sendingMessage.RemoveAt(0);
						}
					}
					try
					{
						Thread.Sleep(5);
					}
					catch (Exception ex)
					{
						Cout.LogError(ex.ToString());
					}
				}
				catch (Exception)
				{
					Res.outz("error send message! ");
				}
			}
		}
	}

	private class MessageCollector
	{
		public void run()
		{
			try
			{
				while (connected)
				{
					Message message = readMessage();
					if (message == null)
						break;
					try
					{
						if (message.command == -27)
							getKey(message);
						else
							onRecieveMsg(message);
					}
					catch (Exception)
					{
						Cout.println("LOI NHAN  MESS THU 1");
					}
					try
					{
						Thread.Sleep(5);
					}
					catch (Exception)
					{
						Cout.println("LOI NHAN  MESS THU 2");
					}
				}
			}
			catch (Exception ex3)
			{
				Debug.Log("error read message!");
				Debug.Log(ex3.Message.ToString());
			}
			if (!connected)
				return;
			if (messageHandler != null)
			{
				if (currentTimeMillis() - timeConnected > 500)
					messageHandler.onDisconnected(isMainSession);
				else
					messageHandler.onConnectionFail(isMainSession);
			}
			if (sc != null)
				cleanNetwork();
		}

		private void getKey(Message message)
		{
			try
			{
				sbyte b = message.reader().readSByte();
				key = new sbyte[b];
				for (int i = 0; i < b; i++)
				{
					key[i] = message.reader().readSByte();
				}
				for (int j = 0; j < key.Length - 1; j++)
				{
					ref sbyte reference = ref key[j + 1];
					reference = (sbyte)(reference ^ key[j]);
				}
				getKeyComplete = true;
				GameMidlet.IP2 = message.reader().readUTF();
				GameMidlet.PORT2 = message.reader().readInt();
				GameMidlet.isConnect2 = ((message.reader().readByte() != 0) ? true : false);
				if (isMainSession && GameMidlet.isConnect2)
					GameCanvas.connect2();
			}
			catch (Exception)
			{
			}
		}

		private Message readMessage2(sbyte cmd)
		{
			int num = readKey(dis.ReadSByte()) + 128;
			sbyte num2 = readKey(dis.ReadSByte());
			int num3 = ((readKey(dis.ReadSByte()) + 128) * 256 + (num2 + 128)) * 256 + num;
			Cout.LogError("SIZE = " + num3);
			sbyte[] array = new sbyte[num3];
			int num4 = 0;
			Buffer.BlockCopy(dis.ReadBytes(num3), 0, array, 0, num3);
			recvByteCount += 5 + num3;
			int num5 = recvByteCount + sendByteCount;
			strRecvByteCount = num5 / 1024 + "." + num5 % 1024 / 102 + "Kb";
			if (getKeyComplete)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = readKey(array[i]);
				}
			}
			return new Message(cmd, array);
		}

		private Message readMessage()
		{
			try
			{
				sbyte b = dis.ReadSByte();
				if (getKeyComplete)
					b = readKey(b);
				if (b == -32 || b == -66 || b == 11 || b == -67 || b == -74 || b == -87)
					return readMessage2(b);
				int num;
				if (getKeyComplete)
				{
					sbyte b2 = dis.ReadSByte();
					sbyte b3 = dis.ReadSByte();
					num = ((readKey(b2) & 0xFF) << 8) | (readKey(b3) & 0xFF);
				}
				else
					num = (dis.ReadSByte() & 0xFF00) | (dis.ReadSByte() & 0xFF);
				sbyte[] array = new sbyte[num];
				int num2 = 0;
				int num3 = 0;
				Buffer.BlockCopy(dis.ReadBytes(num), 0, array, 0, num);
				recvByteCount += 5 + num;
				int num4 = recvByteCount + sendByteCount;
				strRecvByteCount = num4 / 1024 + "." + num4 % 1024 / 102 + "Kb";
				if (getKeyComplete)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = readKey(array[i]);
					}
				}
				return new Message(b, array);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.StackTrace.ToString());
			}
			return null;
		}
	}

	protected static Session_ME2 instance = new Session_ME2();

	private static NetworkStream dataStream;

	private static BinaryReader dis;

	private static BinaryWriter dos;

	public static IMessageHandler messageHandler;

	public static bool isMainSession = true;

	private static TcpClient sc;

	public static bool connected;

	public static bool connecting;

	private static Sender sender = new Sender();

	public static Thread initThread;

	public static Thread collectorThread;

	public static Thread sendThread;

	public static int sendByteCount;

	public static int recvByteCount;

	private static bool getKeyComplete;

	public static sbyte[] key = null;

	private static sbyte curR;

	private static sbyte curW;

	private static int timeConnected;

	private long lastTimeConn;

	public static string strRecvByteCount = string.Empty;

	public static bool isCancel;

	private string host;

	private int port;

	public static MyVector recieveMsg = new MyVector();

	public Session_ME2()
	{
		Debug.Log("init Session_ME");
	}

	public void clearSendingMessage()
	{
		sender.sendingMessage.Clear();
	}

	public static Session_ME2 gI()
	{
		if (instance == null)
			instance = new Session_ME2();
		return instance;
	}

	public bool isConnected()
	{
		return connected;
	}

	public void setHandler(IMessageHandler msgHandler)
	{
		messageHandler = msgHandler;
	}

	public void connect(string host, int port)
	{
		if (!connected && !connecting)
		{
			this.host = host;
			this.port = port;
			getKeyComplete = false;
			sc = null;
			Debug.Log("connecting...!");
			Debug.Log("host: " + host);
			Debug.Log("port: " + port);
			initThread = new Thread(NetworkInit) { Name = "Session_ME2::NetworkInit" };
			initThread.Start();
		}
	}

	private void NetworkInit()
	{
		isCancel = false;
		connecting = true;
		Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
		connected = true;
		try
		{
			doConnect(host, port);
			messageHandler.onConnectOK(isMainSession);
		}
		catch (Exception)
		{
			if (messageHandler != null)
			{
				close();
				messageHandler.onConnectionFail(isMainSession);
			}
		}
	}

	public void doConnect(string host, int port)
	{
		sc = new TcpClient();
		sc.Connect(host, port);
		dataStream = sc.GetStream();
		dis = new BinaryReader(dataStream, new UTF8Encoding());
		dos = new BinaryWriter(dataStream, new UTF8Encoding());
		new Thread(sender.run) { Name = "Session_ME2.Sender::run" }.Start();
		MessageCollector @object = new MessageCollector();
		Cout.LogError("new -----");
		collectorThread = new Thread(@object.run) { Name = "Session_ME2.MessageCollector::run" };
		collectorThread.Start();
		timeConnected = currentTimeMillis();
		connecting = false;
		doSendMessage(new Message(-27));
	}

	public void sendMessage(Message message)
	{
		Res.outz("SEND MSG: " + message.command);
		sender.AddMessage(message);
	}

	private static void doSendMessage(Message m)
	{
		sbyte[] data = m.getData();
		try
		{
			if (getKeyComplete)
			{
				sbyte value = writeKey(m.command);
				dos.Write(value);
			}
			else
				dos.Write(m.command);
			if (data != null)
			{
				int num = data.Length;
				if (getKeyComplete)
				{
					sbyte num2 = writeKey((sbyte)(num >> 8));
					dos.Write(num2);
					sbyte num3 = writeKey((sbyte)(num & 0xFF));
					dos.Write(num3);
				}
				else
					dos.Write((ushort)num);
				if (getKeyComplete)
				{
					for (int i = 0; i < data.Length; i++)
					{
						sbyte value2 = writeKey(data[i]);
						dos.Write(value2);
					}
				}
				sendByteCount += 5 + data.Length;
			}
			else
			{
				if (getKeyComplete)
				{
					int num4 = 0;
					sbyte num5 = writeKey((sbyte)(num4 >> 8));
					dos.Write(num5);
					sbyte num6 = writeKey((sbyte)(num4 & 0xFF));
					dos.Write(num6);
				}
				else
					dos.Write((ushort)0);
				sendByteCount += 5;
			}
			dos.Flush();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	public static sbyte readKey(sbyte b)
	{
		sbyte[] array = key;
		sbyte num = curR;
		curR = (sbyte)(num + 1);
		sbyte result = (sbyte)((array[num] & 0xFF) ^ (b & 0xFF));
		if (curR >= key.Length)
			curR = (sbyte)(curR % (sbyte)key.Length);
		return result;
	}

	public static sbyte writeKey(sbyte b)
	{
		sbyte[] array = key;
		sbyte num = curW;
		curW = (sbyte)(num + 1);
		sbyte result = (sbyte)((array[num] & 0xFF) ^ (b & 0xFF));
		if (curW >= key.Length)
			curW = (sbyte)(curW % (sbyte)key.Length);
		return result;
	}

	public static void onRecieveMsg(Message msg)
	{
		if (Thread.CurrentThread.Name == Main.mainThreadName)
			messageHandler.onMessage(msg);
		else
			recieveMsg.addElement(msg);
	}

	public static void update()
	{
		while (recieveMsg.size() > 0)
		{
			Message message = (Message)recieveMsg.elementAt(0);
			if (Controller.isStopReadMessage)
				break;
			if (message == null)
			{
				recieveMsg.removeElementAt(0);
				break;
			}
			messageHandler.onMessage(message);
			recieveMsg.removeElementAt(0);
		}
	}

	public void close()
	{
		cleanNetwork();
	}

	private static void cleanNetwork()
	{
		key = null;
		curR = 0;
		curW = 0;
		try
		{
			connected = false;
			connecting = false;
			if (sc != null)
			{
				sc.Close();
				sc = null;
			}
			if (dataStream != null)
			{
				dataStream.Close();
				dataStream = null;
			}
			if (dos != null)
			{
				dos.Close();
				dos = null;
			}
			if (dis != null)
			{
				dis.Close();
				dis = null;
			}
			sendThread = null;
			collectorThread = null;
		}
		catch (Exception)
		{
		}
	}

	public static int currentTimeMillis()
	{
		return Environment.TickCount;
	}

	public static byte convertSbyteToByte(sbyte var)
	{
		if (var > 0)
			return (byte)var;
		return (byte)(var + 256);
	}

	public static byte[] convertSbyteToByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
				array[i] = (byte)var[i];
			else
				array[i] = (byte)(var[i] + 256);
		}
		return array;
	}
}
