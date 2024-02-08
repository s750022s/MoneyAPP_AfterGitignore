using ZMoney.Pages;

namespace ZMoney
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RouterSetting();
        }

        private void RouterSetting() 
        {
            //===主線頁面(TabBar)===

            //首頁
            Routing.RegisterRoute("Home", typeof(HomePage));

            //紀錄新增頁
            Routing.RegisterRoute("RecordAdd", typeof(RecordAddPage));

            //統計首頁
            Routing.RegisterRoute("Statistics", typeof(StatisticsPage));

            //設定首頁
            Routing.RegisterRoute("Setting", typeof(SettingPage));



            //===支線頁面(Back鍵)===

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
