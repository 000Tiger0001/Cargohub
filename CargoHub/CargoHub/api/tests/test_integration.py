import os
import sys
import unittest
import copy
# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_clients(unittest.TestCase):
    def setUp(self) -> None:
        self.inventory = {
        "id": 1,
        "item_id": None,
        "description": "Face-to-face clear-thinking complexity",
        "item_reference": "sjQ23408K",
        "locations": [
        ],
        "total_on_hand": 262,
        "total_expected": 0,
        "total_ordered": 80,
        "total_allocated": 41,
        "total_available": 141,
        "created_at": "2015-02-19 16:08:24",
        "updated_at": "2015-09-26 06:37:56"
        }
        self.inventories = data_provider.Inventories("", True)
        self.item_groups = data_provider.ItemGroups("", True)
        self.item_group = {
        "id": 1,
        "name": "Furniture",
        "description": "",
        "created_at": "2019-09-22 15:51:07",
        "updated_at": "2022-05-18 13:49:28"
        }
        self.warehouses = data_provider.Warehouses("", True)
        self.warehouse = {
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
        self.item_lines = data_provider.ItemLines("", True)
        self.item_line = {
        "id": 1,
        "name": "Home Appliances",
        "description": "",
        "created_at": "1979-01-16 07:07:50",
        "updated_at": "2024-01-05 23:53:25"
        }
        self.shipments = data_provider.Shipments("", True)
        self.shipment1 = {
            "id" : 1,
            "order_id" : None,
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
            "items": []
        }
        self.suppliers = data_provider.Suppliers("", True)
        self.supplier = {
        "id": 1,
        "code": "SUP0001",
        "name": "Lee, Parks and Johnson",
        "address": "5989 Sullivan Drives",
        "address_extra": "Apt. 996",
        "city": "Port Anitaburgh",
        "zip_code": "91688",
        "province": "Illinois",
        "country": "Czech Republic",
        "contact_name": "Toni Barnett",
        "phonenumber": "363.541.7282x36825",
        "reference": "LPaJ-SUP0001",
        "created_at": "1971-10-20 18:06:17",
        "updated_at": "1985-06-08 00:13:46"
        }
        self.items = data_provider.Items("", True)
        self.item = {
            "uid": 1,
            "code": "W",
            "description": "Face-to-face clear-thinking complexity",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": None,
            "item_group": None,
            "item_type": None,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": None,
            "supplier_code": None,
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        }
        self.orders = data_provider.Orders("", True)
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
        "warehouse_id": None,
        "ship_to": 1,
        "bill_to": 2,
        "shipment_id": None,
        "total_amount": 9905.13,
        "total_discount": 150.77,
        "total_tax": 372.72,
        "total_surcharge": 77.6,
        "created_at": "2019-04-03T11:33:15Z",
        "updated_at": "2019-04-05T07:33:15Z",
        "items": []
        }
        self.items_order = [{
                "item_id": 1,
                "amount": 23
            }]
        self.item_types = data_provider.ItemTypes("", True)
        self.item_type = {
        "id": 1,
        "name": "Desktop",
        "description": "",
        "created_at": "1993-07-28 13:43:32",
        "updated_at": "2022-05-12 08:54:35"
        }
        self.transfers = data_provider.Transfers("", True)
        self.transfer = {
        "id": 1,
        "reference": "TR00002",
        "transfer_from": 9229,
        "transfer_to": 9284,
        "transfer_status": "Completed",
        "created_at": "2017-09-19T00:33:14Z",
        "updated_at": "2017-09-20T01:33:14Z",
        "items": [
        ]
        }
        
        self.locations = data_provider.Locations("", True)
        self.location = {
                            "id": 1,
                            "warehouse_id": None,
                            "code": "A.1.0",
                            "name": "Row: A, Rack: 1, Shelf: 0",
                            "created_at": "1992-05-15 03:21:32",
                            "updated_at": "1992-05-15 03:21:32"
                                            }
        
        self.clients = data_provider.Clients("", True)
        self.client = {
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

    def test_delete_item_group(self) -> None:
        self.item_groups.add_item_group(self.item_group)
        self.item["item_group"] = 1
        self.items.add_item(self.item)
        self.item_groups.remove_item_group(1)
        self.assertEqual(self.items.get_item(1)["item_group"], None)

    def test_delete_item_line(self) -> None:
        self.item_lines.add_item_line(self.item_line)
        self.item["item_line"] = 1
        self.items.add_item(self.item)
        self.item_lines.remove_item_line(1)
        self.assertEqual(self.items.get_item(1)["item_line"], None)
    
    def test_delete_item_type(self) -> None:
        self.item_types.add_item_type(self.item_type)
        self.item["item_type"] = 1
        self.items.add_item(self.item)
        self.item_types.remove_item_type(1)
        self.assertEqual(self.items.get_item(1)["item_type"], None)

    def test_delete_item_1(self) -> None:
        self.items.add_item(self.item)
        self.shipment1["items"] = self.items_order
        self.shipments.add_shipment(self.shipment1)
        self.items.remove_item(1)
        self.assertNotEqual(self.items.get_item(1), None)

    def test_delete_item_2(self) -> None:
        self.items.add_item(self.item)
        self.order1["items"] = self.items_order
        self.orders.add_order(self.order1)
        self.items.remove_item(1)
        self.assertNotEqual(self.items.get_item(1), None)

    def test_delete_item_3(self) -> None:
        self.items.add_item(self.item)
        self.transfer["items"] = self.items_order
        self.transfers.add_transfer(self.transfer)
        self.items.remove_item(1)
        self.assertNotEqual(self.items.get_item(1), None)

    def test_delete_item_4(self) -> None:
        self.items.add_item(self.item)
        self.items.remove_item(1)
        self.assertEqual(self.items.get_item(1), None)

    def test_delete_warehouse(self) -> None:
        self.warehouses.add_warehouse(self.warehouse)
        self.order1["warehouse_id"] = 1
        self.location["warehouse_id"] = 1
        self.orders.add_order(self.order1)
        self.locations.add_location(self.location)
        self.warehouses.remove_warehouse(1)
        self.assertEqual(self.orders.get_order(1)["warehouse_id"], None)
        self.assertEqual(self.locations.get_location(1)["warehouse_id"], None)
    
    def test_id_update(self) -> None:
        self.warehouses.add_warehouse(self.warehouse)
        self.locations.add_location(self.location)
        self.orders.add_order(self.order1)
        self.item_groups.add_item_group(self.item_group)
        self.item_lines.add_item_line(self.item_line)
        self.item_types.add_item_type(self.item_type)
        self.shipments.add_shipment(self.shipment1)
        self.items.add_item(self.item)
        self.transfers.add_transfer(self.transfer)
        self.inventories.add_inventory(self.inventory)
        self.clients.add_client(self.client)
        self.suppliers.add_supplier(self.supplier)
        self.warehouses.update_warehouse(1, {**copy.deepcopy(self.warehouse), "id" : 2})
        self.locations.update_location(1, {**copy.deepcopy(self.location), "id" : 2})
        self.orders.update_order(1, {**copy.deepcopy(self.order1), "id" : 2})
        self.item_groups.update_item_group(1, {**copy.deepcopy(self.item_group), "id" : 2})
        self.item_lines.update_item_line(1, {**copy.deepcopy(self.item_line), "id" : 2})
        self.item_types.update_item_type(1, {**copy.deepcopy(self.item_type), "id" : 2})
        self.shipments.update_shipment(1, {**copy.deepcopy(self.shipment1), "id" : 2})
        self.items.update_item(1, {**copy.deepcopy(self.order1), "uid" : 2})
        self.transfers.update_transfer(1, {**copy.deepcopy(self.transfer), "id" : 2})
        self.inventories.update_inventory(1, {**copy.deepcopy(self.warehouse), "id" : 2})
        self.clients.update_client(1, {**copy.deepcopy(self.client), "id" : 2})
        self.suppliers.update_supplier(1, {**copy.deepcopy(self.supplier), "id" : 2})
        self.assertNotEqual(self.warehouses.get_warehouse(1), None)
        self.assertNotEqual(self.locations.get_location(1), None)
        self.assertNotEqual(self.orders.get_order(1), None)
        self.assertNotEqual(self.item_groups.get_item_group(1), None)
        self.assertNotEqual(self.item_lines.get_item_line(1), None)
        self.assertNotEqual(self.item_types.get_item_type(1), None)
        self.assertNotEqual(self.shipments.get_shipment(1), None)
        self.assertNotEqual(self.items.get_item(1), None)
        self.assertNotEqual(self.transfers.get_transfer(1), None)
        self.assertNotEqual(self.inventories.get_inventory(1), None)
        self.assertNotEqual(self.clients.get_client(1), None)
        self.assertNotEqual(self.suppliers.get_supplier(1), None)

    def test_item_add_without_item_line(self) -> None:
        self.item["item_line"] = 1
        self.items.add_item(self.item)
        self.assertEqual(self.items.get_item(1), None)
    def test_item_add_without_item_type(self) -> None:
        self.item["item_type"] = 1
        self.items.add_item(self.item)
        self.assertEqual(self.items.get_item(1), None)
    def test_item_add_without_item_group(self) -> None:
        self.item["item_group"] = 1
        self.items.add_item(self.item)
        self.assertEqual(self.items.get_item(1), None)
    def test_order_without_item(self) -> None:
        self.order1["items"] = self.items_order
        self.orders.add_order(self.order1)
        self.assertEqual(self.orders.get_order(1), None)
    def test_shipment_without_item(self) -> None:
        self.shipment1["items"] = self.items_order
        self.shipments.add_shipment(self.shipment1)
        self.assertEqual(self.shipments.get_shipment(1, None))
    def test_transfer_without_item(self) -> None:
        self.transfer["items"] = self.items_order
        self.transfers.add_transfer(self.transfer)
        self.assertEqual(self.transfers.get_transfer(1), None)
    def test_shipment_without_order(self) -> None:
        self.shipment1["order_id"] = 1
        self.shipments.add_shipment(self.shipment1)
        self.assertEqual(self.shipments.get_shipment(1), None)
    def test_order_without_shipment(self) -> None:
        self.order1["shipment_id"] = 1
        self.orders.add_order(self.shipment1)
        self.assertEqual(self.orders.get_order(1), None)
    def test_location_without_warehouse(self) -> None:
        self.location["warehouse_id"] = 1
        self.locations.add_location(self.location)
        self.assertEqual(self.locations.get_location(1), None)
    def test_item_without_supplier(self) -> None:
        self.item["supplier_id"] = 1
        self.items.add_item(self.item)
        self.assertEqual(self.items.get_item(1), None)
    def test_delete_supplier(self) -> None:
        self.suppliers.add_supplier(self.supplier)
        self.item["supplier_id"] = 1
        self.item["supplier_code"] = "W"
        self.items.add_item(self.item)
        self.suppliers.remove_supplier(1)
        self.assertEqual(self.items.get_item(1)["supplier_id"], None)
        self.assertEqual(self.items.get_item(1)["supplier_code"], None)


        
        
        
        
        
        
        
        

if __name__ == '__main__':
    unittest.main()