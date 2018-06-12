using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.IService.SystemManage
{
    public interface IVerifyCodeService
    {
         /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>已有的验证码</returns>
        string GetVerifyCode(string appSign, string mobile, string terminalSign, VerifyCodeType verifyCodeType);

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns>验证码ID</returns>
        long CheckEntity(string appSign, string verifyCode, string mobile);

        /// <summary>
        /// 使用验证码
        /// </summary>
        /// <param name="verifyCodeId">验证码Id</param>
        void UpdateEntityStatus(string appSign, long verifyCodeId);
    }
}
