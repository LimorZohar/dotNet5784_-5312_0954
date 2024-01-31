using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    internal interface IBI
    {
        public IEngineer Student { get; }
        public IMileStone Course { get; }
        public ITask GradeSheet { get; }
    }
}
