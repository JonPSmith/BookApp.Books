// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using ModMon.Books.ServiceLayer.GoodLinq.Dtos;

namespace ModMon.Books.AppSetup
{
    //This class contains any information that is required by the ModMon.UI startup code needs
    public static class BooksStartupInfo
    {
        //This contain the assemblies that have DTOs with ILinkToEntity<T>. Used to configure GenericServices 
        public static readonly ReadOnlyCollection<Assembly> GenericServiceAssemblies = new List<Assembly>
        {
            Assembly.GetAssembly(typeof(AddReviewDto))
        }.AsReadOnly();
    }
}