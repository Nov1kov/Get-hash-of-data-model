using System;
using DataModel;
using Xunit;

namespace ClassHashCode
{
    public class ClassHashCodeTests
    {
        [Fact]
        public void Consistent_ModelWithIgnoreField()
        {
            var firstCall = ClassHashCode.Get(typeof(Model));
            var secondCall = ClassHashCode.Get(typeof(Model));
            Assert.Equal(secondCall, firstCall);
        }

        [Fact]
        public void Model_IgnoreField_StaticHashCode()
        {
            Assert.Equal(806864982, ClassHashCode.Get(typeof(Model)));
        }

        [Fact]
        public void Model_Abstract_StaticHashCode()
        {
            Assert.Equal(-1437521649, ClassHashCode.Get(typeof(ModelWithAbstraction)));
        }

        [Fact]
        public void Model_Recursive_StaticHashCode()
        {
            Assert.Equal(2053293150, ClassHashCode.Get(typeof(ModelRecursive)));
        }
        
        [Fact]
        public void Model_PrimitiveGeneric_NotEquals()
        {
            var intStringDict = ClassHashCode.Get(typeof(ModelPrimitiveGeneric1));
            var longStringDict = ClassHashCode.Get(typeof(ModelPrimitiveGeneric2));
            Assert.NotEqual(intStringDict, longStringDict);
        }
    }
}