using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCALib
{
    public interface IDataTransformation<T>
    {
        T Transform(T data);
        T Reconstruct(T transformedData);
    }
}
