﻿// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BootstrapBlazor.Components;
using FreeSql.DataAnnotations;
using FreeSql.Internal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Console = System.Console;

namespace Densen.DataAcces.FreeSql
{
    /// <summary>
    /// FreeSql ORM 的 IDataService 数据注入服务接口实现
    /// </summary>
    public class FreeSqlDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
    {
        private readonly IFreeSql fsql;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FreeSqlDataService(IFreeSql fsql)
        {
            this.fsql = fsql;
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
            await fsql.Delete<TModel>(models).ExecuteAffrowsAsync();
            TotalCount = null;
            return true;
        }

        /// <summary>
        /// 级联保存字段名
        /// </summary>
        public string SaveManyChildsPropertyName { get; set; }

        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="changedType"></param>
        /// <returns></returns>
        public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
        {
            var repo = fsql.GetRepository<TModel>();
            await repo.InsertOrUpdateAsync(model);
            if (!string.IsNullOrEmpty(SaveManyChildsPropertyName))
            {
                try
                {
                    //联级保存
                    repo.SaveMany(model, SaveManyChildsPropertyName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"IdlebusDataService联级保存 error , {ex.Message }");
                }
            }

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

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="option"></param>
        /// <param name="WhereCascade">附加查询条件使用and结合</param>
        /// <param name="IncludeByPropertyNames">附加IncludeByPropertyName查询条件</param>
        /// <param name="LeftJoinString">左联查询，使用原生sql语法，LeftJoin("type b on b.id = a.id")</param>
        /// <param name="OrderByPropertyName">强制排序,但是手动排序优先</param>
        /// <returns></returns>
        public Task<QueryData<TModel>> QueryAsyncWithWhereCascade(
                    QueryPageOptions option,
                    DynamicFilterInfo WhereCascade = null,
                    List<string> IncludeByPropertyNames = null,
                    string LeftJoinString = null,
                    List<string> OrderByPropertyName = null)
        {
            var res = FsqlUtil.Fetch(option, Options, TotalCount, Items, fsql, WhereCascade, IncludeByPropertyNames, LeftJoinString, OrderByPropertyName);
            TotalCount = res.TotalCount;
            Items = res.Items.ToList();
            Options = option;
            return Task.FromResult(res);
        }


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
        {
            var res = FsqlUtil.Fetch(option, Options, TotalCount, Items, fsql);
            TotalCount = res.TotalCount;
            Items = res.Items.ToList();
            Options = option;
            return Task.FromResult(res);
        }

        void initTestDatas()
        {
            try
            {
                if (fsql.Select<TModel>().Count() < 200)
                {
                    var sql = "";
                    for (int i = 0; i < 200; i++)
                    {
                        sql += @$"INSERT INTO ""Test""(""Name"", ""DateTime"", ""Address"", ""Count"", ""Complete"", ""Education"") VALUES('周星星{i}', '2021-02-01 00:00:00', '星光大道 , {i}A', {i}, 0, 1);";
                    }
                    fsql.Ado.ExecuteScalar(sql);
                }
            }
            catch (Exception)
            {
            }

        }



    }

