using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Network
{
    /// <summary>
    /// 视频信息
    /// </summary>
    [Serializable]
    public class N_VideoInfo
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string CoverUrl { get; set; }
        public required string PublishDate { get; set; }
        public required string VideoUrl { get; set; }
        public object? ImageObj { get; set; }
    }
}
