using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace TCP_client
{
	class MainClass
	{
		internal static TcpClient TCP;
		internal static Stream TCPStream;
		internal static byte[] Streambyte;
		internal static byte[] TCPbyte;
		internal static int TCPint;
		internal static void Receive()
		{
				while (true)
				{
					TCPbyte = new byte[100];
					TCPint = TCPStream.Read(TCPbyte, 0, 100);
					Console.Write(Encoding.ASCII.GetString(TCPbyte) + Environment.NewLine);
				}
		}
		internal static void Connect(string ip, int port)
		{
			TCP = new TcpClient();
			TCP.Connect(IPAddress.Parse(ip), port);
			TCPStream = TCP.GetStream();
			StreamSend("Client's hostname: " + Dns.GetHostName() + Environment.NewLine + "Client's OS: " + Environment.OSVersion.Platform);
			Receive();
		}
		internal static void StreamSend(string message)
		{
			Streambyte = Encoding.ASCII.GetBytes(message);
			TCPStream.Write(Streambyte, 0, Streambyte.Length);
		}
		public static void Main(string[] args)
		{	
			try
			{
				Connect("127.0.0.1", 8001);
			}
			catch (Exception)
			{
				Console.WriteLine("Connection error!");
				Thread.Sleep(5000);
				Main(args);
			}
		}
	}
}
