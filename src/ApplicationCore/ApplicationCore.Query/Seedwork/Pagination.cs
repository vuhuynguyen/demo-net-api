using AutoMapper;
using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Query.Seedwork
{
    public class QueryOptions
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public bool IsTakeAll { get; set; }

        /// <summary>
        /// "Field1 asc, Field2, Field3 desc "
        /// </summary>
        //[AllowNull]
        public string Sort { get; set; }

        ///// <summary>
        ///// "Field1 eq 123;... Some supported operators: eq, lt, gt, le, ge, .Contains(), ..."
        ///// </summary>
        //[AllowNull]
        public string Filter { get; set; }
    }

    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int Size { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalElements / (double)(Size == 0 ? TotalElements : Size));

        public PageInfo(int pageNumber, int size, int totalElements)
        {
            PageNumber = pageNumber;
            Size = size;
            TotalElements = totalElements;
        }
    }

    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PageInfo PageInfo { get; }

        public PaginatedResult(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            PageInfo = new PageInfo(pageNumber, pageSize, count);
        }

        public static async Task<PaginatedResult<TSource>> CreateAsync<TSource>(IQueryable<TSource> source, QueryOptions option)
        {
            // Sorting
            //if (!string.IsNullOrEmpty(option.Sort))
            //{
            //    source = source.OrderBy(option.Sort);
            //}

            //// Filtering
            //if (!string.IsNullOrEmpty(option.Filter))
            //{
            //    source = source.Where(option.Filter);
            //}

            var count = await source.CountAsync();
            if (option.IsTakeAll)
            {
                var allItems = await source.DecompileAsync().ToListAsync();
                return new PaginatedResult<TSource>(allItems, count, option.PageNumber, count);
            }
            var items = await source.Skip((option.PageNumber - 1) * option.PageSize).Take(option.PageSize).DecompileAsync().ToListAsync();
            return new PaginatedResult<TSource>(items, count, option.PageNumber, option.PageSize);
        }
    }
 }
