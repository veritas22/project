using System;
using System.Data;


namespace tslight
{

    //================================================================================
	#region DataSet_tslight2
	//--------------------------------------------------------------------------------
	public class DataSet_tslight2 : DataSet
	{
        public DataTable_quotation t_quotation;	
	
        
		
		//--------------------------------------------------------------------------------
		public DataSet_tslight2():
			base()
		{
            t_quotation = new DataTable_quotation();
		
            
		}
		//--------------------------------------------------------------------------------
	}
	//--------------------------------------------------------------------------------
	#endregion
    //================================================================================
    #region Data_quotationquotation
    //--------------------------------------------------------------------------------
	public class DataTable_quotation : DataTable
	{
        public DataColumn quotation_id;
        public DataColumn quotation_code;
        public DataColumn quotation_open;
        public DataColumn quotation_biddepth;
        public DataColumn quotation_numbids;
        public DataColumn quotation_last;
		//--------------------------------------------------------------------------------
        public DataTable_quotation()
		{
            TableName = "t_quotation";
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
        public void Add_Row(DataRow_quotation row)
		{
			Rows.Add(row);
		}
        //--------------------------------------------------------------------------------
        public void Remove_Row(DataRow_quotation row)
		{
			Rows.Remove(row);
		}
        //--------------------------------------------------------------------------------
        public DataRow_quotation Add_Row(int quotation_id, string quotation_code, double quotation_open, int quotation_biddepth, int quotation_numbids, double quotation_last)
		{
            DataRow_quotation row = (DataRow_quotation)NewRow();
			object[] aValues = new object[]
			{
				quotation_id,  quotation_code,  quotation_open,  quotation_biddepth,  quotation_numbids,  quotation_last
			};
			row.ItemArray = aValues;
			Rows.Add(row);
			return row;
		}
		//--------------------------------------------------------------------------------
        public DataRow_quotation New_Row()
		{
            return (DataRow_quotation)NewRow();
		}
		//--------------------------------------------------------------------------------
		protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
		{
            return new DataRow_quotation(builder);
		}
		//--------------------------------------------------------------------------------
		public DataRow FindByID(uint id)
		{
            return (DataRow_quotation)Rows.Find(new object[] { id });
		}
		//--------------------------------------------------------------------------------
		internal void InitVars()
		{
            quotation_id = Columns["quotation_id"];
            quotation_code = Columns["quotation_code"];
            quotation_open = Columns["quotation_open"];
            quotation_biddepth = Columns["quotation_biddepth"];
            quotation_numbids = Columns["quotation_numbids"];
            quotation_last = Columns["quotation_last"];
		}
		//--------------------------------------------------------------------------------
		internal void InitClass()
		{
            quotation_id = new DataColumn("quotation_id", typeof(int), null, MappingType.Element);
            Columns.Add(quotation_id);
            quotation_code = new DataColumn("quotation_code ", typeof(string), null, MappingType.Element);
            Columns.Add(quotation_code);
            quotation_open = new DataColumn("quotation_open", typeof(string), null, MappingType.Element);
            Columns.Add(quotation_open);
            quotation_biddepth = new DataColumn("quotation_biddepth", typeof(int), null, MappingType.Element);
            Columns.Add(quotation_biddepth);
            quotation_numbids = new DataColumn("quotation_numbids", typeof(string), null, MappingType.Element);
            Columns.Add(quotation_numbids);
            quotation_last = new DataColumn("quotation_last", typeof(string), null, MappingType.Element);
            Columns.Add(quotation_last);

            PrimaryKey = new DataColumn[] { quotation_id };
            quotation_code.MaxLength = 50;
            quotation_last.MaxLength = 50;
		}
		//--------------------------------------------------------------------------------
		protected override Type GetRowType()
		{
            return typeof(DataRow_quotation);
		}
		//--------------------------------------------------------------------------------
        public DataRow_quotation this[int index]
		{
            get { return (DataRow_quotation)Rows[index]; }
		}
		//--------------------------------------------------------------------------------
	}
	
	public partial class DataRow_quotation : global::System.Data.DataRow
	{
		private DataTable_quotation table;
		
		//--------------------------------------------------------------------------------
        internal DataRow_quotation(global::System.Data.DataRowBuilder rb) :
			base(rb)
		{
            table = (DataTable_quotation)Table;
		}
		//--------------------------------------------------------------------------------
        public int quotation_id
		{
            get { return (int)this[table.quotation_id]; }
            set { this[table.quotation_id] = value; }
		}
        //-------------------------------------------------------------------------------
        public string quotation_code
		{
            get { return (string)this[table.quotation_code]; }
            set { this[table.quotation_code] = value; }
		}
		//--------------------------------------------------------------------------------
        public double quotation_open
		{
            get { return (double)this[table.quotation_open]; }
            set { this[table.quotation_open] = value; }
		}
		//--------------------------------------------------------------------------------
        public int quotation_biddepth
		{
            get { return (int)this[table.quotation_biddepth]; }
            set { this[table.quotation_biddepth] = value; }
		}

        public int quotation_numbids
        {
            get { return (int)this[table.quotation_numbids]; }
            set { this[table.quotation_numbids] = value; }
        }
        public double quotation_last
        {
            get { return (double)this[table.quotation_last]; }
            set { this[table.quotation_last] = value; }
        }


		//--------------------------------------------------------------------------------
	}
	#endregion // Data_quation
	

}
