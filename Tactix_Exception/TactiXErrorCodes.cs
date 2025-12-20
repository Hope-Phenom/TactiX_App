namespace TactiX_Exception;

public enum TactiXErrorCodes
{
    /// <summary>
    ///     成功，无异常，或手动上报/建议
    /// </summary>
    Success = 0,

    /// <summary>
    ///     重复启动进程
    /// </summary>
    ErrorMuiltProcess,

    /// <summary>
    ///     网络连接问题
    /// </summary>
    ErrorNetwork
}