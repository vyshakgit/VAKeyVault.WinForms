# VAKeyVault.WinForms
.Net 4.8 library to encrypt and decrypt using Azure key vault private keys

A .NET Framework 4.8 class library that provides **secure encryption and decryption** using **Azure Key Vault**.  
Designed for **Windows Forms (WinForms)** and other desktop applications.

---

## Purpose

This library allows desktop applications to encrypt and decrypt sensitive data without storing private keys locally.

- Data is encrypted locally
- Key decryption is performed securely in Azure Key Vault
- Private keys never leave Azure

---

## Key Features

- .NET Framework **4.8 compatible**
- Hybrid encryption (AES + RSA)
- Azure Key Vault integration
- Certificate-based Azure AD authentication
- Primary / Secondary key rotation support
- Safe defaults with minimal configuration

---

## Cryptography Used

- **AES-256** for encrypting data locally
- **RSA-OAEP-256 (SHA-256)** for encrypting/decrypting the AES key
- RSA operations use **RSACng** (required for OAEP-SHA256 on .NET 4.8)

---

## Requirements

- Windows OS
- .NET Framework 4.8
- Azure Key Vault
- Azure AD App Registration
- Client certificate installed on the machine

---

## Azure Prerequisites

### Azure Key Vault
- RSA keys created in Key Vault  
  - `va-encryption-key-primary`
  - `va-encryption-key-secondary`
- Key size: 2048 bits or higher
- Key operation: `Decrypt`

### Azure AD App Registration
- Application registered in Azure AD
- Public certificate uploaded
- Access granted to Key Vault keys (`keys/decrypt`)

### Client Certificate
- Installed in:  LocalMachine → Personal → Certificates

- Private key must be present
- Thumbprint used for authentication

---

## Configuration (Consumer Application)

Add the following values to the **calling application's** `app.config`:

```xml
<appSettings>
<add key="VA_KV_TenantId" value="YOUR_TENANT_ID" />
<add key="VA_KV_ClientId" value="YOUR_CLIENT_ID" />
<add key="VA_KV_CertThumbprint" value="CERT_THUMBPRINT" />

<!-- Optional: Defaults to Primary -->
<add key="VA_KV_ActiveSlot" value="Primary" />
</appSettings>

## Usage Example
// Encrypt
byte[] encrypted = HybridEncryptor.Encrypt(
    Encoding.UTF8.GetBytes("Sensitive data"));

// Decrypt
byte[] decrypted = HybridDecryptor.Decrypt(
    encrypted,
    "https://your-keyvault-name.vault.azure.net");

string result = Encoding.UTF8.GetString(decrypted);

## Key Rotation

Key rotation is controlled internally by the library

Switching between Primary and Secondary keys is done via configuration (calling windows forms app config)

## Security Notes

Private RSA keys never exist in application memory

No secrets are stored in source code

Authentication uses certificates (no client secrets)

Uses Microsoft-recommended cryptographic algorithms

## Supported Application Types

Windows Forms (.NET Framework 4.8)

Console applications

Windows Services

## Notes

The application must have permission to access the certificate private key

Internet access is required to communicate with Azure Key Vault

Windows-only (CNG dependency)
