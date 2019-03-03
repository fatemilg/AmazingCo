using System.Collections.Generic;

namespace AmazingCo.Model
{
    public class NodeModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public IEnumerable<int> Children { get; set; }
        public int Level { get; set; }
    }
}
