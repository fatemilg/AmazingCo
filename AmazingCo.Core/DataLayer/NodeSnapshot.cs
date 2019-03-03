namespace AmazingCo.DataLayer
{
    public class NodeSnapshot
    {
        public NodeSnapshot(int id, string title, int parentId)
        {
            Id = id;
            ParentId = parentId;
            Title = title;
        }
        public int Id { get; }
        public int ParentId { get; }
        public string Title { get; }
    }
}