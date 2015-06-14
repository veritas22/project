using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;



using System.Runtime.InteropServices;


namespace tslight
{
    class DoubleComparerDesc : IComparer<Double>
    {
        public int Compare(Double left, Double right)
        {
            return left.CompareTo(right);
        }
    }

    class DoubleComparerAsc : IComparer<Double>
    {
        public int Compare(Double left, Double right)
        {
            return right.CompareTo(left);
        }
    }
	static class MarshalUTF8
	{
		private static UTF8Encoding _utf8;

		//--------------------------------------------------------------------------------
		static MarshalUTF8()
		{
			_utf8 = new UTF8Encoding();
		}

		//--------------------------------------------------------------------------------
		public static IntPtr StringToHGlobalUTF8(String data)
		{
            Byte[] dataEncoded = _utf8.GetBytes(data + "\0");

			int size = Marshal.SizeOf(dataEncoded[0]) * dataEncoded.Length;

			IntPtr pData = Marshal.AllocHGlobal(size);

			Marshal.Copy(dataEncoded, 0, pData, dataEncoded.Length);

			return pData;
		}

     
       

		//--------------------------------------------------------------------------------
		public static String PtrToStringUTF8(IntPtr pData)
		{
			// this is just to get buffer length in bytes
			String errStr = Marshal.PtrToStringAnsi(pData);
			int length = errStr.Length;

			Byte[] data = new byte[length];
			Marshal.Copy(pData, data, 0, length);

			return _utf8.GetString(data);
		}
		//--------------------------------------------------------------------------------
	}

    delegate void NewStringDataHandler(string data);
    delegate void NewBoolDataHandler(bool data);


	static class TXmlConnector
	{
		const String EX_SETTING_CALLBACK = "Не смог установить функцию обратного вызова";
      
        public static string dota2;
        public static string dota3;
        delegate bool CallBackDelegate(IntPtr pData);
        //delegate bool CallBackExDelegate(IntPtr pData, IntPtr userData);
		
		static readonly CallBackDelegate myCallbackDelegate =  new CallBackDelegate(myCallBack);
        static readonly GCHandle callbackHandle = GCHandle.Alloc(myCallbackDelegate);

        //static readonly CallBackExDelegate myCallbackExDelegate = new CallBackExDelegate(myCallBackEx);
        //static readonly GCHandle callbacExkHandle = GCHandle.Alloc(myCallbackExDelegate);

        private static bool bConnected; // флаг наличия подключения к серверу
        
        public static AutoResetEvent statusDisconnected = new AutoResetEvent(true);
        public static int statusTimeout;

      
        public static bool FormReady;
        

        static NewStringDataHandler send_new_form_data;
        static NewStringDataHandler send_new_security;
        static NewStringDataHandler send_new_timeframe;
        static NewBoolDataHandler send_new_status;
        static NewStringDataHandler send_new_quotation;
        static NewStringDataHandler send_new_trade;
        static NewStringDataHandler send_new_quote;
        static NewStringDataHandler send_new_form_data2;
        static NewStringDataHandler send_new_form_data3;
        public static void ConnectorSetCallback(NewStringDataHandler new_form_data
                                                , NewStringDataHandler new_security
                                                , NewStringDataHandler new_timeframe
                                                , NewBoolDataHandler new_status
                                            , NewStringDataHandler new_quotation
                                               , NewStringDataHandler new_trade
                                              , NewStringDataHandler new_quote
                                                  , NewStringDataHandler new_form_data2
                                                   , NewStringDataHandler new_form_data3)
        {


            if (!SetCallback(myCallbackDelegate))
            {
                throw (new Exception(EX_SETTING_CALLBACK));
            }

            //if (!SetCallbackEx(myCallbackExDelegate, IntPtr.Zero))
            //{
            //    throw (new Exception(EX_SETTING_CALLBACK));
            //}

            TXmlConnector.send_new_form_data = new_form_data;
            TXmlConnector.send_new_security = new_security;
            TXmlConnector.send_new_timeframe = new_timeframe;
            TXmlConnector.send_new_status = new_status;
            TXmlConnector.send_new_quotation = new_quotation;
            TXmlConnector.send_new_trade = new_trade;
            TXmlConnector.send_new_quote = new_quote;
            TXmlConnector.send_new_form_data2= new_form_data2;
            TXmlConnector.send_new_form_data3= new_form_data3;
        }


