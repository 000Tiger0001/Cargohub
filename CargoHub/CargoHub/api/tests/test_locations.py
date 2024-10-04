import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_locations(unittest.TestCase):
    def setUp(self) -> None:
        self.warehouses = data_provider.Warehouses("", True)
        self.locations = data_provider.Locations("", True)
        self.location = {
                            "id": 1,
                            "warehouse_id": 1,
                            "code": "A.1.0",
                            "name": "Row: A, Rack: 1, Shelf: 0",
                            "created_at": "1992-05-15 03:21:32",
                            "updated_at": "1992-05-15 03:21:32"
                                            }

    def test_get_transfers(self) -> None:
        self.assertEqual(self.locations.get_locations(), [])
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.remove_location(1)
        self.assertEqual(self.locations.get_locations(), [])
    
    def test_get_location(self) -> None:
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_location(1), self.location)
        self.assertEqual(self.locations.get_location(2), None)
        self.locations.remove_location(1)
        self.assertEqual(self.locations.get_location(1), None)
    
    def test_get_locations_in_warehouse(self) -> None:
        test_location_1 = {
        "id": 2,
        "warehouse_id": 1,
        "code": "A.1.1",
        "name": "Row: A, Rack: 1, Shelf: 1",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        test_location_2 = {
        "id": 3,
        "warehouse_id": 1,
        "code": "A.2.0",
        "name": "Row: A, Rack: 2, Shelf: 0",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        test_warehouse = {
        "id": 1,
        "code": "YQZZNL56",
        "name": "Heemskerk cargo hub",
        "address": "Karlijndreef 281",
        "zip": "4002 AS",
        "city": "Heemskerk",
        "province": "Friesland",
        "country": "NL",
        "contact": {
            "name": "Fem Keijzer",
            "phone": "(078) 0013363",
            "email": "blamore@example.net"
        },
        "created_at": "1983-04-13 04:59:55",
        "updated_at": "2007-02-08 20:11:00"
        }
        self.assertEqual(self.locations.get_locations_in_warehouse(1), [])
        self.locations.add_location(self.location)
        self.locations.add_location(test_location_1)
        self.locations.add_location(test_location_2)
        self.warehouses.add_warehouse(test_warehouse)
        self.assertEqual(self.locations.get_locations_in_warehouse(1), [self.location, test_location_1, test_location_2])
        self.locations.remove_location(3)
        self.assertEqual(self.locations.get_locations_in_warehouse(1), [self.location, test_location_1])
        self.locations.remove_location(2)
        self.assertEqual(self.locations.get_locations_in_warehouse(1), [self.location])
        self.locations.remove_location(1)
        self.assertEqual(self.locations.get_locations_in_warehouse(1), [])
    
    def test_add_location_good(self) -> None:
        self.assertEqual(self.locations.get_locations(), [])
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.remove_location(1)
        self.assertEqual(self.locations.get_locations(), [])
    
    def test_add_location_bad(self) -> None:
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
        self.assertEqual(self.locations.get_locations(), [])
        self.locations.add_location(test_client)
        locations = self.locations.get_locations().copy()
        self.locations.remove_location(1)
        self.assertEqual(locations, [])
        self.assertEqual(self.locations.get_locations(), [])
    
    def test_add_location_with_duplicate_model(self) -> None:
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.add_location(self.location)
        locations = self.locations.get_locations().copy()
        self.locations.remove_location(1)
        self.locations.remove_location(1)
        self.assertEqual(locations, [self.location])
        self.assertEqual(self.locations.get_locations(), [])
    
    def test_add_location_with_duplicate_id(self) -> None:
        test_location = {
        "id": 1,
        "warehouse_id": 1,
        "code": "A.1.1",
        "name": "Row: A, Rack: 1, Shelf: 1",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.add_location(test_location)
        locations = self.locations.get_locations().copy()
        self.locations.remove_location(1)
        self.locations.remove_location(1)
        self.assertEqual(locations, [self.location])
        self.assertEqual(self.locations.get_locations(), [])
    
    def test_update_location(self) -> None:
        test_location = {
        "id": 1,
        "warehouse_id": 1,
        "code": "A.1.1",
        "name": "Row: A, Rack: 1, Shelf: 1",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.update_location(1, test_location)
        self.assertEqual(self.locations.get_locations(), [test_location])
        self.assertNotEqual(self.locations.get_locations(), [self.location])
        self.locations.remove_location(1)
    
    def test_update_location_duplicate_id(self):
        test_location = {
        "id": 2,
        "warehouse_id": 1,
        "code": "A.1.1",
        "name": "Row: A, Rack: 1, Shelf: 1",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        test_updated_location = {
        "id": 1,
        "warehouse_id": 1,
        "code": "A.1.1",
        "name": "Row: A, Rack: 1, Shelf: 1",
        "created_at": "1992-05-15 03:21:32",
        "updated_at": "1992-05-15 03:21:32"
        }
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.add_location(test_location)
        self.assertEqual(self.locations.get_locations(), [self.location, test_location])
        self.locations.update_location(2, test_updated_location)
        locations = self.locations.get_locations().copy()
        self.locations.remove_location(1)
        self.locations.remove_location(1)
        self.assertEqual(locations, [self.location, test_location])
        self.assertEqual(self.locations.get_locations(), [])
    
    def test_remove_location(self) -> None:
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_locations(), [self.location])
        self.locations.remove_location(1)
        self.assertEqual(self.locations.get_locations(), [])

if __name__ == '__main__':
    unittest.main()