namespace QLTK.Models
{
    public class Server
    {
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public int language { get; set; }

        public Server() { }

        public Server(string name, string ip, int port, int language)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
            this.language = language;
        }

        public Server(string name, string ip, int port)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
            this.language = 0;
        }
    }
}
