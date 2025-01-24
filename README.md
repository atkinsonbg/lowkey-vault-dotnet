# lowkey-vault-dotnet
Example of utilizing the lowkey-vault emulator for working with Azure Key Vault locally.

## Requirements
- Use only the `lowkey-vault` container - no other continaers should be required
- Do not install certificates into environment - wanted a solution that would not require any certs being installed on the dev laptop or environment
- Should be accomplished via DI - wanted a solution that would allow for injecting configuration via DI so the core code can remain the same regardless of environment

## Docker Setup
Review the `docker-compose.yml` file for setup, but the only container required is `nagyesta/lowkey-vault:2.8.0`. There is a volume mount that allows for pre-seeding keys into the vault, which is located at `.lowkey-vault/import`. Various `LOWKEY_ARGS` are configured so all this works correctly.

## Environment Variables
The following are set in the `launch.json` file and are required:

- `IDENTITY_ENDPOINT`: This is set the a URL running in the `lowkey-vault` container setup to allowing mocking when using Managed Identities. The `SecretClient` will reach out to the endpoint defined here when using `DefaultAzureCredential` so it allows for a mocked identity to be used locally.

- `IDENTITY_HEADER`: Required to make the `IDENTITY_ENDPOINT` operate correctly

- `KEY_VAULT_URI`: Allows for targeting a Key Vault based on the environment: local, dev, prod, etc.

## Notes
- Keys can be pre-seeded using the Docker mount
- Keys created via code are not persisted after the container is stopped

## References
- https://github.com/nagyesta/lowkey-vault/wiki/Example:-How-can-you-use-Lowkey-Vault-in-your-tests#5-authentication
- https://github.com/nagyesta/assumed-identity
