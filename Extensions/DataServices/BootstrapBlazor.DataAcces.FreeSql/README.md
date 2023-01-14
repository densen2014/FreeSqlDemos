BootstrapBlazor的FreeSql数据注入服务扩展包

FreeSql ORM 的 IDataService 数据注入服务接口实现

 
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="option"></param>
        /// <param name="WhereCascade">附加查询条件使用and结合</param>
        /// <param name="IncludeByPropertyNames">附加IncludeByPropertyName查询条件</param>
        /// <param name="LeftJoinString">左联查询，使用原生sql语法，LeftJoin("type b on b.id = a.id")</param>
        /// <param name="OrderByPropertyName">强制排序,但是手动排序优先</param>
        /// <param name="WhereCascadeOr">附加查询条件使用or结合</param>
        /// <returns></returns>
        public Task<QueryData<TModel>> QueryAsyncWithWhereCascade(
                    QueryPageOptions option,
                    DynamicFilterInfo WhereCascade = null,
                    List<string> IncludeByPropertyNames = null,
                    string LeftJoinString = null,
                    List<string> OrderByPropertyName = null,
                    List<string> WhereCascadeOr = null)