		//--------------------------------------------------------------------------------
        static bool myCallBack(IntPtr pData)
		{
            string res;
			String data = MarshalUTF8.PtrToStringUTF8(pData);
			FreeMemory(pData);

            res = Transaq_HandleData(data);
            if (res == "server_status") New_Status();
            return true;
		}

        //static bool myCallBackEx(IntPtr pData, IntPtr userData)
        //{
        //    String data = MarshalUTF8.PtrToStringUTF8(pData);
        //    FreeMemory(pData);

        //    Transaq_HandleData(data);
        //    //	DataList.Add(data);
        //    return true;
        //}

		//--------------------------------------------------------------------------------

 
        static void Form_AddText(string stringData)
        {
            if (FormReady)
                send_new_form_data.BeginInvoke(stringData, null, null);
        }

        static void On_New_Security(string stringData)
        {
            if (FormReady) 
                send_new_security.BeginInvoke(stringData, null, null);
        }

        static void On_New_quotation(string stringData)
        {
            if (FormReady)
                send_new_quotation.BeginInvoke(stringData, null, null);
        }
        static void On_New_trade(string stringData)
        {
            if (FormReady)
                send_new_trade.BeginInvoke(stringData, null, null);
        }
        static void On_New_quote(string stringData)
        {
            if (FormReady)
                send_new_quote.BeginInvoke(stringData, null, null);
        }



        static void On_New_Timeframe(string stringData)
        {
            if (FormReady) 
                send_new_timeframe.BeginInvoke(stringData, null, null);
        }
 
        static void New_Status()
        {
            if (FormReady) 
                send_new_status.BeginInvoke(bConnected, null, null);
            if (bConnected)
            {
                statusDisconnected.Reset();
            }
            else
            {
                statusDisconnected.Set();
            }
        }


        
		//--------------------------------------------------------------------------------
        public static String ConnectorSendCommand(String command)
		{

			IntPtr pData = MarshalUTF8.StringToHGlobalUTF8(command);
			IntPtr pResult = SendCommand(pData);

			String result = MarshalUTF8.PtrToStringUTF8(pResult);

            

			Marshal.FreeHGlobal(pData);
			FreeMemory(pResult);

			return result;

		}


        public static bool ConnectorInitialize(String Path, Int16 LogLevel)
        {

            IntPtr pPath = MarshalUTF8.StringToHGlobalUTF8(Path);
            IntPtr pResult = Initialize(pPath, LogLevel);

            if (!pResult.Equals(IntPtr.Zero))
            {
                String result = MarshalUTF8.PtrToStringUTF8(pResult);
                Marshal.FreeHGlobal(pPath);
                FreeMemory(pResult);
                log.WriteLog(result);
                return false;
            }
            else
            {
                Marshal.FreeHGlobal(pPath);
                log.WriteLog("Initialize() OK");
                return true;
            }

        }


        public static void ConnectorUnInitialize()
        {

            if (statusDisconnected.WaitOne(statusTimeout))
            {
                IntPtr pResult = UnInitialize();

                if (!pResult.Equals(IntPtr.Zero))
                {
                    String result = MarshalUTF8.PtrToStringUTF8(pResult);
                    FreeMemory(pResult);
                    log.WriteLog(result);
                }
                else
                {
                    log.WriteLog("UnInitialize() OK");
                }
            }
            else
            {
                log.WriteLog("WARNING! Не дождались статуса disconnected. UnInitialize() не выполнено.");
            }

        }


