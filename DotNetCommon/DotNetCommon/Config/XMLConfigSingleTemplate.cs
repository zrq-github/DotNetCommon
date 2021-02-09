using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Config
{
    public class XMLConfigSingleTemplate<T> : XMLConfigTemplate<T> where T : new()
    {
        public XMLConfigSingleTemplate()
        {
        }

    }
}
