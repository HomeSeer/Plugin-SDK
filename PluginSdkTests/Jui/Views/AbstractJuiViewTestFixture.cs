using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    public abstract class AbstractJuiViewTestFixture {
        
        protected const string INVALID_ID_CHARACTERS = "!\"#$%&'()*+,./:;<=>?@[]^`{|}~_\\ \r\n";
        protected const string INVALID_NAME_CHARACTERS = "\r\n";
        protected const string VALID_ID_CHARACTERS = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-.";
        protected const string VALID_NAME_CHARACTERS = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-. ";
        protected const string DEFAULT_ID = "id";
        protected const string DEFAULT_NAME = "name";
        
        protected static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();
        
        protected static IEnumerable<bool> BoolTestCaseSource() {
            yield return false;
            yield return true;
        }
        
    }
}