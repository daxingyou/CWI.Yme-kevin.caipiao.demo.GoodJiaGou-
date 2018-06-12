using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.BaseManage;

namespace Yme.Mcp.Entity
{
    public class LoginInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 用户登陆令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 系统用户信息
        /// </summary>
        public ShopEntity CurrUserInfo
        {
            get
            {
                return new ShopEntity
                {
                    ShopId = this.UserId,
                    ShopAccount = this.Mobile,
                    AccessToken = this.AccessToken
                };
            }
        }
    }
}
