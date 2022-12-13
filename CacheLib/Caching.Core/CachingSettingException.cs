using System;

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
