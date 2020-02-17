using System;
using System.Collections.Generic;
using System.Reflection;
using AutoBogus;
using Bogus;
using SharpObjectGenerator.Models;
using Xunit;

namespace SharpObjectGenerator
{
    public class ModelGenerator
    {
        [Fact]
        public void AutoBogus()
        {
            var faker = AutoFaker.Create(builder => builder.WithOverride(new CustomOverride()));

            var model1 = faker.Generate<Model>();
            var model2 = faker.Generate<Model>();
        }
    }

    public class CustomOverride : AutoGeneratorOverride
    {
        public override bool CanOverride(AutoGenerateContext context)
        {
            return context.GenerateType.IsPrimitive || context.GenerateType == typeof(string);
        }

        public override void Generate(AutoGenerateOverrideContext context)
        {
            switch (context.Instance)
            {
                case string s:
                    context.Instance = context.GenerateName;
                    break;
                case decimal d:
                    context.Instance = (decimal) context.GenerateName.GetHashCode();
                    break;
                case int i:
                    context.Instance = (int) context.GenerateName.GetHashCode();
                    break;
                case long l:
                    context.Instance = (long) context.GenerateName.GetHashCode();
                    break;
                case char c:
                    context.Instance = (char) context.GenerateName[0];
                    break;
                case float f:
                    context.Instance = (float) context.GenerateName.GetHashCode();
                    break;
                case double d:
                    context.Instance = (double) context.GenerateName.GetHashCode();
                    break;
                case bool b:
                    context.Instance = true;
                    break;
                default:
                    throw new ArgumentException("Unknown type for generate value");
            }
        }
    }
}