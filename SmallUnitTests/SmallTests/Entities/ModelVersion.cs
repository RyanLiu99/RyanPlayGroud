using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ringba.Models
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ModelVersionAttribute : Attribute
    {
        private readonly int _version;

        public int Version
        {
            get
            {
                return _version;
            }
        }

        public ModelVersionAttribute(int version)
        {
            _version = version;
        }

    }

   
}
