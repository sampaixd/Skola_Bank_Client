using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Skola_Bank_Client
{
    internal class SocketComm
    {
        public static string RecvMsg()
        {
            byte[] bMsg = new byte[256];
            int bReadSize = TcpInformation.GetTcpStream.Read(bMsg, 0, bMsg.Length);
            string msg = "";

            for (int i = 0; i < bReadSize; i++)
                msg += Convert.ToChar(bMsg[i]);

            return msg;
        }

        public static void SendMsg(string message)
        {
            byte[] bMessage = Encoding.UTF8.GetBytes(message);
            TcpInformation.GetTcpStream.Write(bMessage, 0, bMessage.Length);
            Thread.Sleep(50);   // avoids multiple messages being sent at once
        }
        // checks for a true/false response, used for example in checking if a user exists in the database or not
        public static bool ServerTrueOrFalseResponse()    
        {
            if (RecvMsg() == "False")
                return false;
            return true;
        }
    }
}
