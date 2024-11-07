import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_inventories(unittest.TestCase):
    def setUp(self) -> None:
        self.inventories = data_provider.Inventories("", True)
        self.inventory_1 = {
            "id": 1,
            "item_id": "P000001",
            "description": "Face-to-face clear-thinking complexity",
            "item_reference": "sjQ23408K",
            "locations": [
                3211,
                24700,
                14123,
                19538,
                31071,
                24701,
                11606,
                11817
            ],
            "total_on_hand": 262,
            "total_expected": 0,
            "total_ordered": 80,
            "total_allocated": 41,
            "total_available": 141,
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        }
        self.inventory_2 = {
            "id": 2,
            "item_id": "P000002",
            "description": "Focused transitional alliance",
            "item_reference": "nyg48736S",
            "locations": [
                19800,
                23653,
                3068,
                3334,
                20477,
                20524,
                17579,
                2271,
                2293,
                22717
            ],
            "total_on_hand": 194,
            "total_expected": 0,
            "total_ordered": 139,
            "total_allocated": 0,
            "total_available": 55,
            "created_at": "2020-05-31 16:00:08",
            "updated_at": "2020-11-08 12:49:21"
        }

    def test_get_inventories(self) -> None:
        self.assertEqual(self.inventories.get_inventories(), [])
        self.inventories.add_inventory(self.inventory_1)
        self.assertEqual(self.inventories.get_inventories(), [self.inventory_1])
        self.inventories.remove_inventory(1)
        self.assertEqual(self.inventories.get_inventories(), [])
    
    def test_get_inventory(self) -> None:
        self.inventories.add_inventory(self.inventory_1)
        self.assertEqual(self.inventories.get_inventory(1), self.inventory_1)
        self.assertIsNone(self.inventories.get_inventory(0))
        self.inventories.remove_inventory(1)
        self.assertIsNone(self.inventories.get_inventory(1))
    
    def test_get_inventories_for_items(self) -> None:
        self.inventories.add_inventory(self.inventory_1)
        self.inventories.add_inventory(self.inventory_2)
        self.assertEqual(self.inventories.get_inventories(), [self.inventory_1, self.inventory_2])
        self.assertEqual(self.inventories.get_inventories_for_item("P000001"), [self.inventory_1])
        self.assertEqual(self.inventories.get_inventories_for_item("P000002"), [self.inventory_2])
        self.assertEqual(self.inventories.get_inventories_for_item("P000003"), [])
        self.inventories.remove_inventory(1)
        self.inventories.remove_inventory(2)
        self.assertEqual(self.inventories.get_inventories(), [])
    
    def test_get_inventory_totals_for_item(self) -> None:
        inventory_3 = {
            "id": 3,
            "item_id": "P000001",
            "description": "Cloned actuating artificial intelligence",
            "item_reference": "QVm03739H",
            "locations": [
                5321,
                21960
            ],
            "total_on_hand": 24,
            "total_expected": 0,
            "total_ordered": 90,
            "total_allocated": 68,
            "total_available": -134,
            "created_at": "1994-06-02 06:38:40",
            "updated_at": "1999-10-13 01:10:32"
        }
        self.inventories.add_inventory(self.inventory_1)
        self.inventories.add_inventory(self.inventory_2)
        self.assertEqual(self.inventories.get_inventories(), [
                         self.inventory_1, self.inventory_2])
        self.assertEqual(self.inventories.get_inventory_totals_for_item("P000001"), {
                         "total_expected": 0, "total_ordered": 80, "total_allocated": 41, "total_available": 141})
        self.assertEqual(self.inventories.get_inventory_totals_for_item("P000002"), {
                         "total_expected": 0, "total_ordered": 139, "total_allocated": 0, "total_available": 55})
        self.inventories.add_inventory(inventory_3)
        self.assertEqual(self.inventories.get_inventory_totals_for_item("P000001"), {
                         "total_expected": 0, "total_ordered": 170, "total_allocated": 109, "total_available": 7})
        self.inventories.remove_inventory(1)
        self.inventories.remove_inventory(2)
        self.inventories.remove_inventory(3)
        self.assertEqual(self.inventories.get_inventories(), [])
    
    def test_add_inventory_good(self) -> None:
        self.assertEqual(self.inventories.get_inventories(), [])
        self.inventories.add_inventory(self.inventory_1)
        self.assertEqual(self.inventories.get_inventories(),
                         [self.inventory_1])
        self.inventories.remove_inventory(1)
        self.assertEqual(self.inventories.get_inventories(), [])

    def test_add_inventory_bad(self) -> None:
        test_location = {
            "id": 1,
            "warehouse_id": 1,
            "code": "A.1.0",
            "name": "Row: A, Rack: 1, Shelf: 0",
            "created_at": "1992-05-15 03:21:32",
            "updated_at": "1992-05-15 03:21:32"
        }
        self.assertEqual(self.inventories.get_inventories(), [])
        self.inventories.add_inventory(test_location)
        inventories = self.inventories.get_inventories().copy()
        self.inventories.remove_inventory(1)
        self.assertEqual(inventories, [])
        self.assertEqual(self.inventories.get_inventories(), [])
    
    def test_add_duplicate_inventory(self) -> None:
        self.inventories.add_inventory(self.inventory_1)
        self.assertEqual(self.inventories.get_inventories(), [self.inventory_1])
        self.inventories.add_inventory(self.inventory_1)
        inventories = self.inventories.get_inventories().copy()
        self.inventories.remove_inventory(1)
        self.inventories.remove_inventory(1)
        self.assertEqual(inventories, [self.inventory_1])
        self.assertEqual(self.inventories.get_inventories(), [])

if __name__ == '__main__':
    unittest.main()