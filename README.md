# VRC_EventLogger

https://github.com/ValdemarOrn/SharpOSC/

### This is a repo that makes detecting player joins, leaves, and more possible through a external program.
```c# Example using player join, leave, and portal dropped-> 
var vrcLogger = new VRCEventLogger();
vrcLogger.OnPlayerJoin += new Action<string>(s => lastestLogMessages.Add(s + " Joined!"));
vrcLogger.OnPlayerLeft += new Action<string>(s => lastestLogMessages.Add(s + " Left!"));
vrcLogger.OnPortalDropped += new Action(() => lastestLogMessages.Add("Portal Dropped!"));
```
#### If it seems to not be detecting any logs try upodating the current log file it is looking at by calling the \"UpdateLogFile\" method on your vrcLogger object.


