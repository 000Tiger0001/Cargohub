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
        self.item_line = {
        "id": 1,
        "name": "Home Appliances",
        "description": "",
        "created_at": "1979-01-16 07:07:50",
        "updated_at": "2024-01-05 23:53:25"
        }

    def test_get_item_lines(self) -> None:
        self.assertEqual(self.item_lines.get_item_lines(), [])
        self.item_lines.add_item_line(self.item_line)
        self.assertEqual(self.item_lines.get_item_lines(), [self.item_line])
        self.item_lines.remove_item_line(1)
        self.assertEqual(self.item_lines.get_item_lines(), [])
    
    def test_get_item_line(self) -> None:
        self.item_lines.add_item_line(self.item_line)
        self.assertEqual(self.item_lines.get_item_line(1), self.item_line)
        self.assertEqual(self.item_lines.get_item_line(0), None)
        self.item_lines.remove_item_line(1)
        self.assertEqual(self.item_lines.get_item_line(1), None)
    
    def test_add_item_line_good(self) -> None:
        self.assertEqual(self.item_lines.get_item_lines(), [])
        self.item_lines.add_item_line(self.item_line)
        self.assertEqual(self.item_lines.get_item_lines(), [self.item_line])
        self.item_lines.remove_item_line(1)
        self.assertEqual(self.item_lines.get_item_lines(), [])
    
    def test_add_item_line_bad(self) -> None:
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
        self.assertEqual(self.item_lines.get_item_lines(), [])
        self.item_lines.add_item_line(test_client)
        item_lines = self.item_lines.get_item_lines().copy()
        self.item_lines.remove_item_line(1)
        self.assertEqual(item_lines, [])
        self.assertEqual(self.item_lines.get_item_lines(), [])
    
    def test_add_item_line_with_duplicate_model(self) -> None:
        self.item_lines.add_item_line(self.item_line)
        self.assertEqual(self.item_lines.get_item_lines(), [self.item_line])
        self.item_lines.add_item_line(self.item_line)
        item_lines = self.item_lines.get_item_lines().copy()
        self.item_lines.remove_item_line(1)
        self.item_lines.remove_item_line(1)
        self.assertEqual(item_lines, [self.item_line])
        self.assertEqual(self.item_lines.get_item_lines(), [])
    
    def test_add_item_line_with_duplicate_id(self) -> None:
        test_item_line = {
        "id": 1,
        "name": "Office Supplies",
        "description": "",
        "created_at": "2009-07-18 08:13:40",
        "updated_at": "2020-01-12 14:32:49"
        }
        self.item_lines.add_item_line(self.item_line)
        self.assertEqual(self.item_lines.get_item_lines(), [self.item_line])
        self.item_lines.add_item_line(test_item_line)
        item_lines = self.item_lines.get_item_lines().copy()
        self.item_lines.remove_item_line(1)
        self.item_lines.remove_item_line(1)
        self.assertEqual(item_lines, [self.item_line])
        self.assertEqual(self.item_lines.get_item_lines(), [])
    
    def test_update_item_line(self) -> None:
        test_item_line = {
        "id": 1,
        "name": "Office Supplies",
        "description": "",
        "created_at": "2009-07-18 08:13:40",
        "updated_at": "2020-01-12 14:32:49"
        }
        self.item_lines.add_item_line(self.item_line)
        self.assertEqual(self.item_lines.get_item_lines(), [self.item_line])
        self.item_lines.update_item_line(1, test_item_line)
        self.assertEqual(self.item_lines.get_item_line(1), test_item_line)
        self.assertNotEqual(self.item_lines.get_item_line(1), self.item_line)
        self.item_lines.remove_item_line(1)

if __name__ == '__main__':
    unittest.main()