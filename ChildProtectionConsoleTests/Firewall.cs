using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProtectionConsoleTests
{
    /// <summary>
    /// TODO: Inna obsluga w zaleznosci od WIN
    /// http://stackoverflow.com/questions/1663960/c-sharp-api-to-test-if-a-network-adapter-is-firewalled
    /// 
    /// </summary>
    public class Firewall
    {
        private static string BLOCK_INTERNET_RULE = "Block Internet";

        public static void BlockInternetConnections()
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            firewallRule.Description = "Used to block all internet access.";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = BLOCK_INTERNET_RULE;
            

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallRule);
        }

        public static void UnblockInternetConnections()
        {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Remove(BLOCK_INTERNET_RULE);
        }

        public static void BlockIP(string ip)
        {

            //if (IsAddressValid(IP))
            {
                INetFwRule2 firewallRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

                firewallRule.Name = "BrutalNT: IP Access Block " + ip;
                firewallRule.Description = "Block Incoming Connections from IP Address.";
                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.RemoteAddresses = ip;

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                try
                {
                    firewallPolicy.Rules.Add(firewallRule);
                } catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine("UnauthorizedAccessException: " + e);
                }
                


                // UnauthorizedAccessException to handle
                
            }
        }

        public static Boolean IsFirewallEnabled()
        {
            INetFwPolicy2 fwPolicy2 = getCurrPolicy();
            NET_FW_PROFILE_TYPE2_ fwCurrentProfileTypes;
            //read Current Profile Types (only to increase Performace)
            //avoids access on CurrentProfileTypes from each Property
            fwCurrentProfileTypes = (NET_FW_PROFILE_TYPE2_)fwPolicy2.CurrentProfileTypes;
            return (fwPolicy2.get_FirewallEnabled(fwCurrentProfileTypes));
        }

        private static INetFwPolicy2 getCurrPolicy()
        {
            INetFwPolicy2 fwPolicy2;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            return fwPolicy2;
        }
    }
}
