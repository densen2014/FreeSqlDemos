using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi
{
    public class Item
    {
        [Column(IsIdentity = true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Text { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }

        public int UserID { get; set; }

        [Column(IsIgnore = true)]
        [DisplayName("创建人")]
        public string UserName { get => Userss?.Name; }

        [JsonIgnore]
        [DisplayName("用户表")]
        [Navigate(nameof(UserID))]
        public virtual Users Userss { get; set; }

    }

    public class Users
    {
        [Column(IsIdentity = true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("描述")]
        public string Password { get; set; }
    }

}
