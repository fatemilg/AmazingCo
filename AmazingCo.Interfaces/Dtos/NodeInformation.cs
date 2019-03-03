using System.Collections.Generic;

namespace AmazingCo.Dtos
{
    public class NodeInformation
    {
        public NodeInformation(
            int id, 
            int? parentId,  
            string title, 
            int level,
            IEnumerable<int> children
        )
        {
            ParentId = parentId;
            Id = id;
            Title = title;
            Children = children;
            Level = level;
        }

        public int Id { get; }
        public int Level { get; }
        public int? ParentId { get; }
        public string Title {get; }
        public IEnumerable<int> Children { get; }
    }
}