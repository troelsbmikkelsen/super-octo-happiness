using BibliotekLib.Entities;
using ClientServerLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp {
    class ClientTestApp {
        static void Main(string[] args) {
            SocketConnection client = new SocketConnection("127.0.0.1", 1234);
            client.Connect();

            //Console.ReadKey();

            client.SendText("bibliotekGetAll");

            int size = client.GetDataSize();
            Console.WriteLine(size);

            client.GetDataType();

            List<Bibliotek> bib = (List<Bibliotek>)client.ReceiveObject(size);

            Console.WriteLine(bib[0].Navn);

            Console.ReadKey();
        }

    }

    //public class Client {
    //    Socket server;
    //    IPEndPoint serverIP;

    //    /// <summary>
    //    /// Sets up a client with a server on localhost at port 1234
    //    /// </summary>
    //    public Client() {
    //        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
    //    }

    //    /// <summary>
    //    /// Sets up a client with a server on 127.0.0.1 at the specified port
    //    /// </summary>
    //    /// <param name="port"></param>
    //    public Client(int port) {
    //        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
    //    }

    //    /// <summary>
    //    /// Sets up a client with a server at the specified ip and port
    //    /// </summary>
    //    /// <param name="port"></param>
    //    /// <param name="ip"></param>
    //    public Client(int port, string ip) {
    //        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = new IPEndPoint(IPAddress.Parse(ip), port);
    //    }

    //    /// <summary>
    //    /// Connects client to server
    //    /// </summary>
    //    public void Connect() {
    //        server.Connect(serverIP);
    //    }

    //    public void Send(byte[] data, byte ident) {
    //        // Send data length
    //        server.Send(BitConverter.GetBytes(data.Length));

    //        // Send data identifier
    //        server.Send(new byte[] { ident });

    //        // Send data
    //        server.Send(data);
    //    }

    //    public byte[] Receive(int dataSize) {
    //        byte[] data = new byte[dataSize];
    //        int bytesRead = 0;
    //        int totalBytesRead = 0;

    //        do {
    //            byte[] Buffer = new byte[server.ReceiveBufferSize];

    //            bytesRead = server.Receive(Buffer);

    //            Array.Copy(Buffer, 0, data, totalBytesRead, bytesRead);

    //            totalBytesRead += bytesRead;
    //        } while (totalBytesRead < dataSize);
            
    //        return data;
    //    }

    //    public byte GetDataType() {
    //        byte[] Buffer = new byte[1];

    //        server.Receive(Buffer);

    //        return Buffer[0];
    //    }

    //    /// <summary>
    //    /// Calls Receive to get server data size
    //    /// </summary>
    //    /// <returns></returns>
    //    public int GetDataSize() {
    //        byte[] Buffer = new byte[4];
    //        server.Receive(Buffer);

    //        return BitConverter.ToInt32(Buffer, 0);
    //    }

    //    /// <summary>
    //    /// Sends a string to the server as Unicode
    //    /// </summary>
    //    public void SendText(string text) {
    //        // Send 1 byte identifier 0x54 / T
    //        //server.Send(new byte[] { 0x54 });

    //        // Send text
    //        Send(Encoding.Unicode.GetBytes(text), 0x54);

    //        //throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Receives and returns text from server
    //    /// </summary>
    //    /// <returns></returns>
    //    public string ReceiveText(int dataSize) {
    //        return Encoding.Unicode.GetString(Receive(dataSize));
            
    //        //throw new NotImplementedException();
    //    }

    //    public void SendData(byte[] data) {
    //        // Send 1 byte identifier 0x44 / D
    //        //server.Send(new byte[] { 0x44 });

    //        // Send data
    //        Send(data, 0x44);

    //        throw new NotImplementedException();
    //    }

    //    public byte[] ReceiveData(int dataSize) {
    //        return Receive(dataSize);

    //        //throw new NotImplementedException();
    //    }

    //    public void SendObject(object obj) {
    //        // Send 1 byte identifier 0x4F / O
    //        //server.Send(new byte[] { 0x4F });

    //        // Send deserialized object
    //        Send(SerializeObject(obj), 0x4F);

    //        //throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Serializes object into byte array
    //    /// </summary>
    //    /// <param name="o"></param>
    //    /// <returns></returns>
    //    private byte[] SerializeObject(object o) {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        MemoryStream ms = new MemoryStream();

    //        bf.Serialize(ms, o);

    //        ms.Seek(0, SeekOrigin.Begin);

    //        return ms.ToArray();
    //    }

    //    public object ReceiveObject(int dataSize) {
    //        return DeserializeObject(Receive(dataSize));

    //        //throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Deserializes byte array into object
    //    /// </summary>
    //    /// <param name="data"></param>
    //    /// <returns></returns>
    //    private object DeserializeObject(byte[] data) {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        MemoryStream ms = new MemoryStream();

    //        ms.Write(data, 0, data.Length);
    //        ms.Position = 0;

    //        return bf.Deserialize(ms);
    //    }
    //}
}
