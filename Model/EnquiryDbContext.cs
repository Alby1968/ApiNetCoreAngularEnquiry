using ApiNetCoreAngularEnquiry.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiNetCoreAngular.Model
{
    public class EnquiryDbContext : DbContext
    {
        public EnquiryDbContext(DbContextOptions<EnquiryDbContext> options) : base(options)
        {
        }
        public DbSet<EnquiryStatus> EnquiryStatus { get; set; }
        public DbSet<EnquiryType> EnquiryType { get; set; }
        public DbSet<EnquiryModel> EnquiryModel { get; set; }


    }
}
