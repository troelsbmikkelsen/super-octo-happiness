using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLibrary {
    public class Server {
        Socket listenerSocket;
        IPEndPoint serverIP;

        /// <summary>
        /// Sets up a server on localhost at port 1234
        /// </summary>
        public Server() {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            listenerSocket.Bind(serverIP);
            listenerSocket.Listen(0);
        }

        /// <summary>
        /// Sets up a server on 127.0.0.1 at the specified port
        /// </summary>
        /// <param name="port"></param>
        public Server(int port) {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listenerSocket.Bind(serverIP);
            listenerSocket.Listen(0);
        }

        /// <summary>
        /// Sets up a server at the specified ip and port
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ip"></param>
        public Server(int port, string ip) {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverIP = new IPEndPoint(IPAddress.Parse(ip), port);
            listenerSocket.Bind(serverIP);
            listenerSocket.Listen(0);
        }

        public Server(IPEndPoint ipendpoint) {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverIP = ipendpoint;
            listenerSocket.Bind(serverIP);
            listenerSocket.Listen(0);
        }

        public void Accept() {
            //client = listenerSocket.Accept();
        }

        public Task Start(Action<SocketConnection> serverAction) {
            return Task.Run(() => {
                while (true) {
                    SocketConnection c = new SocketConnection(listenerSocket.Accept());
                    Task.Run(() => serverAction(c));
                }
            });
        }
    }
}
