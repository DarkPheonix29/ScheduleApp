using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
	[FirestoreData]
	public class EmailData
	{
		[FirestoreDocumentId] // Maps to the Firestore document ID
		public string Id { get; set; }

		[FirestoreProperty("email")]
		public string Email { get; set; }

		[FirestoreProperty("role")]
		public string Role { get; set; }
	}
}
