using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace musify.Controllers
{
        public class SongMetadata
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public uint Year { get; set; }
        public uint TrackNumber { get; set; }
        public TimeSpan Duration { get; set; }
        public string Lyrics { get; set; }
        public bool HasCoverArt { get; set; }
    }
}