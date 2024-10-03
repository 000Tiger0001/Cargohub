import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_suppliers(unittest.TestCase):
    def setUp(self) -> None:
        self.suppliers = data_provider.Suppliers("", True)
    
    def test_get_suppliers(self) -> None:
        self.assertEqual(self.suppliers.get_suppliers(), [])

if __name__ == '__main__':
    unittest.main()