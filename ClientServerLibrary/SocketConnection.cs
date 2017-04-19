using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLibrary
{
    public class SocketConnection {
        Socket remote;
        IPEndPoint remoteIP;
        
        public SocketConnection(Socket c) {
            remote = c;
        }

        /// <summary>
        /// Creates a new socket connection, ready to be connected to the provided endpoint.
        /// </summary>
        public SocketConnection(IPEndPoint serverip) {
            remote = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remoteIP = serverip;
        }

        /// <summary>
        /// Creates a new socket connection, ready to be connected to the provided ip and port.
        /// </summary>
        public SocketConnection(string ip, int port) {
            remote = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remoteIP = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public void Connect() {
            remote.Connect(remoteIP);
        }

        public void Send(byte[] data, byte ident) {
            // Send data length
            remote.Send(BitConverter.GetBytes(data.Length));

            // Send data identifier
            remote.Send(new byte[] { ident });

            // Send data
            remote.Send(data);
        }

        public byte[] Receive(int dataSize) {
            byte[] data = new byte[dataSize];
            int bytesRead = 0;
            int totalBytesRead = 0;

            do {
                byte[] Buffer = new byte[remote.ReceiveBufferSize];

                bytesRead = remote.Receive(Buffer);

                Array.Copy(Buffer, 0, data, totalBytesRead, bytesRead);

                totalBytesRead += bytesRead;
            } while (totalBytesRead < dataSize);

            return data;
        }

        public byte GetDataType() {
            byte[] Buffer = new byte[1];

            remote.Receive(Buffer);

            return Buffer[0];
        }

        /// <summary>
        /// Calls Receive to get client data size
        /// </summary>
        /// <returns></returns>
        public int GetDataSize() {
            byte[] Buffer = new byte[4];
            remote.Receive(Buffer);

            return BitConverter.ToInt32(Buffer, 0);
        }

        /// <summary>
        /// Sends a string to the server as Unicode
        /// </summary>
        public void SendText(string text) {
            // Send 1 byte identifier 0x54 / T
            //client.Send(new byte[] { 0x54 });

            // Send text
            Send(Encoding.Unicode.GetBytes(text), 0x54);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Receives and returns text from server
        /// </summary>
        /// <returns></returns>
        public string ReceiveText(int dataSize) {
            return Encoding.Unicode.GetString(Receive(dataSize));

            //throw new NotImplementedException();
        }

        public void SendData(byte[] data) {
            // Send 1 byte identifier 0x44 / D
            //client.Send(new byte[] { 0x44 });

            // Send data
            Send(data, 0x44);

            //throw new NotImplementedException();
        }

        public byte[] ReceiveData(int dataSize) {
            return Receive(dataSize);

            //throw new NotImplementedException();
        }

        public void SendObject(object obj) {
            // Send 1 byte identifier 0x4F / O
            //client.Send(new byte[] { 0x4F });

            // Send deserialized object
            Send(SerializeObject(obj), 0x4F);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Serializes object into byte array
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private byte[] SerializeObject(object o) {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, o);

            ms.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }

        public object ReceiveObject(int dataSize) {
            return DeserializeObject(Receive(dataSize));

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes byte array into object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private object DeserializeObject(byte[] data) {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            ms.Write(data, 0, data.Length);
            ms.Position = 0;

            return bf.Deserialize(ms);
        }
    }
}
