using System.Linq;
using AutoBogus;
using DeepEqual.Syntax;
using SharpObjectGenerator.AutoBogusExtensions;
using SharpObjectGenerator.Models;
using Xunit;

namespace SharpObjectGenerator
{
    public class ModelGenerateTest
    {
        private readonly ObjectGenerator _objectGenerator;

        public ModelGenerateTest()
        {
            _objectGenerator = new ObjectGenerator();
        }
        
        [Fact]
        public void TwoTimesCreate_DeepEqual()
        {
            var faker = AutoFaker.Create(builder => builder.WithOverride(new PrimitiveGeneratorOverride()));

            var model1 = _objectGenerator.Generate<Model>();
            var model2 = _objectGenerator.Generate<Model>();

            model1.WithDeepEqual(model2).SkipDefault<Item>().Assert();
        }

        [Fact]
        public void GenerateObject_NotEmptyFields()
        {
            var model = _objectGenerator.Generate<Model>();
            
            Assert.True(model.Int1 > 0);
            Assert.NotEmpty(model.Str1);
            Assert.Equal(3, model.SomeListItems.Count);
            Assert.NotNull(model.ClassField);
            Assert.True(model.ClassField.DecimalField > 0);
            Assert.True(model.SomeListItems[0].DoubleField > 0);
            Assert.True(model.SomeListItems[0].FloatField > 0);
            
            // ignore NoSerialize fields
            Assert.Equal(0, model.IgnoreField);
        }
        
        [Fact]
        public void GenerateObject_ImplementAbstractions()
        {
            _objectGenerator.AddTypeMap<IItem, ItemImplOne>();
            _objectGenerator.AddTypeMap<IItem, ItemImplTwo>();
            
            var model = _objectGenerator.Generate<ModelWithAbstraction>();

            Assert.Contains(model.Items, i => i is ItemImplOne);
            Assert.Contains(model.Items, i => i is ItemImplTwo);
        }
        
        [Fact]
        public void ObjectWithAbstraction_GetHash_StaticHash()
        {
            _objectGenerator.AddTypeMap<IItem, ItemImplOne>();
            _objectGenerator.AddTypeMap<IItem, ItemImplTwo>();
            
            var model = _objectGenerator.Generate<ModelWithAbstraction>();

            Assert.Equal(177346035, _objectGenerator.GetHash());
        }
        
        [Fact]
        public void ObjectWithPrimitive_GetHash_StaticHash()
        {
            var model = _objectGenerator.Generate<Model>();

            Assert.Equal(-1469613893, _objectGenerator.GetHash());
        }
    }
}