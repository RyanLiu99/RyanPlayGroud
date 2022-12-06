using System;
using System.Collections.Generic;
using System.Text;

namespace Medrio.Caching.Abstraction
{
    public class CachingSettingException : Exception
    {
        public CachingSettingException() : base()
        {
                
        }
        public CachingSettingException(string message) : base(message) 
        {
                
        }
    }
}
