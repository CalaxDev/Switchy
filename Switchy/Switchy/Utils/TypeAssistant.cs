using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switchy.Utils
{
    //Taken from https://stackoverflow.com/questions/33776387/dont-raise-textchanged-while-continuous-typing
    public class TypeAssistant
    {
        public event EventHandler Idled = delegate { };
        public int WaitingMilliSeconds { get; set; }
        private System.Threading.Timer _waitingTimer;

        public TypeAssistant(int waitingMilliSeconds = 100)
        {
            WaitingMilliSeconds = waitingMilliSeconds;
            _waitingTimer = new System.Threading.Timer(p =>
            {
                Idled(this, EventArgs.Empty);
            });
        }
        public void TextChanged()
        {
            _waitingTimer.Change(WaitingMilliSeconds, Timeout.Infinite);
        }
    }
}
