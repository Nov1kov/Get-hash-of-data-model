using AutoBogus;
using DeepEqual.Syntax;
using SharpObjectGenerator.AutoBogusExtensions;
using SharpObjectGenerator.Models;
using Xunit;

namespace SharpObjectGenerator
{
    public class ModelGenerateTest
    {
        [Fact]
        public void TwoTimesCreate_DeepEqual()
        {
            var faker = AutoFaker.Create(builder => builder.WithOverride(new PrimitiveGeneratorOverride()));

            var model1 = faker.Generate<Model>();
            var model2 = faker.Generate<Model>();

            model1.WithDeepEqual(model2).SkipDefault<Item>().Assert();
        }

        [Fact]
        public void GenerateObject_NotEmptyFields()
        {
            var faker = AutoFaker.Create(builder => builder.WithOverride(new PrimitiveGeneratorOverride()));

            var model = faker.Generate<Model>();
            
            Assert.True(model.Int1 > 0);
            Assert.NotEmpty(model.Str1);
            Assert.Equal(3, model.SomeListItems.Count);
            Assert.NotNull(model.ClassField);
            Assert.True(model.ClassField.DecimalField > 0);
            Assert.True(model.SomeListItems[0].DoubleField > 0);
            Assert.True(model.SomeListItems[0].FloatField > 0);
        }
    }
}