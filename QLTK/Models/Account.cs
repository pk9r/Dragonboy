using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;

namespace QLTK.Models
{
    public class Account : INotifyPropertyChanged
    {
        private string _status;
        private string _username;
        private string _password;
        private int _indexServer = 0;
        private string _cName = "-";
        private int _cgender = -1;
        private string _mapName = "Chưa xác định";
        private int _mapID = -1;
        private int _zoneID = -1;
        private int _cx = -1;
        private int _cy = -1;
        private int _cHP = -1;
        private int _cHPFull = -1;
        private int _cMP = -1;
        private int _cMPFull = -1;
        private int _cStamina = -1;
        private long _cPower = -1;
        private long _cTiemNang = -1;
        private int _cHPGoc = -1;
        private int _cMPGoc = -1;
        private int _cDefGoc = -1;
        private int _cCriticalGoc = -1;
        private int _cPetHP = -1;
        private int _cPetHPFull = -1;
        private int _cPetMP = -1;
        private int _cPetMPFull = -1;
        private int _cPetStamina = -1;
        private long _cPetPower = -1;
        private long _cPetTiemNang = -1;
        private long _xu = -1;
        private int _luong = -1;
        private int _luongKhoa = -1;

        public string username
        {
            get => this._username;
            set { this._username = value; this.OnPropertyChanged("username"); }
        }
        public string password
        {
            get => this._password;
            set { this._password = value; this.OnPropertyChanged("password"); }
        }
        public int indexServer
        {
            get => this._indexServer;
            set { this._indexServer = value; this.OnPropertyChanged("indexServer"); this.OnPropertyChanged("server"); }
        }
        public string cName
        {
            get => this._cName;
            set { this._cName = value; this.OnPropertyChanged("cName"); }
        }
        public int cgender
        {
            get => this._cgender;
            set { this._cgender = value; this.OnPropertyChanged("cgender"); this.OnPropertyChanged("planet"); }
        }
        public string mapName
        {
            get => this._mapName;
            set { this._mapName = value; this.OnPropertyChanged("mapName"); }
        }
        public int mapID
        {
            get => this._mapID;
            set { this._mapID = value; this.OnPropertyChanged("mapID"); }
        }
        public int zoneID
        {
            get => this._zoneID;
            set { this._zoneID = value; this.OnPropertyChanged("zoneID"); }
        }
        public int cx
        {
            get => this._cx;
            set { this._cx = value; this.OnPropertyChanged("cx"); }
        }
        public int cy
        {
            get => this._cy;
            set { this._cy = value; this.OnPropertyChanged("cy"); }
        }
        public int cHP
        {
            get => this._cHP;
            set { this._cHP = value; this.OnPropertyChanged("cHP"); }
        }
        public int cHPFull
        {
            get => this._cHPFull;
            set { this._cHPFull = value; this.OnPropertyChanged("cHPFull"); }
        }
        public int cMP
        {
            get => this._cMP;
            set { this._cMP = value; this.OnPropertyChanged("cMP"); }
        }
        public int cMPFull
        {
            get => this._cMPFull;
            set { this._cMPFull = value; this.OnPropertyChanged("cMPFull"); }
        }
        public int cStamina
        {
            get => this._cStamina;
            set { this._cStamina = value; this.OnPropertyChanged("cStamina"); }
        }
        public long cPower
        {
            get => this._cPower;
            set { this._cPower = value; this.OnPropertyChanged("cPower"); }
        }
        public long cTiemNang
        {
            get => this._cTiemNang;
            set { this._cTiemNang = value; this.OnPropertyChanged("cTiemNang"); }
        }
        public int cHPGoc
        {
            get => this._cHPGoc;
            set { this._cHPGoc = value; this.OnPropertyChanged("cHPGoc"); }
        }
        public int cMPGoc
        {
            get => this._cMPGoc;
            set { this._cMPGoc = value; this.OnPropertyChanged("cMPGoc"); }
        }
        public int cDefGoc
        {
            get => this._cDefGoc;
            set { this._cDefGoc = value; this.OnPropertyChanged("cDefGoc"); }
        }
        public int cCriticalGoc
        {
            get => this._cCriticalGoc;
            set { this._cCriticalGoc = value; this.OnPropertyChanged("cCriticalGoc"); }
        }
        public int cPetHP
        {
            get => this._cPetHP;
            set { this._cPetHP = value; this.OnPropertyChanged("cPetHP"); }
        }
        public int cPetHPFull
        {
            get => this._cPetHPFull;
            set { this._cPetHPFull = value; this.OnPropertyChanged("cPetHPFull"); }
        }
        public int cPetMP
        {
            get => this._cPetMP;
            set { this._cPetMP = value; this.OnPropertyChanged("cPetMP"); }
        }
        public int cPetMPFull
        {
            get => this._cPetMPFull;
            set { this._cPetMPFull = value; this.OnPropertyChanged("cPetMPFull"); }
        }
        public int cPetStamina
        {
            get => this._cPetStamina;
            set { this._cPetStamina = value; this.OnPropertyChanged("cPetStamina"); }
        }
        public long cPetPower
        {
            get => this._cPetPower;
            set { this._cPetPower = value; this.OnPropertyChanged("cPetPower"); }
        }
        public long cPetTiemNang
        {
            get => this._cPetTiemNang;
            set { this._cPetTiemNang = value; this.OnPropertyChanged("cPetTiemNang"); }
        }
        public long xu
        {
            get => this._xu;
            set { this._xu = value; this.OnPropertyChanged("xu"); }
        }
        public int luong
        {
            get => this._luong;
            set { this._luong = value; this.OnPropertyChanged("luong"); }
        }
        public int luongKhoa
        {
            get => this._luongKhoa;
            set { this._luongKhoa = value; this.OnPropertyChanged("luongKhoa"); }
        }

        [LitJSON.JsonSkip]
        public Process process;

        [LitJSON.JsonSkip]
        public Socket workSocket;

        [LitJSON.JsonSkip]
        public Server server => MainWindow.Servers[this.indexServer];

        [LitJSON.JsonSkip]
        public string planet
            => this.cgender == 0 ? "Trái đất" :
            this.cgender == 1 ? "Namec" :
            this.cgender == 3 ? "Xayda" : "-";

        [LitJSON.JsonSkip]
        public string status
        {
            get => this._status;
            set { this._status = value; this.OnPropertyChanged("status"); }
        }

        [LitJSON.JsonSkip]
        public int number { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
