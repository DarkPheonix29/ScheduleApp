using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
	public class UserProfile
	{
		public int Id { get; set; }
		public string FirebaseUid { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		// Add any custom fields you need
	}

}
