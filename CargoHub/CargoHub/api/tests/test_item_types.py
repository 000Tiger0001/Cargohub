import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_item_types(unittest.TestCase):
    def setUp(self) -> None:
        self.item_types = data_provider.ItemTypes("", True)

    def test_get_item_types(self) -> None:
        self.assertEqual(self.item_types.get_item_types(), [])

if __name__ == '__main__':
    unittest.main()