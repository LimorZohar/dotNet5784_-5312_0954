using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        public IEngineer Engineer =>  new EngineerImplementation();

        public IMileStone MileStone =>  new MilestoneImplementation();

        public ITask Task =>  new TaskImplementation();
    }
}
