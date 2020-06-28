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
        public void ModelWithIgnoreField_StaticHashCode()
        {
            Assert.Equal(-1207473599, ClassHashCode.Get(typeof(Model)));
        }
    }
}