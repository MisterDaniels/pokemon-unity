using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Admin {

    public class DebugCommandBase {

        private string commandId;
        private string commandDescription;
        private string commandFormat;

        public string CommandId { 
            get { return commandId; }
        }

        public string CommandDescription {
            get { return commandDescription; }
        }

        public string CommandFormat {
            get { return commandFormat; }
        }

        public DebugCommandBase(string id, string description, string format) {
            commandId = id;
            commandDescription = description;
            commandFormat = format;
        }

    }

}