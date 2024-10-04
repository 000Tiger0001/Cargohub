import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_clients(unittest.TestCase):
    def setUp(self) -> None:
        self.clients = data_provider.Clients("", True)

    def test_get_clients(self) -> None:
        self.assertEqual(self.clients.get_clients(), [])

if __name__ == '__main__':
    unittest.main()