using AmazingCo.Dtos;

namespace AmazingCo.Model
{
    public static class Extensions
    {
        public static NodeModel ToModel(this NodeInformation node) =>
            new NodeModel
            {
                Children = node.Children,
                Id = node.Id,
                ParentId = node.ParentId,
                Title = node.Title,
                Level = node.Level
            };
    }
}
