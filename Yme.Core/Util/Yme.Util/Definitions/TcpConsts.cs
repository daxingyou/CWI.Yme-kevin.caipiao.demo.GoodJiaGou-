namespace Yme.Util
{
    /// <summary>
    /// 常用常量
    /// </summary>
    public static class TcpConsts
    {
        /// <summary>
        /// 无效命令
        /// </summary>
        public const int InvalidCmd = 401;

        /// <summary>
        /// 发送打印指令失败
        /// </summary>
        public const int SendPrintCmdFailed = 10000;

        /// <summary>
        /// 发送打印指令成功
        /// </summary>
        public const int SendPrintCmdSuccess = 10001;

        /// <summary>
        /// 未找到打印机
        /// </summary>
        public const int PrinterNotFind = 10002;

        /// <summary>
        /// 打印订单不存在
        /// </summary>
        public const int PrintOrderNotFind = 10003;

        /// <summary>
        /// 订单Key不存在
        /// </summary>
        public const int OrderKeyNotFind = 10004;

        /// <summary>
        /// 注册失败
        /// </summary>
        public const int RegDevFailed = 10101;

        /// <summary>
        /// 更新打印状态失败
        /// </summary>
        public const int UpStatusFailed = 10102;

        /// <summary>
        /// 上报异常失败
        /// </summary>
        public const int UpExceptionFailed = 10103;
    }
}
