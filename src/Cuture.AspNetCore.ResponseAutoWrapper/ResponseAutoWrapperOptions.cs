﻿using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

#if NET5_0_OR_GREATER

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

#endif

namespace Cuture.AspNetCore.ResponseAutoWrapper
{
    /// <summary>
    ///
    /// </summary>
    public class ResponseAutoWrapperOptions
    {
        #region Private 字段

        /// <inheritdoc cref="ActionNoWrapPredicate"/>
        private Func<MemberInfo, bool>? _actionNoWrapPredicate;

        #endregion Private 字段

        /// <summary>
        /// 包装器类型列表
        /// </summary>
        internal List<Type> Wrappers { get; } = new();

        #region Public 属性

        /// <summary>
        /// Action 是否需要包装的筛选委托<para/>
        /// 委托返回 true 时，表明此方法应该被跳过，不进行包装<para/>
        /// </summary>
        public Func<MemberInfo, bool> ActionNoWrapPredicate { get => _actionNoWrapPredicate ?? DefaultActionNoWrapCheck; set => _actionNoWrapPredicate = value; }

        /// <summary>
        /// 禁用 OpenAPI 支持<para/>
        /// 未禁用时，将限制 响应类型 必须为泛型，且具有唯一的泛型参数 <see cref="object"/> 。即 - TResponse&lt;<see cref="object"/>&gt;
        /// </summary>
        /// <value>default value is 'false'</value>
        public bool DisableOpenAPISupport { get; set; } = false;

#if NET5_0_OR_GREATER

        /// <summary>
        /// 处理授权失败响应<para/>
        /// 注册 <see cref="AutoWrapperAuthorizationMiddlewareResultHandler"/> 为 <see cref="IAuthorizationMiddlewareResultHandler"/><para/>
        /// 将授权结果行为替换为响应状态码 <see cref="StatusCodes.Status401Unauthorized"/> 与 <see cref="StatusCodes.Status403Forbidden"/><para/>
        /// </summary>
        /// <value>default value is 'false'</value>
        public bool HandleAuthorizationResult { get; set; } = false;

#endif

        /// <summary>
        /// 处理无效模型绑定状态<para/>
        /// 设置 <see cref="ApiBehaviorOptions.InvalidModelStateResponseFactory"/> 为使用 <see cref="IInvalidModelStateWrapper{TResponse}"/> 处理的委托<para/>
        /// 可以通过注入自定义的 <see cref="IInvalidModelStateWrapper{TResponse}"/> 替换默认行为<para/>
        /// </summary>
        /// <value>default value is 'true'</value>
        public bool HandleInvalidModelState { get; set; } = true;

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 默认的 Action 是否需要包装的筛选委托<para/>
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static bool DefaultActionNoWrapCheck(MemberInfo memberInfo)
            => Attribute.GetCustomAttribute(memberInfo, typeof(NoResponseWrapAttribute)) is not null;

        #endregion Public 方法
    }
}