import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_orders(unittest.TestCase):
    def setUp(self) -> None:
        self.orders = data_provider.Orders("", True)

    def test_get_orders(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])

if __name__ == '__main__':
    unittest.main()