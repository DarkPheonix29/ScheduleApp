using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;

public class FirebaseService
{
	public FirebaseService()
	{
		// Initialize Firebase Admin SDK with the service account credentials
		FirebaseApp.Create(new AppOptions()
		{
			Credential = GoogleCredential.GetApplicationDefault() // Uses ADC automatically
		});
	}
}