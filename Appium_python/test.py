# -*- coding: utf-8 -*-

import unittest
from appium import webdriver
from appium.webdriver.common.appiumby import AppiumBy
from appium.options.android import UiAutomator2Options
import time

capabilities = dict(
    platformName='Android',
    automationName='UiAutomator2',
    deviceName='R58MC1QYRTM',
    appPackage='com.android.settings',
    appActivity='.homepage.SettingsHomepageActivity',
    platformVersion='11.0'
)

appium_server_url = 'http://127.0.0.1:4723'
capabilities_options = UiAutomator2Options().load_capabilities(capabilities)

class TestAppium(unittest.TestCase):
    def setUp(self) -> None:
        self.driver = webdriver.Remote(command_executor=appium_server_url, options=capabilities_options)

    def tearDown(self) -> None:
        if self.driver:
            self.driver.quit()

    def test_find_battery(self) -> None:
        el = self.driver.find_element(by=AppiumBy.XPATH, value='//*[@resource-id="com.android.settings:id/recycler_view"]/android.widget.LinearLayout[6] ')
        el.click()
        #time.sleep(10)
        #print('測試成功')


if __name__ == '__main__':
    unittest.main(exit=False)

