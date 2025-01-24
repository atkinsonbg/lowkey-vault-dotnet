using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

// this is the URL for the default Key Vault running in the lowkey-vault container
string kvUri = Environment.GetEnvironmentVariable("KEY_VAULT_URI")!;

#region THIS IS DONE IN BUILDER/DI IN APP STARTUP

// define a custom HttpClientHandler that will ignore SSL violations
// HERE BE DRAGONS!!!!! DO NOT USE THIS IN A LIVE ENVIRONMENT, THIS IS FOR LOCAL/TESTING ONLY
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
};

// create the options and set the Transport to the use the HttpClientHandler from above
// DisableChallengeResourceVerification allows the client to talk to any vault (*)
// HERE BE DRAGONS!!!!! DO NOT USE THIS IN A LIVE ENVIRONMENT, THIS IS FOR LOCAL/TESTING ONLY
var options = new SecretClientOptions
{
    DisableChallengeResourceVerification = true,
    Transport = new HttpClientTransport(new HttpClient(handler))
};

#endregion

// create the SecretClient using the options we just defined, this will now talk to the local lowkey-vault container without errors
// this code should look identical to your current Production code
var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(), options);

// try to set a new secret
const string newSecretName = "new-secret";
await client.SetSecretAsync(newSecretName, "This is my new secret!");
Console.WriteLine($"{newSecretName} was successfully set");

// get that same secret to ensure its all working
var newSecret = await client.GetSecretAsync(newSecretName);
Console.WriteLine($"{newSecretName} was fetched, its value is: {newSecret.Value.Value}");

// get a pre-loaded secret to ensure seeding is working
const string preLoadedSecretName = "secret-name";
var preloadedSecret = await client.GetSecretAsync(preLoadedSecretName);
Console.WriteLine($"{preLoadedSecretName} was preloaded into the container, its value is: {preloadedSecret.Value.Value}");

// get a pre-loaded secret to ensure seeding is working
const string preLoadedDbSecretName = "team-db-password";
var preloadedDbSecret = await client.GetSecretAsync(preLoadedDbSecretName);
Console.WriteLine($"{preLoadedDbSecretName} was preloaded into the container, its value is: {preloadedDbSecret.Value.Value}");