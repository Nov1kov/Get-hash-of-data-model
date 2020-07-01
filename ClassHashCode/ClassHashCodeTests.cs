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
            Assert.Equal(-321864352, ClassHashCode.Get(typeof(Model)));
        }

        [Fact]
        public void ModelWithAbstract_StaticHashCode()
        {
            Assert.Equal(176586752, ClassHashCode.Get(typeof(ModelWithAbstraction)));
        }
    }
}