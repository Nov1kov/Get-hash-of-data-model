using System;
using System.Collections.Generic;

namespace DataModel
{
    [Serializable]
    public class Model
    {
        public int Int1 { get; set; }
        public string Str1 { get; set; }
        public List<Item> SomeListItems { get; } = new List<Item>();
        public AnotherClass ClassField { get; set; }
        [NonSerialized] public int IgnoreField;
    }

    [Serializable]
    public class AnotherClass
    {
        private readonly int _id;
        public decimal DecimalField { get; set; }

        public AnotherClass(int id)
        {
            _id = id;
        }
    }

    [Serializable]
    public class Item
    {
        public double DoubleField { get; set; }
        public float FloatField { get; set; }
        private byte[] _bytes;
        public Item(byte[] bytes)
        {
            _bytes = bytes;
        }
    }
}