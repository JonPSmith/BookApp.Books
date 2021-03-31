// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using BookApp.Books.ServiceLayer.Common;
using BookApp.Books.ServiceLayer.Common.Dtos;

namespace BookApp.Books.ServiceLayer.GoodLinq
{
    public interface IBookFilterDropdownService
    {
        /// <summary>
        ///     This makes the various Value + text to go in the dropdown based on the FilterBy option
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        IEnumerable<DropdownTuple> GetFilterDropDownValues(BooksFilterBy filterBy);
    }
}