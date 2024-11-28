using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Firebase;


namespace BLL.Interfaces
{
	public interface IFirebaseUserRepos
	{
		Task<UserProfile> GetUserProfileAsync(string uid);
		Task LogoutUserAsync(string uid);
	}
}
