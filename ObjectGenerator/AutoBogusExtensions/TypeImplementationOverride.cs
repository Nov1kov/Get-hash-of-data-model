using System;
using System.Collections.Generic;
using System.Linq;
using AutoBogus;

namespace ObjectGenerator.AutoBogusExtensions
{
    public class TypeImplementationOverride : AutoGeneratorOverride
    {
        private Dictionary<Func<AutoGenerateOverrideContext, object>, Type> _objectImplMap = 
            new Dictionary<Func<AutoGenerateOverrideContext, object>, Type>();
        private Dictionary<Type, int> _generatedTypeIndex = new Dictionary<Type, int>();
        
        public void AddTypeMap<TBase, TImpl>() where TImpl : TBase
        {
            _objectImplMap.Add(GenerateByType<TImpl>, typeof(TBase));
        }
        
        public override bool CanOverride(AutoGenerateContext context)
        {
            var isGoingOverride = _objectImplMap.ContainsValue(context.GenerateType);
            if ((context.GenerateType.IsAbstract || context.GenerateType.IsInterface) && !isGoingOverride)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Class {context.GenerateType.Name} hasn't specific implementation, please use AddTypeMap");
                Console.ResetColor();
            }

            return isGoingOverride;
        }

        public override void Generate(AutoGenerateOverrideContext context)
        {
            Func<AutoGenerateOverrideContext, object> specificImpl = GetGenerateFunc(context.GenerateType);
            context.Instance = specificImpl(context);
        }
        
        private Func<AutoGenerateOverrideContext, object> GetGenerateFunc(Type type)
        {
            var generatorsByBaseType = _objectImplMap.
                Where(p => p.Value == type).
                Select(p => p.Key).ToArray();

            if (!_generatedTypeIndex.TryGetValue(type, out var index))
            {
                _generatedTypeIndex[type] = 0;
            }
            
            var func = generatorsByBaseType.ElementAt(index);

            _generatedTypeIndex[type] = index >= generatorsByBaseType.Count() - 1 ? 0 : index + 1;

            return func;
        }
        
        private static object GenerateByType<T>(AutoGenerateOverrideContext context)
        {
            return context.Generate<T>();
        }
    }
}