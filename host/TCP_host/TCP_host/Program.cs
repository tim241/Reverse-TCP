using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
		internal static void Listen(int port, bool quiet)
		{
			TCPport = port;
			Connect();
			if(!quiet)
				Console.WriteLine("Server port: " + port);
			Console.WriteLine("Waiting for client to connect");
			Accept();
		}
		internal static void StreamSend(string message, bool raw = false)
		{
			if (raw)
				TCPSocket.Send(Encoding.ASCII.GetBytes(message));
			else
				TCPSocket.Send(Encoding.ASCII.GetBytes("CMD " + message));
		}
		internal static void Accept()
		{
			while (true)
			{
				try
				{
					TCPSocket = TcpListen.AcceptSocket();
					Console.WriteLine("Connection accepted: " + TCPSocket.RemoteEndPoint);
					Task TCPreceive = Task.Factory.StartNew(() => AutoReceive());
					Task TCPPing = Task.Factory.StartNew(() => PING());
					Task input = Task.Factory.StartNew(() => TCPinput());
					Task.WaitAll(TCPreceive, TCPPing, input);
				}
				catch (Exception)
				{
					Stop();
					Console.WriteLine("Client has disconnected");
					Start(true);
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
		internal static void AutoReceive()
		{
			while (true)
			{
				receivebytes = new byte[100];
				SocketReceive = TCPSocket.Receive(receivebytes);
				string TCPcommand = null;
				for (int i = 0; i < SocketReceive; i++)
					TCPcommand = TCPcommand + Convert.ToChar(receivebytes[i]);
				if (!string.IsNullOrEmpty(TCPcommand)){
					Console.WriteLine(TCPcommand);
				}
			}
		}
		internal static void PING()
		{
			while (true)
			{
				Thread.Sleep(5000);
				StreamSend("PING", true);
				receivebytes = new byte[100];
				SocketReceive = TCPSocket.Receive(receivebytes);
				string TCPcommand = null;
				for (int i = 0; i < SocketReceive; i++)
					TCPcommand = TCPcommand + Convert.ToChar(receivebytes[i]);
				if (TCPcommand != "PONG")
				{
					Console.WriteLine("Client has timed out!");
					Stop();
					Start(true);
					break;
				}
			}
		}
		internal static void Start(bool quiet = false)
		{
			Listen(8001, quiet);
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
				Start();
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
