# JGP.Core

# ApiKey

	==========
	Storage:
	==========
	[] Self-Generation of KeyStoreContext is currently untested.
	[] Key-Storage is tested with an existing database with the follwing schema:
		Name: KeyStore
		Tables: dbo.Services
			=> Columns:	[] ServiceId (PK, uniqueidentifier, not null)
					[] ServiceName (nvarchar(50), not null)
					[] ApiKey (nchar(36), not null)
			=> Keys:	[] PK_Services (ServiceId)
			=> Indexes:	[] IX_Service_ApiKey (Unique, Non-Clustered)
					[] IX_Service_ServiceName (Unique, Non-Clustered)
					[] PK_Services (Clustered)
	
	==========
	Usage:
	==========
	Variables:
	[] Environmental Variables are Untested, however should function as so:
		"ServiceId" (Guid) - The ServiceId of the Service to retrieve the ApiKey for.
			=> Stored as Key: "ServiceId", Value: "Guid"
		"ServiceName" (String) - The ServiceName of the Service to retrieve the ApiKey for.
			=> Stored as Key: "ServiceName", Value: "String"
	[] For AppSettings, register as following:
		"JGPKeyAuth": {
		"ServiceId": "[SOME GUID]",
		"ServiceName": "[NAME OF SERVICE]"
		}
	
	Registering the Services (IServiceCollection and IConfiguration REQUIRED):
	[STEP 1] Register the Services:
		services.AddApiKeyManagement(configuration);
	[STEP 2] Set Up Authentication:
		var settings = services.BuildServiceProvider().GetService<ApiKeyAuthenticationSettings>();
		services.AddAuthentication(options =>
		{
		    options.DefaultAuthenticateScheme = ApiKeyAuthenticationSettings.DefaultScheme;
		    options.DefaultChallengeScheme = ApiKeyAuthenticationSettings.DefaultScheme;
		}).AddApiKeyManagement(settings);

	==========
	Consumption of API Services
	==========
	Registering the Options (IServiceCollection and IConfiguration REQUIRED):
	[STEP 1] services.RegisterApiKeyOptions(configuration);
	[STEP 2] Inject the options into your HttpClient and use the Url as the Base Url for all requests.
	