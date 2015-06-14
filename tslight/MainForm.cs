using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;





namespace tslight
{


	/// Description of MainForm.
	public partial class MainForm : Form
	{
		public string AppDir; // путь к папке приложения
		public string sLogin; // логин пользователя для сервера Transaq
		public string sPassword; // пароль пользователя для сервера Transaq
		public string ServerIP; // IP адрес сервера Transaq
		public string ServerPort; // номер порта сервера Transaq
        public static int z;

        public string sboard; //  идентификатор режима торгов
        public string sseccode; // код инструмента
 

        public int session_timeout;
        public int request_timeout;

        private bool bConnected;
        private bool bConnecting;
		
		public DataSet_tslight DTS; // DataSet для таблиц с данными
        public DataSet_tslight2 DTS2; // DataSet2 для таблиц с данными
        public DataSet_tslight3 DTS3; // DataSet2 для таблиц с данными
        public DataSet_tslight4 DTS4; // DataSet2 для таблиц с данными

		public char PointChar; // символ для замены точки в числах
        private bool PassHide = true; // флаг прячем ли пароль


        private event NewStringDataHandler onNewFormDataEvent;
        private event NewStringDataHandler onNewSecurityEvent;
        private event NewStringDataHandler onNewTimeframeEvent;
        private event NewBoolDataHandler onNewStatusEvent;
        private event NewStringDataHandler onNewquotationEvent;
        private event NewStringDataHandler onNewtradeEvent;
        private event NewStringDataHandler onNewquoteEvent;
        private event NewStringDataHandler onNewFormDataEvent2;
        private event NewStringDataHandler onNewFormDataEvent3;
		//================================================================================
		public MainForm()
		{
			// определение папки, в которой запущена программа
			string path = Application.ExecutablePath;
			AppDir = path.Substring(0, path.LastIndexOf('\\')+1);

			// The InitializeComponent() call is required for Windows Forms designer support.
			InitializeComponent();

			// определение разделителя в числах на компьютере (запятая или точка)
			PointChar = ',';
			string str = (1.2).ToString();
			if (str.IndexOf('.') > 0) PointChar = '.';
		}

		//================================================================================
		void MainFormLoad(object sender, EventArgs e)
		{
          
			// параметры по умолчанию
            ServerIP = "78.41.194.46";
            ServerPort = "3950";
            sLogin = "00000278753";
            sPassword = "NUyNbt";
            sboard = "TQBR";
            sseccode ="GAZP";
            z = 0;

            session_timeout = 25;
            request_timeout = 10;


			edt_Login.Text = sLogin;
			edt_Password.Text = sPassword;
			edt_IP.Text = ServerIP;
			edt_Port.Text = ServerPort;

            bConnected = false;
            bConnecting = false;

            Enable_Password_Controls(false);

            Init_Data();
            Init_Data2();
            Init_Data3();
            Init_Data4();

            // открытие лог-файла
            log.StartLogging(AppDir + "log" + DateTime.Now.ToString("yyMMdd") + ".txt");

            TXmlConnector.statusTimeout = session_timeout * 1000;

            TXmlConnector.ConnectorSetCallback(OnNewFormData, OnNewSecurity, OnNewTimeframe, OnNewStatus, onNewquotation, OnNewtrade, OnNewquote, OnNewFormData2, OnNewFormData3);
            
            this.onNewFormDataEvent += new NewStringDataHandler(Add_FormData);
            this.onNewSecurityEvent += new NewStringDataHandler(Add_Security);
            this.onNewTimeframeEvent += new NewStringDataHandler(Add_Timeframe);
            this.onNewStatusEvent += new NewBoolDataHandler(ConnectionStatusReflect);
            this.onNewquotationEvent += new NewStringDataHandler(Add_quotation);
            this.onNewtradeEvent += new NewStringDataHandler(Add_trade);
             this.onNewquoteEvent += new NewStringDataHandler(Add_quote);
             this.onNewFormDataEvent += new NewStringDataHandler(Add_FormData2);
             this.onNewFormDataEvent += new NewStringDataHandler(Add_FormData3);
            TXmlConnector.FormReady = true;

            string LogPath = AppDir + "\0";

            if (TXmlConnector.ConnectorInitialize(LogPath, 3)) TXmlConnector.statusDisconnected.Set();
           
		}



