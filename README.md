# VRC_EventLogger
### This is a repo that makes detecting player joins, leaves, and more possible through a external program.
```c# Example using player join, leave, and portal dropped-> 
var vrcLogger = new VRCEventLogger();
vrcLogger.OnPlayerJoin += new Action<string>(s => lastestLogMessages.Add(s + " Joined!"));
vrcLogger.OnPlayerLeft += new Action<string>(s => lastestLogMessages.Add(s + " Left!"));
vrcLogger.OnPortalDropped += new Action(() => lastestLogMessages.Add("Portal Dropped!"));
```
