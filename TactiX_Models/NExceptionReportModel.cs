namespace TactiX_Models
{
    /// <summary>
    /// 异常上报数据结构
    /// </summary>
    [Serializable]
    public class NExceptionReportModel
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Error_Code { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Error_Desc { get; set; } = string.Empty;
        /// <summary>
        /// 用户反馈渠道
        /// </summary>
        public string Feedback_Way { get; set; } = string.Empty;
        /// <summary>
        /// 用户反馈信息
        /// </summary>
        public string Feedback_Info { get; set; } = string.Empty;
        /// <summary>
        /// 反馈创建日期
        /// </summary>
        public DateTime Create_Time { get; set; }
    }
}
