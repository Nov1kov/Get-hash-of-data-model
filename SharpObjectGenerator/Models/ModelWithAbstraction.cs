using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpObjectGenerator.Models
{
    [Serializable]
    public class ModelWithAbstraction
    {
        public IList<IItem> Items { get;  } = new List<IItem>();
    }
    
    public interface IItem
    {
        string SomeStr { get; }
    }

    [Serializable]
    class ItemImplTwo : IItem
    {
        public string SomeStr { get; set; }
    }

    [Serializable]
    public class ItemImplOne : IItem
    {
        public string SomeStr { get; }

        public ItemImplOne(string someStr)
        {
            SomeStr = someStr;
        }
    }
}