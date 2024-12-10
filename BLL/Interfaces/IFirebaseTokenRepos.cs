using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IFirebaseTokenManager
	{
		Task<bool> VerifyTokenAsync(string idToken);
		Task RevokeTokensAsync(string uid);
	}
}
