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
        DalApi.IDal dal = DalApi.Factory.Get;

        public IEngineer Engineer => new EngineerImplementation();

        public ITask Task => new TaskImplementation();

        public DateTime? StartDate
        {
            get { return dal.StartDate; }
            set { dal.StartDate = value; }
        }

        /// <summary>
        /// the end date of the program
        /// </summary>
        public DateTime? EndDate
        {   
            get { return dal.EndDate; }
            set
            {   
                if (StartDate is null ||
                    StartDate > value ||
                    dal.Task.ReadAll(x => x.StartDate == null ||
                    (x.StartDate + x.RequiredEffortTime) > value).Any())
                    throw new Exception();//TODO

                dal.EndDate = value;
            }
        }

        public DateTime Clock { 
            set { dal.Clock = value; }
            get { return dal.Clock; } }
    }
}
