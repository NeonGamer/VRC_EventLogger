using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Net.Sockets;

namespace VRCLogging
{
    public class VRCEventLogger
    {
        public event Action<string> OnPlayerJoin, OnPlayerLeft;
        public event Action OnFoundSDK2Avatar, OnFoundSDK3Avatar, OnCheckForVRCPlus, OnPortalDropped, OnRoomExit, OnRoomEnter;
        public event Action<string, bool, string> OnGetVRCPlusDetails;

        private string outputLogRoot = @"/LocalLow/VRChat/VRChat";
        private FileInfo logFile;
        private string previousContent = "";

        public void UpdateLogFile()
        {
            string appdataLoco = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            foreach (var file in new DirectoryInfo(appdataLoco.Substring(0, appdataLoco.LastIndexOf("\\")) + outputLogRoot).GetFiles().OrderByDescending(x => x.LastWriteTime))
            {
                if (file.Name.EndsWith(".txt"))
                {
                    logFile = file;
                    break;
                }
            }
        }

        public string UpdateContent()
        {
            string newContent;
            using (var fs = new FileStream(logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                newContent = sr.ReadToEnd();
            }
            return newContent;
        }

        public VRCEventLogger()
        {
            if (logFile == null) UpdateLogFile();

            using (var fs = new FileStream(logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                previousContent = sr.ReadToEnd();
            }
        }

        public void Update()
        {
            string newContent = UpdateContent();

            if (newContent != previousContent)
            {
                foreach (var line in newContent.Replace(previousContent, "").Split('\n'))
                {
                    if (line.Contains("[Behaviour] OnPlayerJoined "))
                    {

                        var start = line.IndexOf("[Behaviour] OnPlayerJoined ");
                        OnPlayerJoin?.Invoke(line.Substring(start + "[Behaviour] OnPlayerJoined ".Length));
                    }
                    else if (line.Contains("[Behaviour] OnPlayerLeft "))
                    {
                        var start = line.IndexOf("[Behaviour] OnPlayerLeft ");
                        OnPlayerLeft?.Invoke(line.Substring(start + "[Behaviour] OnPlayerLeft ".Length));
                    }
                    else if (line.Contains("Found SDK2 avatar descriptor.")) OnFoundSDK2Avatar?.Invoke();
                    else if (line.Contains("Found SDK3 avatar descriptor.")) OnFoundSDK3Avatar?.Invoke();
                    else if (line.Contains("[Always] Checking For Active VRChatPlus Subscription")) OnCheckForVRCPlus?.Invoke();
                    else if (line.Contains("[Always] Get VRChat Subscription Details! Subscription "))
                    {
                        var idStart = line.LastIndexOf("Id:") + "Id:".Length;
                        var activeStart = line.LastIndexOf("active:") + "active:".Length;
                        var descStart = line.LastIndexOf("desc:") + "desc:".Length;
                        var id = line.Substring(idStart, line.IndexOf("active:") - idStart);
                        var active = line.Substring(activeStart, line.IndexOf("desc:") - activeStart) == "True";
                        var desc = line.Substring(descStart);
                        OnGetVRCPlusDetails?.Invoke(id, active, desc);
                    }
                    else if (line.Contains("[Behaviour] Beginning room transition")) OnRoomExit?.Invoke();
                    else if (line.Contains("[Behaviour] Finished entering world")) OnRoomEnter?.Invoke();
                }
            }

            previousContent = newContent;
        }
    }
}
