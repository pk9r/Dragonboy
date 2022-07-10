using Mod;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

public class Rms
{
	public static int status;

	public static sbyte[] data;

	public static string filename;

	private const int INTERVAL = 5;

	private const int MAXTIME = 500;

	public static void saveRMS(string filename, sbyte[] data)
	{
		if (Thread.CurrentThread.Name == Main.mainThreadName)
		{
			__saveRMS(filename, data);
		}
		else
		{
			_saveRMS(filename, data);
		}
	}

	public static sbyte[] loadRMS(string filename)
	{
		if (Thread.CurrentThread.Name == Main.mainThreadName)
		{
			return __loadRMS(filename);
		}
		return _loadRMS(filename);
	}

	public static string loadRMSString(string fileName)
	{
		sbyte[] array = loadRMS(fileName);
		if (array == null)
		{
			return null;
		}
        if (fileName == "acc" || fileName == "pass")
        {
            try
            {

                string text = GetiPhoneDocumentsPath() + "\\" + fileName;
                return DecryptString(File.ReadAllBytes(text));
            }
            catch (Exception)
            {
                return null;
            }
        }
        DataInputStream dataInputStream = new DataInputStream(array);
		try
		{
			string result = dataInputStream.readUTF();
			dataInputStream.close();
			return result;
		}
		catch (Exception ex)
		{
			Cout.println(ex.StackTrace);
		}
		return null;
	}

	public static byte[] convertSbyteToByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
			{
				array[i] = (byte)var[i];
			}
			else
			{
				array[i] = (byte)(var[i] + 256);
			}
		}
		return array;
	}

	public static void saveRMSString(string filename, string data)
	{
        if (filename == "acc" || filename == "pass")
        {
            string text = GetiPhoneDocumentsPath() + "\\" + filename;
            FileStream fileStream = new FileStream(text, FileMode.Create);
            byte[] bytes = EncryptString(data);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
            return;
        }
        DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeUTF(data);
			saveRMS(filename, dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception ex)
		{
			Cout.println(ex.StackTrace);
		}
	}

	private static void _saveRMS(string filename, sbyte[] data)
	{
		if (status != 0)
		{
			Debug.LogError("Cannot save RMS " + filename + " because current is saving " + Rms.filename);
			return;
		}
		Rms.filename = filename;
		Rms.data = data;
		status = 2;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Debug.LogError("TOO LONG TO SAVE RMS " + filename);
		}
	}

	private static sbyte[] _loadRMS(string filename)
	{
		if (status != 0)
		{
			Debug.LogError("Cannot load RMS " + filename + " because current is loading " + Rms.filename);
			return null;
		}
		Rms.filename = filename;
		data = null;
		status = 3;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Debug.LogError("TOO LONG TO LOAD RMS " + filename);
		}
		return data;
	}

	public static void update()
	{
		if (status == 2)
		{
			status = 1;
			__saveRMS(filename, data);
			status = 0;
		}
		else if (status == 3)
		{
			status = 1;
			data = __loadRMS(filename);
			status = 0;
		}
	}

	public static int loadRMSInt(string file)
	{
        if (GameEvents.onLoadRMSInt(file, out int result))
            return result;

		sbyte[] array = loadRMS(file);
		return (array != null) ? array[0] : (-1);
	}

	public static void saveRMSInt(string file, int x)
	{
		try
		{
			saveRMS(file, new sbyte[1] { (sbyte)x });
		}
		catch (Exception)
		{
		}
	}

	public static string GetiPhoneDocumentsPath()
	{
		if (GameEvents.onGetRMSPath(out string result)) return result;
		return Application.persistentDataPath;
	}

	private static void __saveRMS(string filename, sbyte[] data)
	{
		string text = GetiPhoneDocumentsPath() + "/" + filename;
		FileStream fileStream = new FileStream(text, FileMode.Create);
		fileStream.Write(ArrayCast.cast(data), 0, data.Length);
		fileStream.Flush();
		fileStream.Close();
		Main.setBackupIcloud(text);
	}

	private static sbyte[] __loadRMS(string filename)
	{
		try
		{
			FileStream fileStream = new FileStream(GetiPhoneDocumentsPath() + "/" + filename, FileMode.Open);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			sbyte[] array2 = ArrayCast.cast(array);
			return ArrayCast.cast(array);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static void clearAllExceptImportantData()
	{
        FileInfo[] files = new DirectoryInfo(GetiPhoneDocumentsPath() + "/").GetFiles();
        foreach (FileInfo fileInfo in files) if (fileInfo.Name != "acc" && fileInfo.Name != "pass" && fileInfo.Name != "levelScreenKN" && fileInfo.Name != "lowGraphic" && fileInfo.Name != "isPlaySound" && fileInfo.Name != "NRlink2" && fileInfo.Name != "svselect") fileInfo.Delete();
    }

    public static void clearAll()
    {
        FileInfo[] files = new DirectoryInfo(GetiPhoneDocumentsPath() + "/").GetFiles();
        foreach (FileInfo fileInfo in files) fileInfo.Delete();
    }

    public static void DeleteStorage(string path)
	{
		try
		{
			File.Delete(GetiPhoneDocumentsPath() + "/" + path);
		}
		catch (Exception)
		{
		}
	}

	public static string ByteArrayToString(byte[] ba)
	{
		string text = BitConverter.ToString(ba);
		return text.Replace("-", string.Empty);
	}

	public static byte[] StringToByteArray(string hex)
	{
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}

	public static void deleteRecord(string name)
	{
		try
		{
			PlayerPrefs.DeleteKey(name);
		}
		catch (Exception ex)
		{
			Cout.println("loi xoa RMS --------------------------" + ex.ToString());
		}
	}

	public static void clearRMS()
	{
		deleteRecord("data");
		deleteRecord("dataVersion");
		deleteRecord("map");
		deleteRecord("mapVersion");
		deleteRecord("skill");
		deleteRecord("killVersion");
		deleteRecord("item");
		deleteRecord("itemVersion");
	}

	public static void saveIP(string strID)
	{
		saveRMSString("NRIPlink", strID);
	}

	public static string loadIP()
	{
		string text = loadRMSString("NRIPlink");
		if (text == null)
		{
			return null;
		}
		return text;
	}
    static byte[] EncryptString(string data)
    {
        TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
        {
            KeySize = 128,
            BlockSize = 64,
            Padding = PaddingMode.PKCS7,
            Mode = CipherMode.CBC,
            Key = Key,
            IV = IV
        };
        ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        return Encoding.UTF8.GetBytes(Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length)));
    }

    static string DecryptString(byte[] data)
    {
        TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
        {
            KeySize = 128,
            BlockSize = 64,
            Padding = PaddingMode.PKCS7,
            Mode = CipherMode.CBC,
            Key = Key,
            IV = IV
        };
        ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
        byte[] array = Convert.FromBase64String(Encoding.UTF8.GetString(data));
        return Encoding.UTF8.GetString(cryptoTransform.TransformFinalBlock(array, 0, array.Length));
    }

    static byte[] Key
    {
        get
        {
            return new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier + "lKVAZvyzsENttZaMJe71PqHxgtVocbcM"));
        }
    }

    static byte[] IV
    {
        get
        {
            byte[] bytes = new byte[8];
            byte[] bytes1 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier + "0s3ZhPjkN83LqSunVimI4yAaMHj1iIIM"));
            for (int i = 0; i < bytes1.Length; i++) bytes1[i] ^= 113;
            System.Random random = new System.Random(~BitConverter.ToInt32(bytes1, 0));
            random.NextBytes(bytes);
            return bytes;
        }
    }
}
