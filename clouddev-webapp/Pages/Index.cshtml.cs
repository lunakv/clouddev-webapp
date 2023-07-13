using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace clouddev_webapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration Configuration;

        public List<BlobItem>? Blobs { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;

            Configuration = configuration;
        }

        public void OnGet()
        {
            Blobs = CreateClient().GetBlobs().ToList();
        }

        public FileStreamResult OnGetDownloadBlob(string name)
        {
            var client = CreateClient().GetBlobClient(name);
            var contentType = client.GetProperties().Value.ContentType;
            var stream = client.OpenRead();
            return new FileStreamResult(stream, contentType); 
        }

        private BlobContainerClient CreateClient()
        {
            //string connectionString = Configuration.GetConnectionString("BlobStorage");
            string containerName = Configuration.GetValue<string>("StorageContainerName");
            //return new BlobContainerClient(connectionString, containerName);

            string storageName = "lunakvstore";
            string containerEndpoint = $"https://{storageName}.blob.core.windows.net/{containerName}";
            return new BlobContainerClient(new Uri(containerEndpoint), new Azure.Identity.DefaultAzureCredential());
        }
    }
}