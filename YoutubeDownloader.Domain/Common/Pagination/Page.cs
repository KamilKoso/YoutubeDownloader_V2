﻿using System.Collections.Generic;

namespace YoutubeDownloader.Domain.Common.Pagination
{
    public class Page<T>
    {
        public Page()
        {
            Items = new List<T>();
            TotalCount = 0;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<T> Items { get; set; }
    }
}
