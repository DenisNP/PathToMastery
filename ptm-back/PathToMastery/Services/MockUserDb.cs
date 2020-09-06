using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PathToMastery.Models;
using PathToMastery.Services.Abstract;

namespace PathToMastery.Services
{
    public class MockUserDb : IDbService
    {
        private User _user = null;
        
        public void Init(string dbName, Func<Type, string> typeToCollection)
        {
            // nothing
        }

        public T ById<T>(string id, bool allowNull = true, string? collection = null) where T : IIdentity
        {
            return _user is T user ? user : default;
        }

        public IQueryable<T> Collection<T>(string? name = null)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T document, string? collection = null) where T : IIdentity
        {
            _user = document as User;
        }

        public void UpdateAsync<T>(T document, string? collection = null) where T : IIdentity
        {
            _user = document as User;
        }

        public void PushAsync<TDocument, TItem>(string docId, Expression<Func<TDocument, IEnumerable<TItem>>> expression, TItem value, string? collection = null) where TDocument : IIdentity
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync<T>(string id, string? collection = null) where T : IIdentity
        {
            throw new NotImplementedException();
        }
    }
}