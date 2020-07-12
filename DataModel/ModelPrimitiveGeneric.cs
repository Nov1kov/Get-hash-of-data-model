using System;
using System.Collections.Generic;

namespace DataModel
{
    [Serializable]
    public class ModelPrimitiveGeneric
    {
        public Dictionary<int, string> Dictionary { get; }
    }
    
    [Serializable]
    public class ModelPrimitiveGeneric2
    {
        public Dictionary<long, string> Dictionary { get; }
    }
}