import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_shipments(unittest.TestCase):
    def setUp(self) -> None:
        self.shipments = data_provider.Shipments("", True)

    def test_get_shipments(self) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])

if __name__ == '__main__':
    unittest.main()