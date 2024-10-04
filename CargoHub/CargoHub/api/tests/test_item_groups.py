import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_item_groups(unittest.TestCase):
    def setUp(self) -> None:
        self.item_groups = data_provider.ItemGroups("", True)
        self.item_group = {
        "id": 1,
        "name": "Furniture",
        "description": "",
        "created_at": "2019-09-22 15:51:07",
        "updated_at": "2022-05-18 13:49:28"
        }

    def test_get_item_groups(self) -> None:
        self.assertEqual(self.item_groups.get_item_groups(), [])
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.remove_item_group(1)
        self.assertEqual(self.item_groups.get_item_groups(), [])
    
    def test_get_item_group(self) -> None:
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_group(1), self.item_group)
        self.assertEqual(self.item_groups.get_item_group(0), None)
        self.item_groups.remove_item_group(1)
        self.assertEqual(self.item_groups.get_item_group(1), None)
    
    def test_add_item_group_good(self) -> None:
        self.assertEqual(self.item_groups.get_item_groups(), [])
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.remove_item_group(1)
        self.assertEqual(self.item_groups.get_item_groups(), [])
    
    def test_add_item_group_bad(self) -> None:
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
        self.assertEqual(self.item_groups.get_item_groups(), [])
        self.item_groups.add_item_group(test_client)
        item_groups = self.item_groups.get_item_groups().copy()
        self.item_groups.remove_item_group(1)
        self.assertEqual(item_groups, [])
        self.assertEqual(self.item_groups.get_item_groups(), [])
    
    def test_add_item_group_with_duplicate_model(self) -> None:
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.add_item_group(self.item_group)
        item_groups = self.item_groups.get_item_groups().copy()
        self.item_groups.remove_item_group(1)
        self.item_groups.remove_item_group(1)
        self.assertEqual(item_groups, [self.item_group])
        self.assertEqual(self.item_groups.get_item_groups(), [])
    
    def test_add_item_group_with_duplicate_id(self) -> None:
        test_item_group = {
        "id": 1,
        "name": "Electronics",
        "description": "",
        "created_at": "1998-05-15 19:52:53",
        "updated_at": "2000-11-20 08:37:56"
        }
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.add_item_group(test_item_group)
        item_groups = self.item_groups.get_item_groups().copy()
        self.item_groups.remove_item_group(1)
        self.item_groups.remove_item_group(1)
        self.assertEqual(item_groups, [self.item_group])
        self.assertEqual(self.item_groups.get_item_groups(), [])
    
    def test_update_item_group(self) -> None:
        test_item_group = {
        "id": 1,
        "name": "Electronics",
        "description": "",
        "created_at": "1998-05-15 19:52:53",
        "updated_at": "2000-11-20 08:37:56"
        }
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.update_item_group(1, test_item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [test_item_group])
        self.assertNotEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.remove_item_group(1)
    
    def test_update_item_group_with_duplicate_id(self) -> None:
        test_item_group = {
        "id": 2,
        "name": "Electronics",
        "description": "",
        "created_at": "1998-05-15 19:52:53",
        "updated_at": "2000-11-20 08:37:56"
        }
        test_updated_item_group = {
        "id": 1,
        "name": "Electronics",
        "description": "",
        "created_at": "1998-05-15 19:52:53",
        "updated_at": "2000-11-20 08:37:56"
        }
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.add_item_group(test_item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group, test_item_group])
        self.item_groups.update_item_group(2, test_updated_item_group)
        item_groups = self.item_groups.get_item_groups().copy()
        self.item_groups.remove_item_group(1)
        self.item_groups.remove_item_group(1)
        self.assertEqual(item_groups, [self.item_group, test_item_group])
        self.assertEqual(self.item_groups.get_item_groups(), [])
    
    def test_remove_item_group(self) -> None:
        self.item_groups.add_item_group(self.item_group)
        self.assertEqual(self.item_groups.get_item_groups(), [self.item_group])
        self.item_groups.remove_item_group(1)
        self.assertEqual(self.item_groups.get_item_groups(), [])

if __name__ == '__main__':
    unittest.main()
