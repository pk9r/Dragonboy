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
        public string username { get; set; }
        public string password { get; set; }
        public int indexServer { get; set; } = 0;

        public string cName { get; set; } = "-";
        public int cgender { get; set; } = -1;
        public string mapName { get; set; } = "Chưa xác định";
        public int mapID { get; set; } = -1;
        public int zoneID { get; set; } = -1;
        public int cx { get; set; } = -1;
        public int cy { get; set; } = -1;
        public int cHP { get; set; } = -1;
        public int cHPFull { get; set; } = -1;
        public int cMP { get; set; } = -1;
        public int cMPFull { get; set; } = -1;
        public int cStamina { get; set; } = -1;
        public long cPower { get; set; } = -1;
        public long cTiemNang { get; set; } = -1;
        public int cHPGoc { get; set; } = -1;
        public int cMPGoc { get; set; } = -1;
        public int cDefGoc { get; set; } = -1;
        public int cCriticalGoc { get; set; } = -1;
        public long xu { get; set; } = -1;
        public int luong { get; set; } = -1;
        public int luongKhoa { get; set; } = -1;

        [LitJSON.JsonSkip]
        public Process process;

        [LitJSON.JsonSkip]
        public Socket workSocket;

        [LitJSON.JsonSkip]
        public Server server => MainWindow.Servers[indexServer];

        [LitJSON.JsonSkip]
        public string planet 
            => cgender == 0 ? "Trái đất" : 
            cgender == 1 ? "Namec" : 
            cgender == 3 ? "Xayda" : "-";

        [LitJSON.JsonSkip]
        public string status { get; set; } = "-";

        [LitJSON.JsonSkip]
        public int number { get; set; }
    }
}
