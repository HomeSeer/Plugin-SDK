using HomeSeer.Jui.Types;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(TestOf = typeof(AbstractView), Author = "JLW")]
    public class AbstractViewTests {
        
        private const string _invalidIdCharacters = "!\"#$%&'()*+,./:;<=>?@[]^`{|}~_\\ \r\n";
        private const string _invalidNameCharacters = "\r\n";
        private const string _validIdCharacters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-.";
        private const string _validNameCharacters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-. ";
        private const string _defaultId = "id";
        private const string _defaultName = "name";
        
        private static readonly Randomizer _randomizer = Randomizer.CreateRandomizer();
        
        private static IEnumerable<string> ValidIdCaseSource() {
            yield return _randomizer.GetString(4, _validIdCharacters);
            yield return _randomizer.GetString(8, _validIdCharacters);
            yield return _randomizer.GetString(16, _validIdCharacters);
        }

        [TestCaseSource(nameof(ValidIdCaseSource))]
        [Description("Create a new instance of a SimpleView using a valid ID and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_ValidId_DoesNotThrow(string id) {
            Assert.DoesNotThrow(() => {
                _ = new SimpleView(id);
            });
        }
        
        private static IEnumerable<string> InvalidIdCaseSource() {
            yield return null;
            yield return string.Empty;
            foreach (var c in _invalidIdCharacters.ToCharArray()) {
                yield return c.ToString();
            }
        }
        
        [TestCaseSource(nameof(InvalidIdCaseSource))]
        [Description("Create a new instance of a SimpleView using an invalid ID and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_InvalidId_Throws(string id) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new SimpleView(id);
            });
        }
        
        private static IEnumerable<object[]> ValidIdNameCaseSource() {
            yield return new object[] {
                _randomizer.GetString(8, _validIdCharacters),
                _randomizer.GetString(8, _validNameCharacters)
            };
        }

        [TestCaseSource(nameof(ValidIdNameCaseSource))]
        [Description("Create a new instance of a SimpleView using a valid ID and name and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_ValidIdName_DoesNotThrow(string id, string name) {
            Assert.DoesNotThrow(() => {
                _ = new SimpleView(id, name);
            });
        }
        
        private static IEnumerable<object[]> InvalidIdValidNameCaseSource() {
            yield return new object[] {
                null,
                _randomizer.GetString(8, _validNameCharacters)
            };
        }
        
        [TestCaseSource(nameof(InvalidIdValidNameCaseSource))]
        [Description("Create a new instance of a SimpleView using an invalid ID and valid name and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_InvalidIdValidName_Throws(string id, string name) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new SimpleView(id, name);
            });
        }
        
        //TODO check for invalid names

        [TestCaseSource(nameof(ValidIdCaseSource))]
        [Description("Get the Id of a SimpleView and expect the same Id it was created with to be returned.")]
        [Author("JLW")]
        public void Id_Get_ReturnsExpected(string id) {
            var view = new SimpleView(id);
            Assert.AreEqual(id, view.Id);
        }

        private static IEnumerable<string> ValidNameCaseSource() {
            yield return _randomizer.GetString(8, _validNameCharacters);
            yield return _randomizer.GetString(64, _validNameCharacters);
        }

        [TestCaseSource(nameof(ValidNameCaseSource))]
        [Description("Get the name of a SimpleView and expect the same name it was created with to be returned.")]
        [Author("JLW")]
        public void Name_Get_ReturnsExpected(string name) {
            var view = new SimpleView(_defaultId, name);
            Assert.AreEqual(name, view.Name);
        }
        
        [Test]
        [Description("Get the type of a SimpleView and expect EViewType.Undefined to be returned.")]
        [Author("JLW")]
        public void Type_Get_ReturnsExpected() {
            var view = new SimpleView(_defaultId);
            Assert.AreEqual(EViewType.Undefined, view.Type);
        }
        
        private static IEnumerable<string> ValidValueCaseSource() {
            yield return _randomizer.GetString(1);
            yield return _randomizer.GetString(64);
            yield return _randomizer.GetString(128);
            yield return _randomizer.GetString(256);
        }
        
        [TestCaseSource(nameof(ValidValueCaseSource))]
        [Description("Call GetStringValue and expect the same value that was set to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsExpected(string value) {
            var view = new SimpleView(_defaultId) {
                Value = value
            };
            Assert.AreEqual(value, view.GetStringValue());
        }
        
        [Test]
        [Description("Call Update with a valid view and expect no exception to be thrown.")]
        [Author("JLW")]
        public void Update_Valid_DoesNotThrow() {
            var view = new SimpleView(_defaultId);
            Assert.DoesNotThrow(() => {
                view.Update(view);
            });
        }
        
        [Test]
        [Description("Call Update with null and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Update_Null_Throws() {
            var view = new SimpleView(_defaultId);
            Assert.Throws<ArgumentNullException>(() => {
                view.Update(null);
            });
        }
        
        [Test]
        [Description("Call Update with a view that has a different ID and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Update_DifferentId_Throws() {
            var view = new SimpleView(_defaultId);
            Assert.Throws<InvalidOperationException>(() => {
                view.Update(new SimpleView("invalid"));
            });
        }
        
        [Test]
        [Description("Call Update with a view that has a different type and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Update_DifferentType_Throws() {
            var view = new SimpleView(_defaultId);
            Assert.Throws<InvalidOperationException>(() => {
                view.Update(new LabelView(_defaultId, _defaultName));
            });
        }

        [Test]
        [Description("Call UpdateValue and expect no exception to be thrown.")]
        [Author("JLW")]
        public void UpdateValue_DoesNotThrow() {
            var view = new SimpleView(_defaultId);
            Assert.DoesNotThrow(() => {
                view.UpdateValue("");
            });
        }
        
        [Test]
        [Description("Call ToHtml and expect no exception to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            var view = new SimpleView(_defaultId);
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }
        
        //TODO IdContainsNonAllowedCharacters

        /// <summary>
        /// A minimal implementation of <see cref="AbstractView"/> used to unit test <see cref="AbstractView"/> functionality.
        /// </summary>
        private class SimpleView : AbstractView {

            public string Value { get; set; } = "";
            
            public SimpleView(string id) : base(id) {
                Type = EViewType.Undefined;
            }

            public SimpleView(string id, string name) : base(id, name) {
                Type = EViewType.Undefined;
            }

            public override string GetStringValue() {
                return Value;
            }

            public override void Update(AbstractView newViewState) {
                base.Update(newViewState);
                if (newViewState is SimpleView simpleView) {
                    Value = simpleView.Value;
                }
            }

            public override void UpdateValue(string value) {
                Value = value;
            }

            public override string ToHtml(int indent = 0) {
                return Value;
            }
        }
    }
}