import os
import sys
import unittest

# Add the parent directory to the sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))


from providers import auth_provider
from providers import data_provider

class test_suppliers(unittest.TestCase):
    def setUp(self) -> None:
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
    
    def test_get_suppliers(self) -> None:
        self.assertEqual(self.suppliers.get_suppliers(), [])
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_suppliers(), [self.supplier])
        self.suppliers.remove_supplier(1)
        self.assertEqual(self.suppliers.get_suppliers(), [])
        
    def test_get_supplier(self) -> None:
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_supplier(1), self.supplier)
        self.assertEqual(self.suppliers.get_supplier(0), None)
        self.suppliers.remove_supplier(1)
        self.assertEqual(self.suppliers.get_supplier(1), None)
    
    def test_add_supplier_good(self) -> None:
        self.assertEqual(self.suppliers.get_suppliers(), [])
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_suppliers(), [self.supplier])
        self.suppliers.remove_supplier(1)
        self.assertEqual(self.suppliers.get_suppliers(), [])
    
    def test_add_supplier_bad(self) -> None:
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
        self.assertEqual(self.suppliers.get_suppliers(), [])
        self.suppliers.add_supplier(test_client)
        suppliers = self.suppliers.get_suppliers().copy()
        self.suppliers.remove_supplier(1)
        self.assertEqual(suppliers, [])
        self.assertEqual(self.suppliers.get_suppliers(), [])
    
    def test_add_supplier_with_duplicate_model(self) -> None:
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_suppliers(), [self.supplier])
        self.suppliers.add_supplier(self.supplier)
        suppliers = self.suppliers.get_suppliers().copy()
        self.suppliers.remove_supplier(1)
        self.suppliers.remove_supplier(1)
        self.assertEqual(suppliers, [self.supplier])
        self.assertEqual(self.suppliers.get_suppliers(), [])
    
    def test_add_supplier_with_duplicate_id(self) -> None:
        test_supplier = {
        "id": 1,
        "code": "SUP0002",
        "name": "Holden-Quinn",
        "address": "576 Christopher Roads",
        "address_extra": "Suite 072",
        "city": "Amberbury",
        "zip_code": "16105",
        "province": "Illinois",
        "country": "Saint Martin",
        "contact_name": "Kathleen Vincent",
        "phonenumber": "001-733-291-8848x3542",
        "reference": "H-SUP0002",
        "created_at": "1995-12-18 03:05:46",
        "updated_at": "2019-11-10 22:11:12"
        }
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_suppliers(), [self.supplier])
        self.suppliers.add_supplier(test_supplier)
        suppliers = self.suppliers.get_suppliers().copy()
        self.suppliers.remove_supplier(1)
        self.suppliers.remove_supplier(1)
        self.assertEqual(suppliers, [self.supplier])
        self.assertEqual(self.suppliers.get_suppliers(), [])
    
    def test_update_supplier(self) -> None:
        test_supplier = {
        "id": 1,
        "code": "SUP0002",
        "name": "Holden-Quinn",
        "address": "576 Christopher Roads",
        "address_extra": "Suite 072",
        "city": "Amberbury",
        "zip_code": "16105",
        "province": "Illinois",
        "country": "Saint Martin",
        "contact_name": "Kathleen Vincent",
        "phonenumber": "001-733-291-8848x3542",
        "reference": "H-SUP0002",
        "created_at": "1995-12-18 03:05:46",
        "updated_at": "2019-11-10 22:11:12"
        }
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_supplier(1), self.supplier)
        self.suppliers.update_supplier(1, test_supplier)
        self.assertEqual(self.suppliers.get_supplier(1), test_supplier)
        self.assertNotEqual(self.suppliers.get_supplier(1), self.supplier)
        self.suppliers.remove_supplier(1)
    
    def test_update_supplier_to_duplicate_id(self) -> None:
        test_supplier = {
        "id": 2,
        "code": "SUP0002",
        "name": "Holden-Quinn",
        "address": "576 Christopher Roads",
        "address_extra": "Suite 072",
        "city": "Amberbury",
        "zip_code": "16105",
        "province": "Illinois",
        "country": "Saint Martin",
        "contact_name": "Kathleen Vincent",
        "phonenumber": "001-733-291-8848x3542",
        "reference": "H-SUP0002",
        "created_at": "1995-12-18 03:05:46",
        "updated_at": "2019-11-10 22:11:12"
        }
        test_updated_supplier = {
        "id": 1,
        "code": "SUP0002",
        "name": "Holden-Quinn",
        "address": "576 Christopher Roads",
        "address_extra": "Suite 072",
        "city": "Amberbury",
        "zip_code": "16105",
        "province": "Illinois",
        "country": "Saint Martin",
        "contact_name": "Kathleen Vincent",
        "phonenumber": "001-733-291-8848x3542",
        "reference": "H-SUP0002",
        "created_at": "1995-12-18 03:05:46",
        "updated_at": "2019-11-10 22:11:12"
        }
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_suppliers(), [self.supplier])
        self.suppliers.add_supplier(test_supplier)
        self.assertEqual(self.suppliers.get_suppliers(), [self.supplier, test_supplier])
        self.suppliers.update_supplier(2, test_updated_supplier)
        suppliers = self.suppliers.get_suppliers().copy()
        self.suppliers.remove_supplier(1)
        self.suppliers.remove_supplier(1)
        self.assertEqual(suppliers, [self.supplier, test_supplier])
        self.assertEqual(self.suppliers.get_suppliers(), [])
        
    def test_remove_supplier(self) -> None:
        self.suppliers.add_supplier(self.supplier)
        self.assertEqual(self.suppliers.get_supplier(1), self.supplier)
        self.suppliers.remove_supplier(1)
        self.assertEqual(self.suppliers.get_suppliers(), [])

if __name__ == '__main__':
    unittest.main()