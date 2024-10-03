import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

    
class TestWarehouses(unittest.TestCase):
    def setUp(self) -> None:
        self.warehouse = data_provider.Warehouses("", True)
        self.test_warehouse = {
            "id": 1,
            "code": "YIHLKHKGUYI56",
            "name": "Rotterdam cargo hub",
            "address": "Wijnhaven 107",
            "zip": "1000 SC",
            "city": "Rotterdam",
            "province": "Zuid-Holland",
            "country": "NL",
            "contact": {
                "name": "Daan van der Meer",
                "phone": "06 12345678",
                "email": "Daan@nepemail.com"
            },
            "created_at": "2024-10-02 12:41:55",
            "updated_at": "2024-10-02 12:41:55"
            }
        
    def test_get_warehouses(self) -> None:
        self.assertEqual(self.warehouse.get_warehouses(), [])
        self.warehouse.add_warehouse(self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouses(), [self.test_warehouse])
        self.warehouse.remove_warehouse(1)
        self.assertEqual(self.warehouse.get_warehouses(), [])
    
    def test_remove_warehouse(self) -> None:
        self.warehouse.add_warehouse(self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(1), self.test_warehouse)
        self.warehouse.remove_warehouse(1)
        self.assertEqual(self.warehouse.get_warehouses(), [])
    
    def test_add_warehouse_good(self) -> None:
        self.assertEqual(self.warehouse.get_warehouses(), [])
        self.warehouse.add_warehouse(self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouses(), [self.test_warehouse])
        self.warehouse.remove_warehouse(1)
        self.assertEqual(self.warehouse.get_warehouses(), [])

    
    def test_add_warehouse_bad(self) -> None:
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
        self.assertEqual(self.warehouse.get_warehouses(), [])
        self.warehouse.add_warehouse(test_client)
        warehouses = self.warehouse.get_warehouses().copy()
        if len(warehouses) > 0:
            self.warehouse.remove_warehouse(1)
        self.assertEqual(warehouses, [])
        self.assertEqual(self.warehouse.get_warehouses(), [])
    
    def test_add_duplicate_warehouse(self) -> None:
        self.warehouse.add_warehouse(self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouses(), [self.test_warehouse])
        self.warehouse.add_warehouse(self.test_warehouse)
        warehouses = self.warehouse.get_warehouses().copy()
        if len(warehouses) > 0:
            self.warehouse.remove_warehouse(1)
            self.warehouse.remove_warehouse(1)
        self.assertEqual(warehouses, [self.test_warehouse])
        self.assertEqual(self.warehouse.get_warehouses(), [])
    
    def test_get_warehouse(self) -> None:
        self.warehouse.add_warehouse(self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(1), self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(0), None)
        self.warehouse.remove_warehouse(1)
        self.assertEqual(self.warehouse.get_warehouse(1), None)
    
    def test_update_warehouse(self) -> None:
        test_update_warehouse = {"id": 1,
            "code": "DKAYGISHF78",
            "name": "Hellevoetsluis cargo hub",
            "address": "Wijnhaven 111",
            "zip": "1111 SC",
            "city": "Hellevoetsluis",
            "province": "Zuid-Holland",
            "country": "NL",
            "contact": {
                "name": "Daan van der Meer",
                "phone": "06 12345678",
                "email": "Daan@nepemail.com"
            },
            "created_at": "2024-10-02 12:41:55",
            "updated_at": "2024-10-02 12:41:55"}
        self.warehouse.add_warehouse(self.test_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(1), self.test_warehouse)
        self.warehouse.update_warehouse(1, test_update_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(1), test_update_warehouse)
        self.assertNotEqual(self.warehouse.get_warehouse(1), self.test_warehouse)
        self.warehouse.remove_warehouse(1)

if __name__ == '__main__':
    unittest.main()