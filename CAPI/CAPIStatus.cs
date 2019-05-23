using System;

namespace HomeSeer.PluginSdk.CAPI {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class CAPIStatus : ICAPIStatus {

        private string mvarStatus     = "";
        private string mvarStatusHTML = "";
        private string mvarImageFile  = "";
        private string mvarClass      = "";
        private double mvarValue;

        public string Status {
            get => mvarStatus ?? (mvarStatus = "");
            set => mvarStatus = value ?? "";
        }
        public string StatusHTML {
            get => mvarStatusHTML ?? (mvarStatusHTML = "");
            set => mvarStatusHTML = value ?? "";
        }
        public string ImageFile {
            get => mvarImageFile ?? (mvarImageFile = "");
            set => mvarImageFile = value ?? "";
        }
        public string ClassName {
            get => mvarClass ?? (mvarClass = "");
            set => mvarClass = value ?? "";
        }

        public double Value {
            get => mvarValue;
            set => mvarValue = value;
        }

    }

}