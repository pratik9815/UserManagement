using DataLayer.Entities;

namespace UserManagement.Domain.Entities
{
    public class ApplicationUserClaim
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClaimId { get; set; }
        public string ClaimValue { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationClaim Claim { get; set; }
    }
}
