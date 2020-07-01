using System;
using System.Collections;
using System.Collections.Generic;

namespace DataModel
{
    [Serializable]
    public class ModelRecursive
    {
        public IDictionary<int, string> Map { get; set; }
        public SomeClass SomeField { get; set; }
    }

    [Serializable]
    public class SomeClass
    {
        private readonly ModelRecursive _modelRecursive;

        public SomeClass(ModelRecursive modelRecursive)
        {
            _modelRecursive = modelRecursive;
        }
    }
}