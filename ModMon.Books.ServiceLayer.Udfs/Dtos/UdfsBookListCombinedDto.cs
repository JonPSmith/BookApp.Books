﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using ModMon.Books.ServiceLayer.Common;

namespace ModMon.Books.ServiceLayer.Udfs.Dtos
{
    public class UdfsBookListCombinedDto
    {
        public UdfsBookListCombinedDto(SortFilterPageOptions sortFilterPageData, IEnumerable<UdfsBookListDto> booksList)
        {
            SortFilterPageData = sortFilterPageData;
            BooksList = booksList;
        }

        public SortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<UdfsBookListDto> BooksList { get; private set; }
    }
}