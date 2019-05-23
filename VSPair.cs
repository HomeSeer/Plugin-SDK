using System;
using HomeSeer.PluginSdk.CAPI;

namespace HomeSeer.PluginSdk {

    public class VSPair {

        public Constants.VSVGPairType PairType;
        private string mvarStatus = "";
        private double mvarValue;
        private Constants.ePairStatusControl mvarPairStatusControl;
        // Private mvarPairProtection As Integer 'ePairProtection = ePairProtection.Off
        private Constants.CAPIControlType mvarPairRender = Constants.CAPIControlType.Not_Specified;
        public Constants.CAPIControlButtonImage PairButtonImageType = Constants.CAPIControlButtonImage.Not_Specified;
        public string PairButtonImage = "";
        private System.Collections.Generic.List<string> mvarStringList;
        public CAPIControlLocation Render_Location;
        public double RangeStart;
        public double RangeEnd;
        public string RangeStatusPrefix = "";
        public string RangeStatusSuffix = "";
        public int RangeStatusDecimals = 0;
        public double RangeStatusDivisor = 0;
        public bool IncludeValues = true;
        public double ValueOffset = 0;
        public bool HasAdditionalData = false;
        public bool HasScale = false;
        public bool ZeroPadding = false;
        public const string ScaleReplace = "@S@";
        private Constants.ePairControlUse mvarControlUse = Constants.ePairControlUse.Not_Specified;
    
