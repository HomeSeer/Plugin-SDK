using HomeSeer.PluginSdk.CAPI;
using HSCF.Communication.ScsServices.Service;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [ScsService]
    public interface IHSApplication2 {

        double APIVersion { get; }

        CAPIControl[] GetCAPIControl(int dvRef);
        
        CAPIControl CAPIGetSingleControlByUse(int dvRef, Constants.ePairControlUse UseType);

    }

}