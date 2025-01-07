namespace BLL.Models
{
	public class UserProfile
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string PickupAddress { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
