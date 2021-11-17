﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using FreeSql.Internal.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Console = System.Console;

namespace Densen.DataAcces.FreeSql
{
    /// <summary>
    /// FreeSql ORM 的 IDataService 接口实现
    /// </summary>
    internal class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
    {
        private readonly IFreeSql _db;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDataService(IFreeSql db)
        {
            _db = db;
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
        {
            // 通过模型获取主键列数据
            // 支持批量删除
            await _db.Delete<TModel>(models).ExecuteAffrowsAsync();
            TotalCount = null;
            return true;
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
        {
            await _db.GetRepository<TModel>().InsertOrUpdateAsync(model);
            TotalCount = null;
            return true;
        }

        /// <summary>
        /// 缓存记录总数
        /// </summary>
        long? TotalCount { get; set; }

        /// <summary>
        /// 缓存记录
        /// </summary>
        List<TModel> Items { get; set; }

        /// <summary>
        /// 缓存查询条件
        /// </summary>
        QueryPageOptions Options { get; set; }
        void initTestDatas()
        {
            try
            {
                if (_db.Select<TModel>().Count() < 200)
                {
                    var sql = "";
                    for (int i = 0; i < 200; i++)
                    {
                        sql += @$"INSERT INTO ""Test""(""Name"", ""DateTime"", ""Address"", ""Count"", ""Complete"", ""Education"") VALUES('周星星{i}', '2021-02-01 00:00:00', '星光大道 , {i}A', {i}, 0, 1);";
                    }
                    _db.Ado.ExecuteScalar(sql);
                }
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
        {
            FetchAsync(option);

            var ret = new QueryData<TModel>()
            {
                TotalCount = (int)(TotalCount ?? 0),
                Items = Items
            };
            Options = option;
            return Task.FromResult(ret);
        }

        private void FetchAsync(QueryPageOptions options)
        {
#if DEBUG
            initTestDatas();
#endif 

            //.WhereDynamicFilter(dyfilter)

            //两种版本 var lambda = MakeWhereLambda(option, out var isSerach);
            var dynamicFilterInfo = MakeDynamicFilterInfo(options, out var isSerach);

            if (TotalCount != null && !isSerach && options.PageItems != Options.PageItems && TotalCount <= Options.PageItems)
            {
                //当选择的每页显示数量大于总数时，强制认为是一页

                //无搜索,并且总数<=分页总数直接使用内存排序和搜索
                Console.WriteLine($"无搜索,分页数相等{ options.PageItems}/{ Options.PageItems},直接使用内存排序和搜索");
            }
            else
            {
                var fsql_select = _db.Select<TModel>();

                if (isSerach)
                    fsql_select = fsql_select.WhereDynamicFilter(dynamicFilterInfo);

                fsql_select = fsql_select.OrderByPropertyNameIf(options.SortOrder != SortOrder.Unset, options.SortName, options.SortOrder == SortOrder.Asc);

                //分页==1才获取记录总数量,省点性能
                long count = 0;
                if (options.PageIndex == 1) fsql_select = fsql_select.Count(out count);

                //判断是否分页
                if (options.IsPage) fsql_select = fsql_select.Page(options.PageIndex, options.PageItems);
                
                Items = fsql_select.ToList();
                
                TotalCount = options.PageIndex == 1 ? count : TotalCount;

            }
        }

#region 生成Where子句的Lambda表达式

        /// <summary>
        /// 生成Where子句的Lambda表达式
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private Expression<Func<TModel, bool>>? MakeWhereLambda(QueryPageOptions option, out bool isSerach)
        {
            Expression<Func<TModel, bool>> expression = null;
            object? searchModel = option.SearchModel;
            Type type = searchModel.GetType();

            var instance = Activator.CreateInstance(type);

            if (string.IsNullOrEmpty(option.SearchText))
            {
                //生成高级搜索子句
                PropertyInfo[] propertys = type.GetProperties();
                foreach (var propertyinfo in propertys)
                {

                    //Console.WriteLine($"Name=> {propertyinfo.Name}  Default=> {propertyinfo.GetValue(instance)}  Value=> { propertyinfo.GetValue(searchModel)}");
                    if (propertyinfo.GetValue(searchModel) != null && !propertyinfo.GetValue(searchModel).Equals(propertyinfo.GetValue(instance)))
                    {
                        string propertyValue = propertyinfo.GetValue(searchModel).ToString();
                        //TODO : 支持更多类型
                        LambdaExpression lambda = LambadaExpression.GetContains<TModel>(propertyinfo.Name, propertyValue);
                        expression = expression.Or((Expression<Func<TModel, bool>>)lambda);
                    }
                }

            }
            else
            {
                //生成默认搜索子句
                PropertyInfo[] propertys = type.GetProperties();
                foreach (var propertyinfo in propertys)
                {
                    //TODO : 支持更多类型
                    LambdaExpression? lambda = null;
                    //Console.WriteLine($"Name=> {propertyinfo.Name}");
                    if (propertyinfo.PropertyType == typeof(string))
                    {
                        lambda = LambadaExpression.GetContains<TModel>(propertyinfo.Name, option.SearchText);
                    }
                    //else if (propertyinfo.PropertyType == typeof(bool))
                    //{
                    //    //try
                    //    //{
                    //    //    lambda = LambadaExpression.CreateEqual<TModel>(propertyinfo.Name, Convert.ToBoolean(option.SearchText));
                    //    //}
                    //    //catch { 
                    //    //}
                    //}
                    //else if (propertyinfo.PropertyType == typeof(int)  )
                    //{
                    //    lambda = LambadaExpression.CreateEqual<TModel>(propertyinfo.Name,Convert .ToInt32 ( option.SearchText));
                    //}
                    //else if ( propertyinfo.PropertyType == typeof(long))
                    //{
                    //    lambda = LambadaExpression.CreateEqual<TModel>(propertyinfo.Name,Convert .ToInt64 ( option.SearchText));
                    //}
                    if (lambda != null) expression = expression.Or((Expression<Func<TModel, bool>>)lambda);
                }

            }
            isSerach = expression != null;
            return expression;
        }
#endregion

        /// <summary>
        /// 生成Where子句的DynamicFilterInfo对象
        /// </summary>
        /// <param name="option"></param>
        /// <param name="isSerach"></param>
        /// <returns></returns>
        private DynamicFilterInfo? MakeDynamicFilterInfo(QueryPageOptions option, out bool isSerach)
        {
            var filters = new List<DynamicFilterInfo>();

            object? searchModel = option.SearchModel;
            Type type = searchModel.GetType();

            var instance = Activator.CreateInstance(type);

            if (string.IsNullOrEmpty(option.SearchText))
            {
                //生成高级搜索子句
                //TODO : 支持更多类型
                foreach (var propertyinfo in type.GetProperties().Where(a => a.PropertyType == typeof(string) || a.PropertyType == typeof(int)).ToList())
                {
                    if (propertyinfo.GetValue(searchModel) != null && !propertyinfo.GetValue(searchModel).Equals(propertyinfo.GetValue(instance)))
                    {
                        string propertyValue = propertyinfo.GetValue(searchModel).ToString();
                        if (propertyinfo.PropertyType == typeof(int) && !IsNumeric(propertyValue)) continue;

                        filters.Add(new DynamicFilterInfo()
                        {
                            Field = propertyinfo.Name,
                            Operator = propertyinfo.PropertyType == typeof(int) ? DynamicFilterOperator.Equal : DynamicFilterOperator.Contains,
                            Value = propertyinfo.PropertyType == typeof(int) ? Convert.ToInt32(propertyValue) : propertyValue,
                        });
                    }
                }

            }
            else
            {
                //生成默认搜索子句
                //TODO : 支持更多类型
                foreach (var propertyinfo in type.GetProperties().Where(a => a.PropertyType == typeof(string) || a.PropertyType == typeof(int)).ToList())
                {
                    if (propertyinfo.PropertyType == typeof(int) && !IsNumeric(option.SearchText)) continue;

                    filters.Add(new DynamicFilterInfo()
                    {
                        Field = propertyinfo.Name,
                        Operator = propertyinfo.PropertyType == typeof(int) ? DynamicFilterOperator.Equal : DynamicFilterOperator.Contains,
                        Value = propertyinfo.PropertyType == typeof(int) ? Convert.ToInt32(option.SearchText) : option.SearchText,
                    });
                }

            }

            if (option.Filters.Any())
            {
                foreach (var item in option.Filters)
                {
                    var filter = item.GetFilterConditions().First();
                    var filterOperator = DynamicFilterOperator.Contains;

                    switch (filter.FilterAction)
                    {
                        case FilterAction.Contains:
                            filterOperator = DynamicFilterOperator.Contains;
                            break;
                        case FilterAction.NotContains:
                            filterOperator = DynamicFilterOperator.NotContains;
                            break;
                        case FilterAction.NotEqual:
                            filterOperator = DynamicFilterOperator.NotEqual;
                            break;
                        case FilterAction.Equal:
                            filterOperator = DynamicFilterOperator.Equal;
                            break;
                    }

                    filters.Add(new DynamicFilterInfo()
                    {
                        Field = filter.FieldKey,
                        Operator = filterOperator,
                        Value = filter.FieldValue,
                    });
                }
            }

            if (filters.Any())
            {
                DynamicFilterInfo dyfilter = new DynamicFilterInfo()
                {
                    Logic = string.IsNullOrEmpty(option.SearchText) ? DynamicFilterLogic.And : DynamicFilterLogic.Or,
                    Filters = filters
                };
                isSerach = true;
                return dyfilter;

            }

            isSerach = false;
            return null;
        }
        private bool IsNumeric(string text) => double.TryParse(text, out _);

    }
}