        private void OnNewFormData(string data)
        {
            this.Invoke(onNewFormDataEvent, new object[] { data }); 
        }
        private void OnNewSecurity(string data)
        {
            this.Invoke(onNewSecurityEvent, new object[] { data }); 
        }

        private void onNewquotation(string data)
        {
            this.Invoke(onNewquotationEvent, new object[] { data });
        }
        private void OnNewTimeframe(string data)
        {
            this.Invoke(onNewTimeframeEvent, new object[] { data }); 
        }
        private void OnNewStatus(bool status)
        {
            this.Invoke(onNewStatusEvent, new object[] { status }); 
        }
        private void OnNewtrade(string data)
        {
            this.Invoke(onNewtradeEvent, new object[] { data });
        }
        private void OnNewquote(string data)
        {
            this.Invoke(onNewquoteEvent, new object[] { data });
        }
        private void OnNewFormData2(string data)
        {
            this.Invoke(onNewFormDataEvent, new object[] { data });
        }
        private void OnNewFormData3(string data)
        {
            this.Invoke(onNewFormDataEvent, new object[] { data });
        }

		//================================================================================
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
            TXmlConnector.FormReady = false;

            if (bConnected || bConnecting)
			{
				Transaq_Disconnect();
			}

            TXmlConnector.ConnectorUnInitialize();

            log.StopLogging();
		}


       


		//================================================================================
		public void ShowStatus(string status_str)
		{
			// вывод сообщения в строке статуса формы
			txt_Status.Text = status_str;
			txt_Status.Refresh();
		}
		
		//================================================================================
		public void Init_Data()
		{
			// создание объекта DataSet с таблицами 
			DTS = new DataSet_tslight();
		}

        public void Init_Data2()
        {
            // создание объекта DataSet с таблицами 
            DTS2 = new DataSet_tslight2();
        }

        public void Init_Data3()
        {
            // создание объекта DataSet с таблицами 
            DTS3 = new DataSet_tslight3();
        }
        public void Init_Data4()
        {
            // создание объекта DataSet с таблицами 
            DTS4 = new DataSet_tslight4();
        }
		//================================================================================
		void Transaq_Connect()
		{

			// чтение параметров из формы
			sLogin = edt_Login.Text;
			sPassword = edt_Password.Text;
			ServerIP = edt_IP.Text;
			ServerPort = edt_Port.Text;
			
			// проверка наличия параметров
			if (sLogin.Length == 0)
			{
				ShowStatus("Не указан логин");
				return;
			}
			if (sPassword.Length == 0)
			{
				ShowStatus("Не указан пароль");
				return;
			}
			if (ServerIP.Length == 0)
			{
				ShowStatus("Не указан IP-адрес");
				return;
			}
			if (ServerPort.Length == 0)
			{
				ShowStatus("Не указан порт");
				return;
			}


            ConnectingReflect();

			// очистка таблиц с данными
			DTS.t_timeframe.Clear();
			DTS.t_security.Clear();
            DTS.t_candle.Clear();

   

			
			// формирование текста команды
			string cmd = "<command id=\"connect\">";
			cmd = cmd + "<login>" + sLogin + "</login>";
			cmd = cmd + "<password>" + sPassword + "</password>";
			cmd = cmd + "<host>" + ServerIP + "</host>";
			cmd = cmd + "<port>" + ServerPort + "</port>";
			cmd = cmd + "<logsdir>" + AppDir + "</logsdir>";
            cmd = cmd + "<rqdelay>100</rqdelay>";
            cmd = cmd + "<session_timeout>" + session_timeout.ToString() + "</session_timeout> ";
            cmd = cmd + "<request_timeout>" + request_timeout.ToString() + "</request_timeout>"; 
			cmd = cmd + "</command>";

			// отправка команды
            log.WriteLog("SendCommand: <command id=\"connect\"><login>" + sLogin + "</login><password>*</password><host>" + ServerIP + "</host><port>" + ServerPort + "</port><logsdir>" + AppDir + "</logsdir><rqdelay>100</rqdelay></command>");
            TXmlConnector.statusDisconnected.Reset();
            string res = TXmlConnector.ConnectorSendCommand(cmd);
            log.WriteLog("ServerReply: " + res);
			
		}