        //================================================================================
        public static string Transaq_HandleData(string data)
        {
            // обработка данных, полученных коннектором от сервера Транзак
            string sTime = DateTime.Now.ToString("HH:mm:ss.fff");
            string info = "";
    
            // включить полученные данные в строку вывода в лог-файл
            string textForWindow = data;
            log.WriteLog("ServerData: " + data);
            

            XmlReaderSettings xs = new XmlReaderSettings();
            xs.IgnoreWhitespace = true;
            xs.ConformanceLevel = ConformanceLevel.Fragment;
            xs.ProhibitDtd = false;
            XmlReader xr = XmlReader.Create(new System.IO.StringReader(data), xs);
            string section = "";
            string line = "";
            char PointCha;
          
            double pri = 0.0;
            int eva = 0;
     
            var rand = new Random();
            string str = "";
            string ename = "";
            string evalue="" ;
            string attr = "";
            PointCha = ',';
            double[,] myArr =new double[10, 1];
            double[,] myArr2 = new double[10, 10];
            
                SortedDictionary<Double, int> sellPositions = new SortedDictionary<double, int>(new DoubleComparerDesc());
                SortedDictionary<Double, int> buyPositions = new SortedDictionary<double, int>(new DoubleComparerAsc());
            
         
            //string values = "";

            // обработка "узлов" 
            while (xr.Read())
            {
                switch (xr.NodeType)
                {   
                    case XmlNodeType.Element:
                    case XmlNodeType.EndElement:
                        ename = xr.Name; break;
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Comment:
                    case XmlNodeType.XmlDeclaration:
                        evalue = xr.Value; break;
                    case XmlNodeType.DocumentType:
                        ename = xr.Name; evalue = xr.Value; break;
                    default: break;
                }

                //................................................................................
                // определяем узел верхнего уровня - "секцию"
                if (xr.Depth == 0)
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        section = ename;

                        if ((section != "boards") && (section != "securities") && (section != "pits") && (section != "sec_info_upd") && (section != "quotations") && (section != "alltrades") && (section != "quotes") && (textForWindow.Length > 0))
                        {
                            Form_AddText(textForWindow);
                            textForWindow = "";
                        }

                        line = "";
                        str = "";
                        for (int i = 0; i < xr.AttributeCount; i++)
                        {
                            str = str + xr.GetAttribute(i) + ";";
                        }
                    }
                    if (xr.NodeType == XmlNodeType.EndElement)
                    {
                        //line = "";
                        //section = "";
                    }
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        str = str + evalue + ";";
                    }
                }
                //................................................................................
                // данные для рынков
                if (section == "markets")
                {
                    //xe = (XElement)XNode.ReadFrom(xr);

                    if (ename == "market")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add market: " + str;
                            str = "";
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }
                //................................................................................
                // данные для таймфреймов
                if (section == "candlekinds")
                {
                    if (ename == "kind")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add kind: " + str;
                            On_New_Timeframe(str);
                            str = "";
                        }
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }
                //................................................................................
                // данные для инструментов
                if (section == "securities")
                {
                    if (ename == "security")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add security: " + str;
                            On_New_Security(str);
                            str = "";
                        }
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }
                //................................................................................
                // котировки по инструментам
                if (section == "quotations")
                {
                    if (ename == "quotation")
                    {





                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add quotation: " + str;
                            Form_AddText(textForWindow);
                            On_New_quotation(str);
                            str = "";
                        }
                       

                      
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                
                    
                       
                    



                    
                }
                //................................................................................

            
                // данные для инструментов
                if (section == "alltrades")
                {
                    if (ename == "trade")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add trade: " + str;
                            Form_AddText(textForWindow);

                            On_New_trade(str);
                            str = "";
                        }
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }
                //................................................................................
                // котировки по инструментам
                if (section == "quotes")
                {
                   
                    if (ename == "quote")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "quote: " + str;
                            Form_AddText(textForWindow);

                            On_New_quote(str);
                            str = "";
                        }
                        

                        
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }

                        if (ename == "price" && xr.NodeType == XmlNodeType.Text)
                        {

                           
                            pri = double.Parse(evalue.Replace('.', PointCha));
                           
                        }

                        if (ename == "buy" && xr.NodeType == XmlNodeType.Text)
                        {
                            eva = int.Parse(evalue.Replace('.', PointCha));
                            if (buyPositions.ContainsKey(pri))
                            {
                                buyPositions.Remove(pri);
                            }
                            buyPositions.Add(pri, eva);
                            
                            var sortDict2 = buyPositions.OrderByDescending(y => y.Value);
                               foreach (KeyValuePair<Double, int> kvp in sortDict2)
                                {
                                    string dt2 = "";
                                     dt2= kvp.Key + "  "  + kvp.Value + '\n';
                                    dota3 = dota3 + '\n' + dt2;

                                  }
                         /*         myArr[x, 0] = d.Key;
                                myArr[x, 1] = d.Value;
                                x++;
                            }

                           int x =0;x++ ;
                            myArr[x, 0] =eva;
                            myArr[x, 1] = pri; массиы для красоты*/
                            }
                        }
                        if (ename == "sell" && xr.NodeType == XmlNodeType.Text) 
                        {

                            eva = int.Parse(evalue.Replace('.', PointCha));
                            if (sellPositions.ContainsKey(pri))
                            {
                            sellPositions.Remove(pri);}
                            sellPositions.Add(pri, eva);
                            
                       /*     MainForm.z++;
                            if ( MainForm.z == 3)
                            {
                                
                            foreach (var d in sellPositions)
                            {
                                double u = d.Key;
                                int w = d.Value;
                        * 
                            }*/
                       
                                var sortDict = sellPositions.OrderByDescending(x => x.Value);
                                foreach (KeyValuePair<Double, int> kvp in sortDict)
                                {
                                    string dt2 = "";
                                     dt2 = kvp.Key + "  "  + kvp.Value + '\n';
                                    dota2 = dota2 + '\n' + dt2;
                             
                                }
                               
                            
                            /*        var keys = sellPositions.Keys;
                            foreach (Double p in keys)
                            {
                                int val = 0;
                               if (sellPositions.TryGetValue(p, out val))
                                {
                             * System.Console.WriteLine("For key = {0}, value = {1}.", p, val);
                            http://www.cyberforum.ru/csharp-beginners/thread883169.html     
                                } запутался бывает))))  
                            /*int x = 0; x++;
                            int y = 0;
                            myArr[x, y] = Convert.ToDouble(eva);
                            y = 1;
                            myArr[x, y] = Convert.ToChar(pri);*/
                       }

                        

                    
                }
                //................................................................................



                // данные по свечам
                if (section == "candles")
                {
                    if (ename == "candles")
                    {

                    }
                    if (ename == "candle")
                    {

                    }
                }
                //................................................................................




                // данные по клиенту
                if (section == "client")
                {
                    if (ename == "client")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (int i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                            // определение параметров клиента
                            //string[] с_attrs = str.Split(';');
                            //if (с_attrs.Length > 0)
                            //{
                            //    ClientCode = с_attrs[0];
                            //}
                            line = "add client: " + str;
                        }
                    }
                    else
                    {
                        line = "";
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                            line = "set: " + ename + "=" + evalue;

                        }
                    }
                }
                //................................................................................
                // данные для позиций
                if (section == "positions")
                {
                    line = "";
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        line = ename + ": " + evalue;
                    }
                }
                //................................................................................
                if (section == "overnight")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        line = "";
                        str = "";
                        for (int i = 0; i < xr.AttributeCount; i++)
                        {
                            str = str + "<" + xr.GetAttribute(i) + ">;";
                        }
                        line = "set overnight status: " + str;
                    }
                }
                //................................................................................
                // данные о статусе соединения с сервером
                if (section == "server_status")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        line = "";
                        str = "";
                        string attr_connected = xr.GetAttribute("connected");
                        if (attr_connected == "true") bConnected = true;
                        if (attr_connected == "false") bConnected = false;

                        for (int i = 0; i < xr.AttributeCount; i++)
                        {
                            attr = xr.GetAttribute(i);
                            str = str + i.ToString() + ":<" + attr + ">;";
                        }
                        line = "set server_status: " + str;
                    }

                }

                if (section == "orders") //обрабатываем заявки
                {

                }

                if (section == "alltrades")
                {

                }
                if (section == "ticks")
                {

                }

                //................................................................................
                if (line.Length > 0)
                {
                    //line = new string(' ',xr.Depth*2) + line;
                    if (info.Length > 0) info = info + (char)13 + (char)10;
                    info = info + line;
                }
            }
            if (info.Length > 0) log.WriteLog(info);


           




            return section;
            // вывод дополнительной информации для удобства отладки
        }


 
		//--------------------------------------------------------------------------------
		// файл библиотеки TXmlConnector.dll должен находиться в одной папке с программой

        [DllImport("txmlconnector.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetCallback(CallBackDelegate pCallback);

        //[DllImport("txmlconnector.dll", CallingConvention = CallingConvention.StdCall)]
        //private static extern bool SetCallbackEx(CallBackExDelegate pCallbackEx, IntPtr userData);

        [DllImport("txmlconnector.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr SendCommand(IntPtr pData);

        [DllImport("txmlconnector.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern bool FreeMemory(IntPtr pData);

        [DllImport("TXmlConnector.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr Initialize(IntPtr pPath, Int32 logLevel);

        [DllImport("TXmlConnector.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr UnInitialize();

        [DllImport("TXmlConnector.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr SetLogLevel(Int32 logLevel);
        //--------------------------------------------------------------------------------
	}
	

	//================================================================================


}
