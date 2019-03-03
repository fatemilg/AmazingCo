using System.Collections.Generic;

namespace AmazingCo.DataLayer
{
    public class FakeCompanyStructureDataLayer : ICompanyStructureDataLayer
    {
        public IEnumerable<NodeSnapshot> GetAll()
        {
            yield return new NodeSnapshot(0, "00", -1);
            yield return new NodeSnapshot(1, "01", 0);
            yield return new NodeSnapshot(2, "02", 0);
            yield return new NodeSnapshot(10, "10", 1);
            yield return new NodeSnapshot(11, "11", 1);
            yield return new NodeSnapshot(20, "20", 2);
            yield return new NodeSnapshot(21, "21", 2);
        }

        public void WriteAll(IEnumerable<NodeSnapshot> nodes)
        {
        }
    }
}