		//================================================================================
		void Transaq_Disconnect()
		{
			// отключение коннектора от сервера Транзак


            DisconnectingReflect();
			
			string cmd = "<command id=\"disconnect\"/>";

            log.WriteLog("SendCommand: " + cmd);
            TXmlConnector.statusDisconnected.Reset();
            string res = TXmlConnector.ConnectorSendCommand(cmd);
            log.WriteLog("ServerReply: " + res);

		}
		
		//================================================================================
		void Get_Transaq_History(int SecurityID, int TimeframeID, int HistoryLength, bool ResetFlag)
		{
			// запрос исторических данных для инструмента
			string cmd = "<command id=\"gethistorydata\" ";
			cmd = cmd + "secid=\"" + SecurityID.ToString() + "\" ";
			cmd = cmd + "period=\"" + TimeframeID.ToString() + "\" ";
			cmd = cmd + "count=\"" + HistoryLength.ToString() + "\" ";
			string s = "false";
			if (ResetFlag) s = "true";
			cmd = cmd + "reset=\""+s+"\"/>";

            log.WriteLog("SendCommand: " + cmd);
            string res = TXmlConnector.ConnectorSendCommand(cmd);
            log.WriteLog("ServerReply: " + res);
        }


        void ConnectingReflect()
        {
            bConnecting = true;
            txt_Connect.Text = "connecting";
            btn_Connect.Text = "Подключаю";
            btn_Connect.Refresh();
            txt_Connect.Refresh();
            ShowStatus("Подключение к серверу...");
         }

        void DisconnectingReflect()
        {
            bConnecting = false;
            txt_Connect.Text = "disconnecting";
            btn_Connect.Text = "Отключаю";
            btn_Connect.Refresh();
            txt_Connect.Refresh();
            ShowStatus("Отключение от сервера...");
        }

            
		//================================================================================
		void ConnectionStatusReflect(bool connected)
		{
			// отображение состояния подключения на форме

            bConnected = connected; 
            bConnecting = false;

            if (connected)
                {
                    dg_Security.DataSource = DTS.t_security;
                    dg_Security.Refresh();
                    dg_quotation.DataSource = DTS2.t_quotation;
                    dg_quotation.Refresh();

                    txt_Connect.Text = "online";
                    btn_Connect.Text = "Отключить";
                    ShowStatus("Подключение установлено");
                    Enable_Password_Controls(true);
                    
                }
                else
                {

                    DTS.t_timeframe.Clear();
                    DTS.t_security.Clear();
                    DTS.t_candle.Clear();
       
                    dg_Security.Refresh();

                    txt_Connect.Text = "offline";
			        btn_Connect.Text  = "Подключить";
			        ShowStatus("Подключение разъединено");
                    Enable_Password_Controls(false);
                }

 
			btn_Connect.Refresh();
			txt_Connect.Refresh();
		}

        public void Add_FormData(string data)
        {
            txtBoxAns.Text = txtBoxAns.Text + DateTime.Now.ToString("HH:mm:ss.fff") + "   " + data + "\n =================================================== \n";
        }




        public void Add_FormData2(string dota2)
        {
            txtBoxAns2.Text = TXmlConnector.dota2 + "\n =================================================== \n"; 
        }

        public void Add_FormData3(string dota3)
        {
            txtBoxAns3.Text = TXmlConnector.dota3 + "\n =================================================== \n";
        }



		//================================================================================
		public void Add_Timeframe(string data)
		{
			// добавление записи о таймфрейме
			string[] tf = data.Split(';');
			if (tf.Length < 3) return;
			
			int id = int.Parse(tf[0]);
			int length = int.Parse(tf[1]);
			string name = tf[2];
			
			DTS.t_timeframe.Add_Row(id, length, name);
		}
		
