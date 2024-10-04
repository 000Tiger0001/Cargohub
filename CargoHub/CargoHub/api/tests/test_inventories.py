import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_inventories(unittest.TestCase):
    def setUp(self) -> None:
        self.inventories = data_provider.Inventories("", True)

    def test_get_inventories(self) -> None:
        self.assertEqual(self.inventories.get_inventories(), [])

if __name__ == '__main__':
    unittest.main()