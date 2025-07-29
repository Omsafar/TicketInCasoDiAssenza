// GraphAuthProvider.cs
using Azure.Identity;
using Microsoft.Graph;
using System.Threading.Tasks;
using System;
using Azure.Core;

namespace TicketingApp.Graph
{
    public static class GraphAuthProvider
    {
        private const string CLIENT_ID = "210dc5bc-cbb0-4cf5-bd62-7aea37c84608";
        private const string TENANT_ID = "419d9c9c-52c4-4cd1-8001-a350f5526c44";

        // Gli scope *delegated* che vogliamo
        private static readonly string[] SCOPES = { "User.Read", "Mail.Read.Shared" };

        public static GraphServiceClient GraphClient { get; private set; }
        public static string? CurrentUserEmail { get; private set; }

        public static async Task InitializeAsync()
        {
            TokenCredential credential;

            // Per l'esecuzione non interattiva (es. Task Scheduler) permettiamo
            // l'uso di una client secret tramite variabile d'ambiente
            var clientSecret = Environment.GetEnvironmentVariable("GRAPH_CLIENT_SECRET");
            if (!string.IsNullOrEmpty(clientSecret))
            {
                credential = new ClientSecretCredential(TENANT_ID, CLIENT_ID, clientSecret);
            }
            else
            {
                // Fallback interattivo
                credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
                {
                    TenantId = TENANT_ID,
                    ClientId = CLIENT_ID
                });
            }

            GraphClient = new GraphServiceClient(credential, SCOPES);

            // recupera l'utente per ottenere l'email e forzare il token
            var me = await GraphClient.Me.GetAsync();
            CurrentUserEmail = me.Mail ?? me.UserPrincipalName;
        }
    }
}