    public static class FsqlUtil
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="options">查询条件</param>
        /// <param name="optionsLast">缓存查询条件</param>
        /// <param name="TotalCount"></param>
        /// <param name="Items"></param>
        /// <param name="fsql"></param>
        /// <param name="WhereCascade">附加查询条件使用and结合</param>
        /// <param name="IncludeByPropertyNames">附加IncludeByPropertyName查询条件</param>
        /// <param name="LeftJoinString">左联查询，使用原生sql语法，LeftJoin("type b on b.id = a.id")</param>
        /// <param name="OrderByPropertyName">强制排序,但是手动排序优先</param>
        public static QueryData<TModel> Fetch<TModel>(QueryPageOptions options,
                                QueryPageOptions optionsLast,
                                long? TotalCount,
                                List<TModel> Items,
                                IFreeSql fsql,
                                DynamicFilterInfo WhereCascade = null,
                                List<string> IncludeByPropertyNames = null,
                                string LeftJoinString = null,
                                List<string> OrderByPropertyName = null) where TModel : class, new()
        {
            try
            {

                var dynamicFilterInfo = MakeDynamicFilterInfo(options, out var isSerach, WhereCascade);

                if (TotalCount != null && !isSerach && options.PageItems != optionsLast.PageItems && TotalCount <= optionsLast.PageItems)
                {
                    //当选择的每页显示数量大于总数时，强制认为是一页
                    //无搜索,并且总数<=分页总数直接使用内存排序和搜索
                    Console.WriteLine($"无搜索,分页数相等{ options.PageItems}/{ optionsLast.PageItems},直接使用内存排序和搜索");
                }
                else
                {
                    var fsql_select = fsql.Select<TModel>();

                    if (LeftJoinString != null)
                    {
                        fsql_select = fsql_select.LeftJoin(LeftJoinString);
                    }
                    if (IncludeByPropertyNames != null)
                    {
                        foreach (var item in IncludeByPropertyNames)
                        {
                            fsql_select = fsql_select.IncludeByPropertyName(item);
                        }
                    }
                    if (isSerach)
                        fsql_select = fsql_select.WhereDynamicFilter(dynamicFilterInfo);

                    fsql_select = fsql_select.OrderByPropertyNameIf(options.SortOrder != SortOrder.Unset, options.SortName, options.SortOrder == SortOrder.Asc);

                    if (OrderByPropertyName != null)
                    {
                        foreach (var item in OrderByPropertyName)
                        {
                            try
                            {
                                fsql_select = fsql_select.OrderByPropertyName(item);
                            }
                            catch
                            {
                                fsql_select = fsql_select.OrderBy(item);
                            }
                        }
                    }

                    //分页==1才获取记录总数量,省点性能
                    long count = 0;
                    if (options.PageIndex == 1) fsql_select = fsql_select.Count(out count);

                    //判断是否分页
                    if (options.IsPage) fsql_select = fsql_select.Page(options.PageIndex, options.PageItems);

                    Items = fsql_select.ToList();

                    TotalCount = options.PageIndex == 1 ? count : TotalCount;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("IdlebusDataService Error: " + e.Message);
                Items = new List<TModel>();
                TotalCount = 0;
            }
            var ret = new QueryData<TModel>()
            {
                TotalCount = (int)(TotalCount ?? 0),
                Items = Items
            };

            return ret;
        }

        #region "生成Where子句的DynamicFilterInfo对象"
        /// <summary>
        /// 生成Where子句的DynamicFilterInfo对象
        /// </summary>
        /// <param name="option"></param>
        /// <param name="isSerach"></param>
        /// <param name="WhereCascade">强制and的条件</param>
        /// <returns></returns>
        public static DynamicFilterInfo MakeDynamicFilterInfo(QueryPageOptions option,
                                                         out bool isSerach,
                                                         DynamicFilterInfo WhereCascade = null)
        {
            var filters = new List<DynamicFilterInfo>();
#nullable enable
            object? searchModel = option.SearchModel;
#nullable disable
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
                        var isInt = propertyinfo.PropertyType == typeof(int);
                        string propertyValue = propertyinfo.GetValue(searchModel).ToString();
                        if (isInt && !propertyValue.IsNumeric()) continue;
                        var attr = propertyinfo.GetCustomAttribute<ColumnAttribute>();
                        if (attr?.IsIgnore ?? false) continue;
                        object val;
                        try
                        {
                            val = isInt ? Convert.ToInt32(propertyValue) : propertyValue;
                        }
                        catch (Exception)
                        {
                            val = propertyValue;
                            isInt = false;
                        }

                        filters.Add(new DynamicFilterInfo()
                        {
                            Field = propertyinfo.Name,
                            Operator = isInt ? DynamicFilterOperator.Equal : DynamicFilterOperator.Contains,
                            Value = val,
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
                    var isInt = propertyinfo.PropertyType == typeof(int);
                    if (isInt && !option.SearchText.IsNumeric()) continue;
                    object val;
                    try
                    {
                        val = isInt ? Convert.ToInt32(option.SearchText) : option.SearchText;
                    }
                    catch (Exception)
                    {
                        val = option.SearchText;
                        isInt = false;
                    }
                    var attr = propertyinfo.GetCustomAttribute<ColumnAttribute>();
                    if (attr?.IsIgnore ?? false) continue;

                    filters.Add(new DynamicFilterInfo()
                    {
                        Field = propertyinfo.Name,
                        Operator = isInt ? DynamicFilterOperator.Equal : DynamicFilterOperator.Contains,
                        Value = val,
                    });
                }

            }

            if (option.Filters.Any())
            {
                foreach (var item in option.Filters)
                {
                    foreach (var filter in item.GetFilterConditions())
                    {
                        var filterOperator = DynamicFilterOperator.Contains;

                        filterOperator = filter.FilterAction switch
                        {
                            FilterAction.Equal => DynamicFilterOperator.Equal,
                            FilterAction.NotEqual => DynamicFilterOperator.NotEqual,
                            FilterAction.Contains => DynamicFilterOperator.Contains,
                            FilterAction.NotContains => DynamicFilterOperator.NotContains,
                            FilterAction.GreaterThan => DynamicFilterOperator.GreaterThan,
                            FilterAction.GreaterThanOrEqual => DynamicFilterOperator.GreaterThanOrEqual,
                            FilterAction.LessThan => DynamicFilterOperator.LessThan,
                            FilterAction.LessThanOrEqual => DynamicFilterOperator.LessThanOrEqual,
                            _ => throw new System.NotSupportedException()
                        };

                        filters.Add(new DynamicFilterInfo()
                        {
                            Field = filter.FieldKey,
                            Operator = filterOperator,
                            Value = filter.FieldValue,
                        });
                    }
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

                //生成带预设条件的复合查询
                if (WhereCascade != null)
                {
                    var filtersWhereCascade = new List<DynamicFilterInfo>();
                    filtersWhereCascade.Add(WhereCascade);
                    filtersWhereCascade.Add(dyfilter);
                    DynamicFilterInfo dyfilterWhereCascade = new DynamicFilterInfo()
                    {
                        Logic = DynamicFilterLogic.And,
                        Filters = filtersWhereCascade
                    };
                    return dyfilterWhereCascade;
                }

                return dyfilter;

            }
            else if (WhereCascade != null)
            {
                isSerach = true;
                return WhereCascade;
            }

            isSerach = false;
            return null;
        }



             public static bool IsNumeric(this string text) => double.TryParse(text, out _);

            /// <summary>
            /// String转Decimal
            /// </summary>
            /// <param name="t"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
            public static decimal ToDecimal(this string t, decimal defaultValue = 0m)
            {
                try
                {
                    var x = t.IsNumeric() ? Convert.ToDecimal(t) : defaultValue;
                    return x;
                }
                catch
                {
                }
                return defaultValue;
            }
            public static double ToDouble(this string t, double defaultValue = 0d)
            {
                try
                {
                    var x = t.IsNumeric() ? Convert.ToDouble(t) : defaultValue;
                    return x;
                }
                catch
                {
                }
                return defaultValue;
            } 
        
        #endregion
    }

}