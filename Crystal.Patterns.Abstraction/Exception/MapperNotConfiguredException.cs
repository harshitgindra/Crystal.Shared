using System;
using System.Collections.Generic;
using System.Text;

namespace Crystal.Patterns.Abstraction
{
    public class MapperNotConfiguredException : Exception
    {
        public MapperNotConfiguredException() : base("Mapper is not configured")
        {

        }
    }
}
