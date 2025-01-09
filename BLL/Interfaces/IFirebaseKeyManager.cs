
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
		Task<bool> UseRegistrationKeyAsync(string key);
		Task<List<KeyData>> GetAllKeysAsync();
		Task<string> GenerateKeyAsync();
		Task<List<KeyData>> FetchKeysAsync();
		Task<bool> MarkKeyAsUsedAsync(string key);
	}
}
