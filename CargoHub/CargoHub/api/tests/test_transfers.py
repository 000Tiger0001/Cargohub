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
        ...

if __name__ == '__main__':
    unittest.main()