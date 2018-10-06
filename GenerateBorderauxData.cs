using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;


namespace ParagonAccountsNew
{
    public static class GenerateBorderauxData
    {
        
        public static void BorderauxData(string Insco)
        {
            try
            {

                Console.WriteLine("Generating " + Insco + " Bordereaux Excel File...........");


                SqlConnection cnn;
                string connectionString = null;
                string sql = null;
                string data = null;
                int i = 0;
                int j = 0;

                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
               
                connectionString = "Data Source=INFOCENTER\\SQLPARAGON;Initial Catalog=Paragon_Accounts;Persist Security Info=True;User ID=Data User;Password=data";




                cnn = new SqlConnection(connectionString);
                cnn.Open();


                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("pr_Accounts_GenerateBordeaux", conn)
                
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    command.Parameters.Add(new SqlParameter("@Insco", Insco));
                    command.ExecuteNonQuery();
                }

                
                var accountyears = new string[2] {
               DateTime.Now.ToString("yyyy"),DateTime.Now.AddYears(-1).ToString("yyyy")
                };

                foreach(var date in accountyears)
                {
                    Console.WriteLine("Year of Account - " + date + " is being processed");
                    
                    var xlSheets = xlWorkBook.Sheets as Microsoft.Office.Interop.Excel.Sheets;
                    var xlNewSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                    xlNewSheet1.Name = date + " Account";

                    xlNewSheet1.Cells[1, 1] = Insco + " " + DateTime.Now.ToString("MMMMyyyy") + " Bordereaux";
                    xlNewSheet1.Cells[1, 1].ColumnWidth = 40;
                    xlNewSheet1.Cells[1,1].Font.bold = true;
                    xlNewSheet1.Cells[1,1].Font.Size = 12;

                    cnn = new SqlConnection(connectionString);
                    cnn.Open();
                    sql = "SELECT * FROM tblBordereauxAll WHERE Insco ='" + Insco + "' and [Year of Account] = '" + date + "'";
                    SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
                    DataSet ds = new DataSet();
                    dscmd.Fill(ds);

                    foreach (System.Data.DataTable dt in ds.Tables)
                    {
                        for (int i1 = 0; i1 < dt.Columns.Count; i1++)
                        {
                            xlNewSheet1.Cells[2, i1 + 1] = dt.Columns[i1].ColumnName;
                            xlNewSheet1.Cells[2, i1 + 1].Font.bold = true;
                            xlNewSheet1.Cells[2, i1 + 1].RowHeight = 40;


                        }
                    }

                    for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        int s = i + 2;
                        for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                        {
                            data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                            xlNewSheet1.Cells[s + 1, j + 1] = data;
                            Console.WriteLine("Policy ref " + ds.Tables[0].Rows[i].ItemArray[1].ToString() + " has been processed.....");
                        }
                    }


                    xlNewSheet1.Columns.ColumnWidth = 20;


                    xlNewSheet1.Columns[1].ColumnWidth = 30;
                    xlNewSheet1.Columns[2].ColumnWidth = 30;
                    xlNewSheet1.Columns[6].ColumnWidth = 30;
                    xlNewSheet1.Columns[7].ColumnWidth = 30;
                    xlNewSheet1.Columns[8].ColumnWidth = 30;
                    xlNewSheet1.Columns[11].ColumnWidth = 30;
                    xlNewSheet1.Columns[21].ColumnWidth = 30;
                    xlNewSheet1.Columns[23].ColumnWidth = 30;
                    xlNewSheet1.Columns[29].ColumnWidth = 30;
                    xlNewSheet1.Columns[30].ColumnWidth = 30;


                    Console.WriteLine("Year of Account - " + date + " has been processed.....");

                    cnn.Close();
                    cnn.Dispose();
                    Marshal.ReleaseComObject(xlNewSheet1);

                }

                Microsoft.Office.Interop.Excel.Sheets worksheets = xlWorkBook.Worksheets;
                worksheets["Sheet1"].Delete();
                worksheets["Sheet2"].Delete();
                worksheets["Sheet3"].Delete();

                xlWorkBook.SaveAs(Insco + " Bordereaux " + DateTime.Now.ToString("MMMMyyyy") +
                ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);

               Console.WriteLine("Excel file created , you can find the file c:\\User\\UserName\\MyDocuments\\" + Insco + " Bordereaux " + DateTime.Now.ToString("MMMMyyyy") +
                ".xls");
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error....." + Ex);

            }

            
        }

    }
}
