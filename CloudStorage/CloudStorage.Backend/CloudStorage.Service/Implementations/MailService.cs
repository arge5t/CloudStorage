using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using CloudStorage.Services.Interfaces;
using CloudStorage.Domain.Entities;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using Google.Apis.Util;

namespace CloudStorage.Services.Implementations
{
    internal class MailService : IMailService
    {
        private readonly MailClient _mailClient;

        public MailService(IOptions<MailClient> mailClient)
        {
            _mailClient = mailClient.Value;
        }

        public async Task SendEmailConfirm(string activeLink, string email)
        {
            var clientSecrets = new ClientSecrets()
            {
                ClientId = _mailClient.Id,
                ClientSecret = _mailClient.Secret
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },

                ClientSecrets = clientSecrets
            });

            var authCode = new AuthorizationCodeInstalledApp(codeFlow, new LocalServerCodeReceiver());

            var credential = await authCode.AuthorizeAsync(_mailClient.Email, CancellationToken.None);

            if (credential.Token.IsExpired(SystemClock.Default))
                await credential.RefreshTokenAsync(CancellationToken.None);

            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

            var emailMassage = new MimeMessage();

            emailMassage.From.Add(new MailboxAddress(_mailClient.Name, _mailClient.Email));
            emailMassage.To.Add(new MailboxAddress(_mailClient.Name, email));
            emailMassage.Subject = _mailClient.Subject;
            emailMassage.Body = new TextPart(TextFormat.Html)
            {
                Text = GetEmailMassage(activeLink)
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailClient.Host, _mailClient.Port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(oauth2);
                await client.SendAsync(emailMassage);
                await client.DisconnectAsync(true);
            }
        }

        private string GetEmailMassage(string activeLink)
        {
            return $"<div>" +
                        $"<h1>click on the link to confirm your email address</h1>" +
                        $"<a style=\"padding:5px 12px;color:#fff;background-color:blue;\" href=\"{_mailClient.Url + activeLink}\">" +
                            $"Click me" +
                        $"</a>" +
                   $"</div>";
        }
    }
}
