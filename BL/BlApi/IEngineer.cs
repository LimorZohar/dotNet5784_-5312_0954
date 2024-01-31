using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IEngineer
    {
        public IEnumerable<BO.Engineer> ReadAll (Func<BO.Engineer, bool> ?filter=null);//get the all engineers
        public int Create(BO.Engineer boEngineer);//add a new engineer
        public BO.Engineer? Read(int id);//get a engineer details
        public void Update(BO.Engineer boEngineer);//update the engineer details
        public void Delete(int id);//delete engineer
    }
}
