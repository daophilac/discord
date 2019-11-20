using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Censorship.Tools {
    internal class CustomTimer : Timer {
        private int numInterval = 0;
        internal int NumInterval {
            get => numInterval;
            set {
                if (State != TimerState.Rest) {
                    throw new InvalidOperationException("Cannot set NumInterval while timer is not in Rest state.");
                }
                if (value < 0) {
                    throw new ArgumentOutOfRangeException("NumInterval");
                }
                numInterval = value;
            }
        }
        internal int CurrentNumInterval { get; private set; } = 0;
        internal TimerState State { get; private set; } = TimerState.Rest;
        internal new event EventHandler<ElapsedEventArgs> Elapsed;
        internal event EventHandler Stopped;
        internal CustomTimer(double interval) : base(interval) { }
        internal new void Start() {
            State = TimerState.Running;
            if (NumInterval != 0) {
                base.Elapsed += CustomTimer_Elapsed;
            }
            base.Start();
        }

        internal new void Stop() {
            base.Elapsed -= CustomTimer_Elapsed;
            base.Stop();
            State = TimerState.Rest;
            CurrentNumInterval = 0;
            Stopped?.Invoke(this, EventArgs.Empty);
        }
        internal void Pause() {
            State = TimerState.Pausing;
        }
        internal void Resume() {
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
        internal enum TimerState {
            Rest, Running, Pausing
        }
    }
}
