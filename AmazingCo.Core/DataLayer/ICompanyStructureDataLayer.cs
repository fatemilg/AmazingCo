using System.Collections.Generic;

namespace AmazingCo.DataLayer
{
    public interface ICompanyStructureDataLayer
    {
        IEnumerable<NodeSnapshot> GetAll();
        void WriteAll(IEnumerable<NodeSnapshot> nodes);
    }
}