		//================================================================================
		 void Add_Security(string data)
		{
			// добавление записи об инструменте
			string[] sec = data.Split(';');
			if (sec.Length < 16) return;
			
			int id = int.Parse(sec[0]);
            bool active = bool.Parse(sec[1]);
			string code = sec[2];
            string board = sec[3];
            string secname = sec[4];
            int decimals = int.Parse(sec[5]);
            int market = int.Parse(sec[6]);
            string sectype = sec[7];
            bool usecredit = bool.Parse(sec[8].Replace("yes", "true").Replace("no","false"));
            bool bymarket = bool.Parse(sec[9].Replace("yes", "true").Replace("no", "false"));
            bool nosplit = bool.Parse(sec[10].Replace("yes", "true").Replace("no", "false"));
            bool immorcancel = bool.Parse(sec[11].Replace("yes", "true").Replace("no", "false"));
            bool cancelbalance = bool.Parse(sec[12].Replace("yes", "true").Replace("no", "false"));
            double minstep = double.Parse(sec[13].Replace('.', PointChar));
			int lotsize = int.Parse(sec[14]);
            double pointcost = double.Parse(sec[15].Replace('.', PointChar));
			
			// ограничимся инструментами ММВБ, только акции
            if (market == 1 && active && sectype == "SHARE") 
			{
                DTS.t_security.Add_Row(id, secname, code, board, lotsize, minstep);
			}
		}


         void Add_quotation(string data)
         {
      
             // добавление записи об инструменте
             string[] sec = data.Split(';');
             if (sec.Length < 26) return;

             int id = int.Parse(sec[0]);
             string board = sec[1];
             string code = sec[2];
             double pointcost = double.Parse(sec[3].Replace('.', PointChar));
             string status = sec[4];
             string trandingstatus = sec[5];
             double last = double.Parse(sec[6].Replace('.', PointChar));
             //int quantity = int.Parse(sec[7]);
            //bool time = sec[8];
             double change = double.Parse(sec[9].Replace('.', PointChar));
             double priceminusprevwaprice = double.Parse(sec[10].Replace('.', PointChar));
             double bid = double.Parse(sec[11].Replace('.', PointChar));
             int biddepth = int.Parse(sec[12]);
             int biddeptht = int.Parse(sec[13]);
             int numbids = int.Parse(sec[14]);
             double offer = double.Parse(sec[15].Replace('.', PointChar));
             int offerdepth = int.Parse(sec[16]);
             int offerdeptht = int.Parse(sec[17]);
             int numoffers = int.Parse(sec[18]);
             double open = double.Parse(sec[19].Replace('.', PointChar));
             double high = double.Parse(sec[20].Replace('.', PointChar));
             double low = double.Parse(sec[21].Replace('.', PointChar));
             double highbid = double.Parse(sec[22].Replace('.', PointChar));
             double lowoffer = double.Parse(sec[23].Replace('.', PointChar));
             int voltoday = int.Parse(sec[24]);
             int numtrades = int.Parse(sec[25]);
             double valtoday = double.Parse(sec[26].Replace('.', PointChar));
             double waprice = double.Parse(sec[27].Replace('.', PointChar));
             double marketpricetoday = double.Parse(sec[28].Replace('.', PointChar));

      
         

             DTS2.t_quotation.Add_Row(id, code, open, biddepth, numbids, last);

         }



                   void Add_trade(string data)
            {
                /*    // добавление записи об инструменте
                     string[] sec = data.Split(';');
                     if (sec.Length < 10) return;
                     int id = int.Parse(sec[0]);
                     int tradeno = int.Parse(sec[1]);
                     string board = sec[2];
                     DateTime time = DateTime.Parse(sec[3]);
                     double price = double.Parse(sec[4].Replace('.', PointChar));
                     int quantity = int.Parse(sec[5]);
                     string buysell = sec[6];
                     int openinterest = int.Parse(sec[7]);
                     string seccode = sec[8];
                     string period = sec[9];

                     DTS3.t_trade.Add_Row(id, time, price, quantity, buysell, period);*/
            } 





         void Add_quote(string data)
         {
             // добавление записи об инструменте
             string[] sec = data.Split(';');
             if (sec.Length < 7) return;


             int id = int.Parse(sec[0]);
             string board = sec[1];
             string seccode = sec[2];
             double price = double.Parse(sec[3].Replace('.', PointChar));
             int yield = int.Parse(sec[4]);
             int buy = int.Parse(sec[5]);
             double source = double.Parse(sec[3].Replace('.', PointChar));////////его нету

             DTS4.t_quote.Add_Row(id, board, price, source, yield, buy);

         }


