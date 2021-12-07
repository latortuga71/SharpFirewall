using System;
using NetFwTypeLib;

namespace SharpFireWall
{

    public class FirewallRule
    {
        public NET_FW_RULE_DIRECTION_ Direction;
        public NET_FW_ACTION_ Action;
        public NET_FW_IP_PROTOCOL_ Protocol;
        public FirewallRuleType RuleType;
        public string RuleName;
        public string ApplicationName;
        public string ServiceName;
        public string InterfaceTypes;
        public string Description;
        public string LocalAddr;
        public string LocalPorts;
        public string RemoteAddr;
        public string RemotePorts;

        public FirewallRule(string name, string type, string direction, string action, string proto, string lports = "*", string laddr = "*", string rports = "*", string raddr = "*", string appName = "", string serviceName = "")
        {
            // required
            RuleName = name;
            RuleType = GetType(type);
            Direction = GetDirection(direction);
            Action = GetAction(action);
            Protocol = GetProtocol(proto);
            InterfaceTypes = "All";
            // optional
            // ports and addresses
            LocalAddr = laddr;
            LocalPorts = lports;
            RemoteAddr = raddr;
            RemotePorts = rports;
            // only used for when not custom rule.
            ApplicationName = appName;
            ServiceName = serviceName;
        }
        public INetFwRule NewRuleByApplication()
        {
            if (string.IsNullOrWhiteSpace(ApplicationName))
            {
                throw new Exception("Application Name/FullPath was not provided.");
            }
            INetFwRule newFirewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            newFirewallRule.Protocol = (int)Protocol;
            newFirewallRule.Action = Action;
            newFirewallRule.Direction = Direction;
            newFirewallRule.Enabled = true;
            newFirewallRule.InterfaceTypes = InterfaceTypes;
            newFirewallRule.Name = RuleName;
            newFirewallRule.Description = Description;
            newFirewallRule.ApplicationName = ApplicationName;
            return newFirewallRule;
        }
        public INetFwRule NewRuleByService()
        {
            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                throw new Exception("ServiceName was not provided.");
            }
            INetFwRule newFirewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            newFirewallRule.Protocol = (int)Protocol;
            newFirewallRule.Action = Action;
            newFirewallRule.Direction = Direction;
            newFirewallRule.Enabled = true;
            newFirewallRule.InterfaceTypes = InterfaceTypes;
            newFirewallRule.Name = RuleName;
            newFirewallRule.Description = Description;
            newFirewallRule.serviceName = ServiceName;
            return newFirewallRule;
        }

        public INetFwRule NewCustomRule()
        {
            INetFwRule newFirewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            newFirewallRule.Protocol = (int)Protocol;
            newFirewallRule.Action = Action;
            newFirewallRule.Direction = Direction;
            newFirewallRule.Enabled = true;
            newFirewallRule.InterfaceTypes = InterfaceTypes;
            newFirewallRule.LocalAddresses = LocalAddr;
            newFirewallRule.LocalPorts = LocalPorts;
            newFirewallRule.RemoteAddresses = RemoteAddr;
            newFirewallRule.RemotePorts = RemotePorts;
            newFirewallRule.Name = RuleName;
            newFirewallRule.Description = Description;
            return newFirewallRule;
        }


        private NET_FW_IP_PROTOCOL_ GetProtocol(string proto)
        {
            if (string.Equals(proto, "tcp", StringComparison.CurrentCultureIgnoreCase)) { return NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP; }
            if (string.Equals(proto, "udp", StringComparison.CurrentCultureIgnoreCase)) { return NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP; }
            throw new Exception("Invalid protocol provided. Only tcp or udp allowed.");
        }
        private NET_FW_RULE_DIRECTION_ GetDirection(string dir)
        {
            if (string.Equals(dir, "In", StringComparison.CurrentCultureIgnoreCase)) { return NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN; }
            if (string.Equals(dir, "Out", StringComparison.CurrentCultureIgnoreCase)) { return NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT; }
            throw new Exception("Invalid direction provided. Only in or out allowed.");
        }
        private NET_FW_ACTION_ GetAction(string action)
        {
            if (string.Equals(action, "Block", StringComparison.CurrentCultureIgnoreCase)) { return NET_FW_ACTION_.NET_FW_ACTION_BLOCK; }
            if (string.Equals(action, "Allow", StringComparison.CurrentCultureIgnoreCase)) { return NET_FW_ACTION_.NET_FW_ACTION_ALLOW; }
            throw new Exception("Invalid action provided. Only block or allow valid.");
        }
        private FirewallRuleType GetType(string type)
        {
            if (string.Equals(type, "Custom", StringComparison.CurrentCultureIgnoreCase)) { return FirewallRuleType.Custom; }
            if (string.Equals(type, "ApplicationName", StringComparison.CurrentCultureIgnoreCase)) { return FirewallRuleType.ApplicationName; }
            if (string.Equals(type, "ServiceName", StringComparison.CurrentCultureIgnoreCase)) { return FirewallRuleType.ServiceName; }
            return FirewallRuleType.Error;
        }
    }
}
