namespace DiscplinaMobileNoite.Domain.Dto
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public int PhoneNumber { get; set; }
        public int Workload { get; set; }
    }
}