         //стакан цен отправка команд
         void Transaq_Connect2()
         {

             // формирование текста команды
             string cmd = "<command id=\"subscribe\">";
             cmd = cmd + "<alltrades>";
             cmd = cmd + "<security>";
             cmd = cmd + "<board>" + sboard + "</board>";
             cmd = cmd + "<seccode>" + sseccode + "</seccode>";
             cmd = cmd + "</security>";
             cmd = cmd + "</alltrades>";
             cmd = cmd + "<quotations>";
             cmd = cmd + "<security>";
             cmd = cmd + "<board>" + sboard + "</board>";
             cmd = cmd + "<seccode>" + sseccode + "</seccode>";
             cmd = cmd + "</security>";
             cmd = cmd + "</quotations>";
             cmd = cmd + "<quotes>";
             cmd = cmd + "<security>";
             cmd = cmd + "<board>" +  sboard + "</board>";
             cmd = cmd + "<seccode>" + sseccode + "</seccode>";
             cmd = cmd + "</security>";
             cmd = cmd + "</quotes>";
             cmd = cmd + "</command>";


             // отправка команды
             log.WriteLog("SendCommand: <command id=\"subscribe\"><alltrades><security><board>" + sboard + "</board><seccode>" + sseccode + "</seccode></security></alltrades><quotations><security><board>" + sboard + "</board><seccode>" + sseccode + "<seccode></security></quotations><quotes><security><board>" + sboard + "</board><seccode>" + sseccode + "</seccode></security></quotes></command>");
             TXmlConnector.statusDisconnected.Reset();
             string res = TXmlConnector.ConnectorSendCommand(cmd);
             log.WriteLog("ServerReply: " + res);

         }

		//================================================================================
		void btn_Connect_Click(object sender, EventArgs e)
		{
            
            // если подключен или подключается
            if (bConnected || bConnecting)
			{
				Transaq_Disconnect();
			}
			else
			{
                if (!bConnecting) Transaq_Connect();
			}

         


			
		}

        //=================================== стакан
        private void button1_Click(object sender, EventArgs e)
        {
           
            Transaq_Connect2();
         
        }


        void Enable_Password_Controls(bool bEnable)
        {

            // установка состояния элементов управления для смены пароля
            txtOldPass.Enabled = bEnable;
            txtNewPass.Enabled = bEnable;
            checkHide.Enabled = bEnable;
            btnPassChange.Enabled = bEnable;
        }

        private void btnPassChange_Click(object sender, EventArgs e)
        {
            // Проверяем пароль
            // Правило: только латинские буквы и цифры, минимум 4, максимум 19
            string passPattern = @"^[a-zA-Z0-9]{4,20}$";

            string oldPass = txtOldPass.Text;
            string newPass = txtNewPass.Text;

            string result;
            Match m = Regex.Match(newPass, passPattern);

            if (!bConnected)
            {
                ShowStatus("Для смены пароля подключитесь к серверу");
            }
            else if (txtNewPass.Text.Length == 0)
            {
                ShowStatus("Введите новый пароль");
            }
            else if (m.Success)
            {
                string cmd = String.Format("<command id=\"change_pass\" oldpass=\"{0}\" newpass=\"{1}\" />", oldPass, newPass);
                log.WriteLog("SendCommand: <command id=\"change_pass\" oldpass=\"*\" newpass=\"*\" />");
                result = TXmlConnector.ConnectorSendCommand(cmd);
                log.WriteLog("ServerReply: " + result);
                ShowStatus(result);
                XDocument aXmlDoc = XDocument.Load(new System.IO.StringReader(result));
                if (aXmlDoc.Root.Name == "result")
                {
                    if (aXmlDoc.Root.Attribute("success").Value == "true")
                    {
                        ShowStatus("Пароль изменен");
                        log.WriteLog("Password was changed");
                        txtOldPass.Text = "";
                        txtNewPass.Text = "";
                    }
                    else if (aXmlDoc.Root.Attribute("success").Value == "false")
                    {
                        ShowStatus(aXmlDoc.Root.Value);
                    }
                }
                else
                {
                    ShowStatus("Произошла ошибка");
                }
            }
            else
            {
                ShowStatus("Введите верный новый пароль");
            }

        }

        private void checkHide_CheckedChanged(object sender, EventArgs e)
        {
            PassHide = checkHide.Checked;
            if (PassHide)
            {
                txtNewPass.PasswordChar = '*';
            }
            else
            {
                txtNewPass.PasswordChar = (char)0;
            }

        }

        private void lbl_Login_Click(object sender, EventArgs e)
        {

        }

        private void dg_Security_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_quotation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_quotation_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //================================================================================

	}
}
