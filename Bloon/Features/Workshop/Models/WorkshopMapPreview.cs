using System;
using System.Collections.Generic;
using System.Text;

namespace Bloon.Features.Workshop.Models
{
    public class WorkshopMapPreview
    {
            public string previewid { get; set; }
            public int sortorder { get; set; }
            public string url { get; set; }
            public int size { get; set; }
            public string filename { get; set; }
            public int preview_type { get; set; }
            public string youtubevideoid { get; set; }
            public string external_reference { get; set; }
    }
}
