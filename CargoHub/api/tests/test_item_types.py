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
        self.item_type = {
        "id": 1,
        "name": "Desktop",
        "description": "",
        "created_at": "1993-07-28 13:43:32",
        "updated_at": "2022-05-12 08:54:35"
        }

    def test_get_item_types(self) -> None:
        self.assertEqual(self.item_types.get_item_types(), [])
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.remove_item_type(1)
        self.assertEqual(self.item_types.get_item_types(), [])
    
    def test_get_item_type(self) -> None:
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_type(1), self.item_type)
        self.assertEqual(self.item_types.get_item_type(0), None)
        self.item_types.remove_item_type(1)
        self.assertEqual(self.item_types.get_item_type(1), None)
    
    def test_add_item_type_good(self) -> None:
        self.assertEqual(self.item_types.get_item_types(), [])
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.remove_item_type(1)
        self.assertEqual(self.item_types.get_item_types(), [])
    
    def test_add_item_type_bad(self) -> None:
        test_client = {
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
        self.assertEqual(self.item_types.get_item_types(), [])
        self.item_types.add_item_type(test_client)
        item_types = self.item_types.get_item_types().copy()
        self.item_types.remove_item_type(1)
        self.assertEqual(item_types, [])
        self.assertEqual(self.item_types.get_item_types(), [])
    
    def test_add_item_type_with_duplicate_model(self) -> None:
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.add_item_type(self.item_type)
        item_types = self.item_types.get_item_types().copy()
        self.item_types.remove_item_type(1)
        self.item_types.remove_item_type(1)
        self.assertEqual(item_types, [self.item_type])
        self.assertEqual(self.item_types.get_item_types(), [])
    
    def test_add_item_type_with_duplicate_id(self) -> None:
        {
        "id": 1,
        "name": "Tablet",
        "description": "",
        "created_at": "1977-05-01 00:05:04",
        "updated_at": "2001-04-14 02:41:59"
        }
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.add_item_type(self.item_type)
        item_types = self.item_types.get_item_types().copy()
        self.item_types.remove_item_type(1)
        self.item_types.remove_item_type(1)
        self.assertEqual(item_types, [self.item_type])
        self.assertEqual(self.item_types.get_item_types(), [])
    
    def test_update_item_type(self) -> None:
        test_update_item_type = {
        "id": 1,
        "name": "Tablet",
        "description": "",
        "created_at": "1977-05-01 00:05:04",
        "updated_at": "2001-04-14 02:41:59"
        }
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.update_item_type(1, test_update_item_type)
        self.assertEqual(self.item_types.get_item_type(1), test_update_item_type)
        self.assertNotEqual(self.item_types.get_item_type(1), self.item_type)
        self.item_types.remove_item_type(1)
    
    def test_update_item_type_to_duplicate_id(self) -> None:
        test_update_item_type = {
        "id": 2,
        "name": "Tablet",
        "description": "",
        "created_at": "1977-05-01 00:05:04",
        "updated_at": "2001-04-14 02:41:59"
        }
        test_duplicate_update_item_type = {
        "id": 1,
        "name": "Tablet",
        "description": "",
        "created_at": "1977-05-01 00:05:04",
        "updated_at": "2001-04-14 02:41:59"
        }
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.add_item_type(test_update_item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type, test_update_item_type])
        self.item_types.update_item_type(2, test_duplicate_update_item_type)
        item_types = self.item_types.get_item_types().copy()
        self.item_types.remove_item_type(1)
        self.item_types.remove_item_type(1)
        self.assertEqual(item_types, [self.item_type])
        self.assertEqual(self.item_types.get_item_types(), [])
    
    def test_remove_item_type(self) -> None:
        self.item_types.add_item_type(self.item_type)
        self.assertEqual(self.item_types.get_item_types(), [self.item_type])
        self.item_types.remove_item_type(1)
        self.assertEqual(self.item_types.get_item_types(), [])

if __name__ == '__main__':
    unittest.main()