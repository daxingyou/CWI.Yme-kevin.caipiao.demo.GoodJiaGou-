using System;
using System.Collections.Generic;

namespace Yme.Mcp.Model.WeChat
{
    /// <summary>
    ///  微信指定创建菜单结构
    /// </summary>
    [Serializable]
    public class WeChatMenuButton
    {
        public string type { get; set; }

        public string name { get; set; }

        public string key { get; set; }

        public string url { get; set; }

        public List<WeChatMenuButton> sub_button { get; set; }
    }
}
