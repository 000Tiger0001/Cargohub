import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_items(unittest.TestCase):
    def setUp(self) -> None:
        self.items = data_provider.Items("", True)

    def test_get_items(self) -> None:
        self.assertEqual(self.items.get_items(), [])

if __name__ == '__main__':
    unittest.main()