using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFwTypeLib;

namespace SharpFireWall
{
    public enum FirewallRuleType
    {
        Error,
        Custom,
        ServiceName,
        ApplicationName
    }

    public static class FirewallManager
    {
        public static bool IsNameTaken(string ruleName)
        {
            var results = new SharpSploitResultList<FireWallRulesResult>();
            INetFwPolicy2 pol = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWPolicy2"));
            foreach (INetFwRule rule in pol.Rules)
            {
                if (string.Equals(ruleName, rule.Name, StringComparison.CurrentCultureIgnoreCase)) { return true; }
            }
            return false;
        }
        public static bool IsEnabled()
        {
            INetFwPolicy pol = GetLocalPolicy();
            return pol.CurrentProfile.FirewallEnabled;
        }
        public static string GetCurrentProfileType()
        {
            INetFwPolicy pol = GetLocalPolicy();
            NET_FW_PROFILE_TYPE_ currentProfile = pol.CurrentProfile.Type;
            if (currentProfile.HasFlag(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN)) { return "Domain Profile"; }
            if (currentProfile.HasFlag(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD)) { return "Standard Profile"; }
            return "";

        }
        public static INetFwPolicy GetLocalPolicy()
        {
            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
            return mgr.LocalPolicy;
        }
        public static bool AddCustomRule(string protocol,string name,string direction,string action,string localports,string localaddr,string remoteports,string remoteaddr) 
        {
            try
            {
                INetFwPolicy2 currentPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWPolicy2"));
                FirewallRule newRule = new FirewallRule(name, "custom", direction, action, protocol, localports, localaddr, remoteports, remoteaddr);
                INetFwRule newFWRule = newRule.NewCustomRule();
                currentPolicy.Rules.Add(newFWRule);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static bool AddRuleByServiceName(string protocol, string name, string direction, string action, string service)
        {
            try
            {
                INetFwPolicy2 currentPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWPolicy2"));
                FirewallRule newRule = new FirewallRule(name, "ApplicationName", direction, action, protocol, serviceName: service);
                INetFwRule newFWRule = newRule.NewRuleByService();
                currentPolicy.Rules.Add(newFWRule);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        public static bool AddRuleByApplicationName(string protocol,string name, string direction,string action,string applicationPath)
        {
            try
            {
                INetFwPolicy2 currentPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWPolicy2"));
                FirewallRule newRule = new FirewallRule(name, "ApplicationName", direction, action, protocol,appName:applicationPath);
                INetFwRule newFWRule = newRule.NewRuleByApplication();
                currentPolicy.Rules.Add(newFWRule);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static bool DeleteRule(string ruleName)
        {
            try {
                INetFwPolicy2 currentPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWPolicy2"));
                currentPolicy.Rules.Remove(ruleName);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        // use sharpsploit visual.
        public static string DumpRules(string getName = "")
        {
            var results = new SharpSploitResultList<FireWallRulesResult>();
            INetFwPolicy2 pol = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWPolicy2"));
            foreach (INetFwRule rule in pol.Rules)
            {
                //setup
                var result = new FireWallRulesResult();
                var ruleName = "";
                var groupName = "";
                var localAddresses = "";
                var remoteAddresses = "";
                var localPorts = "";
                var remotePorts = "";
                var proto = "";
                var action = "";
                var direction = "";
                var serviceName = "";
                var appName = "";
                //checks
                if (!string.IsNullOrEmpty(rule.Name)) { ruleName = rule.Name; }
                if (!string.IsNullOrEmpty(rule.Grouping)) { groupName = rule.Grouping; }
                if (!string.IsNullOrEmpty(rule.LocalAddresses)) { localAddresses = rule.LocalAddresses; }
                if (!string.IsNullOrEmpty(rule.LocalPorts)) { localPorts = rule.LocalPorts; }
                if (!string.IsNullOrEmpty(rule.RemoteAddresses)) { remoteAddresses = rule.RemoteAddresses; }
                if (!string.IsNullOrEmpty(rule.RemotePorts)) { remotePorts = rule.RemotePorts; }
                if (rule.Protocol == 6) { proto = "TCP"; }
                else if (rule.Protocol == 17){ proto = "UDP"; }
                else if (rule.Protocol == 1) { proto = "ICMPv4"; }
                else { proto = rule.Protocol.ToString(); }

                if (rule.Action == 0) { action = "BLOCK"; } else { action = "ALLOW"; }

                if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) { direction = "IN"; } 
                else if(rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT) { direction = "OUT"; }
                else { direction = "?"; }

                if (!string.IsNullOrEmpty(rule.serviceName)) { serviceName = rule.serviceName; }
                if (!string.IsNullOrEmpty(rule.ApplicationName)) { appName = rule.ApplicationName; }

                // get only get rules with specific name
                if (!string.IsNullOrEmpty(getName))
                {
                    if (string.Equals(getName, rule.Name, StringComparison.CurrentCultureIgnoreCase)) {
                        results.Add(new FireWallRulesResult
                        {
                            RuleName = ruleName, //$"{ruleName} ::: {appName} ::: {serviceName}",
                            Direction = direction,
                            Group = groupName,
                            Profile = rule.Profiles,
                            Enabled = rule.Enabled,
                            Action = action,
                            Local = $"{localAddresses}:{localPorts}",
                            Remote = $"{remoteAddresses}:{remotePorts}",
                            Protocol = proto,
                        });
                    }
                    else
                    {
                        continue;
                    }
                }


                results.Add(new FireWallRulesResult
                {
                    RuleName = ruleName, //$"{ruleName} ::: {appName} ::: {serviceName}",
                    Direction = direction,
                    Group = groupName,
                    Profile = rule.Profiles,
                    Enabled = rule.Enabled,
                    Action = action,
                    Local = $"{localAddresses}:{localPorts}",
                    Remote = $"{remoteAddresses}:{remotePorts}",
                    Protocol = proto,
                });
            }
            return results.ToString();
        }
    }
    public sealed class FireWallRulesResult : SharpSploitResult
    {
        public string RuleName { get; set; }
        public string Direction { get; set; }
        public string Group { get; set; }
        public int Profile { get; set; }
        public bool Enabled { get; set; }
        public string Action { get; set; }
        public string Local { get; set; }
        public string Remote { get; set; }
        public string Protocol { get; set; }

        protected internal override IList<SharpSploitResultProperty> ResultProperties => new List<SharpSploitResultProperty>
        {
            new SharpSploitResultProperty{Name = nameof(RuleName),Value = RuleName},
            //new SharpSploitResultProperty{Name = nameof(Group),Value = Group},
            //new SharpSploitResultProperty{Name = nameof(Profile),Value = Profile},
            new SharpSploitResultProperty{Name = nameof(Direction),Value = Direction},
            new SharpSploitResultProperty{Name = nameof(Enabled),Value = Enabled},
            new SharpSploitResultProperty{Name = nameof(Action),Value = Action},
            new SharpSploitResultProperty{Name = nameof(Local),Value = Local},
            new SharpSploitResultProperty{Name = nameof(Remote),Value = Remote},
            new SharpSploitResultProperty{Name = nameof(Protocol),Value = Protocol },
        };
    }
}
