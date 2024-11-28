using BLL.Firebase;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;	

namespace BLL.Interfaces
{
	public interface IFirebaseKeyManager
	{
		Task<string> GenerateRegistrationKeyAsync();
		Task<bool> ValidateRegistrationKeyAsync(string key);
		Task<List<KeyData>> GetAllKeysAsync();
	}
}
