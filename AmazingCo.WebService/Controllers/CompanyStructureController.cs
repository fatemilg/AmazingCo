using AmazingCo.Model;
using AmazingCo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AmazingCo.Controllers
{
    [Route("CompanyStructure")]
    [ApiController]
    public class CompanyStructureController : ControllerBase
    {
        public CompanyStructureController(ICompanyStructureService companyStructureDataLayer) 
        {
            _companyStructureService = 
                companyStructureDataLayer ?? 
                throw new System.ArgumentNullException(nameof(companyStructureDataLayer));
        }

        private readonly ICompanyStructureService _companyStructureService;

        [HttpGet]
        [Route("GetNode/{id}")]
        public ActionResult<NodeModel> GetNode(int id)
        {
            if (_companyStructureService.TryGetNode(id, out var node))
                return node.ToModel();

            return NotFound(id);
        }

        [HttpPatch]
        [Route("MoveNode/{id}/{parentId}")]
        public ActionResult<NodeModel> MoveNode(int id, int parentId)
        {
            if (_companyStructureService.TryUpdateNode(id, parentId, out var node))
                return node.ToModel();

            return NotFound(
                new
                {
                    Id = id,
                    ParentId = parentId,
                    ErrorMessage= "One or both of the node ids is/are not valid."
                }
            );
        }
    }
}