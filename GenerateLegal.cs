using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ParagonAccountsNew
{
    public static class GenerateLegal
    {
        public static void LegalData(string Insco)
        {
            try
            {
                Console.WriteLine("Generating " + Insco + " Bordereaux Legal Excel File...........");

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

                xlWorkSheet.Cells[1, 1] = Insco + " Legal " + DateTime.Now.ToString("MMMMyyyy") + " Bordereaux";
                xlWorkSheet.Cells[1, 1].ColumnWidth = 50;
                xlWorkSheet.Cells[1, 1].Font.bold = true;
                xlWorkSheet.Cells[1, 1].Font.Size = 12;


                cnn = new SqlConnection(connectionString);
                cnn.Open();


                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("pr_Accounts_GenerateLegal", conn)

                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    command.Parameters.Add(new SqlParameter("@Insco", Insco));
                    command.ExecuteNonQuery();
                }



                sql = "SELECT * FROM tblLegalAll WHERE Insco ='" + Insco +"'";
                SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
                DataSet ds = new DataSet();
                dscmd.Fill(ds);

                foreach (System.Data.DataTable dt in ds.Tables)
                {
                    for (int i1 = 0; i1 < dt.Columns.Count; i1++)
                    {
                        xlWorkSheet.Cells[2, i1 + 1] = dt.Columns[i1].ColumnName;
                        xlWorkSheet.Cells[2, i1 + 1].Font.bold = true;
                        xlWorkSheet.Cells[2, i1 + 1].RowHeight = 40;


                    }
                }

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    int s = i + 2;
                    for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                    {
                        data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                        xlWorkSheet.Cells[s + 1, j + 1] = data;
                        Console.WriteLine("Policy ref " + ds.Tables[0].Rows[i].ItemArray[1].ToString() + " has been processed.....");
                    }
                }

                xlWorkSheet.Columns.ColumnWidth = 15;


                xlWorkSheet.Columns[1].ColumnWidth = 30;
                xlWorkSheet.Columns[2].ColumnWidth = 20;
                xlWorkSheet.Columns[3].ColumnWidth = 30;
                xlWorkSheet.Columns[5].ColumnWidth = 30;
                

                xlWorkBook.SaveAs(Insco + " Bordereaux - Legal " + DateTime.Now.ToString("MMMMyyyy") +
                ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);

                Console.WriteLine("Excel file created , you can find the file c:\\User\\UserName\\MyDocuments\\" + Insco + " Bordereaux - Legal " + DateTime.Now.ToString("MMMMyyyy") +
                 ".xls");
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error....." + Ex);

            }


        }

    }
}
