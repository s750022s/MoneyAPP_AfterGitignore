# -*- coding: utf-8 -*-

#單元測試的標準python函式庫
import unittest
#appium的驅動程式
from appium import webdriver
#定位元素
from appium.webdriver.common.appiumby import AppiumBy
#設置和配置 UiAutomator2 測試引擎的選項
from appium.options.android import UiAutomator2Options



#告訴伺服器app執行的環境。
capabilities = dict(
    #使用的行動平台
    platformName="Android",
    #指定平台的系統版本
    platformVersion="11.0",
    #使用的自動化引擎，用appium伺服器執行時提供的資訊
    automationName="UiAutomator2",
    #啟動的裝置、實機、模擬機(adb devices 查詢名稱)
    deviceName="R58MC1QYRTM",
    #被測試APP的Package名，取自WEditor
    appPackage="com.android.settings",
    #被測試APP的Activity名，取自WEditor
    appActivity=".homepage.SettingsHomepageActivity",
    #每次測試運行之前會不會重置應用程式的狀態，True時會保持打開APP，預設為False
    #官網無此項
    #False目的：確保測試開始時應用程式處於預期的初始狀態
    #noReset=True
)

#Appium Server的Ip位置，取自執行中的Appium
appium_server_url = "http://127.0.0.1:4723"

#使用 UiAutomator2Options 加載設定的選項
capabilities_options = UiAutomator2Options().load_capabilities(capabilities)


class TestAppium(unittest.TestCase): # class A (class B) A繼承至B
    """
    利用python標準庫unittest建立測試類別。
    繼承至unittest.TestCase類別(必要);
    目的：標準化的測試結果。
    """
    def setUp(self) -> None: #回傳None
        """
        定義測試使用案例的前置動作:建立與AppiumServer的聯繫。
        command_executor:指向 Appium 服務器的 URL。
        options:app執行的環境設定, 已使用 UiAutomator2Options 加載。
        """
        self.driver = webdriver.Remote(command_executor=appium_server_url, options=capabilities_options)

    def tearDown(self) -> None: #回傳None
        """
        定義測試使用案例的結束動作。
        檢查是否有 Appium 驅動程式實例存在，如果有則退出。
        """
        if self.driver:
            self.driver.quit()

    def test_find_wifi(self) -> None: #回傳None
        """
        測試案例：找到WIFI設定頁面。
        unittest的規則：測試方法一律以test_開頭。
        """
        el = self.driver.find_element(by=AppiumBy.XPATH, value="//*[@text='桌布']")
        el.click()

        """
        原希望找到電池，但一直失敗。
        後來發現是因為需要捲動頁面，
        程式剛開始會再開啟後的畫面尋找，因此老是找不到電池，因為尋找範圍為設定到桌布。
        """

#執行unittest的main方法。
if __name__ == "__main__":
    unittest.main(exit=False) #若無exit=False，會一直跳SystemExit，需再研究。

"""
以unittest.main()執行測試時，
unittest會依照方式名稱排序並依序執行，先執行test_axx再執行test_bxx。
若要自訂順序，需使用TextTestRunner。
"""

