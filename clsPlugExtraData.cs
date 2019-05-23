using System;

namespace HomeSeer.PluginSdk {

    public class clsPlugExtraData {

        private System.Collections.SortedList Ncol = null;
        private System.Collections.ArrayList UNcol = null;
        // 
        // ======================================================================
        // 
        private void CheckNamed()
        {
            if (Ncol == null)
                Ncol = new System.Collections.SortedList();
        }
        private void CheckUnNamed()
        {
            if (UNcol == null)
                UNcol = new System.Collections.ArrayList();
        }
        // 
        // ======================================================================
        // 
        public int NamedCount()
        {
            if (Ncol == null)
                return 0;
            return Ncol.Count;
        }
    
        public int UnNamedCount()
        {
            if (UNcol == null)
                return 0;
            return UNcol.Count;
        }
        // 
        // ======================================================================
        // 
        public bool AddNamed(string Key, object Obj)
        {
            try
            {
                if (Key == null)
                    return false;
                if (string.IsNullOrEmpty(Key.Trim()))
                    return false;
                if (Obj == null)
                    return false;
                CheckNamed();
                lock (Ncol.SyncRoot)
                {
                    try
                    {
                        if (Ncol.ContainsKey(Key.Trim().ToLower()))
                            return false;
                        lock (Ncol.SyncRoot)
                            Ncol.Add(Key.Trim().ToLower(), Obj);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    
        public int AddUnNamed(object Obj)
        {
            try
            {
                if (Obj == null)
                    return -1;
                CheckUnNamed();
                lock (UNcol.SyncRoot)
                    return UNcol.Add(Obj);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        // 
        // ======================================================================
        // 
        public bool RemoveNamed(string Key)
        {
            try
            {
                if (Key == null)
                    return false;
                if (string.IsNullOrEmpty(Key.Trim()))
                    return false;
                CheckNamed();
                try
                {
                    lock (Ncol.SyncRoot)
                    {
                        if (!Ncol.ContainsKey(Key.Trim().ToLower()))
                            return false;
                        Ncol.Remove(Key.Trim().ToLower());
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    
        public bool RemoveUnNamed(int Index)
        {
            try
            {
                if (Index < 0)
                    return false;
                CheckUnNamed();
                if (Index > UNcol.Count - 1)
                    return false;
                try
                {
                    lock (UNcol.SyncRoot)
                        UNcol.RemoveAt(Index);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool RemoveUnNamed(object Obj)
        {
            try
            {
                if (Obj == null)
                    return false;
                CheckUnNamed();
                try
                {
                    lock (UNcol.SyncRoot)
                        UNcol.Remove(Obj);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // 
        // ======================================================================
        // 
        public object GetNamed(string Key)
        {
            try
            {
                if (Key == null)
                    return null;
                if (string.IsNullOrEmpty(Key.Trim()))
                    return null;
                CheckNamed();
                lock (Ncol.SyncRoot)
                {
                    if (!Ncol.ContainsKey(Key.Trim().ToLower()))
                        return null;
                    return Ncol[Key.Trim().ToLower()];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public object GetNamed(int Index)
        {
            try
            {
                if (Index < 0)
                    return null;
                CheckNamed();
                if (Index > (Ncol.Count - 1))
                    return null;
                return Ncol.GetByIndex(Index);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetNamedKey(int Index)
        {
            try
            {
                if (Index < 0)
                    return "";
                CheckNamed();
                lock (Ncol)
                {
                    if (Index > (Ncol.Count - 1))
                        return "";
                    return Ncol.GetKey(Index).ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string[] GetNamedKeys()
        {
            try
            {
                if (Ncol == null)
                    return null;
                System.Collections.IList Keys;
                lock (Ncol.SyncRoot)
                    Keys = Ncol.GetKeyList();
                System.Collections.Generic.List<string> col = new System.Collections.Generic.List<string>();
                for (int i = 0; i <= Keys.Count - 1; i++)
                    col.Add(Keys[i].ToString());
                return col.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    
        public object GetUnNamed(int Index)
        {
            try
            {
                if (Index < 0)
                    return null;
                CheckUnNamed();
                if (Index > (UNcol.Count - 1))
                    return null;
                lock (UNcol.SyncRoot)
                    return UNcol[Index];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public object[] GetAllUnNamed()
        {
            try
            {
                if (UNcol == null)
                    return null;
                CheckUnNamed();
                if (UNcol.Count < 1)
                    return null;
                lock (UNcol.SyncRoot)
                    return UNcol.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // 
        // ======================================================================
        // 
        public void ClearAllNamed(bool Confirm)
        {
            if (!Confirm)
                return;
            try
            {
                CheckNamed();
                lock (Ncol.SyncRoot)
                    Ncol.Clear();
            }
            catch (Exception ex)
            {
            }
        }
    
        public void ClearAllUnNamed(bool Confirm)
        {
            if (!Confirm)
                return;
            try
            {
                CheckUnNamed();
                lock (UNcol.SyncRoot)
                    UNcol.Clear();
            }
            catch (Exception ex)
            {
            }
        }

    }

}