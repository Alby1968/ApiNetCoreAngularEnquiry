using ApiNetCoreAngularEnquiry.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]  // <--- ora corrisponde alla policy
[ApiExplorerSettings(GroupName = "Enquiry")]
public class EnquiryMasterController : ControllerBase
{
    //private readonly EnquiryDbContext _context;
    //public EnquiryMasterController(EnquiryDbContext context)
    //{
    //    _context = context;
    //}

    // Per chiamate a Supabase
    private readonly HttpClient _httpClient;
    private readonly string _supabaseUrl = "https://zanjsybekkkadfqfbilc.supabase.co/rest/v1/";
    private readonly string _apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InphbmpzeWJla2trYWRmcWZiaWxjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzczNzc1OTIsImV4cCI6MjA5Mjk1MzU5Mn0.jP2PlBhacXCirKmyu8nSLAuKjIRCRH7LKYoKK0BU4SA";

    public EnquiryMasterController(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    //[HttpGet("GetAllStatus")]
    //public List<EnquiryStatus> GetAllStatus() => _context.EnquiryStatus.ToList();
    [HttpGet("GetAllStatus")]
    public async Task<List<EnquiryStatus>> GetAllStatus()
    {
        var response = await _httpClient.GetAsync($"{_supabaseUrl}EnquiryStatus?select=*");

        if (!response.IsSuccessStatusCode)
            return new List<EnquiryStatus>();

        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<EnquiryStatus>>(json);
    }

    [HttpGet("GetAllTypes")]
    public async Task<List<EnquiryType>> GetAllTypes()
    {
        var response = await _httpClient.GetAsync($"{_supabaseUrl}EnquiryType?select=*");

        if (!response.IsSuccessStatusCode)
            return new List<EnquiryType>();

        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<EnquiryType>>(json);
    }
    //[HttpGet("GetAllEnquiry")]
    //public List<EnquiryModel> GetAllEnquiry() => _context.EnquiryModel.ToList();

    //[HttpGet("GetAllEnquiry")]
    //public async Task<List<EnquiryModel>> GetAllEnquiry()
    //{
    //    var response = await _httpClient.GetAsync($"{_supabaseUrl}EnquiryModel?select=*");
    //    var json = await response.Content.ReadAsStringAsync();

    //    return JsonConvert.DeserializeObject<List<EnquiryModel>>(json);
    //}

    //[HttpPost("CreateNewEnquiry")]
    //public EnquiryModel AddNewEnquiry(EnquiryModel newEnquiry)
    //{
    //    newEnquiry.createdDate = DateTime.Now;
    //    _context.EnquiryModel.Add(newEnquiry);
    //    _context.SaveChanges();
    //    return newEnquiry;
    //}
    [HttpPost("CreateNewEnquiry")]
public async Task<EnquiryModel?> AddNewEnquiry(EnquiryModel newEnquiry)
{
    newEnquiry.createdDate = DateTime.UtcNow;

    var content = new StringContent(
        JsonConvert.SerializeObject(newEnquiry),
        Encoding.UTF8,
        "application/json"
    );

    // 🔥 HEADER FONDAMENTALE PER SUPABASE
    var request = new HttpRequestMessage(HttpMethod.Post, $"{_supabaseUrl}EnquiryModel");
    request.Content = content;
    request.Headers.Add("Prefer", "return=representation");

    var response = await _httpClient.SendAsync(request);

    if (!response.IsSuccessStatusCode)
        return null;

    var json = await response.Content.ReadAsStringAsync();

    var result = JsonConvert.DeserializeObject<List<EnquiryModel>>(json);

    return result?.FirstOrDefault();
}
    [HttpPut("UpdateEnquiry")]
    public async Task<bool> Update(EnquiryModel updateEnquiry)
    {
        var content = new StringContent(
            JsonConvert.SerializeObject(updateEnquiry),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PatchAsync(
            $"{_supabaseUrl}EnquiryModel?enquiryId=eq.{updateEnquiry.enquiryId}",
            content
        );

        return response.IsSuccessStatusCode;
    }
    //    
    //[HttpDelete("DeleteEnquiryById")]
    //public async Task<bool> Delete(int id)
    //{
    //    var response = await _httpClient.DeleteAsync(
    //        $"{_supabaseUrl}EnquiryModel?enquiryId=eq.{id}"
    //    );

    //    return response.IsSuccessStatusCode;
    //}
    [HttpDelete("DeleteEnquiryById")]
    public async Task<bool> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_supabaseUrl}EnquiryModel?enquiryId=eq.{id}"
        );

        return response.IsSuccessStatusCode;
    }
}
