using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.API
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }

            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { 
                new ApiScope("LoanAPI.read"), 
                new ApiScope("LoanAPI.write"), 
                new ApiScope("StockAPI.read"), 
                new ApiScope("StockAPI.write") 
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("LoanAPI")
                {
                    Scopes = new List<string> 
                    { 
                        "LoanAPI.read", 
                        "LoanAPI.write" 
                    }
                },
                new ApiResource("StockAPI")
                {
                    Scopes = new List<string> 
                    { 
                        "StockAPI.read", 
                        "StockAPI.write" 
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "AngularClient",
                    ClientName = "AngularClient",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    AuthorizationCodeLifetime = 1800,
                    IdentityTokenLifetime = 1800,
                    ClientSecrets = 
                    { 
                        new Secret("AngularSecret".Sha256()) 
                    },
                    AllowedScopes = 
                    {   
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        "LoanAPI.read",
                        "LoanAPI.write",
                        "StockAPI.read",
                        "StockAPI.write",
                        "role"
                    },
                    RedirectUris = {"http://localhost:4200/callback"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    PostLogoutRedirectUris = {"http://localhost:4200"},
                    RequirePkce = true
                }
            };
    }
}