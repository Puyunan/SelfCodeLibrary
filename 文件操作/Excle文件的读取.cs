# 方法1 利用OleDB进行读取，读出来是public static DataSet ToDataTable()
public static DataSet ToDataTable()
{
	string connStr = " ";
	OpenFileDialog openFile = new OpenFileDialog();
	openFile.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
	openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	openFile.Multiselect = false;
	if (openFile.ShowDialog() == DialogResult.Cancel) return null;
	var filePath = openFile.FileName;
	string fileType = System.IO.Path.GetExtension(filePath);
	if (string.IsNullOrEmpty(fileType)) return null;
	if (fileType == ".xls")
		connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
	else
		connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
	string sql_F = "Select * FROM [{0}]";
	OleDbConnection conn = null;
	OleDbDataAdapter da = null;
	DataTable dtSheetName = null;
	DataSet ds = new DataSet();
	try
	{
		// 初始化连接，并打开  
		conn = new OleDbConnection(connStr);
		conn.Open();
		// 获取数据源的表定义元数据                         
		string SheetName = "";
		dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
		// 初始化适配器  
		da = new OleDbDataAdapter();
		for (int i = 0; i < dtSheetName.Rows.Count; i++)
		{
			SheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];
			if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
			{
				continue;
			}
			da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
			DataSet dsItem = new DataSet();
			da.Fill(dsItem, SheetName);
			ds.Tables.Add(dsItem.Tables[0].Copy());
		}
	}
	catch (Exception ex)
	{
		MessageBox.Show(ex.Message)
	}
	finally
	{
		// 关闭连接  
		if (conn.State == ConnectionState.Open)
		{
			conn.Close();
			da.Dispose();
			conn.Dispose();
		}
	}
	return ds;
}
