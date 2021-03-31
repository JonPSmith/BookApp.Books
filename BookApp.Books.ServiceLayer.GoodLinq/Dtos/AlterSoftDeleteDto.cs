// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices;
using ModMon.Books.Domain;

namespace ModMon.Books.ServiceLayer.GoodLinq.Dtos
{
    public class AlterSoftDeleteDto : ILinkToEntity<Book>
    {
        public int BookId { get; set; }
        public bool SoftDeleted { get; set; }
    }
}