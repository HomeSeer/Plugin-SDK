using System;
using HomeSeer.PluginSdk.Devices;
using NUnit.Framework;

namespace HomeSeer.PluginSdkTests.Devices.Tests
{
    [TestFixture()]
    public class ValueRangeTests
    {
        //Create tests

        [Description("Test the creation of a new value range using good min and max values.")]
        [TestCase(0, 1)]
        [TestCase(0.1, 1)]
        [TestCase(1.79, 2)]
        [TestCase(0, 255)]
        [TestCase(50.0, 99.99)]
        [TestCase(-10, 20)]
        public void New_ValueRange_AssertPass(double min, double max)
        {
            new ValueRange(min, max);
            Assert.Pass();
        }

        [Description("Test the creation of a new value range using bad min and/or max values so an exception is thrown.")]
        [TestCase(1, -2)]
        [TestCase(27, 26)]
        [TestCase(1.01, 1)]
        public void New_ValueRange_Throws(double min, double max)
        {
            Assert.Throws<ArgumentException>(() => new ValueRange(min, max));
        }

        //Min tests

        [Description("Test setting Min when Max is 10 to make sure no exceptions are thrown.")]
        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1.5)]
        [TestCase(9)]
        [TestCase(9.99)]
        public void Min_SetWithMax10_AssertPass(double min)
        {
            var testValueRange = new ValueRange(0, 10);
            testValueRange.Min = min;
            Assert.Pass();
        }

        [Description("Test setting Min when Max is 10 to make sure that an exception is thrown when a number > Max is provided for Min.")]
        [TestCase(10.1)]
        [TestCase(10.01)]
        [TestCase(10.001)]
        [TestCase(11)]
        public void Min_SetWithMax10_Throws(double min)
        {
            var testValueRange = new ValueRange(0, 10);
            Assert.Throws<ArgumentOutOfRangeException>(() => testValueRange.Min = min);
        }

        [Description("Test getting Min when Max is 10 to make sure the same number is returned.")]
        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1.5)]
        [TestCase(9)]
        [TestCase(9.99)]
        public void Min_GetWithMax10_ReturnsSameNumber(double min)
        {
            var testValueRange = new ValueRange(min, 10);
            Assert.AreEqual(min, testValueRange.Min);
        }

        //Max tests

        [Description("Test setting Max when Min is 1 to make sure no exceptions are thrown.")]
        [TestCase(1.001)]
        [TestCase(1.01)]
        [TestCase(1.1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(255)]
        public void Max_SetWithMin1_AssertPass(double max)
        {
            var testValueRange = new ValueRange(1, 10);
            testValueRange.Max = max;
            Assert.Pass();
        }

        [Description("Test setting Max when Min is 1 to make sure that an exception is thrown when a number < Min is provided for Max.")]
        [TestCase(0.999)]
        [TestCase(0.99)]
        [TestCase(0.9)]
        [TestCase(0)]
        [TestCase(-1)]
        public void Max_SetWithMin1_Throws(double max)
        {
            var testValueRange = new ValueRange(1, 10);
            Assert.Throws<ArgumentOutOfRangeException>(() => testValueRange.Max = max);
        }

        [Description("Test getting Max when Min is 1 to make sure the same number is returned.")]
        [TestCase(1.001)]
        [TestCase(1.01)]
        [TestCase(1.1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(255)]
        public void Max_GetWithMin1_ReturnsSameNumber(double max)
        {
            var testValueRange = new ValueRange(1, max);
            Assert.AreEqual(max, testValueRange.Max);
        }

        //Offset tests

        [Description("Test setting the offset to make sure no exceptions are thrown.")]
        [TestCase(-100)]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(-0.1)]
        [TestCase(-0.01)]
        [TestCase(0)]
        [TestCase(0.01)]
        [TestCase(0.1)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void Offset_Set_AssertPass(double offset)
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.Offset = offset;
            Assert.Pass();
        }

        [Description("Test getting the offset to make sure it returns the same number it was set to.")]
        [TestCase(-100)]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(-0.1)]
        [TestCase(-0.01)]
        [TestCase(0)]
        [TestCase(0.01)]
        [TestCase(0.1)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void Offset_Get_ReturnsSameNumber(double offset)
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.Offset = offset;
            Assert.AreEqual(offset, testValueRange.Offset);
        }

        //Decimal Places tests

        [Description("Test setting Decimal Places to make sure no exceptions are thrown.")]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void DecimalPlaces_Set_AssertPass(int decimalPlaces) 
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.DecimalPlaces = decimalPlaces;
            Assert.Pass();
        }

        [Description("Test setting Decimal Places to make sure an exception is thrown when trying to set it to a value less than 0.")]
        [TestCase(-1)]
        public void DecimalPlaces_SetLessThan0_Throws(int decimalPlaces) 
        {
            var testValueRange = new ValueRange(0, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => testValueRange.DecimalPlaces = decimalPlaces);
        }

        [Description("Test getting Decimal Places to make sure it returns the same number it was set to.")]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void DecimalPlaces_Get_ReturnSameNumber(int decimalPlaces) 
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.DecimalPlaces = decimalPlaces;
            Assert.AreEqual(decimalPlaces, testValueRange.DecimalPlaces);
        }

        //Prefix tests

        [Description("Test setting Prefix to make sure no exceptions are thrown.")]
        [TestCase("test")]
        [TestCase("f")]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("°")]
        public void Prefix_Set_AssertPass(string prefix) 
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.Prefix = prefix;
            Assert.Pass();
        }

        [Description("Test getting Prefix to make sure it returns the same value it was set to or empty string if set to null or whitespace.")]
        [TestCase("test")]
        [TestCase("f")]
        [TestCase(" ")]
        [TestCase("°")]
        public void Prefix_Get_ReturnSameOrEmpty(string prefix) 
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.Prefix = prefix;
            Assert.AreEqual(string.IsNullOrWhiteSpace(prefix) ? "" : prefix, testValueRange.Prefix);
        }

        //TODO invalid prefix values

        //Suffix tests

        [Description("Test setting Suffix to make sure no exceptions are thrown.")]
        [TestCase("test")]
        [TestCase("f")]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("°")]
        public void Suffix_Set_AssertPass(string suffix)
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.Suffix = suffix;
            Assert.Pass();
        }

        [Description("Test getting Suffix to make sure it returns the same value it was set to or empty string if set to null or whitespace.")]
        [TestCase("test")]
        [TestCase("f")]
        [TestCase(" ")]
        [TestCase("°")]
        public void Suffix_Get_ReturnSameOrEmpty(string suffix)
        {
            var testValueRange = new ValueRange(0, 1);
            testValueRange.Suffix = suffix;
            Assert.AreEqual(string.IsNullOrWhiteSpace(suffix) ? "" : suffix, testValueRange.Suffix);
        }

        //TODO invalid suffix values

        //GetStringForValue tests

        [Description("Test GetStringForValue when Min is 0, Max is 100, Prefix and Suffix are empty, offset is 0, and decimal places is 0.")]
        [TestCase(-2, "-2")]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(1.1, "1")]
        [TestCase(1.111, "1")]
        [TestCase(10, "10")]
        [TestCase(50, "50")]
        public void GetStringForValue_Min0Max100_ValOnlyNoDec(double value, string expected) 
        {
            var testValueRange = new ValueRange(0, 100);
            Assert.AreEqual(expected, testValueRange.GetStringForValue(value));
        }

        [Description("Test GetStringForValue when Min is 0, Max is 100, Prefix and Suffix are empty, offset is 0, and decimal places is 1.")]
        [TestCase(-2, "-2.0")]
        [TestCase(0, "0.0")]
        [TestCase(1, "1.0")]
        [TestCase(1.1, "1.1")]
        [TestCase(1.111, "1.1")]
        [TestCase(10, "10.0")]
        [TestCase(50, "50.0")]
        public void GetStringForValue_Min0Max100_ValOnly1Dec(double value, string expected)
        {
            var testValueRange = new ValueRange(0, 100);
            testValueRange.DecimalPlaces = 1;
            Assert.AreEqual(expected, testValueRange.GetStringForValue(value));
        }

        [Description("Test GetStringForValue when Min is 0, Max is 100, Prefix and Suffix are empty, offset is 0, and decimal places is 2.")]
        [TestCase(-2, "-2.00")]
        [TestCase(0, "0.00")]
        [TestCase(1, "1.00")]
        [TestCase(1.1, "1.10")]
        [TestCase(1.111, "1.11")]
        [TestCase(10, "10.00")]
        [TestCase(50, "50.00")]
        public void GetStringForValue_Min0Max100_ValOnly2Dec(double value, string expected)
        {
            var testValueRange = new ValueRange(0, 100);
            testValueRange.DecimalPlaces = 2;
            Assert.AreEqual(expected, testValueRange.GetStringForValue(value));
        }

        [Description("Test GetStringForValue when Min is 0, Max is 100, offset is 0, decimal places is 0, and prefix and suffix are both set to test.")]
        [TestCase(-2, "test-2test")]
        [TestCase(0, "test0test")]
        [TestCase(1, "test1test")]
        [TestCase(1.1, "test1test")]
        [TestCase(1.111, "test1test")]
        [TestCase(10, "test10test")]
        [TestCase(50, "test50test")]
        public void GetStringForValue_Min0Max100_NoOffNoDec(double value, string expected)
        {
            var testValueRange = new ValueRange(0, 100);
            testValueRange.Prefix = "test";
            testValueRange.Suffix = "test";
            Assert.AreEqual(expected, testValueRange.GetStringForValue(value));
        }

        [Description("Test GetStringForValue when Min is 0, Max is 100, Prefix and Suffix are empty, offset is 100, and decimal places is 1.")]
        [TestCase(-2, "-102.0")]
        [TestCase(0, "-100.0")]
        [TestCase(1, "-99.0")]
        [TestCase(1.1, "-98.9")]
        [TestCase(1.111, "-98.9")]
        [TestCase(10, "-90.0")]
        [TestCase(50, "-50.0")]
        public void GetStringForValue_Min0Max100_Off100NoPreNoSuf1Dec(double value, string expected)
        {
            var testValueRange = new ValueRange(0, 100);
            testValueRange.Offset = 100;
            testValueRange.DecimalPlaces = 1;
            Assert.AreEqual(expected, testValueRange.GetStringForValue(value));
        }

        [Description("Test GetStringForValue when Min is 0, Max is 100, offset is 100, decimal places is 1, and prefix and suffix are both set to test.")]
        [TestCase(-2, "test-102.0test")]
        [TestCase(0, "test-100.0test")]
        [TestCase(1, "test-99.0test")]
        [TestCase(1.1, "test-98.9test")]
        [TestCase(1.111, "test-98.9test")]
        [TestCase(10, "test-90.0test")]
        [TestCase(50, "test-50.0test")]
        public void GetStringForValue_Min0Max100_ReturnExpected(double value, string expected)
        {
            var testValueRange = new ValueRange(0, 100);
            testValueRange.Offset = 100;
            testValueRange.DecimalPlaces = 1;
            testValueRange.Prefix = "test";
            testValueRange.Suffix = "test";
            Assert.AreEqual(expected, testValueRange.GetStringForValue(value));
        }

        //IsValueInRange tests

        [Description("Test IsValueInRange with a value that is in range and should return true.")]
        [TestCase(0, 1, 0)]
        [TestCase(0, 1, 0.001)]
        [TestCase(0, 1, 0.01)]
        [TestCase(0, 1, 0.1)]
        [TestCase(0, 1, 0.5)]
        [TestCase(0, 1, 0.9)]
        [TestCase(0, 1, 0.99)]
        [TestCase(0, 1, 0.999)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 100, 0)]
        [TestCase(0, 100, 100)]
        [TestCase(0, 100, 99.9999)]
        [TestCase(0, 100000, 100000)]
        public void IsValueInRange_Min0Max1_AssertTrue(double min, double max, double value) 
        {
            var testValueRange = new ValueRange(min, max);
            Assert.IsTrue(testValueRange.IsValueInRange(value));
        }

        [Description("Test IsValueInRange with a value that is not in range and should return false.")]
        [TestCase(0, 1, -10)]
        [TestCase(0, 1, -0.1)]
        [TestCase(0, 1, -0.01)]
        [TestCase(0, 1, -0.001)]
        [TestCase(0, 1, 1.001)]
        [TestCase(0, 1, 1.01)]
        [TestCase(0, 1, 1.1)]
        [TestCase(0, 1, 10)]
        [TestCase(0, 100, 100.00001)]
        [TestCase(0, 100, -0.00001)]
        [TestCase(0, 100000, 100000.00001)]
        public void IsValueInRange_Min0Max1_AssertFalse(double min, double max, double value)
        {
            var testValueRange = new ValueRange(min, max);
            Assert.IsFalse(testValueRange.IsValueInRange(value));
        }

        //Equals tests

        [Description("Test Equals to make sure that two identical value ranges are correctly evaluated to be equal.")]
        [TestCase(0, 1, 0, 0, "", "")]
        [TestCase(0.01, 1.01, 0.01, 0, "-", "-")]
        [TestCase(0, 1, 0, 2, "°", "°")]
        [TestCase(1.1111, 2.2222, 5.5555, 4, "", "")]
        public void Equals_AssertEqual(double min, double max, 
            double offset, int decimalPlaces, 
            string prefix, string suffix)
        {
            var testValueRange1 = new ValueRange(min, max) {
                Offset = offset,
                DecimalPlaces = decimalPlaces,
                Prefix = prefix,
                Suffix = suffix
            };
            var testValueRange2 = new ValueRange(min, max)
            {
                Offset = offset,
                DecimalPlaces = decimalPlaces,
                Prefix = prefix,
                Suffix = suffix
            };
            Assert.AreEqual(testValueRange1, testValueRange2);
        }

        [Description("Test Equals to make sure that two value ranges are correctly evaluated to be unequal when they are not equal.")]
        [TestCase(0, 1, 0, 0, "", "")]
        [TestCase(0.01, 1.01, 0.01, 0, "-", "-")]
        [TestCase(0, 1, 0, 2, "°", "°")]
        [TestCase(1.1111, 2.2222, 5.5555, 4, "", "")]
        public void Equals_AssertNotEqual(double min, double max,
            double offset, int decimalPlaces,
            string prefix, string suffix)
        {
            var testValueRange1 = new ValueRange(min, max)
            {
                Offset = offset,
                DecimalPlaces = decimalPlaces,
                Prefix = prefix,
                Suffix = suffix
            };
            var testValueRange2 = new ValueRange(0, 1)
            {
                Offset = 1,
                DecimalPlaces = 1,
                Prefix = "not-equal",
                Suffix = "not-equal"
            };
            Assert.AreNotEqual(testValueRange1, testValueRange2);
        }
    }
}