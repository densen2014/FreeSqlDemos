using System;
using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace BootstrapBlazorApp1_freesql.Models
{
    /// <summary>
    /// 项目
    /// </summary>
    public class Item
    {
        [Column(IsIdentity =true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Text { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }
    }
}