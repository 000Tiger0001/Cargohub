import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_items(unittest.TestCase):
    def setUp(self) -> None:
        self.items = data_provider.Items("", True)
        self.item_1 = {
            "uid": "P000001",
            "code": "sjQ23408K",
            "description": "Face-to-face clear-thinking complexity",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 11,
            "item_type": 11,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 11,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        }
        self.item_2 = {
            "uid": "P000002",
            "code": "nyg48736S",
            "description": "Focused transitional alliance",
            "short_description": "may",
            "upc_code": "9733132830047",
            "model_number": "ck-109684-VFb",
            "commodity_code": "y-20588-owy",
            "item_line": 11,
            "item_group": 11,
            "item_type": 11,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 15,
            "pack_order_quantity": 23,
            "supplier_id": 11,
            "supplier_code": "SUP312",
            "supplier_part_number": "j-10730-ESk",
            "created_at": "2020-05-31 16:00:08",
            "updated_at": "2020-11-08 12:49:21"
        }
        self.item_3 = {
            "uid": "P000003",
            "code": "QVm03739H",
            "description": "Cloned actuating artificial intelligence",
            "short_description": "we",
            "upc_code": "3722576017240",
            "model_number": "aHx-68Q4",
            "commodity_code": "t-541-F0g",
            "item_line": 12,
            "item_group": 12,
            "item_type": 12,
            "unit_purchase_quantity": 30,
            "unit_order_quantity": 17,
            "pack_order_quantity": 11,
            "supplier_id": 12,
            "supplier_code": "SUP237",
            "supplier_part_number": "r-920-z2C",
            "created_at": "1994-06-02 06:38:40",
            "updated_at": "1999-10-13 01:10:32"
        }
        self.item_4 = {
            "uid": "P000004",
            "code": "zdN19039A",
            "description": "Pre-emptive asynchronous throughput",
            "short_description": "take",
            "upc_code": "9668154959486",
            "model_number": "pZ-7816",
            "commodity_code": "IFq-47R1",
            "item_line": 12,
            "item_group": 12,
            "item_type": 12,
            "unit_purchase_quantity": 21,
            "unit_order_quantity": 20,
            "pack_order_quantity": 20,
            "supplier_id": 12,
            "supplier_code": "SUP140",
            "supplier_part_number": "T-210-I4M",
            "created_at": "2005-08-23 00:48:17",
            "updated_at": "2017-04-29 15:25:25"
        }

    def test_get_items(self) -> None:
        self.assertEqual(self.items.get_items(), [])
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.remove_item("P000001")
        self.assertEqual(self.items.get_items(), [])
    
    def test_get_item(self) -> None:
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_item("P000001"), self.item_1)
        self.assertEqual(self.items.get_item("P000000"), None)
        self.items.remove_item("P000001")
        self.assertEqual(self.items.get_item("P000001"), None)
    
    def test_get_items_for_item_line(self) -> None:
        self.items.add_item(self.item_1)
        self.items.add_item(self.item_2)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_line(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_line(10), [])
        self.assertEqual(self.items.get_items_for_item_line(12), [])
        self.items.add_item(self.item_3)
        self.items.add_item(self.item_4)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2, self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_item_line(12), [self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_item_line(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_line(13), [])
        self.items.remove_item("P000001")
        self.items.remove_item("P000002")
        self.items.remove_item("P000003")
        self.items.remove_item("P000004")
        self.assertEqual(self.items.get_items(), [])
    
    def test_get_items_for_item_group(self) -> None:
        self.items.add_item(self.item_1)
        self.items.add_item(self.item_2)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_group(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_group(10), [])
        self.assertEqual(self.items.get_items_for_item_group(12), [])
        self.items.add_item(self.item_3)
        self.items.add_item(self.item_4)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2, self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_item_group(12), [self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_item_group(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_group(13), [])
        self.items.remove_item("P000001")
        self.items.remove_item("P000002")
        self.items.remove_item("P000003")
        self.items.remove_item("P000004")
        self.assertEqual(self.items.get_items(), [])
    
    def test_get_items_for_item_type(self) -> None:
        self.items.add_item(self.item_1)
        self.items.add_item(self.item_2)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_type(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_type(10), [])
        self.assertEqual(self.items.get_items_for_item_type(12), [])
        self.items.add_item(self.item_3)
        self.items.add_item(self.item_4)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2, self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_item_type(12), [self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_item_type(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_item_type(13), [])
        self.items.remove_item("P000001")
        self.items.remove_item("P000002")
        self.items.remove_item("P000003")
        self.items.remove_item("P000004")
        self.assertEqual(self.items.get_items(), [])

    def test_get_items_for_supplier(self) -> None:
        self.items.add_item(self.item_1)
        self.items.add_item(self.item_2)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_supplier(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_supplier(10), [])
        self.assertEqual(self.items.get_items_for_supplier(12), [])
        self.items.add_item(self.item_3)
        self.items.add_item(self.item_4)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2, self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_supplier(12), [self.item_3, self.item_4])
        self.assertEqual(self.items.get_items_for_supplier(11), [self.item_1, self.item_2])
        self.assertEqual(self.items.get_items_for_supplier(13), [])
        self.items.remove_item("P000001")
        self.items.remove_item("P000002")
        self.items.remove_item("P000003")
        self.items.remove_item("P000004")
        self.assertEqual(self.items.get_items(), [])

    def test_add_item_good(self) -> None:
        self.assertEqual(self.items.get_items(), [])
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.remove_item("P000001")
        self.assertEqual(self.items.get_items(), [])
    
    def test_add_item_wrong(self) -> None:
        test_location = {
            "uid": 1,
            "warehouse_id": 1,
            "code": "A.1.0",
            "name": "Row: A, Rack: 1, Shelf: 0",
            "created_at": "1992-05-15 03:21:32",
            "updated_at": "1992-05-15 03:21:32"
        }
        self.assertEqual(self.items.get_items(), [])
        self.items.add_item(test_location)
        items = self.items.get_items().copy()
        self.items.remove_item(1)
        self.assertEqual(items, [])
        self.assertEqual(self.items.get_items(), [])
        
    def test_add_item_duplicate(self) -> None:
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.add_item(self.item_1)
        items = self.items.get_items().copy()
        self.items.remove_item("P000001")
        self.items.remove_item("P000001")
        self.assertEqual(items, [self.item_1])
        self.assertEqual(self.items.get_items(), [])
    
    def test_add_item_with_duplicate_id(self) -> None:
        test_item = {
            "uid": "P000001",
            "code": "mHo61152n",
            "description": "Stand-alone 24hour emulation",
            "short_description": "there",
            "upc_code": "0943113854446",
            "model_number": "j-587-L3H",
            "commodity_code": "67-vxkaB7P",
            "item_line": 16,
            "item_group": 50,
            "item_type": 28,
            "unit_purchase_quantity": 44,
            "unit_order_quantity": 2,
            "pack_order_quantity": 20,
            "supplier_id": 35,
            "supplier_code": "SUP347",
            "supplier_part_number": "NzG-36a1",
            "created_at": "2016-03-28 10:35:32",
            "updated_at": "2024-05-20 22:42:05"
        }
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.add_item(test_item)
        items = self.items.get_items().copy()
        self.items.remove_item("P000001")
        self.items.remove_item("P000001")
        self.assertEqual(items, [self.item_1])
        self.assertEqual(self.items.get_items(), [])
    
    def test_update_item(self) -> None:
        test_item = {
            "uid": "P000001",
            "code": "mHo61152n",
            "description": "Stand-alone 24hour emulation",
            "short_description": "there",
            "upc_code": "0943113854446",
            "model_number": "j-587-L3H",
            "commodity_code": "67-vxkaB7P",
            "item_line": 16,
            "item_group": 50,
            "item_type": 28,
            "unit_purchase_quantity": 44,
            "unit_order_quantity": 2,
            "pack_order_quantity": 20,
            "supplier_id": 35,
            "supplier_code": "SUP347",
            "supplier_part_number": "NzG-36a1",
            "created_at": "2016-03-28 10:35:32",
            "updated_at": "2024-05-20 22:42:05"
        }
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.update_item("P000001", test_item)
        self.assertEqual(self.items.get_items(), [test_item])
        self.assertNotEqual(self.items.get_items(), [self.item_1])
        self.items.remove_item("P000001")
    
    def test_update_item_duplicate_id(self) -> None:
        test_item = {
            "uid": "P000001",
            "code": "nyg48736S",
            "description": "Focused transitional alliance",
            "short_description": "may",
            "upc_code": "9733132830047",
            "model_number": "ck-109684-VFb",
            "commodity_code": "y-20588-owy",
            "item_line": 11,
            "item_group": 11,
            "item_type": 11,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 15,
            "pack_order_quantity": 23,
            "supplier_id": 11,
            "supplier_code": "SUP312",
            "supplier_part_number": "j-10730-ESk",
            "created_at": "2020-05-31 16:00:08",
            "updated_at": "2020-11-08 12:49:21"
        }
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.add_item(self.item_2)
        self.assertEqual(self.items.get_items(), [self.item_1, self.item_2])
        self.items.update_item("P000001", test_item)
        items = self.items.get_items().copy()
        self.items.remove_item("P000001")
        self.items.remove_item("P000001")
        self.assertEqual(items, [self.item_1, self.item_2])
    
    def test_remove_item(self) -> None:
        self.items.add_item(self.item_1)
        self.assertEqual(self.items.get_items(), [self.item_1])
        self.items.remove_item("P000001")
        self.assertEqual(self.items.get_items(), [])

if __name__ == '__main__':
    unittest.main()