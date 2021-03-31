// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Reflection;
using BookApp.Books.Infrastructure.CachedValues;
using BookApp.Books.Infrastructure.Seeding;
using BookApp.Books.ServiceLayer.Cached;
using BookApp.Books.ServiceLayer.Common.Dtos;
using BookApp.Books.ServiceLayer.GoodLinq;
using BookApp.Books.ServiceLayer.Udfs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace BookApp.Books.AppSetup
{
    public static class BooksStartup
    {
        public static string RegisterBooksServices(this IServiceCollection services, IConfiguration configuration)
        {
            var diLogs = services.RegisterAssemblyPublicNonGenericClasses(
                    Assembly.GetAssembly(typeof(ICheckFixCacheValuesService)),
                    Assembly.GetAssembly(typeof(BookListDto)),
                    Assembly.GetAssembly(typeof(IBookGenerator)),
                    Assembly.GetAssembly(typeof(IListBooksCachedService)),
                    Assembly.GetAssembly(typeof(IListBooksService)),
                    Assembly.GetAssembly(typeof(IListUdfsBooksService)))
                .AsPublicImplementedInterfaces();

            //put any non-standard DI registration, e.g. generic types, here

            //FOR TEST OF UPDATED EXISTING NuGet
            return "second string";
        }
    }
}