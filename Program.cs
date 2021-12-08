using System;
using System.Collections.Generic;

namespace SharpFireWall
{
    public class Program
    {
        public static void Help() {
            Console.WriteLine(@"
   _____ __                     _____                         ____
  / ___// /_  ____ __________  / __(_)_______ _      ______ _/ / /
  \__ \/ __ \/ __ `/ ___/ __ \/ /_/ / ___/ _ \ | /| / / __ `/ / / 
 ___/ / / / / /_/ / /  / /_/ / __/ / /  /  __/ |/ |/ / /_/ / / /  
/____/_/ /_/\__,_/_/  / .___/_/ /_/_/   \___/|__/|__/\__,_/_/_/   
                     /_/                                         
                ::: latortuga71 :::
                                                                ");
            Console.WriteLine("SharpFileWall.exe <mode> <args...>");
            Console.WriteLine("Modes -> dump, custom, app, service, delete, disable, enable");
            // custom mode args
            Console.WriteLine("\n ::: Custom mode required args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            Console.WriteLine("/proto:<protocol>            Protocol (tcp,udp)");
            Console.WriteLine("/direction:<direction>       Traffic Direction (in,out)");
            Console.WriteLine("/action:<action>             Action to take (block,allow)");
            Console.WriteLine("/remoteport:<rports>         Remote ports to allow (55,1000,99-125) or (* for wildcard)");
            Console.WriteLine("/remoteaddr:<raddr>          Remote addresses to allow (10.0.0.19,192.168.56.1/24,10.0.0.1-10.0.0.255) or (* for wildcard)");
            Console.WriteLine("/localport:<lports>          Local ports to allow (55,1000,99-125) or (* for wildcard)");
            Console.WriteLine("/localaddr:<laddr>           Local addresses to allow (10.0.0.19,192.168.56.1/24,10.0.0.1-10.0.0.255) or (* for wildcard)");
            // app mode args
            Console.WriteLine("\n ::: App mode required args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            Console.WriteLine("/proto:<protocol>            Protocol (tcp,udp)");
            Console.WriteLine("/direction:<direction>       Traffic Direction (in,out)");
            Console.WriteLine("/action:<action>             Action to take (block,allow)");
            Console.WriteLine("/appname:<name>              Application name/path to block or allow");
            // service mode args
            Console.WriteLine("\n ::: Service mode required args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            Console.WriteLine("/proto:<protocol>            Protocol (tcp,udp)");
            Console.WriteLine("/direction:<direction>       Traffic Direction (in,out)");
            Console.WriteLine("/action:<action>             Action to take (block,allow)");
            Console.WriteLine("/service:<name>              Service name to block or allow");
            // delete mode args
            Console.WriteLine("\n ::: Delete mode required args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            // delete mode args
            Console.WriteLine("\n ::: Disable mode required args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            // delete mode args
            Console.WriteLine("\n ::: Enable mode required args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            // dump mode args
            Console.WriteLine("\n ::: Dump mode optional args :::");
            Console.WriteLine("/name:<name>                 Name for the rule");
            /// example usage
            Console.WriteLine("\n ::: Example Usage :::");
            Console.WriteLine("SharpFireWall.exe custom     /name:AllowRdp /proto:tcp /direction:in /action:allow /remoteport:5000-6000 /remoteaddr:192.168.56.0/24,10.0.0.1 /localport:3389,80 /localaddr:*");
            Console.WriteLine("SharpFilewall.exe service    /name:BlockWinDefend /proto:tcp /direction:out /action:block /service:sense");
            Console.WriteLine("SharpFilewall.exe app        /name:BlockWinDefend /proto:tcp /direction:out /action:block /appname:windefend");
            Console.WriteLine("SharpFirewall.exe delete     /name:AllowRdp");
            Console.WriteLine("SharpFirewall.exe enable     /name:AllowRdp");
            Console.WriteLine("SharpFirewall.exe disable    /name:AllowRdp");
            Console.WriteLine("SharpFirewall.exe dump       /name:Skype");
            Console.WriteLine("SharpFirewall.exe dump");
            Console.WriteLine("\n ::: Notes :::");
            Console.WriteLine("When passing ports the following are all valid -> [80,80-90,*]");
            Console.WriteLine("When passing ips the following are all valid -> [10.0.0.1,10.0.0.1/24,10.0.0.1-10.0.0.255,*]");
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Help();
                return;
            }
            var command = args[0];
            Dictionary<string, string> parsedArgs = ArgParse.Parse(args);
            switch (command)
            {
                case "dump":
                    DumpRules(parsedArgs);
                    break;
                case "custom":
                    if (!CustomRule(parsedArgs))
                    {
                        Console.WriteLine("Failed to create custom rule.");
                        Console.WriteLine("\nExample Usage");
                        Console.WriteLine("SharpFireWall.exe custom   /name:AllowRdp /proto:tcp /direction:in /action:allow /remoteport:* /remoteaddr:192.168.56.100,10.0.0.1/24 /localport:3389 /localaddr:*");
                        break;
                    }
                    Console.WriteLine("Successfully Created Rule.");
                    break;
                case "app":
                    if (!AppRule(parsedArgs))
                    {
                        Console.WriteLine("Failed to create application rule.");
                        Console.WriteLine("\nExample Usage");
                        Console.WriteLine("SharpFilewall.exe app      /name:BlockWinDefend /proto:tcp /direction:out /action:block /appname:windefend");
                        break;
                    }
                    Console.WriteLine("Successfully Created Rule.");
                    break;
                case "service":
                    if (!ServiceRule(parsedArgs))
                    {
                        Console.WriteLine("Failed to create service rule.");
                        Console.WriteLine("\nExample Usage");
                        Console.WriteLine("SharpFilewall.exe service  /name:BlockWinDefend /proto:tcp /direction:out /action:block /service:sense");
                        break;
                    }
                    Console.WriteLine("Successfully Created Rule.");
                    break;
                case "delete":
                    if (!DeleteRule(parsedArgs))
                    {
                        Console.WriteLine("Failed to delete rule.");
                        Console.WriteLine("\nExample Usage");
                        Console.WriteLine("SharpFirewall.exe delete   /name:AllowRdp");
                        break;
                    }
                    Console.WriteLine("Successfully Deleted Rule.");
                    break;
                case "disable":
                    if (!DisableRule(parsedArgs))
                    {
                        Console.WriteLine("Failed to disable rule.");
                        Console.WriteLine("\nExample Usage");
                        Console.WriteLine("SharpFirewall.exe disable  /name:AllowRdp");
                    }
                    Console.WriteLine("Successfully Disabled Rule.");
                    break;
                case "enable":
                    if (!EnableRule(parsedArgs))
                    {
                        Console.WriteLine("Failed to enable rule.");
                        Console.WriteLine("\nExample Usage");
                        Console.WriteLine("SharpFirewall.exe enable  /name:AllowRdp");
                    }
                    Console.WriteLine("Successfully Enabled Rule.");
                    break;
                default:
                    Help();
                    break;
            }

        }

        public static void DumpRules(Dictionary<string, string> args)
        {
            if (args.TryGetValue("/name", out string rulename)) { Console.WriteLine(FirewallManager.DumpRules(rulename)); }
            else { Console.WriteLine(FirewallManager.DumpRules()); }
        }
        public static bool CustomRule(Dictionary<string,string> args)
        {
            if (!args.TryGetValue("/name", out string rulename)) { return false; }
            if (!args.TryGetValue("/proto", out string protocol)) { return false; }
            if (!args.TryGetValue("/direction", out string direction)) { return false; }
            if (!args.TryGetValue("/action", out string action)) { return false; }
            if (!args.TryGetValue("/localport", out string localport)) { return false; }
            if (!args.TryGetValue("/localaddr", out string localaddr)) { return false; }
            if (!args.TryGetValue("/remoteport", out string remoteport)) { return false; }
            if (!args.TryGetValue("/remoteaddr", out string remoteaddr)) { return false; }
            return FirewallManager.AddCustomRule(protocol, rulename, direction, action, localport, localaddr, remoteport, remoteaddr);
        }
        public static bool AppRule(Dictionary<string,string> args) {
            if (!args.TryGetValue("/name",out string rulename)) { return false; }
            if (!args.TryGetValue("/proto",out string protocol)) { return false; }
            if (!args.TryGetValue("/direction", out string direction)) { return false; }
            if (!args.TryGetValue("/action", out string action)) { return false; }
            if (!args.TryGetValue("/appname", out string appname)) { return false; }
            return FirewallManager.AddRuleByApplicationName(protocol, rulename, direction, action, appname);
        }

        public static bool ServiceRule(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("/name", out string rulename)) { return false; }
            if (!args.TryGetValue("/proto", out string protocol)) { return false; }
            if (!args.TryGetValue("/direction", out string direction)) { return false; }
            if (!args.TryGetValue("/action", out string action)) { return false; }
            if (!args.TryGetValue("/service", out string service)) { return false; }
            return FirewallManager.AddRuleByServiceName(protocol, rulename, direction, action,service);
        }
        public static bool DeleteRule(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("/name", out string rulename)) { return false; }
            return FirewallManager.DeleteRule(rulename);
        }
        public static bool DisableRule(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("/name", out string rulename)) { return false; }
            return FirewallManager.DisableRule(rulename);
        }
        public static bool EnableRule(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("/name", out string rulename)) { return false; }
            return FirewallManager.EnableRule(rulename);
        }
    }
}