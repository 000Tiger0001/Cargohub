import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_shipments(unittest.TestCase):
    def setUp(self) -> None:
        self.items = [
                {
                "item_id" : 1,
                "amount" : 5
                },
                {
                    "item_id" : 2,
                    "amount" : 10
                }
            ]
        self.items2 = [
            {
                "item_id" : 1,
                "amount" : 8
            },
            {
                "item_id" : 3,
                "amount" : 5
            },
            {
                "item_id" : 2,
                "amount" : 8
            }
        ]
        self.shipments = data_provider.Shipments("", True)
        self.inventories = data_provider.Inventories("", True)
        self.shipment1 = {
            "id" : 1,
            "order_id" : 2,
            "source_id" : 5,
            "order_date": "2000-03-09", 
            "request_date": "2000-03-11",
            "shipment_date": "2000-03-13",
            "shipment_type" : "I",
            "shipment_status": "Pending", 
            "notes": "",
            "carrier_code" : "ADP",
            "carrier_description": "",
            "service_code": "Fastest",
            "payment_type": "Manual", 
            "transfer_mode": "Ground", 
            "total_package_count": 31, 
            "total_package_weight": 594.42, 
            "created_at": "2000-03-10T11:11:14Z", 
            "updated_at": "2000-03-11T13:11:14Z", 
            "items": self.items
        }
        self.shipment2 = {
            "id" : 2,
            "order_id" : 8,
            "source_id" : 5,
            "order_date": "2000-03-09", 
            "request_date": "2000-03-11",
            "shipment_date": "2000-03-13",
            "shipment_type" : "I",
            "shipment_status": "Pending", 
            "notes": "",
            "carrier_code" : "ADP",
            "carrier_description": "",
            "service_code": "Cool",
            "payment_type": "Manual", 
            "transfer_mode": "Ground", 
            "total_package_count": 40, 
            "total_package_weight": 600.82, 
            "created_at": "2000-03-10T11:11:14Z", 
            "updated_at": "2000-03-11T13:11:14Z", 
            "items": [
            ]
        }
        self.shipment3 = {
            "id" : 3,
            "order_id" : 8,
            "source_id" : 5,
            "order_date": "2000-03-09", 
            "request_date": "2000-03-11",
            "shipment_date": "2000-03-16",
            "shipment_type" : "I",
            "shipment_status": "Pending", 
            "notes": "",
            "carrier_code" : "ADP",
            "carrier_description": "",
            "service_code": "Cool",
            "payment_type": "Manual", 
            "transfer_mode": "Ground", 
            "total_package_count": 80, 
            "total_package_weight": 600.82, 
            "created_at": "2000-03-10T11:11:14Z", 
            "updated_at": "2000-03-11T13:11:14Z", 
            "items": [
            ]
        }

        self.inventory1 = {
        "id": 1,
        "item_id": "1",
        "description": "Inverse background open system",
        "item_reference": "jCu52502W",
        "locations": [
            16796,
            9019,
            21922,
            18108,
            11232,
            34474,
            34482,
            27996,
            28117,
            19616,
            19465
        ],
        "total_on_hand": 80,
        "total_expected": 2,
        "total_ordered": 40,
        "total_allocated": 22,
        "total_available": 20,
        "created_at": "1979-04-30 04:42:28",
        "updated_at": "2022-03-14 21:19:09"
        }

        self.inventory2 = {
        "id": 2,
        "item_id": "2",
        "description": "Inverse background open system",
        "item_reference": "jCu52502W",
        "locations": [
            16796,
            9019,
            21922,
            18108,
            11232,
            34474,
            34482,
            27996,
            28117,
            19616,
            19465
        ],
        "total_on_hand": 35,
        "total_expected": 10,
        "total_ordered": 5,
        "total_allocated": 6,
        "total_available": 34,
        "created_at": "1979-04-30 04:42:28",
        "updated_at": "2022-03-14 21:19:09"
        }

        self.inventory3 = {
        "id": 3,
        "item_id": "3",
        "description": "Inverse background open system",
        "item_reference": "jCu52502W",
        "locations": [
            16796,
            9019,
            21922,
            18108,
            11232,
            34474,
            34482,
            27996,
            28117,
            19616,
            19465
        ],
        "total_on_hand": 10,
        "total_expected": 25,
        "total_ordered": 1,
        "total_allocated": 2,
        "total_available": 32,
        "created_at": "1979-04-30 04:42:28",
        "updated_at": "2022-03-14 21:19:09"
        }

    def get_update(self, index) -> None:
        return {
            "id" : index,
            "order_id" : 8,
            "source_id" : 5,
            "order_date": "2000-03-09", 
            "request_date": "2000-03-11",
            "shipment_date": "2000-03-16",
            "shipment_type" : "I",
            "shipment_status": "Pending", 
            "notes": "This is a note",
            "carrier_code" : "ADP",
            "carrier_description": "",
            "service_code": "Cool",
            "payment_type": "Manual", 
            "transfer_mode": "Ground", 
            "total_package_count": 80, 
            "total_package_weight": 600.82, 
            "created_at": "2000-03-10T11:11:14Z", 
            "updated_at": "2000-03-11T13:11:14Z", 
            "items": [
            ]
        }
        
    #this test also covers the 'Create' endpoint.
    def test_get_shipments(self) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])
        self.shipments.add_shipment(self.shipment1)
        self.AssertEqual(self.shipments.get_shipments(), [self.shipment1])
        self.shipments.add_shipment(self.shipment2)
        self.assertEqual(self.shipments.get_shipments(), [self.shipment1, self.shipment2])
    
    def test_get_shipment(self) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])
        self.shipments.add_shipment(self.shipment1)
        self.assertEqual(self.shipments.get_shipment(self.shipment1["id"]), self.shipment1)

    def test_get_items_in_shipment(self) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])
        self.shipments.add_shipment(self.shipment1)
        self.assertEqual(self.shipments.get_shipment(self.shipment1["id"])["items"], self.items)

    def test_delete_shipment(self, index, expected_shipments) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])
        self.shipments.add_shipment(self.shipment1)
        self.shipments.add_shipment(self.shipment2)
        self.shipments.add_shipment(self.shipment3)
        self.shipments.remove_shipment(index)
        self.assertEqual(self.shipments.get_shipments(), expected_shipments)

    def test_delete_shipment_1(self) -> None:
        self.test_delete_shipment(1, [self.shipment2, self.shipment3])

    def test_delete_shipment_2(self) -> None:
        self.test_delete_shipment(2, [self.shipment1, self.shipment3])

    def test_delete_shipment_3(self) -> None:
        self.test_delete_shipment(3, [self.shipment1, self.shipment2])

    def test_update_shipment(self, index, index1, index2) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])
        self.shipments.add_shipment(self.shipment1)
        self.shipments.add_shipment(self.shipment2)
        self.shipments.add_shipment(self.shipment3)
        self.shipments.update_shipment(index, self.get_update(index))
        self.assertEqual(self.shipments.get_shipment(index)["notes"], "This is a note")
        self.assertEqual(self.shipments.get_shipment(index1)["notes"], "")
        self.assertEqual(self.shipments.get_shipment(index2)["notes"], "")

    def test_update_shipment_1(self) -> None:
        self.test_delete_shipment(1, 2, 3)

    def test_update_shipment_2(self) -> None:
        self.test_delete_shipment(2,1,3)

    def test_update_shipment_3(self) -> None:
        self.test_delete_shipment(3,1,2)
    
    def test_update_items_in_shipment(self) -> None:
        self.assertEqual(self.shipments.get_shipments(), [])
        self.assertEqual(self.inventories.get_inventories(), [])
        self.inventories.add_inventory(self.inventory1)
        self.inventories.add_inventory(self.inventory2)
        self.inventories.add_inventory(self.inventory3)
        self.shipments.add_shipment(self.shipment1)
        self.shipments.update_items_in_shipment(1, self.items2)
        self.assertEqual(self.shipments.get_items_in_shipment(1), self.items2)
        self.assertEqual(self.inventories.get_inventory(1)["total_ordered"], 37)
        self.assertEqual(self.inventories.get_inventory(1)["total_allocated"], 25)
        self.assertEqual(self.inventories.get_inventory(2)["total_available"], 36)
        self.assertEqual(self.inventories.get_inventory(2)["total_allocated"], 4)
        self.assertEqual(self.inventories.get_inventory(3)["total_ordered"], 1)
        self.assertEqual(self.inventories.get_inventory(3)["total_allocated"], 7)

if __name__ == '__main__':
    unittest.main()