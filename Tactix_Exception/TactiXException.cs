using System.Runtime.Serialization;

using TactiX_I18N;

namespace TactiX_Exception
{
    /// <summary>
    /// TactiX 内部异常
    /// </summary>
    [Serializable]
    public class TactiXException : Exception
    {
        private ILanguage language => I18N.Instance.Language;

        /// <summary>
        /// 错误码
        /// </summary>
        public TactiXErrorCodes ErrorCode { get; private set; }
        /// <summary>
        /// 错误描述文本
        /// </summary>
        public string ErrorDesc { get; private set; }

        public TactiXException() 
        {
            ErrorDesc = string.Empty;
        }

        /// <summary>
        /// 传参构建异常
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorDesc">错误信息</param>
        public TactiXException(TactiXErrorCodes errorCode, string errorDesc)
        {
            ErrorCode = errorCode;
            ErrorDesc = errorDesc;
        }

        public override string ToString() 
        {
            return language.ERROR_DESC_TEMPLATE
                .Replace("{0}", $"{ErrorCode}")
                .Replace("{1}", ErrorDesc);
        }
    }
}
