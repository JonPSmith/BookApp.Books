// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using BookApp.Books.Domain.SupportTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookApp.Books.Persistence
{
    public enum MyQueryFilterTypes { SoftDelete, UserId }       

    public static class SoftDeleteQueryExtensions              
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodName = nameof(GetSoftDeleteFilter);       
            var methodToCall = typeof(SoftDeleteQueryExtensions)  
                .GetMethod(methodName,                            
                    BindingFlags.NonPublic | BindingFlags.Static) 
                .MakeGenericMethod(entityData.ClrType);           
            var filter = methodToCall                             
                .Invoke(null, new object[] {});   
            entityData.SetQueryFilter((LambdaExpression)filter);   
            entityData.AddIndex(entityData.FindProperty(nameof(ISoftDelete.SoftDeleted)));
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()                                   
            where TEntity : class, ISoftDelete                        
        {                                                             
            Expression<Func<TEntity, bool>> filter =                  
                x => !x.SoftDeleted;                                  
            return filter;                                            
        }
    }
}