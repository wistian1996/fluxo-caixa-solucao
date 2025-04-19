using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;

namespace SaldoConsolidado.API.Infrastructure.IdentityServer
{
    public class IdentityServerJwksService
    {
        private const string Jwks = "{\"keys\":[{\"alg\":\"RS256\",\"e\":\"AQAB\",\"key_ops\":[],\"kid\":\"2940bfb8-7056-418f-8ec8-9a4c0bbe68be\",\"kty\":\"RSA\",\"n\":\"p37wtyTP8H6nrXcnDqvXhuMNTdppuhAqh23wqA2ejuOSt07RRDoP80A-5CfAOI-uiVqa7yxjM9pLrzgHsjgLFaGoX-ZtZACe3vCJzeCYf5aFFLE4voVRcPM8-N7jIE5t-dqJoSPrRjM0cqRhN3hOOCUaDDItZsioRe2TtL-3Z0YfDrU5q1WzYzAME1-4OEyjq-TgjEZ2jhB7g9w7ouHJYpm9cvTyGj4xH9gaKMZdAjg9yYBvnQDy6WkT61LOfCgZhPotZ7xKB1Yu_5moMhglQiqNEk06nnh02Z2gd-MuWbE_bxYH1EDZs54XXcxUq2pSqMzUrNAAD4-oC4x88uUaWw\",\"oth\":[],\"use\":\"sig\",\"x5c\":[]}]}\r\n";

        public static SecurityKey? GetKeyJwksById(string keyId)
        {
            //var client = new HttpClient();
            //var jwksUri = "identity-server/.well-known/jwks.json";
            //var response = client.GetAsync(jwksUri).Result;
            //var jwks = response.Content.ReadAsStringAsync().Result;

            var keys = JsonSerializer.Deserialize<JsonWebKeySet>(Jwks);

            return keys!.GetSigningKeys().FirstOrDefault(s => s.KeyId == keyId);
        }
    }
}
