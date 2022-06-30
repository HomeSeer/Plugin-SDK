using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Devices.Controls {
    
    [TestFixture(
        TestOf = typeof(StatusControlCollection),
        Description = "Tests of the StatusControlCollection class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class StatusControlCollectionTests {

        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();
        private const int _controlValueMin = -256;
        private const int _controlValueMax = 256;

        private StatusControlCollection _statusControlCollection;

        [SetUp]
        public void InitStatusControlCollection() {
            _statusControlCollection = new StatusControlCollection();
        }
        
        private static IEnumerable<StatusControl> ValidStatusControlsTestCaseSource() {
            yield return new StatusControl(EControlType.Button) {
                TargetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax)
            };
        }
        
        private static IEnumerable<StatusControl> InvalidStatusControlsTestCaseSource() {
            yield return null;
        }
        
        private static IEnumerable<List<StatusControl>> ValidControlListsTestCaseSource() {
            yield return new List<StatusControl>();
            var controlValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            yield return new List<StatusControl> {
                new StatusControl(EControlType.Button) {
                    TargetValue = controlValue
                },
                new StatusControl(EControlType.Button) {
                    TargetValue = controlValue + 1
                },
            };
        }
        
        private static IEnumerable<List<StatusControl>> InvalidControlListsTestCaseSource() {
            //yield return null; TODO : handle this case
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            var rangeControl = new StatusControl(EControlType.ValueRangeSlider) {
                TargetRange = new ValueRange(targetValue - 1, targetValue + 1),
                IsRange = true
            };
            yield return new List<StatusControl> {
                control,
                control
            };
            yield return new List<StatusControl> {
                rangeControl,
                control
            };
            //TODO : handle this case - Currently, the range is added without issue because only the min value is checked
            /*yield return new List<StatusControl> {
                control,
                rangeControl
            };*/
        }
        
        private static IEnumerable<object[]> ControlListAndValidControlTestCaseSource() {
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            var rangeControl = new StatusControl(EControlType.ValueRangeSlider) {
                TargetRange = new ValueRange(targetValue - 1, targetValue + 1),
                IsRange = true
            };
            yield return new object[] {
                new List<StatusControl> {
                    control
                },
                control
            };
            yield return new object[] {
                new List<StatusControl> {
                    rangeControl
                },
                rangeControl
            };
        }
        
        private static IEnumerable<object[]> ControlListAndInvalidControlTestCaseSource() {
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            var rangeControl = new StatusControl(EControlType.ValueRangeSlider) {
                TargetRange = new ValueRange(targetValue + 10, targetValue + 12),
                IsRange = true
            };
            yield return new object[] {
                new List<StatusControl> {
                    control
                },
                rangeControl
            };
            yield return new object[] {
                new List<StatusControl> {
                    rangeControl
                },
                control
            };
        }
        
        private static IEnumerable<object[]> ControlListAndValidValueTestCaseSource() {
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            var rangeControl = new StatusControl(EControlType.ValueRangeSlider) {
                TargetRange = new ValueRange(targetValue - 1, targetValue + 1),
                IsRange = true
            };
            yield return new object[] {
                new List<StatusControl> {
                    control
                },
                control.TargetValue
            };
            yield return new object[] {
                new List<StatusControl> {
                    rangeControl
                },
                rangeControl.TargetRange.Min
            };
        }
        
        private static IEnumerable<object[]> ControlListAndInvalidValueTestCaseSource() {
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            var rangeControl = new StatusControl(EControlType.ValueRangeSlider) {
                TargetRange = new ValueRange(targetValue - 1, targetValue + 1),
                IsRange = true
            };
            yield return new object[] {
                new List<StatusControl> {
                    control
                },
                rangeControl.TargetRange.Min
            };
            yield return new object[] {
                new List<StatusControl> {
                    rangeControl
                },
                rangeControl.TargetRange.Max + 1
            };
        }
        
        private static IEnumerable<object[]> ControlListAndValidMinMaxTestCaseSource() {
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            yield return new object[] {
                new List<StatusControl> {
                    control
                },
                control.TargetValue - 1,
                control.TargetValue + 1
            };
            yield return new object[] {
                new List<StatusControl> {
                    control,
                    new StatusControl(EControlType.Button) {
                        TargetValue = targetValue + 1
                    }
                },
                control.TargetValue - 1,
                control.TargetValue + 2
            };
        }
        
        private static IEnumerable<object[]> ControlListAndInvalidMinMaxTestCaseSource() {
            var targetValue = RANDOMIZER.NextShort(_controlValueMin, _controlValueMax);
            var control = new StatusControl(EControlType.Button) {
                TargetValue = targetValue
            };
            yield return new object[] {
                new List<StatusControl> {
                    control
                },
                control.TargetValue + 10,
                control.TargetValue + 12
            };
            yield return new object[] {
                new List<StatusControl> {
                    control,
                    new StatusControl(EControlType.Button) {
                        TargetValue = targetValue + 1
                    }
                },
                control.TargetValue - 10,
                control.TargetValue - 5
            };
        }

        private static IEnumerable<object[]> StatusControlsWithControlUsePresent() {
            var scList = new List<StatusControl>();
            double targetValue = 0;
            
            foreach(EControlUse cu in Enum.GetValues(typeof(EControlUse))) {
                scList.Add(new StatusControl(EControlType.Button) {
                    TargetValue = targetValue++, 
                    ControlUse = cu
                });
            }

            foreach (EControlUse controlUse in Enum.GetValues(typeof(EControlUse))) {
                yield return new object[] {
                    scList,
                    controlUse
                };
            }
        }

        private static IEnumerable<object[]> StatusControlsWithControlUseAbsent() {

            var scList = new List<StatusControl>();
            double targetValue = 0;

            foreach (EControlUse absentControlUse in Enum.GetValues(typeof(EControlUse))) {
                foreach (EControlUse controlUse in Enum.GetValues(typeof(EControlUse))) {
                    if (controlUse == absentControlUse) {
                        continue;
                    }

                    scList.Add(new StatusControl(EControlType.Button) { 
                        TargetValue = targetValue, 
                        ControlUse = controlUse 
                    });
                    targetValue++;
                }
                yield return new object[] {
                    scList,
                    absentControlUse
                };
                targetValue = 0;
                scList = new List<StatusControl>();
            }
        }
        
        [TestCaseSource(nameof(ValidStatusControlsTestCaseSource))]
        [Description("Call Add using a valid StatusControl and expect it to be added to the collection.")]
        [Author("JLW")]
        public void Add_Valid_AddsElement(StatusControl control) {
            _statusControlCollection.Add(control);
            Assert.AreEqual(1, _statusControlCollection.Count);
            Assert.AreEqual(_statusControlCollection[control.TargetValue], control);
        }
        
        //TODO : Add this back once null values are handled in Add
        /*[TestCaseSource(nameof(InvalidStatusControlsTestCaseSource))]
        [Description("Call Add using an invalid StatusControl and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Add_Invalid_ThrowsException(StatusControl control) {
            Assert.Throws<ArgumentException>(() => _statusControlCollection.Add(control));
        }*/
        
        [TestCaseSource(nameof(ValidControlListsTestCaseSource))]
        [Description("Call AddRange using a valid list of StatusControls and expect them to be added to the collection.")]
        [Author("JLW")]
        public void AddRange_Valid_AddsElements(List<StatusControl> controls) {
            _statusControlCollection.AddRange(controls);
            Assert.AreEqual(controls.Count, _statusControlCollection.Count);
            foreach (StatusControl control in controls) {
                Assert.AreEqual(_statusControlCollection[control.TargetValue], control);
            }
        }
        
        [TestCaseSource(nameof(InvalidControlListsTestCaseSource))]
        [Description("Call AddRange using an invalid list of StatusControls and expect an exception to be thrown.")]
        [Author("JLW")]
        public void AddRange_Invalid_ThrowsException(List<StatusControl> controls) {
            Assert.Throws<ArgumentException>(() => _statusControlCollection.AddRange(controls));
        }
        
        [TestCaseSource(nameof(ControlListAndValidControlTestCaseSource))]
        [Description("Call this[] using a value that is in the collection and expect the right control to be returned.")]
        [Author("JLW")]
        public void This_Valid_ReturnsElement(List<StatusControl> controlList, StatusControl control) {
            foreach (StatusControl statusControl in controlList) {
                _statusControlCollection.Add(statusControl);
                var sb = new StringBuilder($"Added {statusControl.TargetValue}");
                if (statusControl.IsRange) {
                    sb.Append($" (range {statusControl.TargetRange.Min} to {statusControl.TargetRange.Max})");
                }
                Console.WriteLine(sb.ToString());
            }
            Assert.AreEqual(_statusControlCollection[control.IsRange ? control.TargetRange.Min : control.TargetValue], control);
        }
        
        [TestCaseSource(nameof(ControlListAndInvalidControlTestCaseSource))]
        [Description("Call this[] using a value that is not in the collection and expect an exception to be thrown.")]
        [Author("JLW")]
        public void This_Invalid_ThrowsException(List<StatusControl> controlList, StatusControl control) {
            foreach (StatusControl statusControl in controlList) {
                _statusControlCollection.Add(statusControl);
                var sb = new StringBuilder($"Added {statusControl.TargetValue}");
                if (statusControl.IsRange) {
                    sb.Append($" (range {statusControl.TargetRange.Min} to {statusControl.TargetRange.Max})");
                }
                Console.WriteLine(sb.ToString());
            }
            Assert.Throws<KeyNotFoundException>(() => _ = _statusControlCollection[control.IsRange ? control.TargetRange.Min : control.TargetValue]);
        }

        [TestCaseSource(nameof(ControlListAndValidControlTestCaseSource))]
        [Description("Call Contains using an element that is in the collection and expect true to be returned.")]
        [Author("JLW")]
        public void Contains_Valid_ReturnsTrue(List<StatusControl> controlList, StatusControl control) {
            _statusControlCollection.AddRange(controlList);
            Assert.IsTrue(_statusControlCollection.Contains(control));
        }
        
        [TestCaseSource(nameof(ControlListAndInvalidControlTestCaseSource))]
        [Description("Call Contains using an element that is not in the collection and expect false to be returned.")]
        [Author("JLW")]
        public void Contains_Invalid_ReturnsFalse(List<StatusControl> controlList, StatusControl control) {
            _statusControlCollection.AddRange(controlList);
            Assert.IsFalse(_statusControlCollection.Contains(control));
        }
        
        [TestCaseSource(nameof(ControlListAndValidValueTestCaseSource))]
        [Description("Call ContainsValue using a value that is in the collection and expect true to be returned.")]
        [Author("JLW")]
        public void ContainsValue_Valid_ReturnsTrue(List<StatusControl> controlList, double value) {
            _statusControlCollection.AddRange(controlList);
            Assert.IsTrue(_statusControlCollection.ContainsValue(value));
        }
        
        [TestCaseSource(nameof(ControlListAndInvalidValueTestCaseSource))]
        [Description("Call ContainsValue using a value that is not in the collection and expect false to be returned.")]
        [Author("JLW")]
        public void ContainsValue_Invalid_ReturnsFalse(List<StatusControl> controlList, double value) {
            _statusControlCollection.AddRange(controlList);
            Assert.IsFalse(_statusControlCollection.ContainsValue(value));
        }
        
        [TestCaseSource(nameof(ControlListAndValidMinMaxTestCaseSource))]
        [Description("Call GetControlsForRange using a valid range that is in the collection and expect elements to be returned.")]
        [Author("JLW")]
        public void GetControlsForRange_Valid_ReturnsElements(List<StatusControl> controlList, double min, double max) {
            _statusControlCollection.AddRange(controlList);
            Assert.IsNotEmpty(_statusControlCollection.GetControlsForRange(min, max));
        }
        
        [TestCaseSource(nameof(ControlListAndInvalidMinMaxTestCaseSource))]
        [Description("Call GetControlsForRange using an invalid range that is in the collection and expect an empty list to be returned.")]
        [Author("JLW")]
        public void GetControlsForRange_Invalid_ReturnsEmptyList(List<StatusControl> controlList, double min, double max) {
            _statusControlCollection.AddRange(controlList);
            Assert.IsEmpty(_statusControlCollection.GetControlsForRange(min, max));
        }
        
        [TestCaseSource(nameof(ValidControlListsTestCaseSource))]
        [Description("Call Values after adding a valid list of StatusControls to the collection and expect the right size collection to be returned.")]
        [Author("JLW")]
        public void Values_ReturnsList(List<StatusControl> controlList) {
            _statusControlCollection.AddRange(controlList);
            Assert.AreEqual(controlList.Count, _statusControlCollection.Values.Count);
        }
        
        [TestCaseSource(nameof(ValidControlListsTestCaseSource))]
        [Description("Call Count after adding a valid list of StatusControls to the collection and expect the right count to be returned.")]
        [Author("JLW")]
        public void Count_ReturnsCorrectCount(List<StatusControl> controlList) {
            _statusControlCollection.AddRange(controlList);
            Assert.AreEqual(controlList.Count, _statusControlCollection.Count);
        }
        
        [TestCaseSource(nameof(ControlListAndValidValueTestCaseSource))]
        [Description("Call RemoveKey using a value that is in the collection and expect the element to be removed.")]
        [Author("JLW")]
        public void RemoveKey_RemovesKey(List<StatusControl> controlList, double value) {
            _statusControlCollection.AddRange(controlList);
            _statusControlCollection.RemoveKey(value);
            Assert.IsFalse(_statusControlCollection.ContainsValue(value));
        }
        
        [TestCaseSource(nameof(ControlListAndValidControlTestCaseSource))]
        [Description("Call Remove using an element that is in the collection and expect the element to be removed.")]
        [Author("JLW")]
        public void Remove_RemovesElement(List<StatusControl> controlList, StatusControl control) {
            _statusControlCollection.AddRange(controlList);
            _statusControlCollection.Remove(control);
            Assert.IsFalse(_statusControlCollection.Contains(control));
        }

        [TestCaseSource(nameof(ValidControlListsTestCaseSource))]
        [Description("Call RemoveAll after adding a valid list of StatusControls to the collection and expect the collection to be cleared.")]
        [Author("JLW")]
        public void RemoveAll_RemovesAllElements(List<StatusControl> controlList) {
            _statusControlCollection.AddRange(controlList);
            _statusControlCollection.RemoveAll();
            Assert.AreEqual(0, _statusControlCollection.Count);
        }

        [TestCaseSource(nameof(StatusControlsWithControlUsePresent))]
        [Description("Call HasControlForUse with an EControlUse present in the list of StatusControl and expect TRUE to be returned.")]
        [Author("CP")]
        public void HasControlForUse_Valid_ReturnsTrue(List<StatusControl> statusControls, EControlUse controlUse) {
            _statusControlCollection.AddRange(statusControls);
            Assert.IsTrue(_statusControlCollection.HasControlForUse(controlUse));
        }

        [TestCaseSource(nameof(StatusControlsWithControlUseAbsent))]
        [Description("Call HasControlForUse with an EControlUse absent from the list of StatusControl and expect FALSE to be returned.")]
        [Author("CP")]
        public void HasControlForUse_Invalid_ReturnsFalse(List<StatusControl> statusControls, EControlUse controlUse) {
            _statusControlCollection.AddRange(statusControls);
            Assert.IsFalse(_statusControlCollection.HasControlForUse(controlUse));
        }

        [TestCaseSource(nameof(StatusControlsWithControlUsePresent))]
        [Description("Call GetFirstControlForUse with an EControlUse present in the list of StatusControl and expect a non null StatusControl to be returned.")]
        [Author("CP")]
        public void GetFirstControlForUse_Valid_ReturnsNonNull(List<StatusControl> statusControls, EControlUse controlUse) {
            _statusControlCollection.AddRange(statusControls);
            StatusControl sc = _statusControlCollection.GetFirstControlForUse(controlUse);
            Assert.IsNotNull(sc);
            Assert.AreEqual(sc.ControlUse, controlUse);
        }

        [TestCaseSource(nameof(StatusControlsWithControlUseAbsent))]
        [Description("Call GetFirstControlForUse with an EControlUse absent from the list of StatusControl and expect an exception to be thrown.")]
        [Author("CP")]
        public void GetFirstControlForUse_Invalid_ThrowsException(List<StatusControl> statusControls, EControlUse controlUse) {
            _statusControlCollection.AddRange(statusControls);
            Assert.Throws<InvalidOperationException>(() => {
                _statusControlCollection.GetFirstControlForUse(controlUse);
            });
        }
        
        [TestCaseSource(nameof(StatusControlsWithControlUsePresent))]
        [Description("Call GetControlsForUse with an EControlUse present in the list and expect a list of StatusControls to be returned.")]
        [Author("JLW")]
        public void GetControlsForUse_Valid_ReturnsElements(List<StatusControl> statusControls, EControlUse controlUse) {
            _statusControlCollection.AddRange(statusControls);
            Assert.IsNotEmpty(_statusControlCollection.GetControlsForUse(controlUse));
        }
        
        [TestCaseSource(nameof(StatusControlsWithControlUseAbsent))]
        [Description("Call GetControlsForUse with an EControlUse absent in the list and expect an empty list to be returned.")]
        [Author("JLW")]
        public void GetControlsForUse_Invalid_ReturnsEmptyList(List<StatusControl> statusControls, EControlUse controlUse) {
            _statusControlCollection.AddRange(statusControls);
            Assert.IsEmpty(_statusControlCollection.GetControlsForUse(controlUse));
        }

    }
}