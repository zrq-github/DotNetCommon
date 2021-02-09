using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Config
{
    interface ConfigInterface
    {
        void Load();
        void Save();
    }
}
