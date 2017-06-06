using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
namespace TCP_host
{
	class MainClass
	{
		internal static TcpListener TcpListen;
		internal static Socket TCPSocket;
		internal static byte[] receivebytes;
		internal static int SocketReceive;
		internal static int TCPport;
		internal static void Connect()
		{
			TcpListen = new TcpListener(IPAddress.Any, TCPport);
			TcpListen.Start();
		}
		internal static void Listen(int port)
		{
			TCPport = port;
			Connect();
			Console.WriteLine("Server port: " + port);
			Console.WriteLine("Waiting for client to connect");
			Accept();
		}
		internal static void StreamSend(string message)
		{
			TCPSocket.Send(Encoding.ASCII.GetBytes(message));
		}
		internal static void Accept()
		{
			while (true)
			{
				try
				{
					TCPSocket = TcpListen.AcceptSocket();
					Console.WriteLine("Connection accepted: " + TCPSocket.RemoteEndPoint);
					Receive();
					TCPinput();
				}
				catch (Exception)
				{
					Stop();
					Console.WriteLine("Client has disconnected");
					Connect();
					Accept();
				}
			}
		}
		internal static void TCPinput()
		{
			string input;
			while ((input = Console.ReadLine()) != null)
			{
				StreamSend(input);
			}
		}
		internal static void Receive()
		{
				receivebytes = new byte[100];
				SocketReceive = TCPSocket.Receive(receivebytes);
				for (int i = 0; i < SocketReceive; i++)
					Console.Write(Convert.ToChar(receivebytes[i]));
				Console.Write(Environment.NewLine);
		}
		internal static void PING()
		{
			StreamSend("PING");
		}
		internal static void Stop()
		{
			TCPSocket.Close();
			TcpListen.Stop();
		}
		public static void Main(string[] args)
		{
			try
			{
				Listen(8001);
			}
			catch (Exception e)
			{
				Stop();
				Console.WriteLine("Error: " + e.ToString());
				Main(args);
			}
		}
	}
}
