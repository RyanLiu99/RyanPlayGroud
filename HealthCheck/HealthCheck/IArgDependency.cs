using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck
{
    public interface IArgDependency
    {
        string Id { get;  }

    }

    public class ArgDependency : IArgDependency
    {
        public ArgDependency()
        {
                Id = Guid.NewGuid().ToString();
                Console.WriteLine("ArgDependency created with auto " + Id);
        }

        public ArgDependency(string id)
        {
            Id = id;
            Console.WriteLine("ArgDependency created with given " + Id);
        }
        public string Id { get; private set; }
    }
}
