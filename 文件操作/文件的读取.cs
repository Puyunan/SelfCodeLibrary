# 方法1 利用OleDB进行读取Excel文件，读出来是DataSet
# 每个DataTable的名称，是Excel页Sheet的名称，DataTable每列的值是
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

# 方法2 导入Csv文件，直接使用IO.Stream读取
public static void CsvRead()
{
    string curFileName = null;
    //后缀
	openFileDialogData.Filter = "文本文件(*csv)|*csv";
	openFileDialogData.Title = "打开Csv文件";
	if (openFileDialogData.ShowDialog() == DialogResult.OK)
	{
		curFileName = openFileDialogData.FileName;
		try
		{
			Initialize();//初始化函数就是把所要存储数据的集合给Clear掉
			string strLine;
			string[] aryLine;
			//读取数据
			System.IO.StreamReader SpectrumDataStr = new System.IO.StreamReader(curFileName);
            while ((strLine = SpectrumDataStr.ReadLine()) != null)
			{
				aryLine = strLine.Split(new char[] { ',', ' ' });
				//现在这个aryLine存储的就是每一行的数据
				//for循环里面根据自己的需求进行编写即可
				for (int i = 0; i < aryLine.Length; i++)
				{
					if(i%2==0)
						xdata.Add(aryLine[i]);
					else
						ydata.Add(aryLine[i]);
				}
			}
		}
		catch (Exception a)
		{
			MessageBox.Show(a.Message);
			return;
		}
	}
}
