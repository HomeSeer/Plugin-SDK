using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework.Internal;

namespace HomeSeer.PluginSdk.Devices.Controls {
    
    [TestFixture(Description = "Tests of the StatusControlCollection class to ensure it behaves as expected under normal conditions.",
        TestOf = typeof(StatusControlCollection))]
    public class StatusControlCollectionTests {


        private StatusControlCollection _scc;

        [SetUp]
        public void InitStatusControlCollection() {
            _scc = new StatusControlCollection();
        }

        //TODO Add
        //TODO AddRange
        //TODO this[double value]
        //TODO Contains
        //TODO ContainsValue
        //TODO GetControlsForRange
        //TODO Values
        //TODO Count
        //TODO RemoveKey
        //TODO Remove
        //TODO RemoveAll

        private static IEnumerable<object[]> StatusControlsWithControlUsePresent() {

            List<StatusControl> scList = new List<StatusControl>();
            StatusControl sc;
            double targetValue = 0;
            
            foreach(EControlUse cu in Enum.GetValues(typeof(EControlUse))) {
                sc = new StatusControl(EControlType.Button);
                sc.TargetValue = targetValue++;
                sc.ControlUse = cu;
                scList.Add(sc);
            }

            foreach (EControlUse cu in Enum.GetValues(typeof(EControlUse))) {
                yield return new object[] {
                    scList,
                    cu
                };
            }
        }

        private static IEnumerable<object[]> StatusControlsWithControlUseAbsent() {

            List<StatusControl> scList = new List<StatusControl>();
            StatusControl sc;
            double targetValue = 0;

            foreach (EControlUse cu in Enum.GetValues(typeof(EControlUse))) {
                if (cu > EControlUse.ThermModeAuto)
                    break;
                sc = new StatusControl(EControlType.Button);
                sc.TargetValue = targetValue++;
                sc.ControlUse = cu;
                scList.Add(sc);
            }

            foreach (EControlUse cu in Enum.GetValues(typeof(EControlUse))) {
                if (cu <= EControlUse.ThermModeAuto)
                    continue;
                yield return new object[] {
                    scList,
                    cu
                };
            }
        }

        [TestCaseSource(nameof(StatusControlsWithControlUsePresent))]
        [Description("Test HasControlForUse with an EControlUse present in the list of StatusControl and expect TRUE to be returned.")]
        public void HasControlForUse_ReturnsTrue(List<StatusControl> statusControls, EControlUse controlUse) {
            _scc.AddRange(statusControls);
            Assert.IsTrue(_scc.HasControlForUse(controlUse));
        }

        [TestCaseSource(nameof(StatusControlsWithControlUseAbsent))]
        [Description("Test HasControlForUse with an EControlUse absent from the list of StatusControl and expect FALSE to be returned.")]
        public void HasControlForUse_ReturnsFalse(List<StatusControl> statusControls, EControlUse controlUse) {
            _scc.AddRange(statusControls);
            Assert.IsFalse(_scc.HasControlForUse(controlUse));
        }

        [TestCaseSource(nameof(StatusControlsWithControlUsePresent))]
        [Description("Test GetFirstControlForUse with an EControlUse present in the list of StatusControl and expect a non null StatusControl to be returned.")]
        public void GetFirstControlForUse_ReturnsNonNull(List<StatusControl> statusControls, EControlUse controlUse) {
            _scc.AddRange(statusControls);
            StatusControl sc = _scc.GetFirstControlForUse(controlUse);
            Assert.IsNotNull(sc);
            Assert.AreEqual(sc.ControlUse, controlUse);
        }

        [TestCaseSource(nameof(StatusControlsWithControlUseAbsent))]
        [Description("Test GetFirstControlForUse with an EControlUse absent from the list of StatusControl and expect null to be returned.")]
        public void GetFirstControlForUse_ReturnsNull(List<StatusControl> statusControls, EControlUse controlUse) {
            _scc.AddRange(statusControls);
            Assert.IsNull(_scc.GetFirstControlForUse(controlUse));
        }

    }
}