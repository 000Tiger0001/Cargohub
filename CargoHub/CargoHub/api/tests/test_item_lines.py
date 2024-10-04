import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_item_lines(unittest.TestCase):
    def setUp(self) -> None:
        self.item_lines = data_provider.ItemLines("", True)

    def test_get_transfers(self) -> None:
        self.assertEqual(self.item_lines.get_item_lines(), [])

if __name__ == '__main__':
    unittest.main()