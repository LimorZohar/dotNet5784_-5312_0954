

namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Collections.Generic;

internal class MilestoneImplementation : IMileStone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEnumerable<Dependency> Create()
    {
        throw new NotImplementedException();
    }

    public MileStone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(MileStone item)
    {
        throw new NotImplementedException();
    }
}

