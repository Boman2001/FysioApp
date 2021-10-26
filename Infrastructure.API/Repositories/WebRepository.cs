using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace Infrastructure.API.Repositories
{
    public class WebRepository<T> : IWebRepository<T> where T : Entity
    {
        private readonly HttpClient client;

        public WebRepository()
        {
            this.client = new HttpClient() { BaseAddress = new Uri("https://localhost:5003/api/")};
        }

        public IEnumerable<T> Get()
        {
            IEnumerable<T> responseBody = null;
            try	
            {
                HttpResponseMessage response = this.client.GetAsync(typeof(T).ToString().Split(".")[3]).Result;
                response.EnsureSuccessStatusCode();
                responseBody = response.Content.ReadFromJsonAsync<IEnumerable<T>>().Result;
                return responseBody;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return responseBody;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            IEnumerable<T> responseBody = null;
            try	
            {
                HttpResponseMessage response = await this.client.GetAsync(typeof(T).ToString().Split(".")[3]);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
                return responseBody;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return responseBody;
        }

        public async Task<T> Get(int id)
        {
            T responseBody = null;
            try	
            {
                HttpResponseMessage response = await this.client.GetAsync(typeof(T).ToString().Split(".")[3]+"/"+id);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadFromJsonAsync<T>();
                return responseBody;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return responseBody;
        }

        public T Get(int id, IEnumerable<string> includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(IEnumerable<string> includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<T> Add(T model)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T model)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public void Detach(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Detach(T entity)
        {
            throw new NotImplementedException();
        }

        // private string Login()
        // {
        //     try	
        //     {
        //         HttpResponseMessage response = await this.client.PostAsync("auth/Login");
        //         response.EnsureSuccessStatusCode();
        //         var responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
        //         return responseBody;
        //     }
        //     catch(HttpRequestException e)
        //     {
        //         Console.WriteLine("\nException Caught!");	
        //         Console.WriteLine("Message :{0} ",e.Message);
        //     }
        // }
    }
}