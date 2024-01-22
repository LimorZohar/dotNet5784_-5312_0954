using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface ICrud<T> where T : class
    {
        /// <summary>
        /// Creates new entity object in DAL
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Create(T item); 

        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Read(int id);

        T? Read(Func<T, bool> filter);
        IEnumerable<T> ReadAll(Func<T, bool> filter = null!);// stage 2
        void Update(T item); //Updates entity object
        void Delete(int id); //Deletes an object by its Id
    }
}
