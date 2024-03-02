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
        public IEngineer Engineer =>  new EngineerImplementation(this);

        public IMileStone MileStone =>  new MilestoneImplementation(this);

        public ITask Task =>  new TaskImplementation(this);

        private static DateTime dateTime = DateTime.Now.Date;

        public DateTime Clock { get { return dateTime; } private set { dateTime = value; } }
        public void AddDay()
        {
            dateTime = dateTime.AddDays(1);
        }
        public void AddMonth()
        {
            dateTime = dateTime.AddMonths(1);
        }
        public void AddYear()
        {
            dateTime = dateTime.AddYears(1);
        }
        public void ResetClock()
        {
            dateTime = DateTime.Now.Date;
        }

     }
}
