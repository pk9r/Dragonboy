using System;

namespace Mod.AccountManager
{
    internal class Server
    {
        internal int index;
        internal string name;
        internal string hostnameOrIPAddress;
        internal ushort port;

        internal Server(int index)
        {
            this.index = index;
            name = hostnameOrIPAddress = "";
            port = 0;
        }

        internal Server(string name, string hostnameOrIPAddress, ushort port)
        {
            index = -1;
            this.name = name;
            this.hostnameOrIPAddress = hostnameOrIPAddress;
            this.port = port;
        }

        internal Server(string serverInfo)
        {
            name = "";
            hostnameOrIPAddress = "";
            port = 0;
            if (int.TryParse(serverInfo, out index))
                return;
            else
            {
                index = -1;
                string[] info = serverInfo.Split(':');
                if (info.Length == 2)
                {
                    hostnameOrIPAddress = info[0];
                    port = ushort.Parse(info[1]);
                }
                else if (info.Length == 3)
                {
                    name = info[0];
                    hostnameOrIPAddress = info[1];
                    port = ushort.Parse(info[2]);
                }
                else
                    throw new ArgumentException();
            }
        }

        public override string ToString()
        {
            if (index > -1)
                return index.ToString();
            return $"{name}:{hostnameOrIPAddress}:{port}".TrimStart(' ');
        }

        internal string GetIPPort() => $"{hostnameOrIPAddress}:{port}".TrimStart(' ');

        internal bool IsCustomIP() => index == -1;

        public override bool Equals(object obj)
        {
            if (obj is Server server)
            {
                if (server.IsCustomIP() != IsCustomIP())
                    return false;
                if (IsCustomIP())
                    return server.hostnameOrIPAddress == hostnameOrIPAddress && server.port == port;
                return server.index == index;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(index, name, hostnameOrIPAddress, port);
        }

        public static bool operator ==(Server a, Server b)
        {
            if (a is null)
                return b is null;
            return a.Equals(b);
        }

        public static bool operator !=(Server a, Server b)
        {
            if (a is null)
                return b is not null;
            return !a.Equals(b);
        }
    }
}