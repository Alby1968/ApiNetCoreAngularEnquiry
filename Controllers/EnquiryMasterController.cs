using ApiNetCoreAngular.Model;
using ApiNetCoreAngularEnquiry.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]  // <--- ora corrisponde alla policy
[ApiExplorerSettings(GroupName = "Enquiry")]
public class EnquiryMasterController : ControllerBase
{
    private readonly EnquiryDbContext _context;
    public EnquiryMasterController(EnquiryDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetAllStatus")]
    public List<EnquiryStatus> GetAllStatus() => _context.EnquiryStatus.ToList();

    [HttpGet("GetAllTypes")]
    public List<EnquiryType> GetAllTypes() => _context.EnquiryType.ToList();

    [HttpGet("GetAllEnquiry")]
    public List<EnquiryModel> GetAllEnquiry() => _context.EnquiryModel.ToList();

    [HttpPost("CreateNewEnquiry")]
    public EnquiryModel AddNewEnquiry(EnquiryModel newEnquiry)
    {
        newEnquiry.createdDate = DateTime.Now;
        _context.EnquiryModel.Add(newEnquiry);
        _context.SaveChanges();
        return newEnquiry;
    }

    [HttpPut("UpdateEnquiry")]
    public EnquiryModel Update(EnquiryModel updateEnquiry)
    {
        var existing = _context.EnquiryModel.Find(updateEnquiry.enquiryId);
        if (existing != null)
        {
            existing.enquiryTypeId = updateEnquiry.enquiryTypeId;
            existing.enquiryStatusId = updateEnquiry.enquiryStatusId;
            existing.customerName = updateEnquiry.customerName;
            existing.mobileNo = updateEnquiry.mobileNo;
            existing.email = updateEnquiry.email;
            existing.message = updateEnquiry.message;
            existing.resolution = updateEnquiry.resolution;
            _context.SaveChanges();
        }
        return updateEnquiry;
    }

    [HttpDelete("DeleteEnquiryById")]
    public bool Delete(int id)
    {
        var existing = _context.EnquiryModel.Find(id);
        if (existing != null)
        {
            _context.EnquiryModel.Remove(existing);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}