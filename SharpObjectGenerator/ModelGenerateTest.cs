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
        public void AutoBogus_DeepEqual()
        {
            var faker = AutoFaker.Create(builder => builder.WithOverride(new PrimitiveGeneratorOverride()));

            var model1 = faker.Generate<Model>();
            var model2 = faker.Generate<Model>();

            model1.WithDeepEqual(model2).SkipDefault<Item>().Assert();
        }
    }
}