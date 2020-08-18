using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PathToMastery.Models;

namespace PathToMastery.Services.Abstract
{
    public interface IDbService
    {
        void Init(string dbName, Func<Type, string> typeToCollection);

        T ById<T>(string id, bool allowNull = true, string? collection = null) where T : IIdentity;
        
        IQueryable<T> Collection<T>(string? name = null);
        
        void Update<T>(T document, string? collection = null) where T : IIdentity;
        
        void UpdateAsync<T>(T document, string? collection = null) where T : IIdentity;
        
        void PushAsync<TDocument, TItem>(
            string docId,
            Expression<Func<TDocument, IEnumerable<TItem>>> expression,
            TItem value,
            string? collection = null
        ) where TDocument : IIdentity;
        
        void DeleteAsync<T>(string id, string? collection = null) where T : IIdentity;
    }
}