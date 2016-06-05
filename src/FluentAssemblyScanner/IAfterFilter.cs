using System;
using System.Collections.Generic;

namespace FluentAssemblyScanner
{
    public interface IAfterFilter
    {
        List<Type> Scan();
    }
}