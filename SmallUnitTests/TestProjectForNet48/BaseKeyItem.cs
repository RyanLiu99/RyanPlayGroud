using System;
using System.Collections.Generic;

namespace TestProjectForNet48
{
    internal class KeyItem : BaseKeyItem
    {
    }

    internal class KeyItem2 : KeyItem
    {
        public KeyItem2(BaseKeyItem keyItem) 
        {
            this.Id = keyItem.Id;
            this.Name = keyItem.Name;
        }
    }


    public abstract class BaseKeyItem
    {
        public int Id { get; set; }

        public virtual string Name { get; set; }
    }
}
