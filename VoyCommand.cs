using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static SessionFlats.VoyEvent;

namespace SessionFlats
{
    internal class VoyCommand
    {
        private int methodCounter = 1;
        private string UID;

        public VoyCommand()
        {
            Guid myuuid = Guid.NewGuid();
            UID = myuuid.ToString();
        }

         public string PollServerCmd(string voyagerHostName)
        {
            CommonEvent poll = new CommonEvent()
            {
                Event = "Polling",
                Timestamp = (int)DateTime.Now.Ticks,
                Host = voyagerHostName,
                Inst = 1
            };
            return (JsonConvert.SerializeObject(poll));
        }

        public string AuthenticateConnectionCmd(string user, string password)
        {
            AuthenticateUserCmdType authParams = new AuthenticateUserCmdType()
            {
                UID = Guid.NewGuid().ToString(),
                Base = AuthenticationBaseGenerator(user, password)
            };
            CommandWithParamsWrapper authMethod = new CommandWithParamsWrapper
            {
                method = "AuthenticateUserBase",
                @params = authParams,
                id = methodCounter++
            };
            return (JsonConvert.SerializeObject(authMethod));
        }

        public string RemoteGetVoyagerProfilesCmd()
        {
            RemoteGetVoyagerProfilesCmdType rgp = new RemoteGetVoyagerProfilesCmdType()
            {
                UID = this.UID
            };
            CommandWithParamsWrapper rgpMethod = new CommandWithParamsWrapper
            {
                method = "RemoteGetVoyagerProfiles",
                @params = rgp,
                id = methodCounter++
            };
            return (JsonConvert.SerializeObject(rgpMethod));
        }


        public string DisconnectCmd()
        {
            RemoteGetVoyagerProfilesCmdType dis = new RemoteGetVoyagerProfilesCmdType()
            {
                UID = this.UID
            };
            CommandWithParamsWrapper disconnectMethod = new CommandWithParamsWrapper
            {
                method = "disconnect",
                id = methodCounter++
            };
            return (JsonConvert.SerializeObject(disconnectMethod));
        }

        public string RemoteRotatorMoveToCmd(int rotPA)
        {
            RemoteRotatorMoveToCmdType rot = new()
            {
                UID = this.UID,
                PA = rotPA,
                IsWaitAfter = false,
                WaitAfterSeconds = 0,
            };
            CommandWithParamsWrapper remoteRotatorMoveToMethod = new CommandWithParamsWrapper
            {
                method = "RemoteRotatorMoveTo",
                @params = rot,
                id = methodCounter++
            };
            return JsonConvert.SerializeObject(remoteRotatorMoveToMethod);
        }

        public string RemoteFlatCmd(string fPath, string seqBase)
        {
            RemoteFlatCmdType rfs = new()
            {
                UID = this.UID,
                IsOnlyForRemote = true,
                RemoteConfigurationFile = fPath,
                DataBase64 = seqBase
            };
            CommandWithParamsWrapper remoteFlatMethod = new CommandWithParamsWrapper
            {
                method = "RemoteFlat",
                @params = rfs,
                id = methodCounter++
            };
            return JsonConvert.SerializeObject(remoteFlatMethod);
        }

        public string RemoteMountFastCommandCmd(MountAction cmdType)
        {
            RemoteMountFastCmdType mCmd = new()
            {
                UID = this.UID,
                CommandType = (int)cmdType
            };
            CommandWithParamsWrapper mCmdMethod = new CommandWithParamsWrapper
            {
                method = "RemoteMountFastCommand",
                @params = mCmd,
                id = methodCounter++
            };
            return JsonConvert.SerializeObject(mCmdMethod);
        }

        class CommandWithParamsWrapper
        {
            public string? method { get; set; }
            public object? @params { get; set; }
            public int? id { get; set; }
        }

        class CommandWithoutParamsWrapper
        {
            public string? method { get; set; }
            public int? id { get; set; }
        }

        class DisconnectCmdType()
        {
            public int? id { get; set; }
        }

        class RemoteGetVoyagerProfilesCmdType
        {
            public string UID { get; set; }
        }

        class AuthenticateUserCmdType
        {
            public string UID { get; set; }
            public string Base { get; set; }
        }

        class GetArrayElementsDataCmdType
        {

        }

        class RemoteRotatorMoveToCmdType
        {
            public string UID; //String Unique identifier of the Action to abort.
                               //Use a Guide Window identifier or a unique
                               //key string generated
            public int PA; //Number Position angle in Degree
            public bool IsWaitAfter; // Boolean true if you want to wait an interval of
                                     // seconds after driver report rotation
                                     //finished
            public int WaitAfterSeconds; //Integer Number of second to wait
        }

        class RemoteFlatCmdType()
        {
            public string UID; // String Unique identifier of the Action to abort.Use
                               //a Guide Window identifier or a unique key
                               //string generated
            public bool IsOnlyForRemote; // Boolean Use always true
            public string RemoteConfigurationFile; // String Only File name with extension of Voyager
                                                   //Sequence Flat File to use
            public string DataBase64; // String File data of the Sequence Flat File to use
                                      //converted to Base64 coding ascii text
        }

        class RemoteMountFastCmdType()
        {
            public string UID;
            public int CommandType;
        }

        string AuthenticationBaseGenerator(string username, string password)
        {
            string concat = username + ":" + password;
            byte[] octet = Encoding.UTF8.GetBytes(concat);
            string base64 = System.Convert.ToBase64String(octet);
            return base64;
        }

        public enum MountAction
        {
            Track_On = 1,
            Track_Off = 2,
            Park = 3,
            Unpark = 4,
            Goto_Near_Zenith = 5,
            Home = 6
        }
    }
}
