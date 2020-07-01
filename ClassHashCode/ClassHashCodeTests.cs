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
            Assert.Equal(-1990839252, ClassHashCode.Get(typeof(Model)));
        }

        [Fact]
        public void Model_Abstract_StaticHashCode()
        {
            Assert.Equal(-1343364564, ClassHashCode.Get(typeof(ModelWithAbstraction)));
        }

        [Fact]
        public void Model_Recursive_StaticHashCode()
        {
            Assert.Equal(617865651, ClassHashCode.Get(typeof(ModelRecursive)));
        }
    }
}