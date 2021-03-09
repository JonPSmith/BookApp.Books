// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using GenericEventRunner.ForHandlers;
using ModMon.Books.Domain;
using ModMon.Books.Domain.DomainEvents;
using StatusGeneric;

namespace ModMon.Books.Infrastructure.CachedValues.EventHandlers
{
    public class ReviewRemovedHandler : IBeforeSaveEventHandler<BookReviewRemovedEvent>
    {
        public IStatusGeneric Handle(object callingEntity, BookReviewRemovedEvent domainEvent)
        {
            var book = (Book)callingEntity;

            var numReviews = book.ReviewsCount - 1;
            var totalStars = Math.Round(book.ReviewsAverageVotes * book.ReviewsCount)
                             - domainEvent.ReviewRemoved.NumStars;
            domainEvent.UpdateReviewCachedValues(numReviews, totalStars / numReviews);

            return null;
        }
    }
}