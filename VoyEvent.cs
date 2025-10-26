using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SessionFlats
{
    internal class VoyEvent
    {
        public string PollingEvent()
        {
            Polling poll = new Polling();
            CommonEvent vent = new CommonEvent()
            {
                Event = "Polling",
                Timestamp = (int)DateTime.Now.Ticks,
                Host = "McOlympia",
                Inst = 1
            };
            string json = JsonConvert.SerializeObject(vent);
            return json;
        }

        class AuthenticateUserEventType
        {
            public string Username { get; set; }    // string Username of the user for unique identify
            public string FirstName { get; set; }   // string First name of remote user
            public string LastName { get; set; }    // string Last name of remote user
            public string Mail { get; set; }        // string Mail of remote user
            public int PermissionsA { get; set; }   // integer Permission associated to the user.Information
                                                    //Reserved to NDA and agreement.Please ask to
                                                    //Voyager support for a contact.
            public int PermissionsB { get; set; }   // integer Permission associated to the user.Information
                                                    //Reserved to NDA and agreement. Please ask to
                                                    //Voyager support for a contact.
            public string Note { get; set; }         // string Note for the remote use
        }



        public List<dynamic> VoyDeserializeAndCull(string responseData)
        {
            List<dynamic> eventList = new();
            List<string> evl = responseData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string cmd in evl)
            {
                dynamic cmdStream = Newtonsoft.Json.JsonConvert.DeserializeObject(cmd);
                if (cmdStream != null && cmdStream.Event != "Polling")
                    eventList.Add(cmdStream);
            }
            return eventList;
        }

        public class CommonEvent
        {
            public string? Event { get; set; }       // String the name of the event
            public int? Timestamp { get; set; }   // number the timestamp of the event in seconds from the epoch, including fractional seconds
            public string? Host { get; set; }       // String the hostname of the machine running VOYAGER
            public int? Inst { get; set; }           // Integer the VOYAGER instance number (1-based
        }

        public class Version
        {
            public string? VOYVersion { get; set; }  // String the version of Voyager
            public string? VOYSubver { get; set; }   // String the subversion of Voyager if present
            public int? MsgVersion { get; set; }     // Integer The numeric version of protocol implemented in this version of Voyager
        }

        public class Polling
        {

        }

        public struct Signal
        {
            public int? SignalCode { get; set; }      //Integer The numeric index of Signal happen.
        }

        public enum ActionResult
        {
            NEED_INIT = 0, // Wait to Running
            READY = 1, //Ready to Running
            RUNNING = 2,//Running
            PAUSE = 3, //Paused
            OK = 4, //Finished
            FINISHED_ERROR = 5, //Finished with Error
            ABORTING = 6, //Abort request waiting during running
            ABORTED = 7, //Finished aborted
            TIMEOUT = 8,//Finished timeout
            TIME_END = 9, //Finished for timer end
            OK_PARTIAL = 10 //FInished with some task not executec
        }

        public enum SignalCodeType
        {
            AutoFocusError = 1,
            RunningQueueIsEmpty = 2,
            PrecisePointingAutoFocusAllNodes = 3,
            AutoFocus = 4,
            AutoFlatSingleNode = 5,
            AutoFocusSingleNode = 6,
            ConnectSetupnode = 7,
            DisconnectSetupAllNodes = 8,
            FilterChangeAllNodes = 9,
            GetActualSingleNode = 10,
            FocuserMoveFilterSingleNode = 11,
            FocuserOffsetToSingleNode = 12,
            RotatorMoveSingleNode = 13,
            SetupConnectSingleNode = 14,
            SetupDisconnect = 15,
            CameraShot = 16,
            CCDCooling = 18,
            FocuserMove = 19,
            FocuserOffSetTo = 20,
            RotatorGoto = 21,
            AutoFlat = 22,
            FilterChange = 23,
            PlateSolvingTo = 24,
            StarkeeperSoftwareActualLocation = 25,
            Sequence = 26,
            CreateDirectoryonFileSystemSingleNode = 27,
            CCDCoolingSingleNode = 28,
            GetCCDTemperatureSingleNode = 29,
            CameraShotSingleNode = 30,
            TelescopeGoto = 31,
            RunExternalScript = 32,
            AutoFocusLocalFieldAllNodes = 33,
            AutoFocusLocalFieldSingleNode = 34,

            GeneralSignalError = 500,
            Idle = 501,
            Running = 502,
            Stopped = 503,
            Undefined = 504,
            Warning = 505,
            Unknown = 506
        }

    }
}
