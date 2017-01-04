using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Common.Helpers
{
    public class DiContainerHelper
    {
        public static void RegisterComponents<T>(UnityContainer container) where T : LifetimeManager, new()
        {

        }
    }
}
