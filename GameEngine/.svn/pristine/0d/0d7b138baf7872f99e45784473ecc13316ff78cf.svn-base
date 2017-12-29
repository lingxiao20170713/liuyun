using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class ByteArray
{
    private int maxBufferSize = ushort.MaxValue;

    protected byte[] mDataBuff;
    protected int mLength = 0;
    protected int mPosition = 0;

    public ByteArray()
    {
        mDataBuff = new byte[maxBufferSize];
    }

    public bool CreateFromSocketBuff(byte[] buff, int nSize)
    {
        if (buff == null)
        {
            return false;
        }

        this.mLength = nSize;

        if ((this.mLength > maxBufferSize) || (this.mLength <= 2))
        {
            return false;
        }
        Array.Copy(buff, 0, this.mDataBuff, 0, this.mLength);
        mPosition = 0;
        return true;
    }

    public void Write(byte value)
    {
        mDataBuff[mPosition] = (byte)value;
        mPosition += sizeof(byte);
        mLength = mPosition;
        CheckBuffSize();
    }

    public void WriteByte(int value)
    {
        mDataBuff[mPosition] = (byte)value;
        mPosition++;
        mLength = mPosition;
        CheckBuffSize();
    }

    public void WriteInt(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        EndianReverse(bytes);

        Array.Copy(bytes, 0, mDataBuff, mPosition, 4);
        mPosition += sizeof(int);
        mLength = mPosition;
        CheckBuffSize();
    }

    public void WriteLong(long value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        EndianReverse(bytes);

        Array.Copy(bytes, 0, mDataBuff, mPosition, 8);
        mPosition += sizeof(long);
        mLength = mPosition;
        CheckBuffSize();
    }

    public void WriteShort(short value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        EndianReverse(bytes);

        Array.Copy(bytes, 0, mDataBuff, mPosition, 2);
        mPosition += sizeof(short);
        mLength = mPosition;
        CheckBuffSize();
    }

    public void WriteShort(int value)
    {
        byte[] bytes = BitConverter.GetBytes((short)value);
        EndianReverse(bytes);

        Array.Copy(bytes, 0, mDataBuff, mPosition, 2);
        mPosition += 2;
        mLength = mPosition;
        CheckBuffSize();
    }
    public void WriteString(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        //字符串大小端不用转换，只有字符串的长度需要转换
        short strLength = (short)bytes.Length;
        byte[] strLengthBytes = BitConverter.GetBytes(strLength);
        EndianReverse(strLengthBytes);
        Array.Copy(strLengthBytes, 0, mDataBuff, mPosition, 2);

        mPosition += 2;
        Array.Copy(bytes, 0, mDataBuff, mPosition, strLength);
        mPosition += strLength;
        mLength = mPosition;
        CheckBuffSize();
    }

    public void WriteBytes(byte[] bytes)
    {
        int byteslen = bytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(byteslen);
        EndianReverse(lengthBytes);
        Array.Copy(lengthBytes, 0, mDataBuff, mPosition, 4);
        mPosition += 4;

        Array.Copy(bytes, 0, mDataBuff, mPosition, byteslen);
        mPosition += byteslen;
        mLength = mPosition;

        CheckBuffSize();
    }

    public byte ReadByte()
    {
        if ((mPosition + 1) > mLength)
        {
            throw new Exception("ReadByte读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        byte value = (byte)mDataBuff[mPosition];
        mPosition+=sizeof(byte);
        return value;
    }

    public int ReadInt()
    {
        if ((mPosition + 4) > mLength)
        {
            throw new Exception("ReadInt读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        //先把对应位数的Byte拿出来，看是否需要大小端转换，再转换具体的数据类型
        byte[] byteTemp = new byte[4];
        Array.Copy(mDataBuff, mPosition, byteTemp, 0, 4);
        EndianReverse(byteTemp);
        int value = BitConverter.ToInt32(byteTemp, 0);

        mPosition += sizeof(int);
        return value;
    }

    public long ReadLong()
    {
        if ((mPosition + 8) > mLength)
        {
            throw new Exception("ReadInt读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        //先把对应位数的Byte拿出来，看是否需要大小端转换，再转换具体的数据类型
        byte[] byteTemp = new byte[8];
        Array.Copy(mDataBuff, mPosition, byteTemp, 0, 8);
        EndianReverse(byteTemp);
        long value = BitConverter.ToInt64(byteTemp, 0);

        mPosition += sizeof(long);
        return value;
    }

    public short ReadShort()
    {
        if ((mPosition + 2) > mLength)
        {
            throw new Exception("ReadShort读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        //先把对应位数的Byte拿出来，看是否需要大小端转换，再转换具体的数据类型
        byte[] byteTemp = new byte[2];
        Array.Copy(mDataBuff, mPosition, byteTemp, 0, 2);
        EndianReverse(byteTemp);
        short value = BitConverter.ToInt16(byteTemp, 0);
        mPosition += 2;
        return value;
    }

    public ushort ReadUShort()
    {
        if ((mPosition + 2) > mLength)
        {
            throw new Exception("ReadShort读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        //先把对应位数的Byte拿出来，看是否需要大小端转换，再转换具体的数据类型
        byte[] byteTemp = new byte[2];
        Array.Copy(mDataBuff, mPosition, byteTemp, 0, 2);
        EndianReverse(byteTemp);
        ushort value = BitConverter.ToUInt16(byteTemp, 0);
        mPosition += 2;
        return value;
    }

    public string ReadString()
    {
        if ((mPosition + 2) > mLength)
        {
            throw new Exception("ReadString[0]读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        //先把对应位数的Byte拿出来，看是否需要大小端转换，再转换具体的数据类型
        byte[] byteTemp = new byte[2];
        Array.Copy(mDataBuff, mPosition, byteTemp, 0, 2);
        EndianReverse(byteTemp);
        ushort count = BitConverter.ToUInt16(byteTemp, 0);

        mPosition += 2;
        string value = Encoding.UTF8.GetString(mDataBuff, mPosition, count);
        mPosition += count;
        return value;
    }

    public byte[] ReadBytes()
    {
        if ((mPosition + 4) > mLength)
        {
            throw new Exception("ReadString[0]读取数据失败，读取数据超出" + mLength + "字节流范围");
        }
        //先把对应位数的Byte拿出来，看是否需要大小端转换，再转换具体的数据类型
        byte[] byteTemp = new byte[4];
        Array.Copy(mDataBuff, mPosition, byteTemp, 0, 4);
        EndianReverse(byteTemp);
        int bytesLength = BitConverter.ToInt32(byteTemp, 0);
        mPosition += 4;

        byte[] byteBuff = new byte[bytesLength];
        Array.Copy(mDataBuff, mPosition, byteBuff, 0, bytesLength);
        mPosition += bytesLength;
        return byteBuff;
    }

    public byte[] Buff
    {
        get
        {
            return mDataBuff;
        }
    }

    public int Length
    {
        get
        {
            return mLength;
        }
    }

    public int Postion
    {
        get
        {
            return mPosition;
        }
        set
        {
            mPosition = value;
        }
    }

    private void CheckBuffSize()
    {
        if (mPosition > maxBufferSize)
        {
            throw new Exception("InitBytesArray初始化失败，超出字节流最大限制");
        }
    }

    public void EndianReverse(Array array)
    {
        if (BitConverter.IsLittleEndian)//指示数据在此计算机结构中存储时的字节顺序（如果结构为 Little-endian，则该值为 true；如果结构为 Big-endian，则该值为 false。不同的计算机结构采用不同的字节顺序存储数据。“Big-endian”表示最大的有效字节位于单词的左端。“Little-endian”表示最大的有效字节位于单词的右端。
        {
            Array.Reverse(array);//倒序
        }
    }
}
