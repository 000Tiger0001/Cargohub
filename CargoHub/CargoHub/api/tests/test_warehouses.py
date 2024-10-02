import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

    
class TestWarehouses(unittest.TestCase):
    def setUp(self):
        self.path = os.path.abspath(os.getcwd()) + "/CargoHub/CargoHub/data/"
        self.warehouse = data_provider.Warehouses(self.path, True)
        
    def test_get_warehouses(self):
        self.assertEqual(self.warehouse.get_warehouses(), [])
    
    def test_add_warehouse_good(self):
        test_warehouse = {
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
        self.warehouse.add_warehouse(test_warehouse)
        self.assertEqual(self.warehouse.get_warehouses(), [test_warehouse])
        self.warehouse.remove_warehouse(1)

    
    def test_add_warehouse_bad(self):
        test_item_group = {
        "id": 0,
        "name": "Electronics",
        "description": "",
        "created_at": "1998-05-15 19:52:53",
        "updated_at": "2000-11-20 08:37:56"
        }
        self.warehouse.add_warehouse(test_item_group)
        warehouses = self.warehouse.get_warehouses().copy()
        if len(warehouses) > 0:
            self.warehouse.remove_warehouse(0)
        self.assertEqual(warehouses, [])
    
    def test_get_warehouse(self):
        test_warehouse = {
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
        self.warehouse.add_warehouse(test_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(1), test_warehouse)
        self.assertEqual(self.warehouse.get_warehouse(0), None)
        self.warehouse.remove_warehouse(1)

if __name__ == '__main__':
    unittest.main()