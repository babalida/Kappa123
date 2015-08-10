using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// Кастомный сокет. Получает значение IP и порта. 
    /// Имеет список клиентов. Позволяет подключится к удаленной точке.
    /// </summary>
    public class ClientSocket
    {
        int counter = 0;

        public delegate void ChangeSocketParams(object message);
        public delegate void ChangeParams(object message, EndPoint remoteIP);
        public event ChangeSocketParams OnRecieveData;
        public event ChangeSocketParams OnConnected;
        public event ChangeSocketParams OnSendingError;

        public Socket clientSocket;       
        byte[] buffer = new byte[] { };
        public ClientSocket()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        public void Connect(string ip, int port)
        {
            try
            {
                clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(ConnectCallBack), null);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        private void ConnectCallBack(IAsyncResult result)
        {
            try
            {
                buffer = new byte[102400];
                clientSocket.EndConnect(result);
                Console.WriteLine("Client connected!");
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
        private void ReceiveCallBack(IAsyncResult result)
        {
            try
            {
                int recived = clientSocket.EndReceive(result);
                // Array.Resize(ref buffer, recived);
                string text = Encoding.UTF8.GetString(buffer, 0, recived);
                if (OnRecieveData != null)
                {
                    OnRecieveData(text);
                }
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
            }
            catch
            {

                
            }
        }

        public void Send(string text)
        {
            try
            {
                while (!clientSocket.Connected)
                {

                }
                byte[] _buffer = Encoding.UTF8.GetBytes(text);
                clientSocket.BeginSend(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), null);
            }
            catch (Exception ex)
            {
                if (OnSendingError != null)
                {
                    OnSendingError(ex.ToString());
                }
            }
        }

        private void SendCallBack(IAsyncResult result)
        {
            clientSocket.EndSend(result);
        }
        public void Disconnect()
        {
            clientSocket.Disconnect(false);
            clientSocket.Dispose();
        }

    }
}
