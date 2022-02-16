using HomeSeer.Jui.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(TestOf = typeof(AbstractView), Author = "JLW")]
    public class AbstractViewTests : AbstractJuiViewTestFixture {

        private static IEnumerable<string> ValidIdCaseSource() {
            yield return RANDOMIZER.GetString(4, VALID_ID_CHARACTERS);
            yield return RANDOMIZER.GetString(8, VALID_ID_CHARACTERS);
            yield return RANDOMIZER.GetString(16, VALID_ID_CHARACTERS);
        }
        
        private static IEnumerable<string> InvalidIdCaseSource() {
            yield return null;
            yield return string.Empty;
            foreach (var c in INVALID_ID_CHARACTERS.ToCharArray()) {
                yield return c.ToString();
            }
        }
        
        private static IEnumerable<object[]> ValidIdNameCaseSource() {
            yield return new object[] {
                RANDOMIZER.GetString(8, VALID_ID_CHARACTERS),
                RANDOMIZER.GetString(8, VALID_NAME_CHARACTERS)
            };
        }
        
        private static IEnumerable<object[]> InvalidIdValidNameCaseSource() {
            yield return new object[] {
                null,
                RANDOMIZER.GetString(8, VALID_NAME_CHARACTERS)
            };
        }
        
        private static IEnumerable<string> ValidNameCaseSource() {
            yield return RANDOMIZER.GetString(8, VALID_NAME_CHARACTERS);
            yield return RANDOMIZER.GetString(64, VALID_NAME_CHARACTERS);
        }
        
        private static IEnumerable<string> ValidValueCaseSource() {
            yield return RANDOMIZER.GetString(1);
            yield return RANDOMIZER.GetString(64);
            yield return RANDOMIZER.GetString(128);
            yield return RANDOMIZER.GetString(256);
        }

        [TestCaseSource(nameof(ValidIdCaseSource))]
        [Description("Create a new instance of a SimpleView using a valid ID and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_ValidId_DoesNotThrow(string id) {
            Assert.DoesNotThrow(() => {
                _ = new SimpleView(id);
            });
        }

        [TestCaseSource(nameof(InvalidIdCaseSource))]
        [Description("Create a new instance of a SimpleView using an invalid ID and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_InvalidId_Throws(string id) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new SimpleView(id);
            });
        }

        [TestCaseSource(nameof(ValidIdNameCaseSource))]
        [Description("Create a new instance of a SimpleView using a valid ID and name and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_ValidIdName_DoesNotThrow(string id, string name) {
            Assert.DoesNotThrow(() => {
                _ = new SimpleView(id, name);
            });
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

        [TestCaseSource(nameof(ValidNameCaseSource))]
        [Description("Get the name of a SimpleView and expect the same name it was created with to be returned.")]
        [Author("JLW")]
        public void Name_Get_ReturnsExpected(string name) {
            var view = new SimpleView(DEFAULT_ID, name);
            Assert.AreEqual(name, view.Name);
        }
        
        [Test]
        [Description("Get the type of a SimpleView and expect EViewType.Undefined to be returned.")]
        [Author("JLW")]
        public void Type_Get_ReturnsExpected() {
            var view = new SimpleView(DEFAULT_ID);
            Assert.AreEqual(EViewType.Undefined, view.Type);
        }

        [TestCaseSource(nameof(ValidValueCaseSource))]
        [Description("Call GetStringValue and expect the same value that was set to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsExpected(string value) {
            var view = new SimpleView(DEFAULT_ID) {
                Value = value
            };
            Assert.AreEqual(value, view.GetStringValue());
        }
        
        [Test]
        [Description("Call Update with a valid view and expect no exception to be thrown.")]
        [Author("JLW")]
        public void Update_Valid_DoesNotThrow() {
            var view = new SimpleView(DEFAULT_ID);
            Assert.DoesNotThrow(() => {
                view.Update(view);
            });
        }
        
        [Test]
        [Description("Call Update with null and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Update_Null_Throws() {
            var view = new SimpleView(DEFAULT_ID);
            Assert.Throws<ArgumentNullException>(() => {
                view.Update(null);
            });
        }
        
        [Test]
        [Description("Call Update with a view that has a different ID and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Update_DifferentId_Throws() {
            var view = new SimpleView(DEFAULT_ID);
            Assert.Throws<InvalidOperationException>(() => {
                view.Update(new SimpleView("invalid"));
            });
        }
        
        [Test]
        [Description("Call Update with a view that has a different type and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Update_DifferentType_Throws() {
            var view = new SimpleView(DEFAULT_ID);
            Assert.Throws<InvalidOperationException>(() => {
                view.Update(new LabelView(DEFAULT_ID, DEFAULT_NAME));
            });
        }

        [Test]
        [Description("Call UpdateValue and expect no exception to be thrown.")]
        [Author("JLW")]
        public void UpdateValue_DoesNotThrow() {
            var view = new SimpleView(DEFAULT_ID);
            Assert.DoesNotThrow(() => {
                view.UpdateValue("");
            });
        }
        
        [Test]
        [Description("Call ToHtml and expect no exception to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            var view = new SimpleView(DEFAULT_ID);
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