using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Admin {

    public class DebugCommand : DebugCommandBase {

        private Action command;

        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format) {
            this.command = command;
        }

        public void Invoke() {
            command.Invoke();
        }

    }

    public class DebugCommand<T1> : DebugCommandBase {

        private Action<T1> command;

        public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format) {
            this.command = command;
        }

        public void Invoke(T1 value) {
            command.Invoke(value);
        }

    }

    public class DebugCommand<T1, T2> : DebugCommandBase {

        private Action<T1, T2> command;

        public DebugCommand(string id, string description, string format, Action<T1, T2> command) : base(id, description, format) {
            this.command = command;
        }

        public void Invoke(T1 value, T2 optional) {
            command.Invoke(value, optional);
        }

    }

}