using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeSeer.PluginSdk.Devices {

    [Serializable]
    public class DeviceCollection {

        // Inherits MarshalByRefObject

        private int  CurrentIndex = -1;
        public  bool filterByUser = false;

        private int LastDeviceListSLCount = -1;

        // Private LocalCopy As New HSCollections.HSCollection
        public  HsDevice[] LocalCopy;
        private int           TotalCount = -1;

        public string user = "";

        public bool CountChanged { get; private set; } = true;

        public bool Finished { get; private set; } = true;

        /*public HsDevice GetNext() {
            HsDevice dv = null;
            // Dim dvCheck As DeviceClass
            var bNoGood = false;
            if (Finished)
                return null;
            CurrentIndex += 1;
            if (LastDeviceListSLCount != DeviceListSync.Count)
                CountChanged = true;
            do {
                if (CurrentIndex > TotalCount - 1) {
                    Finished = true;
                    return null;
                }

                bNoGood = false;
                try {
                    // dv = LocalCopy.GetByIndex(CurrentIndex)
                    dv = LocalCopy[CurrentIndex];
                }
                catch (Exception ex) {
                    bNoGood = true;
                }

                if ((dv == null) | bNoGood)
                    CurrentIndex += 1;
                else
                    break;
            } while (true);

            if (CurrentIndex == TotalCount - 1)
                Finished = true;

            return dv;
        }

        public void LoadIt() {
            DictionaryEntry   DE;
            HsDevice dv = null;
            List<HsDevice> col;
            var               iFail = 0;

            try {
                Retry_Error: ;
                col = new List<HsDevice>();
                if (DeviceListSL == null) {
                    DeviceListSync = new SortedList();
                    DeviceListSL   = SortedList.Synchronized(DeviceListSync);
                }

                if (DeviceListSL.Count == 0) {
                    LocalCopy  = null;
                    TotalCount = 0;
                    Finished   = true;
                    return;
                }

                Finished = false;

                lock (DeviceListSync.SyncRoot) {
                    try {
                        foreach (var DE in DeviceListSync) {
                            dv = DE.Value;
                            if (dv != null) {
                                if (filterByUser) {
                                    if (CheckUserHasAccess(user, dv.HSUserAccess))
                                        col.Add(dv);
                                }
                                else {
                                    col.Add(dv);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException exIE) {
                        iFail += 1;
                        if (iFail > 2) {
                            WriteMon("Error",
                                     "Exception creating device enumerator at " + Information.Erl.ToString() + ": " +
                                     exIE.Message + " / " + exIE.InnerException.Message);
                            LocalCopy  = null;
                            TotalCount = 0;
                            Finished   = true;
                            return;
                        }

                        goto Retry_Error;
                    }
                }

                LocalCopy             = col.ToArray();
                TotalCount            = col.Count;
                LastDeviceListSLCount = DeviceListSync.Count;
                CountChanged          = false;
            }
            catch (Exception ex) {
                WriteMon("Error",
                         "Exception creating device enumerator at " + Information.Erl.ToString() + ": " + ex.Message);
                LocalCopy  = null;
                TotalCount = 0;
                Finished   = true;
            }
        }*/

        public void Restart() {
            Finished     = false;
            CurrentIndex = -1;
        }

    }

}