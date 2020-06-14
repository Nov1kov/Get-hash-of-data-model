using System;
using AutoBogus;

namespace ObjectGenerator.AutoBogusExtensions
{
    public class PrimitiveGeneratorOverride : AutoGeneratorOverride
    {
        private static object GetDefaultValueByType(object obj)
        {
            if (obj.GetType().IsEnum)
            {
                return 1;
            }
            switch (obj)
            {
                case string s:
                    return "Some String";
                    
                case decimal d:
                    return 1.2M;
                    
                case int i:
                    return 4;
                    
                case long l:
                    return 100L;
                    
                case char c:
                    return 'c';
                    
                case float f:
                    return 1.4f;
                    
                case double d:
                    return 1.6D;
                    
                case bool b:
                    return true;
                
                case byte b:
                    return (byte) 2;
                    
                default:
                    throw new ArgumentException($"Unknown type for generate value {obj.GetType().Name}");
            }
        }
        
        public override bool CanOverride(AutoGenerateContext context)
        {
            var result = context.GenerateType.IsPrimitive || context.GenerateType == typeof(string) ||
                         context.GenerateType == typeof(decimal) || context.GenerateType.IsEnum;
            return result;
        }

        public override void Generate(AutoGenerateOverrideContext context)
        {
            context.Instance = GetDefaultValueByType(context.Instance);
        }
    }
}