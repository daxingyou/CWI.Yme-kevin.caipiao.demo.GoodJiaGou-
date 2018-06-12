using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Util.Exceptions
{
    /// <summary>
    /// 代表一个权限认证异常。
    /// </summary>
    public class AuthenticationException : Exception
    {
        /// <summary>
        /// _isMessageException
        /// </summary>
        private bool _isMessageException = true;

        /// <summary>
        /// 是否消息性业务异常。
        /// </summary>
        public bool IsMessageException
        {
            get { return _isMessageException; }
        }

        /// <summary>
        /// 初始化 PDW.CRM.Common.BusinessException 类的新实例。
        /// </summary>
        public AuthenticationException()
            : base()
        {
        }

        /// <summary>
        /// 初始化 PDW.CRM.Common.BusinessException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public AuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化 PDW.CRM.Common.BusinessException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="isMessageException">是否消息性异常。</param>
        public AuthenticationException(string message, bool isMessageException)
            : base(message)
        {
            _isMessageException = isMessageException;
        }

        /// <summary>
        /// 初始化 PDW.CRM.Common.BusinessException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">内部异常。</param>
        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// 用序列化数据初始化 PDW.CRM.Common.BusinessException 类的新实例。
        /// </summary>
        /// <param name="info">它存有有关所引发异常的序列化的对象数据。</param>
        /// <param name="context">它包含有关源或目标的上下文信息。</param>
        protected AuthenticationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
