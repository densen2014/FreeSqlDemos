// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BootstrapBlazorApp.Server.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TablesBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected ToastService? ToastService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected static readonly Random random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static List<BindItem> GenerateItems() => Enumerable.Range(1, 80).Select(i => new BindItem()
        {
            Id = i,
            Name = $"张三 {i:d4}",
            DateTime = DateTime.Now.AddDays(i - 1),
            Address = $"上海市普陀区金沙江路 {random.Next(1000, 2000)} 弄",
            Count = random.Next(1, 100),
            Complete = random.Next(1, 100) > 50,
            Education = random.Next(1, 100) > 50 ? EnumEducation.Primary : EnumEducation.Middel
        }).ToList();

        /// <summary>
        /// 
        /// </summary>
        protected static List<BindItem> Items { get; } = GenerateItems();
         
    }

    /// <summary>
    /// 
    /// </summary>
    //[TableName("Test")]
    //[PrimaryKey("Id", AutoIncrement = true)]
    [FreeSql.DataAnnotations.Table(Name = "Test")]
    [Table("Test")]
    public class BindItem
    {
        // 列头信息支持 ColumnName Display DisplayName 三种标签

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "主键")]
        [AutoGenerateColumn(Ignore = true, Searchable = false, Editable = false)]
        [Key]
        [FreeSql.DataAnnotations.Column(IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [AutoGenerateColumn(Order = 10, Filterable = true,Sortable = true)]
        [ColumnName(Name = "姓名",  ResourceType = typeof(BindItem))]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [AutoGenerateColumn(Order = 1, FormatString = "yyyy-MM-dd", Width = 180, Sortable = true)]
        [ColumnName(Name = "日期",  ResourceType = typeof(BindItem))]
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName(Name = "地址",   ResourceType = typeof(BindItem))]
        [Required(ErrorMessage = "地址不能为空")]
        [AutoGenerateColumn(Order = 20, Filterable = true, Sortable = true)]
        public string? Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName(Name = "数量",   ResourceType = typeof(BindItem))]
        [AutoGenerateColumn(Order = 40, Sortable = true)]
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName(Name = "是/否",   ResourceType = typeof(BindItem))]
        [AutoGenerateColumn(Order = 50, Sortable = true)]
        public bool Complete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "请选择学历")]
        [ColumnName(Name = "学历",   ResourceType = typeof(BindItem))]
        [AutoGenerateColumn(Order = 60, Sortable = true)]
        //[EnumConverter(typeof(EnumEducation))]
        public EnumEducation? Education { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumEducation
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("小学")]
        Primary,

        /// <summary>
        /// 
        /// </summary>
        [Description("中学")]
        Middel,
        /// <summary>
        /// 
        /// </summary>
        [Description("大学")]
        Hight
    }

    ///// <summary>
    ///// BindItemContext 上下文操作类
    ///// </summary>
    //public class BindItemDbContext : Microsoft.EntityFrameworkCore.DbContext
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    /// <param name="options"></param>
    //    public BindItemDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<BindItemDbContext> options) : base(options)
    //    {

    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Microsoft.EntityFrameworkCore.DbSet<BindItem>? BindItems { get; set; }
    //}
}
