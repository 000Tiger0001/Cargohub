import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_locations(unittest.TestCase):
    def setUp(self) -> None:
            self.locations = data_provider.Locations("", True)

    def test_get_transfers(self) -> None:
        self.assertEqual(self.locations.get_locations(), [])

if __name__ == '__main__':
    unittest.main()