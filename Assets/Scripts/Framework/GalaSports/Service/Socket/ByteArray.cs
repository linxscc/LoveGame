using System.IO;
using System.Net;
using System.Text;

namespace Assets.Scripts.Framework.GalaSports.Service.Socket
{
    //对SocketMessage的读写  
    public class ByteArray
    {

        //为了节省传输的流量，所以传输的是二进制  
        //读与写操作都是对一个流来进行的，这里使用MemoryStream  
        private MemoryStream memoryStream;
        private BinaryReader binaryReader;
        private BinaryWriter binaryWriter;

        private int readIndex = 0;
        private int writeIndex = 0;

        public ByteArray()
        {
            memoryStream = new MemoryStream();
            binaryReader = new BinaryReader(memoryStream);
            binaryWriter = new BinaryWriter(memoryStream);
        }

        public void Clear()
        {
            Destroy();
            memoryStream = new MemoryStream();
            binaryReader = new BinaryReader(memoryStream);
            binaryWriter = new BinaryWriter(memoryStream);
            readIndex = 0;
            writeIndex = 0;
        }

        public void Destroy()
        {
            binaryReader.Close();
            binaryWriter.Close();
            memoryStream.Close();
            memoryStream.Dispose();
        }

        public int GetReadIndex()
        {
            return readIndex;
        }

        public int GetLength()
        {
            return (int)memoryStream.Length;
        }

        public int GetPosition()
        {
            //position是从0开始的  
            return (int)memoryStream.Position;
        }

        public byte[] GetByteArray()
        {
            return memoryStream.ToArray();
        }

        public void Seek(int offset, SeekOrigin seekOrigin)
        {
            //offset:相对于 SeekOrigin 所指定的位置的偏移量参数  
            memoryStream.Seek(offset, seekOrigin);
        }


        #region read  
        public bool ReadBoolean()
        {
            Seek(readIndex, SeekOrigin.Begin);
            bool a = binaryReader.ReadBoolean();
            readIndex += 1;
            return a;
        }

        public short ReadInt16()
        {
            Seek(readIndex, SeekOrigin.Begin);
            short a = binaryReader.ReadInt16();
            readIndex += 2;
            return a;
        }

        public int ReadInt32()
        {
            lock (this)
            {
                Seek(readIndex, SeekOrigin.Begin);
                int a = IPAddress.HostToNetworkOrder(binaryReader.ReadInt32());
                readIndex += 4;
                return a;
            }
        }

        public float ReadSingle()
        {
            Seek(readIndex, SeekOrigin.Begin);
            float a = binaryReader.ReadSingle();
            readIndex += 4;
            return a;
        }

        public double ReadDouble()
        {
            Seek(readIndex, SeekOrigin.Begin);
            double a = binaryReader.ReadDouble();
            readIndex += 8;
            return a;
        }

        public string ReadString()
        {
            Seek(readIndex, SeekOrigin.Begin);
            string a = binaryReader.ReadString();
            //因为binaryWriter写字符串时会在字符串前面加一字节，存储字符串的长度  
            readIndex += Encoding.UTF8.GetBytes(a).Length + 1;
            return a;
        }

        public byte[] ReadBytes(int len)
        {
            lock (this)
            {
                Seek(readIndex, SeekOrigin.Begin);
                byte[] a = binaryReader.ReadBytes(len);
                readIndex += len;
                return a;
            }
        }

        #endregion

        #region write  
        public void Write(bool value)
        {
            Seek(writeIndex, SeekOrigin.Begin);
            binaryWriter.Write(value);
            writeIndex += 1;
        }

        public void Write(short value)
        {
            value = IPAddress.HostToNetworkOrder(value);//字节序转换
            Seek(writeIndex, SeekOrigin.Begin);
            binaryWriter.Write(value);
            writeIndex += 2;
        }

        public void Write(int value)
        {
            lock (this)
            {
                value = IPAddress.HostToNetworkOrder(value);//字节序转换
                Seek(writeIndex, SeekOrigin.Begin);
                binaryWriter.Write(value);
                writeIndex += 4;
            }

        }

        public void Write(float value)
        {
            Seek(writeIndex, SeekOrigin.Begin);
            binaryWriter.Write(value);
            writeIndex += 4;
        }

        public void Write(double value)
        {
            Seek(writeIndex, SeekOrigin.Begin);
            binaryWriter.Write(value);
            writeIndex += 8;
        }

        public void Write(string value)
        {
            lock (this)
            {
                Seek(writeIndex, SeekOrigin.Begin);
                binaryWriter.Write(value);
                //因为binaryWriter写字符串时会在字符串前面加一字节，存储字符串的长度  
                writeIndex += Encoding.UTF8.GetBytes(value).Length + 1;
            }
        }

        public void Write(byte[] value)
        {
            lock (this)
            {
                Seek(writeIndex, SeekOrigin.Begin);
                binaryWriter.Write(value);
                writeIndex += value.Length;
            }
        }
        #endregion
    }
}