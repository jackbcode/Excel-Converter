using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ParagonAccountsNew
{
    public static class GenerateAccounts
    {
        public static void BrokerData(string Insco)
        {
            try
            {

                Console.WriteLine("Generating " + Insco + " Broker Account Excel File...........");


                SqlConnection cnn;
                string connectionString2 = null;
                string sql = null;
                string sql1 = null;
                string data = null;
                int i = 0;
                int j = 0;

                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                Microsoft.Office.Interop.Excel.Range xlWorkSheetRange;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                


                

                ///////////////////////////////////////

                var connectionString3 = "Data Source=PARAGON-SVR1\\SQLPARAGON;Initial Catalog=Accounts;Persist Security Info=True;User ID=Data User;Password=data";
                var connectionString1 = "Data Source=INFOCENTER\\SQLPARAGON;Initial Catalog=Paragon_Accounts;Persist Security Info=True;User ID=Data User;Password=data";


                var InscoAgentNew = BrokerAgents.GetAgent(Insco);


                cnn = new SqlConnection(connectionString3);
                cnn.Open();
                sql = "SELECT AGNUMBER FROM " + InscoAgentNew + " ORDER BY AGNUMBER DESC";
                SqlDataAdapter dscmd1 = new SqlDataAdapter(sql, cnn);
                DataSet ds1 = new DataSet();
                dscmd1.Fill(ds1);
                cnn.Close();
                cnn.Dispose();


                using (var conn = new SqlConnection(connectionString1))
                using (var command = new SqlCommand("pr_Accounts_GenerateBroker", conn)

                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    command.Parameters.Add(new SqlParameter("@Insco", Insco));
                    command.ExecuteNonQuery();
                }

                
                //cnn = new SqlConnection(connectionString1);
                //cnn.Open();
                //sql1 = "SELECT * FROM tblAccountsBrokerAll WHERE Insco ='" + Insco + "'";
                //SqlDataAdapter dscmd = new SqlDataAdapter(sql1, cnn);
                //DataSet ds = new DataSet();
                //dscmd.Fill(ds);

                foreach (DataTable table in ds1.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        foreach (object item in row.ItemArray)
                        {



                            cnn = new SqlConnection(connectionString1);
                            cnn.Open();
                            sql = "SELECT * FROM tblAccountsBrokerAll where Insco ='" + Insco + "' and [Policy Number] like'" + item + "%%'";
                            SqlDataAdapter dscmd2 = new SqlDataAdapter(sql, cnn);
                            DataSet ds2 = new DataSet();
                            dscmd2.Fill(ds2);




                            if (ds2.Tables[0].Rows.Count != 0)
                            {
                                var xlSheets = xlWorkBook.Sheets as Microsoft.Office.Interop.Excel.Sheets;
                                var xlNewSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                                xlNewSheet.Name = item.ToString();






                                foreach (System.Data.DataTable dt in ds2.Tables)
                                {
                                    for (int i1 = 0; i1 < dt.Columns.Count; i1++)
                                    {
                                        xlNewSheet.Cells[1, i1 + 1] = dt.Columns[i1].ColumnName;
                                        xlNewSheet.Cells[1, i1 + 1].Font.bold = true;
                                        //xlNewSheet.Columns[i1].ColumnWidth = 18;




                                    }
                                }

                                for (i = 0; i <= ds2.Tables[0].Rows.Count - 1; i++)
                                {
                                    int s = i + 1;
                                    for (j = 0; j <= ds2.Tables[0].Columns.Count - 1; j++)
                                    {
                                        data = ds2.Tables[0].Rows[i].ItemArray[j].ToString();
                                        xlNewSheet.Cells[s + 1, j + 1] = data;

                                    }
                                }


                                //Microsoft.Office.Interop.Excel.Range col = (Microsoft.Office.Interop.Excel.Range)xlNewSheet.Columns["K:K"];

                                Microsoft.Office.Interop.Excel.Range col = xlNewSheet.UsedRange.Columns["K:K", Type.Missing];

                                float netPremiumTotal = 0;

                                

                             foreach (Microsoft.Office.Interop.Excel.Range broker in col.Cells)
                             {
                                    

                                        var brokerString = Convert.ToString(broker.Value);

                                        if (brokerString != "Net_Premium" && brokerString != null)
                                        {
                                            float npValue = Convert.ToInt32(broker.Value);
                                            
                                            netPremiumTotal += npValue;
                                            
                                        }

                                    

                             }

                                //Console.WriteLine("Net Premium total" + netPremiumTotal);

                                xlNewSheet.Columns[1].ColumnWidth = 30;
                                xlNewSheet.Columns[2].ColumnWidth = 20;
                                xlNewSheet.Columns[3].ColumnWidth = 30;
                                xlNewSheet.Columns[4].ColumnWidth = 15;
                                xlNewSheet.Columns[5].ColumnWidth = 15;
                                xlNewSheet.Columns[6].ColumnWidth = 15;
                                xlNewSheet.Columns[7].ColumnWidth = 15;
                                xlNewSheet.Columns[8].ColumnWidth = 15;
                                xlNewSheet.Columns[9].ColumnWidth = 15;
                                xlNewSheet.Columns[10].ColumnWidth = 15;
                                xlNewSheet.Columns[11].ColumnWidth = 15;
                                xlNewSheet.Columns[12].ColumnWidth = 20;


                                xlNewSheet.Cells[1, 12] = "Net Premium Total";
                                xlNewSheet.Cells[1, 12].Font.bold = true;
                                xlNewSheet.Cells[1, 12].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);



                                xlNewSheet.Cells[2, 12] = netPremiumTotal;
                                xlNewSheet.Cells[2, 12].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                                xlNewSheet.Cells[2, 12].Font.bold = true;
                                xlNewSheet.Cells[2, 12].Font.Size = 14;

                                Console.WriteLine("Broker ref " + item + " has been processed.....");
                                cnn.Close();
                                cnn.Dispose();
                                Marshal.ReleaseComObject(xlNewSheet);
                            }

                            else
                            {
                               
                                cnn.Close();
                                cnn.Dispose();

                            }

                        }



                    }


                }

                Microsoft.Office.Interop.Excel.Sheets worksheets = xlWorkBook.Worksheets;
                worksheets["Sheet1"].Delete();
                worksheets["Sheet2"].Delete();
               

                xlWorkBook.SaveAs(Insco + " " + DateTime.Now.ToString("MMMMyyyy") +
                ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();


              
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);



                Console.WriteLine("Excel file created , you can find the file c:\\User\\UserName\\MyDocuments\\" + Insco + " " + DateTime.Now.ToString("MMMMyyyy") +
                ".xls");
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error..........." + Ex);

            }


        }





    }



}

