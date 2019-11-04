using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Monitor {
    public class CustomTimer : Timer{
        private int numInterval = 0;
        public int NumInterval {
            get => numInterval;
            set {
                if (State != TimerState.Rest) {
                    throw new InvalidOperationException("Cannot set NumInterval while timer is not in Rest state.");
                }
                if (value < 0) {
                    throw new ArgumentOutOfRangeException("NumInterval cannot be a negative number.");
                }
                numInterval = value;
            }
        }
        public int CurrentNumInterval { get; private set; } = 0;
        public TimerState State { get; private set; } = TimerState.Rest;
        public new event EventHandler<ElapsedEventArgs> Elapsed;
        public event EventHandler Stopped;
        public CustomTimer(double interval) : base(interval) { }
        public new void Start() {
            State = TimerState.Running;
            if (NumInterval != 0) {
                base.Elapsed += CustomTimer_Elapsed;
            }
            base.Start();
        }

        public new void Stop() {
            base.Elapsed -= CustomTimer_Elapsed;
            base.Stop();
            State = TimerState.Rest;
            CurrentNumInterval = 0;
            Stopped?.Invoke(this, EventArgs.Empty);
        }
        public void Pause() {
            State = TimerState.Pausing;
        }
        public void Resume() {
            State = TimerState.Running;
        }
        private void CustomTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if(State == TimerState.Pausing) {
                return;
            }
            if (++CurrentNumInterval == NumInterval) {
                Stop();
            }
            Elapsed?.Invoke(this, e);
        }
        public enum TimerState {
            Rest, Running, Pausing
        }
    }
}
