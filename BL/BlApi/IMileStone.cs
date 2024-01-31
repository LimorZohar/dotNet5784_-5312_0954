using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IMileStone
    {
        public IEnumerable<DO.Dependency> Create();
        public BO.MileStone? Read(int id);
        public void Update(BO.MileStone item);
    }
}
