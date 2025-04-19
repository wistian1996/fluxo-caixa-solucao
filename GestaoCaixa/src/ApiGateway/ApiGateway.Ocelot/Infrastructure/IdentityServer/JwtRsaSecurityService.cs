using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography;

namespace ApiGateway.Ocelot.Infrastructure.IdentityServer
{
    public class JwtRsaSecurityService
    {
        public const string RsaKeyId = "2940bfb8-7056-418f-8ec8-9a4c0bbe68be";

        public static RSA GetRsaPublicKeyAsPem()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var publicKeyPath = Path.Combine(currentDirectory, "identity_server_public.key");

            if (!File.Exists(publicKeyPath))
            {
                throw new FileNotFoundException(publicKeyPath);
            }

            var publicKey = File.ReadAllText(publicKeyPath);

            var rsa = RSA.Create();

            rsa.ImportFromPem(publicKey.ToCharArray());

            return rsa;
        }

        public static JwksModel GetRsaPublicKeyAsJwks()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var publicKeyPath = Path.Combine(currentDirectory, "identity_server_public.key");

            if (!File.Exists(publicKeyPath))
            {
                throw new FileNotFoundException(publicKeyPath);
            }

            var publicKey = File.ReadAllText(publicKeyPath);

            using var rsa = RSA.Create();

            rsa.ImportFromPem(publicKey.ToCharArray());

            var rsaKey = new RsaSecurityKey(rsa)
            {
                KeyId = RsaKeyId
            };

            var parameters = rsaKey.Rsa.ExportParameters(false);
            var e = Base64UrlEncoder.Encode(parameters.Exponent);
            var n = Base64UrlEncoder.Encode(parameters.Modulus);

            var jwks = new JwksModel
            {
                Keys =
                [
                    new JsonWebKey
                    {
                        Kty = "RSA",
                        Use = "sig",
                        Kid = rsaKey.KeyId,
                        Alg = "RS256",
                        N = n,
                        E = e
                    }
                ]
            };

            return jwks;
        }

        public static RsaSecurityKey GetRsaSecurityKey()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var privateKeyPath = Path.Combine(currentDirectory, "identity_server_private.key");

            if (!File.Exists(privateKeyPath))
            {
                throw new FileNotFoundException(privateKeyPath);
            }

            var privateKey = File.ReadAllText(privateKeyPath);

            var rsa = RSA.Create();

            rsa.ImportFromPem(privateKey.ToCharArray());

            return new RsaSecurityKey(rsa)
            {
                KeyId = RsaKeyId // importante para o JWKS
            };
        }
    }
}
