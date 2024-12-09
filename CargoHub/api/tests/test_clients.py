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
        self.client = {
        "id": 1,
        "name": "Raymond Inc",
        "address": "1296 Daniel Road Apt. 349",
        "city": "Pierceview",
        "zip_code": "28301",
        "province": "Colorado",
        "country": "United States",
        "contact_name": "Bryan Clark",
        "contact_phone": "242.732.3483x2573",
        "contact_email": "robertcharles@example.net",
        "created_at": "2010-04-28 02:22:53",
        "updated_at": "2022-02-09 20:22:35"
        }

    def test_get_clients(self) -> None:
        self.assertEqual(self.clients.get_clients(), [])
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.remove_client(1)
        self.assertEqual(self.clients.get_clients(), [])
    
    def test_get_client(self) -> None:
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_client(1), self.client)
        self.assertEqual(self.clients.get_client(0), None)
        self.clients.remove_client(1)
        self.assertEqual(self.clients.get_client(1), None)
    
    def test_add_client_good(self) -> None:
        self.assertEqual(self.clients.get_clients(), [])
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.remove_client(1)
        self.assertEqual(self.clients.get_clients(), [])
    
    def test_add_client_bad(self) -> None:
        test_location = {
        "id": 1,
        "warehouse_id": 1,
        "code": "A.1.0",
        "name": "Row: A, Rack: 1, Shelf: 0",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        self.assertEqual(self.clients.get_clients(), [])
        self.clients.add_client(test_location)
        clients = self.clients.get_clients().copy()
        self.clients.remove_client(1)
        self.assertEqual(clients, [])
        self.assertEqual(self.clients.get_clients(), [])
    
    def test_add_client_with_duplicate_model(self) -> None:
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.add_client(self.client)
        clients = self.clients.get_clients().copy()
        self.clients.remove_client(1)
        self.clients.remove_client(1)
        self.assertEqual(clients, [self.client])
        self.assertEqual(self.clients.get_clients(), [])
    
    def test_add_client_with_duplicate_id(self) -> None:
        add_client_with_duplicate_id = {
        "id": 1,
        "name": "Williams Ltd",
        "address": "2989 Flores Turnpike Suite 012",
        "city": "Lake Steve",
        "zip_code": "08092",
        "province": "Arkansas",
        "country": "United States",
        "contact_name": "Megan Hayden",
        "contact_phone": "8892853366",
        "contact_email": "qortega@example.net",
        "created_at": "1973-02-24 07:36:32",
        "updated_at": "2014-06-20 17:46:19"
        }
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.add_client(add_client_with_duplicate_id)
        clients = self.clients.get_clients().copy()
        self.clients.remove_client(1)
        self.clients.remove_client(1)
        self.assertEqual(clients, [self.client])
        self.assertEqual(self.clients.get_clients(), [])
    
    def test_update_client(self) -> None:
        updated_client = {
        "id": 1,
        "name": "Williams Ltd",
        "address": "2989 Flores Turnpike Suite 012",
        "city": "Lake Steve",
        "zip_code": "08092",
        "province": "Arkansas",
        "country": "United States",
        "contact_name": "Megan Hayden",
        "contact_phone": "8892853366",
        "contact_email": "qortega@example.net",
        "created_at": "1973-02-24 07:36:32",
        "updated_at": "2014-06-20 17:46:19"
        }
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.update_client(1, updated_client)
        self.assertEqual(self.clients.get_clients(), [updated_client])
        self.assertNotEqual(self.clients.get_clients(), [self.client])
        self.clients.remove_client(1)
    
    def test_update_client_with_duplicate_id(self) -> None:
        test_client = {
        "id": 2,
        "name": "Williams Ltd",
        "address": "2989 Flores Turnpike Suite 012",
        "city": "Lake Steve",
        "zip_code": "08092",
        "province": "Arkansas",
        "country": "United States",
        "contact_name": "Megan Hayden",
        "contact_phone": "8892853366",
        "contact_email": "qortega@example.net",
        "created_at": "1973-02-24 07:36:32",
        "updated_at": "2014-06-20 17:46:19"
        }
        test_updated_client = {
        "id": 1,
        "name": "Williams Ltd",
        "address": "2989 Flores Turnpike Suite 012",
        "city": "Lake Steve",
        "zip_code": "08092",
        "province": "Arkansas",
        "country": "United States",
        "contact_name": "Megan Hayden",
        "contact_phone": "8892853366",
        "contact_email": "qortega@example.net",
        "created_at": "1973-02-24 07:36:32",
        "updated_at": "2014-06-20 17:46:19"
        }
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.add_client(test_client)
        self.assertEqual(self.clients.get_clients(), [self.client, test_client])
        self.clients.update_client(2, test_updated_client)
        clients = self.clients.get_clients().copy()
        self.clients.remove_client(1)
        self.clients.remove_client(1)
        self.assertEqual(clients, [self.client, test_client])
        
    def test_remove_client(self) -> None:
        self.clients.add_client(self.client)
        self.assertEqual(self.clients.get_clients(), [self.client])
        self.clients.remove_client(1)
        self.assertEqual(self.clients.get_clients(), [])

if __name__ == '__main__':
    unittest.main()