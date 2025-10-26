using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionFlats
{
    internal class VoyManager
    {
        WebIO voyWeb;
        VoyCommand voyCmd;
        VoyEvent voyEvnt;

        public VoyManager(string userName, string userPassword)
        {
            string json;
            dynamic cmdRsp;
            string responseData;

            voyWeb = new();
            voyCmd = new();
            voyEvnt = new();

            LogEvent lg = new LogEvent();

            //Connect
            voyWeb = new WebIO();
            responseData = voyWeb.ReadWebIO();
            List<dynamic> eventList1 = voyEvnt.VoyDeserializeAndCull(responseData);
            foreach (dynamic cmd in eventList1)
            {
                if (cmd.Event == "Version")
                    lg.LogIt(String.Format("Connected Host: {0}", cmd.Host));
            }
            //Authenticate
            json = voyCmd.AuthenticateConnectionCmd(userName, userPassword);
            lg.LogIt(String.Format("Authenticate Send: {0}", json));
            voyWeb.WriteWebIO(json);
            responseData = voyWeb.ReadWebIO();
            List<dynamic> eventList2 = voyEvnt.VoyDeserializeAndCull(responseData);
            foreach (dynamic cmd in eventList2)
            {
                if (cmd != null && cmd.authbase != null)
                {
                    lg.LogIt(String.Format("Authorization successful {0}", cmd.authbase.Username));
                    Task.Run(() => { HeartBeatGeneratorTask(); });
                }
                else
                    lg.LogIt(String.Format("Autorization failed {0}", cmd.error.message));
            }
        }

        public void Disconnect()
        {
            //Disconnect
            LogEvent lg = new LogEvent();

            string json = voyCmd.DisconnectCmd();
            lg.LogIt(String.Format("Disconnect Sent: {0}", json));
            string responseData = voyWeb.ReadWebIO();
            lg.LogIt(String.Format("Received: {0}", responseData));
        }

        public void Rotate(int pa)
        {
            string json;

            LogEvent lg = new LogEvent();

            json = voyCmd.RemoteRotatorMoveToCmd(pa);
            lg.LogIt(String.Format("Rotate PA Send: {0}", pa.ToString()));
            voyWeb.WriteWebIO(json);
            do
            {
                string responseData = voyWeb.ReadWebIO();
                List<dynamic> eventList2 = voyEvnt.VoyDeserializeAndCull(responseData);
                foreach (dynamic cmd in eventList2)
                {
                    if (cmd != null && cmd.Event == "RemoteActionResult")
                    {
                        lg.LogIt(String.Format("Rotation Command Complete {0}", cmd.ActionResultInt.ToString()));
                        return;
                    }
                }
            } while (true);
        }

        public void ImageFlat(string seqPath)
        {
            string json;

            LogEvent lg = new LogEvent();
            json = voyCmd.RemoteFlatCmd(Path.GetFileName(seqPath), LoadFlatSequenceFile(seqPath));
            lg.LogIt(String.Format("Flat Sequence Send: {0}", Path.GetFileName(seqPath)));
            voyWeb.WriteWebIO(json);
            do
            {
                string responseData = voyWeb.ReadWebIO();
                List<dynamic> eventList2 = voyEvnt.VoyDeserializeAndCull(responseData);
                foreach (dynamic cmd in eventList2)
                {
                    if (cmd != null && cmd.Event == "RemoteActionResult")
                    {
                        lg.LogIt(String.Format("Flat Sequence Complete {0}", cmd.ActionResultInt.ToString()));
                        return;
                    }
                }
            } while (true);
        }

        public void MountAction(VoyCommand.MountAction mAct)
        {
            string json;

            LogEvent lg = new LogEvent();
            json = voyCmd.RemoteMountFastCommandCmd(mAct);
            lg.LogIt(String.Format("Mount Command Send: {0}", (int)mAct));
            voyWeb.WriteWebIO(json);
            do
            {
                string responseData = voyWeb.ReadWebIO();
                List<dynamic> eventList2 = voyEvnt.VoyDeserializeAndCull(responseData);
                foreach (dynamic cmd in eventList2)
                {
                    if (cmd != null && cmd.Event == "RemoteActionResult")
                    {
                        lg.LogIt(String.Format("Mount Move Complete {0}", cmd.ActionResultInt.ToString()));
                        return;
                    }
                }
            } while (true);
        }

        public List<dynamic> CmdResults(string eventName)
        {
            LogEvent lg = new LogEvent();
            bool isLinked = true;
            do
            {
                string responseData = voyWeb.ReadWebIO();
                List<string> eventList = responseData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string eventString in eventList)
                {
                    lg.LogIt(String.Format("From Server: {0}", responseData));
                    dynamic evnt = voyEvnt.VoyDeserializeAndCull(eventString);
                    if (evnt != null && evnt.Event == eventName)
                        return evnt;
                }
                Thread.Sleep(1000);
            } while (isLinked);
            return null;
        }

        public void HeartBeat()
        {
            //LogEvent lg = new LogEvent();
            //lg.LogIt("Polling Voyager");
            string json = voyCmd.PollServerCmd("McKilolani");
            voyWeb.WriteWebIO(json);
        }

        public Task HeartBeatGeneratorTask()
        {
            PollEvent poll = new PollEvent();
            do
            {
                Thread.Sleep(4000);
                poll.PollIt();
            } while (true);
            return Task.CompletedTask;
        }

        public Task TimeOutGeneratorTask(int timeoutInSec, ref bool flag )
        {
            flag = false;
            Thread.Sleep(timeoutInSec * 1000);
            flag = true;
            return Task.CompletedTask;
        }

        public string LoadFlatSequenceFile(string path)
        {
            //string seqText = null;
            StreamReader tr = new StreamReader(path);
            string? seqText = tr.ReadToEnd();
            tr.Close();
            byte[] asciiText = Encoding.ASCII.GetBytes(seqText);
            string base64 = System.Convert.ToBase64String(asciiText);
            return base64;
        }
    }

}
