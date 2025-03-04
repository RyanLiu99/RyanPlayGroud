using System;
using System.Collections.Generic;

namespace SmallTests.Entities
{
    internal class KeyItem : BaseKeyItem
    {
    }

    
    public abstract class BaseKeyItem
    {

        public int Id { get; set; }

        public virtual string Name { get; set; }
    }
}
