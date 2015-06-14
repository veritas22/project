using System;
using System.Data;


namespace tslight
{

    //================================================================================
	#region DataSet_tslight4
	//--------------------------------------------------------------------------------
	public class DataSet_tslight4 : DataSet
	{
        public DataTable_quote t_quote;	
	
        
		
		//--------------------------------------------------------------------------------
		public DataSet_tslight4():
			base()
		{
            t_quote = new DataTable_quote();
		
            
		}
		//--------------------------------------------------------------------------------
	}
	//--------------------------------------------------------------------------------
	#endregion
    //================================================================================
    #region Data_quote
    //--------------------------------------------------------------------------------
	public class DataTable_quote : DataTable
	{
        public DataColumn quote_id;
        public DataColumn quote_board;
        public DataColumn quote_price;
        public DataColumn quote_source;
        public DataColumn quote_yield;
        public DataColumn quote_buy;
		//--------------------------------------------------------------------------------
        public DataTable_quote()
		{
            TableName = "t_quote";
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
        public void Add_Row(DataRow_quote row)
		{
			Rows.Add(row);
		}
        //--------------------------------------------------------------------------------
        public void Remove_Row(DataRow_quote row)
		{
			Rows.Remove(row);
		}
        //--------------------------------------------------------------------------------
        public DataRow_quote Add_Row(int quote_id, string quote_board, double quote_price, double quote_source, int quote_yield, int quote_buy)
		{
            DataRow_quote row = (DataRow_quote)NewRow();
			object[] aValues = new object[]
			{
				quote_id,  quote_board,  quote_price,  quote_source,  quote_yield,  quote_buy
			};
			row.ItemArray = aValues;
			Rows.Add(row);
			return row;
		}
		//--------------------------------------------------------------------------------
        public DataRow_quote New_Row()
		{
            return (DataRow_quote)NewRow();
		}
		//--------------------------------------------------------------------------------
		protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
		{
            return new DataRow_quote(builder);
		}
		//--------------------------------------------------------------------------------
		public DataRow FindByID(uint id)
		{
            return (DataRow_quote)Rows.Find(new object[] { id });
		}
		//--------------------------------------------------------------------------------
		internal void InitVars()
		{
            quote_id = Columns["quote_id"];
            quote_board = Columns["quote_board"];
            quote_price = Columns["quote_price"];
            quote_source = Columns["quote_source"];
            quote_yield = Columns["quote_yield"];
            quote_buy = Columns["quote_buy"];
		}
		//--------------------------------------------------------------------------------
		internal void InitClass()
		{
            quote_id = new DataColumn("quote_id", typeof(int), null, MappingType.Element);
            Columns.Add(quote_id);
            quote_board = new DataColumn("quote_board ", typeof(string), null, MappingType.Element);
            Columns.Add(quote_board);
            quote_price = new DataColumn("quote_price", typeof(string), null, MappingType.Element);
            Columns.Add(quote_price);
            quote_source = new DataColumn("quote_source", typeof(int), null, MappingType.Element);
            Columns.Add(quote_source);
            quote_yield = new DataColumn("quote_yield", typeof(string), null, MappingType.Element);
            Columns.Add(quote_yield);
            quote_buy = new DataColumn("quote_buy", typeof(string), null, MappingType.Element);
            Columns.Add(quote_buy);

            //PrimaryKey = new DataColumn[] { quote_id };
            quote_board.MaxLength = 50;
            quote_buy.MaxLength = 50;
		}
		//--------------------------------------------------------------------------------
		protected override Type GetRowType()
		{
            return typeof(DataRow_quote);
		}
		//--------------------------------------------------------------------------------
        public DataRow_quote this[int index]
		{
            get { return (DataRow_quote)Rows[index]; }
		}
		//--------------------------------------------------------------------------------
	}
	
	public partial class DataRow_quote : global::System.Data.DataRow
	{
		private DataTable_quote table;
		
		//--------------------------------------------------------------------------------
        internal DataRow_quote(global::System.Data.DataRowBuilder rb) :
			base(rb)
		{
            table = (DataTable_quote)Table;
		}
		//--------------------------------------------------------------------------------
        public int quote_id
		{
            get { return (int)this[table.quote_id]; }
            set { this[table.quote_id] = value; }
		}
        //-------------------------------------------------------------------------------
        public string quote_board
		{
            get { return (string)this[table.quote_board]; }
            set { this[table.quote_board] = value; }
		}
		//--------------------------------------------------------------------------------
        public double quote_price
		{
            get { return (double)this[table.quote_price]; }
            set { this[table.quote_price] = value; }
		}
		//--------------------------------------------------------------------------------
        public int quote_source
		{
            get { return (int)this[table.quote_source]; }
            set { this[table.quote_source] = value; }
		}

        public int quote_yield
        {
            get { return (int)this[table.quote_yield]; }
            set { this[table.quote_yield] = value; }
        }
        public double quote_buy
        {
            get { return (double)this[table.quote_buy]; }
            set { this[table.quote_buy] = value; }
        }


		//--------------------------------------------------------------------------------
	}
	#endregion // Data_quation
	

}
