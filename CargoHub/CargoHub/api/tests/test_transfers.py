import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_transfers(unittest.TestCase):
    def setUp(self) -> None:
        self.transfers = data_provider.Transfers("", True)
        self.test_transfer = {
        "id": 2,
        "reference": "TR00002",
        "transfer_from": 9229,
        "transfer_to": 9284,
        "transfer_status": "Completed",
        "created_at": "2017-09-19T00:33:14Z",
        "updated_at": "2017-09-20T01:33:14Z",
        "items": [
            {
                "item_id": "P007435",
                "amount": 23
            }
        ]
        }
        
    def test_get_transfers(self) -> None:
        self.assertEqual(self.transfers.get_transfers(), [])
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_transfers(), [self.test_transfer])
        self.transfers.remove_transfer(2)
        self.assertEqual(self.transfers.get_transfers(), [])
    
    def test_get_transfer(self) -> None:
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_transfer(2), self.test_transfer)
        self.assertEqual(self.transfers.get_transfer(1), None)
        self.transfers.remove_transfer(2)
        self.assertEqual(self.transfers.get_transfer(2), None)
    
    def test_get_items_in_transfer(self) -> None:
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_items_in_transfer(2)[0]["item_id"], self.test_transfer["items"][0]["item_id"])
        self.assertEqual(self.transfers.get_items_in_transfer(2)[0]["amount"], self.test_transfer["items"][0]["amount"])
        self.assertEqual(len(self.transfers.get_items_in_transfer(2)), 1)
        self.transfers.remove_transfer(2)
        self.assertEqual(self.transfers.get_transfers(), [])
    
    def test_add_transfer_good(self) -> None:
        self.assertEqual(self.transfers.get_transfers(), [])
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_transfers(), [self.test_transfer])
        self.assertEqual(self.transfers.get_transfer(2), self.test_transfer)
        self.transfers.remove_transfer(2)
        self.assertEqual(self.transfers.get_transfers(), [])
    
    def test_add_transfer_bad(self) -> None:
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
        self.assertEqual(self.transfers.get_transfers(), [])
        self.transfers.add_transfer(test_client)
        transfers = self.transfers.get_transfers().copy()
        self.transfers.remove_transfer(1)
        self.assertEqual(transfers, [])
        self.assertEqual(self.transfers.get_transfers(), [])
        
    def test_add_transfer_with_duplicate_id(self) -> None:
        test_transfer = {
        "id": 2,
        "reference": "TR00007",
        "transfer_from": 9252,
        "transfer_to": 9322,
        "transfer_status": "Completed",
        "created_at": "2000-10-23T01:52:14Z",
        "updated_at": "2000-10-24T02:52:14Z",
        "items": [
            {
                "item_id": "P002084",
                "amount": 33
            }
        ]
        }
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_transfers(), [self.test_transfer])
        self.transfers.add_transfer(test_transfer)
        transfers = self.transfers.get_transfers().copy()
        self.transfers.remove_transfer(2)
        self.transfers.remove_transfer(2)
        self.assertEqual(transfers, [self.test_transfer])
        self.assertEqual(self.transfers.get_transfers(), [])
    
    def test_update_transfer(self) -> None:
        test_updated_transfer = {
        "id": 2,
        "reference": "TR00007",
        "transfer_from": 9252,
        "transfer_to": 9322,
        "transfer_status": "Completed",
        "created_at": "2000-10-23T01:52:14Z",
        "updated_at": "2000-10-24T02:52:14Z",
        "items": [
            {
                "item_id": "P002084",
                "amount": 33
            }
        ]
        }
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_transfer(2), self.test_transfer)
        self.assertEqual(self.transfers.get_transfers(), [self.test_transfer])
        self.transfers.update_transfer(2, test_updated_transfer)
        self.assertEqual(self.transfers.get_transfer(2), test_updated_transfer)
        self.assertNotEqual(self.test_transfer, test_updated_transfer)
        self.transfers.remove_transfer(2)
    
    def test_update_transfer_with_duplicate_id(self) -> None:
        test_transfer = {
        "id": 7,
        "reference": "TR00007",
        "transfer_from": 9252,
        "transfer_to": 9322,
        "transfer_status": "Completed",
        "created_at": "2000-10-23T01:52:14Z",
        "updated_at": "2000-10-24T02:52:14Z",
        "items": [
            {
                "item_id": "P002084",
                "amount": 33
            }
        ]
        }
        test_updated_transfer = {
        "id": 2,
        "reference": "TR00007",
        "transfer_from": 9252,
        "transfer_to": 9322,
        "transfer_status": "Completed",
        "created_at": "2000-10-23T01:52:14Z",
        "updated_at": "2000-10-24T02:52:14Z",
        "items": [
            {
                "item_id": "P002084",
                "amount": 33
            }
        ]
        }
        self.transfers.add_transfer(self.test_transfer)
        self.assertEqual(self.transfers.get_transfers(), [self.test_transfer])
        self.transfers.add_transfer(test_transfer)
        self.assertEqual(self.transfers.get_transfers(), [self.test_transfer, test_transfer])
        self.transfers.update_transfer(7, test_updated_transfer)
        transfers = self.transfers.get_transfers().copy()
        self.transfers.remove_transfer(2)
        self.transfers.remove_transfer(2)
        self.assertEqual(transfers, [self.test_transfer, test_transfer])
        self.assertEqual(self.transfers.get_transfers(), [])

if __name__ == '__main__':
    unittest.main()