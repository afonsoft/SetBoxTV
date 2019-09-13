﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SetBoxWebUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository
{
    /// <summary>
    /// Base para um DbSet
    /// </summary>
    /// <typeparam name="TEntity">TEntity</typeparam>
    /// <typeparam name="TKey">TKey</typeparam>
    public class Repository<TEntity, TKey> : IDisposable, IRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// DbContext
        /// </summary>
        protected ApplicationDbContext Context { get; }

        /// <summary>
        /// DbSet of the Table
        /// </summary>
        public DbSet<TEntity> Table { get; private set; }

        /// <summary>
        /// Primary Key Name
        /// </summary>
        public string PrimaryKeyName { get; }
        
        /// <summary>
        /// IEntityType
        /// </summary>
        public IEntityType EntityType { get; private set; }

        /// <summary>
        /// IEnumerable<IProperty>
        /// </summary>
        public IEnumerable<IProperty> Properties { get; private set; }

        /// <summary>
        /// IModel
        /// </summary>
        public IModel Model { get; private set; }

        /// <summary>
        /// Construtor com o RepositoryDbContext
        /// </summary>
        /// <param name="context"></param>
        public Repository(ApplicationDbContext context)
        {
            Context = context;
            Table = Context.Set<TEntity>();

            Model = Context.Model;
            EntityType = Model.FindEntityType(typeof(TEntity));
            Properties = EntityType.GetProperties();
            PrimaryKeyName = EntityType.FindPrimaryKey().Properties.First().Name;
        }

        private Repository()
        {

        }

        /// <summary>
        /// Add Element
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void Add(TEntity entity)
        {
            await Table.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Add Many Element
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void AddRange(IEnumerable<TEntity> entity)
        {
            await Table.AddRangeAsync(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Get All Element
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get() => Table.AsEnumerable();

        /// <summary>
        /// Get Element by primaryKey
        /// </summary>
        /// <param name="id">key</param>
        /// <returns></returns>
        public virtual TEntity GetById(TKey id) => Table.FirstOrDefault(e => id.Equals((TKey)e.GetType().GetProperty(PrimaryKeyName).GetValue(e)));


        /// <summary>
        /// Método que deleta um objeto no banco de dados. 
        /// </summary>
        /// <param name="entity">item que será deletado</param>
        public virtual async void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Table.Attach(entity);
            }

            Table.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Método que deleta um objeto no banco de dados. 
        /// </summary>
        /// <param name="id">Id da primary key</param>
        public virtual async void DeleteById(TKey id)
        {

            var entity = GetById(id);
            if (entity == null)
                throw new KeyNotFoundException($"Id: {id} not found");
            Table.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary> 
        /// Método que deleta um ou varios objetos no banco de dados, mediante uma expressão LINQ. 
        /// </summary> 
        public virtual async void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            Table.RemoveRange(Table.Where(filter));
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete Elements
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void DeleteRange(IEnumerable<TEntity> entity)
        {
            Table.RemoveRange(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Element
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void Update(TEntity entity)
        {
            //pegar o valor da pk do objeto
            var entry = Context.Entry(entity);
            var pkey = entity.GetType().GetProperty(PrimaryKeyName)?.GetValue(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = Context.Set<TEntity>();
                TEntity attachedEntity = set.Find(pkey);
                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                entry.State = EntityState.Modified;
            }

            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Element By primaryKey
        /// </summary>
        /// <param name="entity">Element</param>
        /// <param name="id">primaryKey</param>
        public virtual async void UpdateById(TEntity entity, TKey id)
        {

            TEntity attachedEntity = GetById(id); // access the key 
            if (attachedEntity != null)
            {
                var attachedEntry = Context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
                attachedEntry.State = EntityState.Modified;
                await Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Find Element
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="orderBy">OrderBy</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
             Expression<Func<TEntity, object>> orderBy = null) => orderBy != null ? Table.OrderBy(orderBy).Where(filter) : Table.Where(filter);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Table = null;
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Add
        /// </summary>
        public virtual Task<int> AddAsync(TEntity entity)
        {
            Table.AddAsync(entity).Wait();
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            Table.AddRangeAsync(entity).Wait();
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// get
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAsync() => Table.ToListAsync();

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByIdAsync(TKey id) => Table.FirstOrDefaultAsync(e => id.Equals((TKey)e.GetType().GetProperty(PrimaryKeyName).GetValue(e)));

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Table.Attach(entity);
            }

            Table.Remove(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteByIdAsync(TKey id)
        {

            var entity = GetById(id);
            if (entity == null)
                throw new KeyNotFoundException($"Id: {id} not found");
            Table.Remove(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter)
        {
            Table.RemoveRange(Table.Where(filter));
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteRangeAsync(IEnumerable<TEntity> entity)
        {
            Table.RemoveRange(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Udapte
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(TEntity entity)
        {
            var entry = Context.Entry(entity);
            var pkey = entity.GetType().GetProperty(PrimaryKeyName)?.GetValue(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = Context.Set<TEntity>();
                TEntity attachedEntity = set.Find(pkey); // access the key 
                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // attach the entity 
                }
            }
            else
            {
                entry.State = EntityState.Modified;
            }

            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateByIdAsync(TEntity entity, TKey id)
        {

            TEntity attachedEntity = GetById(id) ?? entity;

            if (entity == null)
                throw new KeyNotFoundException($"Id: {id} not found");

            var attachedEntry = Context.Entry(attachedEntity);
            attachedEntry.CurrentValues.SetValues(entity);
            attachedEntry.State = EntityState.Modified;

            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
             Expression<Func<TEntity, object>> orderBy = null) => orderBy != null
            ? Table.OrderBy(orderBy).Where(filter).ToListAsync()
            : Table.Where(filter).ToListAsync();

        /// <summary>
        /// GetPagination
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<KeyValuePair<int, List<TEntity>>> GetPagination(Expression<Func<TEntity, bool>> filter,
            int page = 1,
            int count = 10)
        {
            KeyValuePair<int, List<TEntity>> keys = new KeyValuePair<int, List<TEntity>>(Table.Count(),await Table
                                                                                                                    .Where(filter)
                                                                                                                    .Skip((page - 1) * count)
                                                                                                                    .Take(count)
                                                                                                                    .ToListAsync());
         
            return keys;
        }

        /// <summary>
        /// GetPagination
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDescending"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<KeyValuePair<int, List<TEntity>>> GetPagination(Expression<Func<TEntity, bool>> filter,
             Expression<Func<TEntity, object>> orderBy ,
            Expression<Func<TEntity, object>> orderByDescending,
            int page = 1,
            int count = 10)
        {
            List<TEntity> itens;
            if (orderBy != null)
            {
                itens = await Table
                          .Where(filter)
                          .OrderBy(orderBy)
                          .Skip((page - 1) * count)
                          .Take(count)
                          .ToListAsync();
            }
            else if (orderByDescending != null)
            {
                itens = await Table
                          .Where(filter)
                          .OrderByDescending(orderByDescending)
                          .Skip((page - 1) * count)
                          .Take(count)
                          .ToListAsync();
            }
            else
            {
                itens = await Table
                         .Where(filter)
                         .Skip((page - 1) * count)
                         .Take(count)
                         .ToListAsync();
            }

            KeyValuePair<int, List<TEntity>> keys = new KeyValuePair<int, List<TEntity>>(Table.Count(), itens);
            return keys;
        }


    }
}