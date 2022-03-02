using NUnit.Framework;

namespace HomeSeer.PluginSdk.Events {

    [TestFixture(
        TestOf = typeof(AbstractActionType),
        Description = "Tests of the AbstractActionType class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class AbstractActionTypeTests {

        //TODO Constructors
        
        //TODO LogDebug
        public void LogDebug_Get_ReturnsDefault() {
            var testActionType = new TestActionType();
            Assert.AreEqual(false, testActionType.LogDebug);
        }

        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var testActionType = new TestActionType {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, testActionType.LogDebug);
        }

        //TODO ActionListener
        //TODO Id
        //TODO EventRef
        //TODO Data
        //TODO Name
        //TODO ToHtml
        //TODO ProcessPostData
        
        //TODO IsFullyConfigured
        //TODO GetPrettyString
        //TODO OnRunAction
        //TODO ReferencesDeviceOrFeature

    }

}