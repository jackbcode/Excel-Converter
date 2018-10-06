using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ParagonAccountsNew
{
    class Program
    {
        static void Main(string[] args)
        {

            //var connectionString = "Data Source=INFOCENTER\\SQLPARAGON;Initial Catalog=Paragon_Accounts;Persist Security Info=True;User ID=Data User;Password=data";
            //var cnn = new SqlConnection(connectionString);
            //cnn.Open();
            //using (var conn = new SqlConnection(connectionString))
            //using (var command = new SqlCommand("pr_Accounts_Data_Extract_RegisterAll", conn)

            //{
            //    CommandType = CommandType.StoredProcedure
            //})
            //{
            //    conn.Open();
            //    command.ExecuteNonQuery();
            //}

            var InsurerArrays = new string[3] {
                "Paragon Monument China", "Paragon Select Let China",
                "Paragon Noble China"
            };
            
            GBAll.GenerateDataAllPolicies(InsurerArrays);

        }

       

    }
}
