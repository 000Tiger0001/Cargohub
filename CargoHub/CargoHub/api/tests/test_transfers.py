import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_transfers(unittest.TestCase):
    def setUp(self) -> None:
        self.transfers = data_provider.Transfers("", True)
        
    def test_get_transfers(self) -> None:
        self.assertEqual(self.transfers.get_transfers(), [])

if __name__ == '__main__':
    unittest.main()