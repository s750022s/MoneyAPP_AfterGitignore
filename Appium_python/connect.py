import unittest
from appium import webdriver
from appium.webdriver.common.appiumby import AppiumBy
from appium.options.android import UiAutomator2Options


class  TestConnect:
    def __init__(self, appPackage, appActivity) -> None:
        capability = dict(
            platformName="Android",
            platformVersion="11.0",
            automationName="UiAutomator2",
            deviceName="R58MC1QYRTM",
            noReset=True
        )
        capability["appPackage"] = appPackage
        capability["appActivity"] = appActivity
    
        self.appium_server_url = "http://127.0.0.1:4723"
        
        self.capability_options = UiAutomator2Options().load_capabilities(capability)

    def connect(self):
        self.driver = webdriver.Remote(
            command_executor=self.appium_server_url, 
            options=self.capability_options)
        
    def quit(self):
        if self.driver:
            self.driver.quit()

    



        

