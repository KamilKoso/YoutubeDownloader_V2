namespace YoutubeDownloader.IdentityServer.Models
{
    public class ResendVerificationEmailModel
    {
        public string Email { get; set; }
        public bool ResendRequested { get; set; } = false;
	}
}
