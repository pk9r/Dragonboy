using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Models
{
    public class Account
    {
        public int indexServer { get; set; } = 0;
        public string username { get; set; }
        public string password { get; set; }

        [LitJSON.JsonSkip]
        public Process process;

        [LitJSON.JsonSkip]
        public Socket workSocket;

        [LitJSON.JsonSkip]
        public string status { get; set; } = "-";

        [LitJSON.JsonSkip]
        public int number { get; set; }
    }
}
