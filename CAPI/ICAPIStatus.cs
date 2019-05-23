namespace HomeSeer.PluginSdk.CAPI {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public interface ICAPIStatus {
        string Status     { get; set; }
        string StatusHTML { get; set; }
        string ImageFile  { get; set; }
        string ClassName  { get; set; }
        double Value      { get; set; }
    }

}