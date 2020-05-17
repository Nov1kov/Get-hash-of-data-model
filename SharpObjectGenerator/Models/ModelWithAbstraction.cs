using System.Collections;
using System.Collections.Generic;

namespace SharpObjectGenerator.Models
{
    public class ModelWithAbstraction
    {
        public IList<IItem> Items { get;  } = new List<IItem>();
    }

    public interface IItem
    {
        string SomeStr { get; }
    }

    class ItemImplTwo : IItem
    {
        public string SomeStr { get; set; }
    }

    public class ItemImplOne : IItem
    {
        public string SomeStr { get; }

        public ItemImplOne(string someStr)
        {
            SomeStr = someStr;
        }
    }
}