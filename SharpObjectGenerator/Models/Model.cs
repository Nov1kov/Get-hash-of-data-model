using System.Collections.Generic;

namespace SharpObjectGenerator.Models
{
    public class Model
    {
        public int Int1 { get; set; }
        public string Str1 { get; set; }

        public List<AnotherClass> InitListItems { get; } = new List<AnotherClass>();

        public Item NestedItem { get; set; }
    }

    public class AnotherClass
    {
        private readonly int _id;
        public string StrField1 { get; set; }

        public AnotherClass(int id)
        {
            _id = id;
        }
    }

    public class Item
    {
        public double Double1 { get; set; }
        public string StrField1 { get; set; }
    }
}