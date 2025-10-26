/// Class for creating event handler for passing poll value to 
/// a subscriber form (HumasonForm) and saving it to a file
/// 
/// 
/// The subscribing form class (which wants to display the poll entry
/// 1) instantiates a public field for this class object
///         public static pollger pollstatus = new pollger();
/// 2) Creates the poll file and subscribes a handler to the event subscriber list when initializing the form class
///             pollstatus.Createpoll();
///             pollstatus.pollEventHandler += StatuspollUpdate_Handler;
/// 3) installs a method for handling the event
///         private void StatuspollUpdate_Handler(object sender, pollger.pollEventArgs e)
///         {
///            StatusStripLine.Text = e.pollEntry;
///            Show();
///            return;
///         }
///Then an event publisher gets the pollger object from the controlling form
///            pollger lg = HumasonForm.pollstatus;
///And generates an event whenever it needs to
///            lg.pollIt("Acquiring guide star");
///            

using System;
using System.IO;

namespace SessionFlats
{
    public class PollEvent
    {
        //Event declaration
        public event EventHandler<PollEventArgs> PollEventHandler;

        private void PollEntry()
        {
            OnPollEventHandler(new PollEventArgs());
        }

        // Wrap event invocations inside a protected virtual method
        protected virtual void OnPollEventHandler(PollEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            PollEventHandler?.Invoke(this, e);
        }

        //Class to hold pollging event arguments, i.e. poll entry string
        public class PollEventArgs : System.EventArgs
        {
            private string privateEntry;
            public PollEventArgs()
            {
                this.privateEntry = null;
            }

            public string pollEntry
            {
                get { return privateEntry; }
            }
        }

        //Method for initiating poll event as called from form or class that wants to raise it
        public void PollIt()
        {
            //Gets the current date/time
            //Creates a new poll file, if  not created
            //Opens poll file and appends date-time, poll line and crlf
            //Closes poll file
            //Raises a poll event for anyone who is listening

            PollEntry();
            FormVoyagerSessionFlats.PollVoyagerEvent.PollEntry();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

  }

}

