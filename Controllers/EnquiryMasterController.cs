using ApiNetCoreAngular.Model;
using ApiNetCoreAngularEnquiry.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiNetCoreAngularEnquiry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [ApiExplorerSettings(GroupName = "Enquiry")]
    public class EnquiryMasterController : ControllerBase
    {
        private readonly EnquiryDbContext _context;
        public EnquiryMasterController(EnquiryDbContext contenxt)
        {
            _context = contenxt;
        }

        [HttpGet("GetAllStatus")]
        public List<EnquiryStatus> GetAllStatus()
        {
            var statusList = _context.EnquiryStatus.ToList();
            return statusList;
        }

        [HttpGet("GetAllTypes")]
        public List<EnquiryType> GetAllTypes()
        {
            var typeList = _context.EnquiryType.ToList();
            return typeList;
        }

        [HttpGet("GetAllEnquiry")]
        public List<EnquiryModel> GetAllEnquiry()
        {
            var enquiryList = _context.EnquiryModel.ToList();
            return enquiryList;
        }

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
            var existingEnquiry = _context.EnquiryModel.Find(updateEnquiry.enquiryId);
            if (existingEnquiry != null)
            {
                existingEnquiry.enquiryTypeId = updateEnquiry.enquiryTypeId;
                existingEnquiry.enquiryStatusId = updateEnquiry.enquiryStatusId;
                existingEnquiry.customerName = updateEnquiry.customerName;
                existingEnquiry.mobileNo = updateEnquiry.mobileNo;
                existingEnquiry.email = updateEnquiry.email;
                existingEnquiry.message = updateEnquiry.message;
                existingEnquiry.resolution = updateEnquiry.resolution;
                _context.SaveChanges();
            }
            return updateEnquiry;
        }

        //[HttpDelete("DeleteEnquiry/{id}")]
        [HttpDelete("DeleteEnquiryById")]
        public bool Delete(int id)
        {
            var existingEnquiry = _context.EnquiryModel.Find(id);
            if (existingEnquiry != null)
            {
                _context.EnquiryModel.Remove(existingEnquiry);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpGet("GetAllTypeById")]
        public string GetAllTypeById(int id)
        {
            EnquiryType? typeObj = _context.EnquiryType.Find(id);
            if (typeObj != null)
            {
                return typeObj.typeName;
            }
            else
            {
                return "NA";
            }
        }

        //[HttpGet("GetAllStatusByid")]
        [ApiExplorerSettings(IgnoreApi = true)]     // per nascondere questo endpoint dalla documentazione API Swaggger, se necessario

        public EnquiryStatus GetAllStatusByid(int id)
        {
            EnquiryStatus? typeObj = _context.EnquiryStatus.Find(id);
            if (typeObj != null)
            {
                return typeObj;
            }
            else
            {
                // Restituisce un oggetto EnquiryStatus di default per evitare il ritorno di null
                return new EnquiryStatus { statusId = id, status = "NA" };
            }
        }

    }
}
