import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_orders(unittest.TestCase):
    def setUp(self) -> None:
        self.orders = data_provider.Orders("", True)
        self.inventories = data_provider.Orders("", True)
        self.items = self.items = [
                {
                "item_id" : 1,
                "amount" : 5
                },
                {
                    "item_id" : 2,
                    "amount" : 10
                }
            ]
        self.order1 = {
        "id": 1,
        "source_id": 33,
        "order_date": "2019-04-03T11:33:15Z",
        "request_date": "2019-04-07T11:33:15Z",
        "reference": "ORD00001",
        "reference_extra": "Bedreven arm straffen bureau.",
        "order_status": "",
        "notes": "",
        "shipping_notes": "Buurman betalen plaats bewolkt.",
        "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.",
        "warehouse_id": 18,
        "ship_to": 1,
        "bill_to": 2,
        "shipment_id": 1,
        "total_amount": 9905.13,
        "total_discount": 150.77,
        "total_tax": 372.72,
        "total_surcharge": 77.6,
        "created_at": "2019-04-03T11:33:15Z",
        "updated_at": "2019-04-05T07:33:15Z",
        "items": self.items
        }
        self.order2 = {
        "id": 2,
        "source_id": 33,
        "order_date": "2019-04-03T11:33:15Z",
        "request_date": "2019-04-07T11:33:15Z",
        "reference": "ORD00001",
        "reference_extra": "Bleep Bloop",
        "order_status": "Delivered",
        "notes": "",
        "shipping_notes": "Buurman betalen plaats bewolkt.",
        "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.",
        "warehouse_id": 18,
        "ship_to": 2,
        "bill_to": 1,
        "shipment_id": 2,
        "total_amount": 9905.13,
        "total_discount": 150.77,
        "total_tax": 372.72,
        "total_surcharge": 77.6,
        "created_at": "2019-04-03T11:33:15Z",
        "updated_at": "2019-04-05T07:33:15Z",
        "items": [
        ]
        }
        self.order3 = {
        "id": 3,
        "source_id": 33,
        "order_date": "2019-04-03T11:33:15Z",
        "request_date": "2019-04-07T11:33:15Z",
        "reference": "ORD00001",
        "reference_extra": "Bedreven arm straffen bureau.",
        "order_status": "",
        "notes": "",
        "shipping_notes": "Buurman betalen plaats bewolkt.",
        "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.",
        "warehouse_id": 18,
        "ship_to": 2,
        "bill_to": 2,
        "shipment_id": 1,
        "total_amount": 9905.13,
        "total_discount": 150.77,
        "total_tax": 372.72,
        "total_surcharge": 77.6,
        "created_at": "2019-04-03T11:33:15Z",
        "updated_at": "2019-04-05T07:33:15Z",
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
        self.inventories = data_provider.Inventories("", True)
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

    def get_update(self, index) -> None:
        return {
        "id": index,
        "source_id": 33,
        "order_date": "2019-04-03T11:33:15Z",
        "request_date": "2019-04-07T11:33:15Z",
        "reference": "ORD00001",
        "reference_extra": "Bedreven arm straffen bureau.",
        "order_status": "Delivered",
        "notes": "",
        "shipping_notes": "Buurman betalen plaats bewolkt.",
        "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.",
        "warehouse_id": 18,
        "ship_to": None,
        "bill_to": None,
        "shipment_id": 3,
        "total_amount": 9905.13,
        "total_discount": 150.77,
        "total_tax": 372.72,
        "total_surcharge": 77.6,
        "created_at": "2019-04-03T11:33:15Z",
        "updated_at": "2019-04-05T07:33:15Z",
        "items": [
        ]
        }
    
    def test_get_orders(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.assertEqual(self.orders.get_orders(), [self.order1])
        self.orders.add_order(self.order2)
        self.assertEqual(self.orders.get_orders(), [self.order1, self.order2])

    def test_get_orders_in_shipment(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.orders.add_order(self.order2)
        self.assertEqual(self.orders.get_orders_in_shipment(1), [self.order1])    

    def test_get_orders_for_client(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.orders.add_order(self.order2)
        self.orders.add_order(self.order3)
        self.assertEqual(self.orders.get_orders_for_client(1), [self.order1, self.order2])   

    def test_update_orders_in_shipment(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.orders.add_order(self.order3)
        self.orders.update_orders_in_shipment(1, [self.order1])
        self.assertEqual(self.orders.get_order(1)["order_status"], "Packed") 
        self.assertEqual(self.orders.get_order(2)["order_status"], "Scheduled")    
        self.assertEqual(self.orders.get_order(1)["shipment_id"], 1) 
        self.assertEqual(self.orders.get_order(2)["shipment_id"], -1)    
    
    def test_get_order(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.assertEqual(self.orders.get_order(self.order1["id"]), self.order1)

    def test_get_items_in_shipment(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.assertEqual(self.orders.get_order(self.order1["id"])["items"], self.items)

    def test_delete_order(self, index, expected_orders) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.orders.add_order(self.order2)
        self.orders.add_order(self.order3)
        self.orders.remove_order(index)
        self.assertEqual(self.orders.get_orders(), expected_orders)

    def test_delete_shipment_1(self) -> None:
        self.test_delete_order(1, [self.order2, self.order3])

    def test_delete_order_2(self) -> None:
        self.test_delete_order(2, [self.order1, self.order3])

    def test_delete_order_3(self) -> None:
        self.test_delete_order(3, [self.order1, self.order2])

    def test_update_order(self, index, index1, index2) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.orders.add_order(self.order1)
        self.orders.add_order(self.order2)
        self.orders.add_order(self.order3)
        self.orders.update_order(index, self.get_update(index))
        self.assertEqual(self.orders.get_order(index)["notes"], "This is a note")
        self.assertEqual(self.orders.get_order(index1)["notes"], "")
        self.assertEqual(self.orders.get_order(index2)["notes"], "")

    def test_update_order_1(self) -> None:
        self.test_delete_order(1, 2, 3)

    def test_update_order_2(self) -> None:
        self.test_delete_order(2,1,3)

    def test_update_order_3(self) -> None:
        self.test_delete_order(3,1,2)
    
    def test_update_items_in_order(self) -> None:
        self.assertEqual(self.orders.get_orders(), [])
        self.assertEqual(self.inventories.get_inventories(), [])
        self.inventories.add_inventory(self.inventory1)
        self.inventories.add_inventory(self.inventory2)
        self.inventories.add_inventory(self.inventory3)
        self.orders.add_order(self.order1)
        self.orders.update_items_in_order(1, self.items2)
        self.assertEqual(self.orders.get_items_in_order(1), self.items2)
        self.assertEqual(self.inventories.get_inventory(1)["total_on_hand"], 77)
        self.assertEqual(self.inventories.get_inventory(1)["total_ordered"], 43)
        self.assertEqual(self.inventories.get_inventory(3)["total_on_hand"], 5)
        self.assertEqual(self.inventories.get_inventory(3)["total_ordered"], 6)

if __name__ == '__main__':
    unittest.main()