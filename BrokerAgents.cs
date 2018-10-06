using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonAccountsNew
{


    /// <summary>
    /// WILL HAVE TO ADD TO FOR OTHER AGENT NAMES 
    /// </summary>


    class BrokerAgents
    {

        public static string GetAgent(string Insco)
        {

            string[] words = Insco.Split(' ');

            var AgentName = words[1] + words[2] + "Agents";

            return (AgentName);
        }
    }

}

