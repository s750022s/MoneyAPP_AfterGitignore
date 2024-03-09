using ZMoney.Pages;

namespace ZMoney
{
    /// <summary>
    /// 設定導覽規則；本專案使用shell下的[註冊詳細數據頁面路由]，不會產生TabBar。
    /// </summary>
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RouterSetting();
        }

        /// <summary>
        /// 註冊詳細數據頁面路由。
        /// </summary>
        private void RouterSetting()
        {
            //===主線頁面(客製TabBar)===

            //首頁
            Routing.RegisterRoute("Home", typeof(HomePage));

            //紀錄新增頁
            Routing.RegisterRoute("RecordAdd", typeof(RecordAddPage));

            //統計首頁
            Routing.RegisterRoute("Statistics", typeof(StatisticsPage));

            //設定首頁
            Routing.RegisterRoute("Setting", typeof(SettingPage));



            //===支線頁面(有Back鍵)===

            //首頁-紀錄更新頁
            Routing.RegisterRoute("Home/RecordUpdate", typeof(RecordUpdatePage));

            //統計-獨類列表
            Routing.RegisterRoute("Statistics/RecordsByClass", typeof(RecordsByClass));

            //設定-選單設定
            Routing.RegisterRoute("Setting/ListSetting", typeof(ListSetting));

            //設定-備份與重置
            Routing.RegisterRoute("Setting/BackupAndReset", typeof(BackupAndReset));

            //設定-系統資訊
            Routing.RegisterRoute("Setting/AppInfo", typeof(ZmoneyInfo));
        }
    }
}
