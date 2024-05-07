using System;

namespace Mod.AccountManager
{
    internal struct Server
    {
        internal int index;
        internal string name;
        internal string hostnameOrIPAddress;
        internal short port;

        internal Server(int index)
        {
            this.index = index;
            name = hostnameOrIPAddress = "";
            port = 0;
        }

        internal Server(string name, string hostnameOrIPAddress, short port)
        {
            index = -1;
            this.name = name;
            this.hostnameOrIPAddress = hostnameOrIPAddress;
            this.port = port;
        }

        internal Server(string serverInfo)
        {
            index = -1;
            string[] data = serverInfo.Split(':');
            if (data.Length == 2)
            {
                name = "";
                hostnameOrIPAddress = data[0];
                port = short.Parse(data[1]);
            }
            else if (data.Length == 3)
            {
                name = data[0];
                hostnameOrIPAddress = data[1];
                port = short.Parse(data[2]);
            }
            else
                throw new ArgumentException();
        }

        public override string ToString()
        {
            if (index > -1)
                return index.ToString();
            return $"{name}:{hostnameOrIPAddress}:{port}".TrimStart(' ');
        }

        internal string GetIPPort() => $"{hostnameOrIPAddress}:{port}".TrimStart(' ');

        internal bool IsCustomIP() => index == -1;
    }
}