﻿// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace Microsoft.Azure.Mobile.Server.Tables
{
    /// <summary>
    /// Provides an abstraction for accessing a backend store for a <see cref="TableController{T}"/>.
    /// The abstraction can be implemented in one of two ways depending on the capabilities of the backend
    /// store. Stores that support a <see cref="IQueryable{T}"/>-based model can implement the <see cref="M:Query"/>
    /// and <see cref="M:Lookup"/> methods whereas stores that don't support <see cref="IQueryable"/> directly
    /// or where it is not the preferred way of accessing them can implement the <see cref="M:QueryAsync"/> and
    /// <see cref="M:LookupAsync"/> methods.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IDomainManager<TData>
        where TData : class, ITableData
    {
        /// <summary>
        /// Builds an <see cref="IQueryable{T}"/> to be executed against a store supporting <see cref="IQueryable{T}"/> for querying data. 
        /// </summary>
        /// <remarks>
        /// See also <see cref="M:Lookup"/> which is the companion method for creating an <see cref="IQueryable{T}"/> representing a single item.
        /// </remarks>
        /// <returns>An <see cref="IQueryable{T}"/> which has not yet been executed.</returns>
        IQueryable<TData> Query();

        /// <summary>
        /// Builds an <see cref="IQueryable{T}"/> to be executed against a store supporting <see cref="IQueryable{T}"/> for looking up a single item.
        /// </summary>
        /// <param name="id">The id representing the item. The id is provided as part of the <see cref="ITableData"/> and is visible to the client. 
        /// However, depending on the backend store and the domain model, the particular implementation may map the id to some other form of unique 
        /// identifier.</param>
        /// <remarks>
        /// See also <see cref="M:Query"/> which is the companion method for creating an <see cref="IQueryable{T}"/> representing multiple items.
        /// </remarks>
        /// <returns>A <see cref="SingleResult{T}"/> containing the <see cref="IQueryable{T}"/> which has not yet been executed.</returns>
        SingleResult<TData> Lookup(string id);

        /// <summary>
        /// Executes the provided <paramref name="query"/> against a store.
        /// </summary>
        /// <remarks>
        /// See also <see cref="M:LookupAsync"/> which is the companion method for executing a lookup for a single item.
        /// </remarks>
        /// <param name="query">The <see cref="ODataQueryOptions"/> query to execute.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> representing the result of the query.</returns>
        Task<IEnumerable<TData>> QueryAsync(ODataQueryOptions query);

        /// <summary>
        /// Looks up a single item in the backend store. 
        /// </summary>
        /// <param name="id">The id representing the item. The id is provided as part of the <see cref="ITableData"/> and is visible to the client. 
        /// However, depending on the backend store and the domain model, the particular implementation may map the id to some other form of unique 
        /// identifier.</param>
        /// <remarks>
        /// See also <see cref="M:QueryAsync"/> which is the companion method for executing a query for multiple items.
        /// </remarks>
        /// <returns>A <see cref="SingleResult{T}"/> representing the result of the lookup. A <see cref="SingleResult{T}"/> represents an 
        /// <see cref="IQueryable"/> containing zero or one entities. This allows it to be composed with further querying such as <c>$select</c>.</returns>
        Task<SingleResult<TData>> LookupAsync(string id);

        /// <summary>
        /// Inserts an item to the backend store.
        /// </summary>
        /// <param name="data">The data to be inserted</param>
        /// <returns>The inserted item.</returns>
        Task<TData> InsertAsync(TData data);

        /// <summary>
        /// Updates an existing item by applying a <see cref="Delta{T}"/> patch to it. The <see cref="Delta{T}"/>
        /// abstraction keeps track of which properties have changed which avoids problems with default values and
        /// the like.
        /// </summary>
        /// <param name="id">The id of the item to patch.</param>
        /// <param name="patch">The patch to apply.</param>
        /// <returns>The patched item.</returns>
        Task<TData> UpdateAsync(string id, Delta<TData> patch);

        /// <summary>
        /// Completely replaces an existing item.
        /// </summary>
        /// <param name="id">The id of the item to replace.</param>
        /// <param name="data">The replacement</param>       
        /// <returns>The replaced item</returns>
        Task<TData> ReplaceAsync(string id, TData data);

        /// <summary>
        /// Deletes an existing item
        /// </summary>
        /// <param name="id">The id of the item to delete.</param>
        /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
        Task<bool> DeleteAsync(string id);
    }
}
