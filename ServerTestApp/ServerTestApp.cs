using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net.Sockets;
using System.Net;
using BibliotekLib.Entities;
using ClientServerLibrary;

namespace ServerTestApp {
    class ServerTestApp {
        static void Main(string[] args) {

            Server server = new Server();
            server.Start(Handler);

            Console.ReadKey();
        }

        public static void Handler(SocketConnection connection) {
            bool isRunning = true;
            while (isRunning) {
                int ds = connection.GetDataSize();
                Console.WriteLine($"Data size: {ds}");
                byte dt = connection.GetDataType();
                Console.WriteLine($"Data type: {dt} {0x54}");
                
                switch (dt) {
                    // Object
                    case 0x4F:
                        break;
                    // Text
                    case 0x54:
                        HandleRequest(connection, connection.ReceiveText(ds));
                        break;
                    default:
                        break;
                }
            }
        }

        public static void HandleRequest(SocketConnection connection, string request) {
            switch (request) {
                case "bibliotekGetAll":
                    connection.SendObject(BibliotekLib.BibliotekWrapper.bibliotekGetAll());
                    break;
                default:
                    break;
            }
        }
    }
    
    //public class Server {
    //    Socket listenerSocket;
    //    IPEndPoint serverIP;

    //    /// <summary>
    //    /// Sets up a server on localhost at port 1234
    //    /// </summary>
    //    public Server() {
    //        listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
    //        listenerSocket.Bind(serverIP);
    //        listenerSocket.Listen(0);
    //    }

    //    /// <summary>
    //    /// Sets up a server on 127.0.0.1 at the specified port
    //    /// </summary>
    //    /// <param name="port"></param>
    //    public Server(int port) {
    //        listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
    //        listenerSocket.Bind(serverIP);
    //        listenerSocket.Listen(0);
    //    }

    //    /// <summary>
    //    /// Sets up a server at the specified ip and port
    //    /// </summary>
    //    /// <param name="port"></param>
    //    /// <param name="ip"></param>
    //    public Server(int port, string ip) {
    //        listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = new IPEndPoint(IPAddress.Parse(ip), port);
    //        listenerSocket.Bind(serverIP);
    //        listenerSocket.Listen(0);
    //    }

    //    public Server(IPEndPoint ipendpoint) {
    //        listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        serverIP = ipendpoint;
    //        listenerSocket.Bind(serverIP);
    //        listenerSocket.Listen(0);
    //    }

    //    public void Accept() {
    //        //client = listenerSocket.Accept();
    //    }

    //    public Task Start(Action<SocketConnection> serverAction) {
    //        return Task.Run(() => {
    //            while (true) {
    //                SocketConnection c = new SocketConnection(listenerSocket.Accept());
    //                Task.Run(() => serverAction(c));
    //            }
    //        });
    //    }
    //}

    //public class SocketConnection {
    //    Socket client;

        
    //    public SocketConnection(Socket c) {
    //        client = c;
    //    }

    //    public void Send(byte[] data, byte ident) {
    //        // Send data length
    //        client.Send(BitConverter.GetBytes(data.Length));

    //        // Send data identifier
    //        client.Send(new byte[] { ident });

    //        // Send data
    //        client.Send(data);
    //    }

    //    public byte[] Receive(int dataSize) {
    //        byte[] data = new byte[dataSize];
    //        int bytesRead = 0;
    //        int totalBytesRead = 0;

    //        do {
    //            byte[] Buffer = new byte[client.ReceiveBufferSize];

    //            bytesRead = client.Receive(Buffer);

    //            Array.Copy(Buffer, 0, data, totalBytesRead, bytesRead);

    //            totalBytesRead += bytesRead;
    //        } while (totalBytesRead < dataSize);

    //        return data;
    //    }

    //    public byte GetDataType() {
    //        byte[] Buffer = new byte[1];

    //        client.Receive(Buffer);

    //        return Buffer[0];
    //    }

    //    /// <summary>
    //    /// Calls Receive to get client data size
    //    /// </summary>
    //    /// <returns></returns>
    //    public int GetDataSize() {
    //        byte[] Buffer = new byte[4];
    //        client.Receive(Buffer);

    //        return BitConverter.ToInt32(Buffer, 0);
    //    }

    //    /// <summary>
    //    /// Sends a string to the server as Unicode
    //    /// </summary>
    //    public void SendText(string text) {
    //        // Send 1 byte identifier 0x54 / T
    //        //client.Send(new byte[] { 0x54 });

    //        // Send text
    //        Send(Encoding.Unicode.GetBytes(text), 0x54);

    //        throw new NotImplementedException();
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
    //        //client.Send(new byte[] { 0x44 });

    //        // Send data
    //        Send(data, 0x44);

    //        //throw new NotImplementedException();
    //    }

    //    public byte[] ReceiveData(int dataSize) {
    //        return Receive(dataSize);

    //        //throw new NotImplementedException();
    //    }

    //    public void SendObject(object obj) {
    //        // Send 1 byte identifier 0x4F / O
    //        //client.Send(new byte[] { 0x4F });

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

    //public class DTO {
    //    /// <summary>
    //    /// Text   : 0x54
    //    /// <para />
    //    /// Object : 0x4F
    //    /// </summary>
    //    public readonly byte DataType;

    //    byte[] data;

    //    public string GetAsText() {
    //        string text = "";
    //        try {
    //            text = Encoding.Unicode.GetString(data);
    //        } catch {
    //            // Rethrow exception, keeping stacktrace
    //            throw;
    //        }

    //        return text;
    //    }

    //    public object GetAsObject() {
    //        object obj = new object();

    //        try {
    //            BinaryFormatter bf = new BinaryFormatter();
    //            MemoryStream ms = new MemoryStream();

    //            ms.Write(data, 0, data.Length);
    //            ms.Position = 0;

    //            obj = bf.Deserialize(ms);
    //        } catch {
    //            throw;
    //        }

    //        return obj;
    //    }
    //}
}
