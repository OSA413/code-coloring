using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public interface Repository<T>
    {
        void Add(Type newInstanceType, string id);
        T Get(string id);
    }
}
