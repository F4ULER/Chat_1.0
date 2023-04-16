using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            //Поключение к серверу
            // ip и port поключение соединения сервера и клиенту
            // реализуется на 1 машине (localhost), поэтому берем стандартные для этого ip и port
            const string ip = "127.0.0.1";
            const int port = 8080;

            //конечная точка (куда можно подключаться) или точка подключения
            //Parse помогает преобразовать строку в текстовом формате в стандартный формат подключения
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //Объявление сокета (дверь, через которую устанавливается соединение)
            //набор параметров для tcp протокола
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Введите сообщение: ");
            var message = Console.ReadLine();

            //кодируем сообщение
            var data = Encoding.UTF8.GetBytes(message);

            //подключение для этого сокета
            tcpSocket.Connect(tcpEndPoint);

            //отправляем сообщение
            tcpSocket.Send(data);

            // куда будем принимать сообщение
            var buffer = new byte[256];

            //количество реально полученных байт
            var size = 0;

            //собирает полученные данные
            var answer = new StringBuilder();

            do
            {
                // получение байт
                size = tcpSocket.Receive(buffer);

                //из большого сообщения потихоньку собираем сообщение, чтобы избежать потерю данных
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0); //будет выполняться считывание пока в подключении есть данные

            Console.WriteLine(answer.ToString());

            //закрываем соединение
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();

            Console.ReadLine();
        }
    }
}
