using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonAccountsNew
{
    public static class GBAll
    {
        public static void GenerateDataAllPolicies( string[] InsurerArrays)
        {
            for (int i = InsurerArrays.Length - 1; i >= 0; i--)
            {
                GenerateBorderauxData.BorderauxData(InsurerArrays[i]);
                GenerateAccounts.BrokerData(InsurerArrays[i]);
                GenerateLegal.LegalData(InsurerArrays[i]);

            }


        }
    }
}