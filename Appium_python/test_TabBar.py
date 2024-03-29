import unittest
from connect import TestConnect
from appium.webdriver.common.appiumby import AppiumBy


testConnect = TestConnect("com.companyname.zmoney","crc64f47fc68d607a8646.MainActivity")
class TestTabBar(unittest.TestCase):
       
    def setUp(self) -> None:
        testConnect.connect()

    def tearDown(self) -> None:
        testConnect.quit()

    def test_print(self):
        el = testConnect.driver.find_element(by=AppiumBy.XPATH, value="//android.widget.Button[@text='歷史 紀錄']")
        el.click()

if __name__ == '__main__':
    suit = unittest.TestSuite()
    suit.addTest(TestTabBar("test_print"))
    runner = unittest.TextTestRunner()
    runner.run(suit) 