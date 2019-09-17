using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fesenmeier_Fujitsu.Services
{
    public class PicsumService : IPicsumService
    {
        public string GetRandomImg()
        {
            return "https://picsum.photos/400/600";
        }
    }
}
