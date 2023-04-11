using System.IO;
using System.Text;
using UnityEngine;

public class myWriter
{
	public sbyte[] buffer = new sbyte[2048];

	private int posWrite;

	private int lenght = 2048;

	public void writeSByte(sbyte value)
	{
		checkLenght(0);
		buffer[posWrite++] = value;
	}

	public void writeSByteUncheck(sbyte value)
	{
		buffer[posWrite++] = value;
	}

	public void writeByte(sbyte value)
	{
		writeSByte(value);
	}

	public void writeByte(int value)
	{
		writeSByte((sbyte)value);
	}

	public void writeChar(char value)
	{
		writeSByte(0);
		writeSByte((sbyte)value);
	}

	public void writeUnsignedByte(byte value)
	{
		writeSByte((sbyte)value);
	}

	public void writeUnsignedByte(byte[] value)
	{
		checkLenght(value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			writeSByteUncheck((sbyte)value[i]);
		}
	}

	public void writeSByte(sbyte[] value)
	{
		checkLenght(value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			writeSByteUncheck(value[i]);
		}
	}

	public void writeShort(short value)
	{
		checkLenght(2);
		for (int num = 1; num >= 0; num--)
		{
			writeSByteUncheck((sbyte)(value >> num * 8));
		}
	}

	public void writeShort(int value)
	{
		checkLenght(2);
		short num = (short)value;
		for (int num2 = 1; num2 >= 0; num2--)
		{
			writeSByteUncheck((sbyte)(num >> num2 * 8));
		}
	}

	public void writeUnsignedShort(ushort value)
	{
		checkLenght(2);
		for (int num = 1; num >= 0; num--)
		{
			writeSByteUncheck((sbyte)(value >> num * 8));
		}
	}

	public void writeInt(int value)
	{
		checkLenght(4);
		for (int num = 3; num >= 0; num--)
		{
			writeSByteUncheck((sbyte)(value >> num * 8));
		}
	}

	public void writeLong(long value)
	{
		checkLenght(8);
		for (int num = 7; num >= 0; num--)
		{
			writeSByteUncheck((sbyte)(value >> num * 8));
		}
	}

	public void writeBoolean(bool value)
	{
		writeSByte((sbyte)(value ? 1 : 0));
	}

	public void writeBool(bool value)
	{
		writeSByte((sbyte)(value ? 1 : 0));
	}

	public void writeString(string value)
	{
		char[] array = value.ToCharArray();
		writeShort((short)array.Length);
		checkLenght(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			writeSByteUncheck((sbyte)array[i]);
		}
	}

	public void writeUTF(string value)
	{
		Encoding unicode = Encoding.Unicode;
		byte[] array = Encoding.Convert(unicode, Encoding.GetEncoding(65001), unicode.GetBytes(value));
		writeShort((short)array.Length);
		checkLenght(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			writeSByteUncheck((sbyte)array[i]);
		}
	}

	public void write(sbyte value)
	{
		writeSByte(value);
	}

	public void write(ref sbyte[] data, int arg1, int arg2)
	{
		if (data == null)
			return;
		for (int i = 0; i < arg2; i++)
		{
			writeSByte(data[i + arg1]);
			if (posWrite > buffer.Length)
				break;
		}
	}

	public void write(sbyte[] value)
	{
		writeSByte(value);
	}

	public sbyte[] getData()
	{
		if (posWrite <= 0)
			return null;
		sbyte[] array = new sbyte[posWrite];
		for (int i = 0; i < posWrite; i++)
		{
			array[i] = buffer[i];
		}
		return array;
	}

	public void checkLenght(int ltemp)
	{
		if (posWrite + ltemp > lenght)
		{
			sbyte[] array = new sbyte[lenght + 1024 + ltemp];
			for (int i = 0; i < lenght; i++)
			{
				array[i] = buffer[i];
			}
			buffer = null;
			buffer = array;
			lenght += 1024 + ltemp;
		}
	}

	private static void convertString(string[] args)
	{
		string path = args[0];
		string path2 = args[1];
		using (StreamReader input = new StreamReader(path, Encoding.Unicode))
			using (StreamWriter output = new StreamWriter(path2, false, Encoding.UTF8))
				CopyContents(input, output);
	}

	private static void CopyContents(TextReader input, TextWriter output)
	{
		char[] array = new char[8192];
		int count;
		while ((count = input.Read(array, 0, array.Length)) != 0)
		{
			output.Write(array, 0, count);
		}
		output.Flush();
		Debug.Log(output.ToString());
	}

	public byte convertSbyteToByte(sbyte var)
	{
		if (var > 0)
			return (byte)var;
		return (byte)(var + 256);
	}

	public byte[] convertSbyteToByte(sbyte[] var)
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

	public void Close()
	{
		buffer = null;
	}

	public void close()
	{
		buffer = null;
	}
}
