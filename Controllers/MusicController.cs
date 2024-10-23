using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace musify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly ILogger<MusicController> _logger;
        private readonly string folder = "music";

        public MusicController(ILogger<MusicController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{fileName}")]
        public IActionResult GetAudio(string fileName)
        {
            try
            {
                string filePath = Path.Combine(folder, fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Audio File Not Found");
                }

                var stream = System.IO.File.OpenRead(filePath);
                return File(stream, "audio/mpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Songs")]
        public IActionResult GetSongsName()
        {
             // Try to get the client IP address from the X-Real-IP header
            var clientIp = HttpContext.Request.Headers["X-Real-IP"];

            // If the X-Real-IP header is not present, fall back to the RemoteIpAddress property
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = HttpContext.Connection.RemoteIpAddress!.ToString();
            }

            Console.WriteLine("Client IP: "+clientIp);
            List<SongMetadata> files = new List<SongMetadata>();

            if (Directory.Exists(folder))
            {
                string[] fileNames = Directory.GetFiles(folder)
                    .Select(Path.GetFileName)
                    .ToArray()!;

                List<SongMetadata> metadataList = new List<SongMetadata>();

                foreach (string fileName in fileNames)
                {
                    string filePath = Path.Combine(folder, fileName);

                    try
                    {
                        using (TagLib.File file = TagLib.File.Create(filePath))
                        {
                            SongMetadata metadata = new SongMetadata
                            {
                                FileName = fileName,
                                Title = file.Tag.Title,
                                Artist = file.Tag.FirstPerformer,
                                Album = file.Tag.Album,
                                Genre = file.Tag.FirstGenre,
                                Year = file.Tag.Year,
                                TrackNumber = file.Tag.Track,
                                Duration = file.Properties.Duration,
                                HasCoverArt = file.Tag.Pictures.Length > 0
                            };

                            // Read and set lyrics (if available)
                            if (file.Tag.Lyrics != null)
                            {
                                metadata.Lyrics = string.Join(Environment.NewLine, file.Tag.Lyrics);
                            }

                            metadataList.Add(metadata);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading metadata for {fileName}: {ex.Message}");
                    }
                }
                files = metadataList;
            }
            else
            {
                Console.WriteLine("The specified folder does not exist.");
                 // Initialize files to an empty array
            }

            return Ok(files);
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }


}