using Microsoft.AspNetCore.Mvc;
using Augur.Web.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Augur.Web.Controllers
{
    [Route("api/[controller]")]
    public abstract class AugurApiController : Controller
    {
        protected AugurApiController()
        {
        }

        protected virtual InvalidValidationResult ValidationError()
        {
            return new InvalidValidationResult();
        }

        protected virtual InvalidValidationResult ValidationError<TData>(TData data) where TData : class
        {
            return new InvalidValidationResult(data);
        }

        protected void AddUpdateRemoveEfCollection<TCollectionDbEntity, TCollectionModelEntity>(
            ICollection<TCollectionDbEntity> dbCollection, ICollection<TCollectionModelEntity> modelCollection,
            Func<TCollectionDbEntity, TCollectionModelEntity, bool> comparer,
            Func<TCollectionModelEntity, TCollectionDbEntity> createAction,
            Action<TCollectionDbEntity, TCollectionModelEntity> updateAction,
            out ICollection<TCollectionDbEntity> deletedEntities, Action<TCollectionDbEntity> onRemove = null)
        {
            var itemsToDelete = dbCollection.ToList();
            deletedEntities = new List<TCollectionDbEntity>();

            foreach (var modelItem in modelCollection)
            {
                var item = dbCollection.SingleOrDefault(dbItem => comparer(dbItem, modelItem));
                if (item != null)
                {
                    updateAction(item, modelItem);

                    itemsToDelete.Remove(item);
                }
                else
                {
                    dbCollection.Add(createAction(modelItem));
                }
            }

            foreach (var item in itemsToDelete)
            {
                dbCollection.Remove(item);

                onRemove?.Invoke(item);

                deletedEntities.Add(item);
            }
        }

        protected void AddUpdateRemoveEfCollection<TCollectionDbEntity, TCollectionModelEntity>(
            ICollection<TCollectionDbEntity> dbCollection, ICollection<TCollectionModelEntity> modelCollection,
            Func<TCollectionDbEntity, TCollectionModelEntity, bool> comparer,
            Func<TCollectionModelEntity, TCollectionDbEntity> createAction,
            Action<TCollectionDbEntity, TCollectionModelEntity> updateAction,
            Action<TCollectionDbEntity> onRemove = null)
        {
            AddUpdateRemoveEfCollection(dbCollection, modelCollection, comparer, createAction, updateAction, out _,
                onRemove);
        }
    }
}