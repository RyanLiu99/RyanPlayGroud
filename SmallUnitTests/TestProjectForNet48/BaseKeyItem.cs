using System;
using System.Collections.Generic;

namespace TestProjectForNet48
{
    public class KeyItem : BaseKeyItem
    {
        public KeyItem()
        {
            
        }
    }

    internal class KeyItem2 : KeyItem
    {
        public KeyItem2()
        {
            
        }
        public KeyItem2(BaseKeyItem keyItem) 
        {
            this.Id = keyItem.Id;
            this.Name = keyItem.Name;
        }
    }


    public abstract class BaseKeyItem
    {
        public int? generation;

        public int Id { get; set; }

        public virtual string Name { get; set; }

        public KeyItem Child1 { get; set; }
        public KeyItem Child2 { get; set; }

    }
}
