
# SharpFirwall
Add or delete firewall rules. With C#

## Examples
```
SharpFireWall.exe custom     /name:AllowRdp /proto:tcp /direction:in /action:allow /remoteport:* /remoteaddr:* /localport:3389 /localaddr:*
SharpFilewall.exe service    /name:BlockWinDefend /proto:tcp /direction:out /action:block /service:sense
SharpFilewall.exe app        /name:BlockWinDefend /proto:tcp /direction:out /action:block /appname:windefend
donut.exe .\SharpFireWall.exe -c 'SharpFirewall.Program' -m 'Main' -p 'custom /name:AllowRdp /proto:tcp /direction:in /action:allow /remoteport:* /remoteaddr:* /localport:3389 /localaddr:*' -o fw.bin
```



```
_____ __                     _____                         ____
  / ___// /_  ____ __________  / __(_)_______ _      ______ _/ / /
  \__ \/ __ \/ __ `/ ___/ __ \/ /_/ / ___/ _ \ | /| / / __ `/ / /
 ___/ / / / / /_/ / /  / /_/ / __/ / /  /  __/ |/ |/ / /_/ / / /
/____/_/ /_/\__,_/_/  / .___/_/ /_/_/   \___/|__/|__/\__,_/_/_/
                     /_/
                ::: latortuga71 :::

SharpFileWall.exe <mode> <args...>
Modes -> dump, custom, app, service, delete

 ::: Custom mode required args :::
/name:<name>                 Name for the rule
/proto:<protocol>            Protocol (tcp,udp)
/direction:<direction>       Traffic Direction (in,out)
/action:<action>             Action to take (block,allow)
/remoteport:<rports>         Remote ports to allow (55,1000,99-125) or (* for wildcard)
/remoteaddr:<raddr>          Remote addresses to allow (10.0.0.19,192.168.56.1/24,10.0.0.1-10.0.0.255) or (* for wildcard)
/localport:<lports>          Local ports to allow (55,1000,99-125) or (* for wildcard)
/localaddr:<laddr>           Local addresses to allow (10.0.0.19,192.168.56.1/24,10.0.0.1-10.0.0.255) or (* for wildcard)

 ::: App mode required args :::
/name:<name>                 Name for the rule
/proto:<protocol>            Protocol (tcp,udp)
/direction:<direction>       Traffic Direction (in,out)
/action:<action>             Action to take (block,allow)
/appname:<name>              Application name/path to block or allow

 ::: Service mode required args :::
/name:<name>                 Name for the rule
/proto:<protocol>            Protocol (tcp,udp)
/direction:<direction>       Traffic Direction (in,out)
/action:<action>             Action to take (block,allow)
/service:<name>              Service name to block or allow

 ::: Delete mode required args :::
/name:<name>                 Name for the rule

 ::: Dump mode optional args :::
/name:<name>                 Name for the rule

 ::: Example Usage :::
SharpFireWall.exe custom     /name:AllowRdp /proto:tcp /direction:in /action:allow /remoteport:5000-6000 /remoteaddr:192.168.56.0/24,10.0.0.1 /localport:3389,80 /localaddr:*
SharpFilewall.exe service    /name:BlockWinDefend /proto:tcp /direction:out /action:block /service:sense
SharpFilewall.exe app        /name:BlockWinDefend /proto:tcp /direction:out /action:block /appname:windefend
SharpFirewall.exe delete     /name:AllowRdp
SharpFirewall.exe dump       /name:Skype
SharpFirewall.exe dump

 ::: Notes :::
When passing ports the following are all valid -> [80,80-90,*]
When passing ips the following are all valid -> [10.0.0.1,10.0.0.1/24,10.0.0.1-10.0.0.255,*]

```

## Notes
* Requires Elevated Privileges 

## To do
* Add update rule mode
