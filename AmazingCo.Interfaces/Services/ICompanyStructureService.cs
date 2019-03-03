using AmazingCo.Dtos;

namespace AmazingCo.Services
{
    public interface ICompanyStructureService
    {
        bool TryGetNode(int id, out NodeInformation node);
        bool TryUpdateNode(int id, int parentId, out NodeInformation modifiedNode);
    }
}