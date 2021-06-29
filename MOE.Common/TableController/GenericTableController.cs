using MOE.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.TableController
{
    public interface IGenericTableController<T>
    {
        ObservableCollection<T> Collection { get; }

        void Create(T item);
        IEnumerable<T> Read();
        void Update(T item);
        void Delete(T item);

        IQueryable<T> Query { get; }
    }

    /// <summary>
    /// Example of IGenericTableController on Entity Framework
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SQLTableController<T> : IGenericTableController<T> where T : Signal
    {
        private DbSet<T> _dataSet;

        public SQLTableController(DbSet<T> dataSet)
        {
            _dataSet = dataSet;
        }

        #region IGenericTableController
        public ObservableCollection<T> Collection => _dataSet.Local;

        public IQueryable<T> Query => _dataSet.AsQueryable();

        public void Create(T item)
        {
            _dataSet.Add(item);
        }

        public void Delete(T item)
        {
            _dataSet.Remove(item);
        }

        public IEnumerable<T> Read()
        {
            return _dataSet.ToList();
        }

        public void Update(T item)
        {
            //not really an update in EF
        }
        #endregion

    }

    /// <summary>
    /// Example of IGenericTableController on a web service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebTableController<T> : IGenericTableController<T> where T : Signal
    {
        private Uri _webServiceBase;

        private void HttpPost()
        {

        }

        private void HttpGet()
        {

        }
        
        public WebTableController(Uri webServiceBase)
        {
            _webServiceBase = webServiceBase;
        }

        #region IGenericTableController
        public ObservableCollection<T> Collection => null;

        public IQueryable<T> Query
        {
            get
            {
                //returns the IQueryable provider for the web service using OData queries
                return null;
            }
        }


        public void Create(T item)
        {
            HttpPost();
        }

        public void Delete(T item)
        {
            HttpPost();
        }

        public IEnumerable<T> Read()
        {
            return null;
        }

        public void Update(T item)
        {
            HttpPost();
        }
        #endregion

    }
}
