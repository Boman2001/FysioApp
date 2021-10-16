namespace Core.Domain.Models
{
    public class Doctor : User
    {
        public string BigNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}