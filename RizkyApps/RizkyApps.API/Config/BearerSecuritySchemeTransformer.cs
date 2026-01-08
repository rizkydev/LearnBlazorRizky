using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RizkyApps.API.Config
{
    internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

    public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    {
        _authenticationSchemeProvider = authenticationSchemeProvider;
    }

    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

        // Only proceed if Bearer authentication is configured
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            // 1. Define the Bearer security scheme
            var bearerScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Enter your token (without 'Bearer' prefix)."
            };

            // 2. Ensure components are initialized and add the scheme[citation:1]
            document.Components ??= new OpenApiComponents();
            document.AddComponent("Bearer", bearerScheme); // Key new .NET 10 method

            // 3. Create a security requirement using the new OpenApiSecuritySchemeReference[citation:1][citation:2]
            var securityRequirement = new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = [] // Empty array = no scopes required
            };

            // 4. Apply the requirement to all operations
            foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations))
            {
                operation.Value.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Value.Security.Add(securityRequirement);
            }
        }
    }
}
}
