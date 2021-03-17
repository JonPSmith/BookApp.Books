// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModMon.Books.Infrastructure.CachedValues;
using ModMon.Books.Infrastructure.Seeding;
using ModMon.Books.ServiceLayer.Cached;
using ModMon.Books.ServiceLayer.Common.Dtos;
using ModMon.Books.ServiceLayer.GoodLinq;
using ModMon.Books.ServiceLayer.Udfs;
using NetCore.AutoRegisterDi;

namespace ModMon.Books.AppSetup
{
    public static class BooksStartup
    {
        public static void RegisterBooksServices(this IServiceCollection services, IConfiguration configuration )
        {
            var diLogs = services.RegisterAssemblyPublicNonGenericClasses(
                    Assembly.GetAssembly(typeof(ICheckFixCacheValuesService)),
                    Assembly.GetAssembly(typeof(BookListDto)),
                    Assembly.GetAssembly(typeof(IBookGenerator)),
                    Assembly.GetAssembly(typeof(IListBooksCachedService)),
                    Assembly.GetAssembly(typeof(IListBooksService)),
                    Assembly.GetAssembly(typeof(IListUdfsBooksService)))
                .AsPublicImplementedInterfaces();

            var x = DateTime.Now;

            //put any non-standard DI registration, e.g. generic types, here
        }
    }
}