using System;
using System.Data;


namespace tslight
{

    //================================================================================
	#region DataSet_tslight3
	//--------------------------------------------------------------------------------
	public class DataSet_tslight3 : DataSet
	{
        public DataTable_trade t_trade;	
	
        
		
		//--------------------------------------------------------------------------------
		public DataSet_tslight3():
			base()
		{
            t_trade = new DataTable_trade();
		
            
		}
		//--------------------------------------------------------------------------------
	}
	//--------------------------------------------------------------------------------
	#endregion
    //================================================================================
    #region Data_trade
    //--------------------------------------------------------------------------------
	public class DataTable_trade : DataTable
	{
        public DataColumn trade_id;
        public DataColumn trade_time;
        public DataColumn trade_price;
        public DataColumn trade_quantity;
        public DataColumn trade_buysell;
        public DataColumn trade_period;
		//--------------------------------------------------------------------------------
        public DataTable_trade()
		{
            TableName = "t_trade";
			BeginInit();
			InitClass();
			EndInit();
		}
		//--------------------------------------------------------------------------------
		public int Count
		{
			get { return Rows.Count; }
		}
		//--------------------------------------------------------------------------------
        public void Add_Row(DataRow_trade row)
		{
			Rows.Add(row);
		}
        //--------------------------------------------------------------------------------
        public void Remove_Row(DataRow_trade row)
		{
			Rows.Remove(row);
		}
        //--------------------------------------------------------------------------------
        public DataRow_trade Add_Row(int trade_id, DateTime trade_time, double trade_price, int trade_quantity, string trade_buysell, string trade_period)
		{
            DataRow_trade row = (DataRow_trade)NewRow();
			object[] aValues = new object[]
			{
				trade_id,  trade_time,  trade_price,  trade_quantity,  trade_buysell,  trade_period
			};
			row.ItemArray = aValues;
			Rows.Add(row);
			return row;
		}
		//--------------------------------------------------------------------------------
        public DataRow_trade New_Row()
		{
            return (DataRow_trade)NewRow();
		}
		//--------------------------------------------------------------------------------
		protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
		{
            return new DataRow_trade(builder);
		}
		//--------------------------------------------------------------------------------
		public DataRow FindByID(uint id)
		{
            return (DataRow_trade)Rows.Find(new object[] { id });
		}
		//--------------------------------------------------------------------------------
		internal void InitVars()
		{
            trade_id = Columns["trade_id"];
            trade_time = Columns["trade_time"];
            trade_price = Columns["trade_price"];
            trade_quantity = Columns["trade_quantity"];
            trade_buysell = Columns["trade_buysell"];
            trade_period = Columns["trade_period"];
		}
		//--------------------------------------------------------------------------------
		internal void InitClass()
		{
            trade_id = new DataColumn("trade_id", typeof(int), null, MappingType.Element);
            Columns.Add(trade_id);
            trade_time = new DataColumn("trade_time ", typeof(string), null, MappingType.Element);
            Columns.Add(trade_time);
            trade_price = new DataColumn("trade_price", typeof(string), null, MappingType.Element);
            Columns.Add(trade_price);
            trade_quantity = new DataColumn("trade_quantity", typeof(int), null, MappingType.Element);
            Columns.Add(trade_quantity);
            trade_buysell = new DataColumn("trade_buysell", typeof(string), null, MappingType.Element);
            Columns.Add(trade_buysell);
            trade_period = new DataColumn("trade_period", typeof(string), null, MappingType.Element);
            Columns.Add(trade_period);

            PrimaryKey = new DataColumn[] { trade_id };
            trade_time.MaxLength = 50;
            trade_period.MaxLength = 50;
		}
		//--------------------------------------------------------------------------------
		protected override Type GetRowType()
		{
            return typeof(DataRow_trade);
		}
		//--------------------------------------------------------------------------------
        public DataRow_trade this[int index]
		{
            get { return (DataRow_trade)Rows[index]; }
		}
		//--------------------------------------------------------------------------------
	}
	
	public partial class DataRow_trade : global::System.Data.DataRow
	{
		private DataTable_trade table;
		
		//--------------------------------------------------------------------------------
        internal DataRow_trade(global::System.Data.DataRowBuilder rb) :
			base(rb)
		{
            table = (DataTable_trade)Table;
		}
		//--------------------------------------------------------------------------------
        public int trade_id
		{
            get { return (int)this[table.trade_id]; }
            set { this[table.trade_id] = value; }
		}
        //-------------------------------------------------------------------------------
        public string trade_time
		{
            get { return (string)this[table.trade_time]; }
            set { this[table.trade_time] = value; }
		}
		//--------------------------------------------------------------------------------
        public double trade_price
		{
            get { return (double)this[table.trade_price]; }
            set { this[table.trade_price] = value; }
		}
		//--------------------------------------------------------------------------------
        public int trade_quantity
		{
            get { return (int)this[table.trade_quantity]; }
            set { this[table.trade_quantity] = value; }
		}

        public int trade_buysell
        {
            get { return (int)this[table.trade_buysell]; }
            set { this[table.trade_buysell] = value; }
        }
        public double trade_period
        {
            get { return (double)this[table.trade_period]; }
            set { this[table.trade_period] = value; }
        }


		//--------------------------------------------------------------------------------
	}
	#endregion // Data_quation
	

}