        public static string AddDataReplace(int Index)
        {
            return "@%" + Index.ToString() + "@";
        }
        // Public ReadOnly Property Protection As Integer 'ePairProtection
        // Get
        // Return mvarPairProtection
        // End Get
        // End Property
        // Public WriteOnly Property ProtectionSet As Integer 'ePairProtection
        // Set(ByVal value As Integer) 'ePairProtection)
        // mvarPairProtection = value
        // End Set
        // End Property
        public Constants.ePairStatusControl ControlStatus
        {
            get
            {
                return mvarPairStatusControl;
            }
        }
        public Constants.ePairControlUse ControlUse
        {
            get
            {
                // If mvarPairStatusControl = ePairStatusControl.Status Then Return ePairControlUse.Not_Specified
                return mvarControlUse;
            }
            set
            {
                mvarControlUse = value;
            }
        }
        public Constants.ePairStatusControl NewControlStatusUpdateForUtilityOnly
        {
            set
            {
                mvarPairStatusControl = value;
            }
        }
        public Constants.CAPIControlType Render
        {
            get
            {
                return mvarPairRender;
            }
            set
            {
                if (value == Constants.CAPIControlType.List_Text_from_List | value == Constants.CAPIControlType.Single_Text_from_List)
                {
                    if (mvarPairStatusControl == Constants.ePairStatusControl.Control)
                    {
                    }
                    else
                        // List and Single Text are Control-Only render types, so we need to make a change.
                        mvarPairStatusControl = Constants.ePairStatusControl.Control;
                }
                mvarPairRender = value;
            }
        }
        public string Status
        {
            set
            {
                if (value == null)
                    value = "";
                if (string.IsNullOrEmpty(value.Trim()))
                    mvarStatus = "";
                else
                    mvarStatus = value;
            }
        }
        internal string intStatus
        {
            get
            {
                return mvarStatus;
            }
        }
        public double Value
        {
            get
            {
                if (this.PairType == Constants.VSVGPairType.SingleValue)
                    return mvarValue;
                else
                    return RangeStart;
            }
            set
            {
                mvarValue = value;
            }
        }
        public string[] StringList
        {
            get
            {
                if (mvarStringList == null)
                    return null;
                return mvarStringList.ToArray();
            }
            set
            {
                if (value == null)
                {
                    mvarStringList = null;
                    return;
                }
                if (value.Length < 1)
                {
                    mvarStringList = null;
                    return;
                }
                mvarStringList = new System.Collections.Generic.List<string>();
                foreach (string s in value)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    mvarStringList.Add(s);
                }
            }
        }
        public string StringListAdd
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (mvarStringList == null)
                    mvarStringList = new System.Collections.Generic.List<string>();
                mvarStringList.Add(value);
            }
        }
        public string GetPairString(double Value, string Scale, string[] AdditionalData)
        {
            string s = GetPairString(Value);
            if (HasScale)
            {
                if (string.IsNullOrEmpty(Scale))
                    Scale = "";
                s = s.Replace(ScaleReplace, Scale);
            }
            if (HasAdditionalData)
            {
                int idx = -1;
                if (AdditionalData != null && AdditionalData.Length > 0)
                {
                    for (int x = 0; x <= AdditionalData.Length - 1; x++)
                    {
                        idx = x;
                        if (string.IsNullOrEmpty(AdditionalData[x]))
                            AdditionalData[x] = "";
                        s = s.Replace(AddDataReplace(x), AdditionalData[x]);
                    }
                }
                if (idx < 9)
                {
                    for (int x = idx + 1; x <= 9; x++)
                        s = s.Replace(AddDataReplace(x), "");
                }
            }
            return s;
        }
        public double ReverseRangeStatusToControl(double Value)
        {
            if (PairType != Constants.VSVGPairType.Range)
                return mvarValue;
            if (RangeStatusDecimals < 1)
            {
                if (RangeStatusDivisor != 0)
                    return Convert.ToInt32((Value + ValueOffset) * RangeStatusDivisor);
                else
                    return Convert.ToInt32(Value + ValueOffset);
            }
            else if (RangeStatusDivisor != 0)
                return Math.Round(((Value + ValueOffset) * RangeStatusDivisor), RangeStatusDecimals);
            else
                return Math.Round((Value + ValueOffset), RangeStatusDecimals);
        }
        private string GetPairString(double Value)
        {
            if (PairType != Constants.VSVGPairType.Range)
                return mvarStatus;
            if ((Value < RangeStart) | (Value > RangeEnd))
                return "";
            if (RangeStatusPrefix == null)
                RangeStatusPrefix = "";
            if (RangeStatusSuffix == null)
                RangeStatusSuffix = "";
            if (string.IsNullOrEmpty(RangeStatusPrefix.Trim()))
                RangeStatusPrefix = "";
            if (string.IsNullOrEmpty(RangeStatusSuffix.Trim()))
                RangeStatusSuffix = "";
            if (IncludeValues)
            {
                if (RangeStatusDecimals < 1)
                {
                    if (RangeStatusDivisor != 0)
                        return RangeStatusPrefix + Convert.ToInt32((Value - ValueOffset) / RangeStatusDivisor).ToString() + RangeStatusSuffix;
                    else
                        return RangeStatusPrefix + Convert.ToInt32(Value - ValueOffset).ToString() + RangeStatusSuffix;
                }
                else
                {
                    string deci = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                    if (ZeroPadding)
                    {
                        string sVal = "";
                        int addto = 0;
                        // If RangeStatusDivisor <> 0 Then
                        // sVal = Math.Round(((Value - ValueOffset) / RangeStatusDivisor), RangeStatusDecimals).ToString
                        // If sVal.Contains(deci) Then
                        // Dim tmp() As String = sVal.Split(deci)
                        // Try
                        // If tmp(1).Trim.Length < RangeStatusDecimals Then
                        // Return RangeStatusPrefix & tmp(0) & deci & tmp(1).Trim.PadRight(RangeStatusDecimals, "0") & RangeStatusSuffix
                        // Else
                        // Return RangeStatusPrefix & sVal & RangeStatusSuffix
                        // End If
                        // Catch ex As Exception
                        // Return RangeStatusPrefix & sVal & RangeStatusSuffix
                        // End Try
                        // Else
                        // Return RangeStatusPrefix & sVal & RangeStatusSuffix
                        // End If
                        // Else
                        // sVal = Math.Round((Value - ValueOffset), RangeStatusDecimals).ToString
                        // If sVal.Contains(deci) Then
                        // Dim tmp() As String = sVal.Split(deci)
                        // Try
                        // If tmp(1).Trim.Length < RangeStatusDecimals Then
                        // Return RangeStatusPrefix & tmp(0).Trim & deci & tmp(1).Trim.PadRight(RangeStatusDecimals, "0") & RangeStatusSuffix
                        // Else
                        // Return RangeStatusPrefix & sVal & RangeStatusSuffix
                        // End If
                        // Catch ex As Exception
                        // Return RangeStatusPrefix & sVal & RangeStatusSuffix
                        // End Try
                        // Else
                        // Return RangeStatusPrefix & sVal & RangeStatusSuffix
                        // End If
                        // End If
                        if (RangeStatusDivisor != 0)
                            sVal = Math.Round(((Value - ValueOffset) / RangeStatusDivisor), RangeStatusDecimals).ToString();
                        else
                            sVal = Math.Round((Value - ValueOffset), RangeStatusDecimals).ToString();
                        if (sVal.Contains(deci))
                        {
                            var tmp = sVal.Split(deci.ToCharArray());
                            try
                            {
                                if (tmp[1].Trim().Length < RangeStatusDecimals)
                                    return RangeStatusPrefix + tmp[0] + deci + tmp[1].Trim().PadRight(RangeStatusDecimals, '0') + RangeStatusSuffix;
                                else
                                    return RangeStatusPrefix + sVal + RangeStatusSuffix;
                            }
                            catch (Exception ex)
                            {
                                return RangeStatusPrefix + sVal + RangeStatusSuffix;
                            }
                        }
                        else
                            return RangeStatusPrefix + sVal + RangeStatusSuffix;
                    }
                    else
                    {
                        // Dim TC() As Char = {"0"c}
                        if (RangeStatusDivisor != 0)
                        {
                            string Check = Math.Round(((Value - ValueOffset) / RangeStatusDivisor), RangeStatusDecimals).ToString();
                            // If Check.Contains(deci) Then
                            // ' There is a decimal place, so make sure the trailing zeros are removed from the end.
                            // Check = Check.Trim.TrimEnd(TC)
                            // If Check.EndsWith(deci) Then
                            // Check = Check.TrimEnd(deci)
                            // End If
                            // End If
                            return RangeStatusPrefix + Check + RangeStatusSuffix;
                        }
                        return RangeStatusPrefix + Math.Round((Value - ValueOffset), RangeStatusDecimals).ToString() + RangeStatusSuffix;
                    }
                }
            }
            else
                return RangeStatusPrefix + RangeStatusSuffix;
        }
        public string GetRangePairString(double Value, string Scale, string[] AdditionalData)
        {
            string s = GetRangePairString(Value);
            if (HasScale)
            {
                if (string.IsNullOrEmpty(Scale))
                    Scale = "";
                s = s.Replace(ScaleReplace, Scale);
            }
            if (HasAdditionalData)
            {
                int idx = -1;
                if (AdditionalData != null && AdditionalData.Length > 0)
                {
                    for (int x = 0; x <= AdditionalData.Length - 1; x++)
                    {
                        idx = x;
                        if (string.IsNullOrEmpty(AdditionalData[x]))
                            AdditionalData[x] = "";
                        s = s.Replace(AddDataReplace(x), AdditionalData[x]);
                    }
                }
                if (idx < 9)
                {
                    for (int x = idx + 1; x <= 9; x++)
                        s = s.Replace(AddDataReplace(x), "");
                }
            }
            return s;
        }
        private string GetRangePairString(double Value)
        {
            if (PairType != Constants.VSVGPairType.Range)
                return mvarStatus;
            if ((Value < RangeStart) | (Value > RangeEnd))
                return "";
            if (RangeStatusPrefix == null)
                RangeStatusPrefix = "";
            if (RangeStatusSuffix == null)
                RangeStatusSuffix = "";
            if (string.IsNullOrEmpty(RangeStatusPrefix.Trim()))
                RangeStatusPrefix = "";
            if (string.IsNullOrEmpty(RangeStatusSuffix.Trim()))
                RangeStatusSuffix = "";
            // Return RangeStatusPrefix & " (" & RangeStart.ToString & "..." & RangeEnd.ToString & ") " & RangeStatusSuffix
            return RangeStatusPrefix + "(value)" + RangeStatusSuffix;
        }
    
        public VSPair(Constants.ePairStatusControl Status_Control) : base()
        {
            mvarPairStatusControl = Status_Control;
        }

    }

}