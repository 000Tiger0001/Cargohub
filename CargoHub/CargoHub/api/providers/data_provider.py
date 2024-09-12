from models.warehouses import Warehouses
from models.locations import Locations
from models.transfers import Transfers
from models.items import Items
from models.item_lines import ItemLines
from models.item_groups import ItemGroups
from models.item_types import ItemTypes
from models.inventories import Inventories
from models.suppliers import Suppliers
from models.orders import Orders
from models.clients import Clients
from models.shipments import Shipments
import os

DEBUG = False

# was eerst: ROOT_PATH = "./data/"
ROOT_PATH = os.path.abspath("CargoHub/CargoHub/data/") + "/"

_warehouses = None
_locations = None
_transfers = None
_items = None
_item_lines = None
_item_groups = None
_item_types = None
_inventories = None
_suppliers = None
_orders = None
_shipments = None
_clients = None


def init() -> None:
    global _warehouses
    _warehouses = Warehouses(ROOT_PATH, DEBUG)
    global _locations
    _locations = Locations(ROOT_PATH, DEBUG)
    global _transfers
    _transfers = Transfers(ROOT_PATH, DEBUG)
    global _items
    _items = Items(ROOT_PATH, DEBUG)
    global _item_lines
    _item_lines = ItemLines(ROOT_PATH, DEBUG)
    global _item_groups
    _item_groups = ItemGroups(ROOT_PATH, DEBUG)
    global _item_types
    _item_types = ItemTypes(ROOT_PATH, DEBUG)
    global _inventories
    _inventories = Inventories(ROOT_PATH, DEBUG)
    global _suppliers
    _suppliers = Suppliers(ROOT_PATH, DEBUG)
    global _orders
    _orders = Orders(ROOT_PATH, DEBUG)
    global _clients
    _clients = Clients(ROOT_PATH, DEBUG)
    global _shipments
    _shipments = Shipments(ROOT_PATH, DEBUG)


def fetch_warehouse_pool() -> Warehouses:
    return _warehouses


def fetch_location_pool() -> Locations:
    return _locations


def fetch_transfer_pool() -> Transfers:
    return _transfers


def fetch_item_pool() -> Items:
    return _items


def fetch_item_line_pool() -> ItemLines:
    return _item_lines


def fetch_item_group_pool() -> ItemGroups:
    return _item_groups


def fetch_item_type_pool() -> ItemTypes:
    return _item_types


def fetch_inventory_pool() -> Inventories:
    return _inventories


def fetch_supplier_pool() -> Suppliers:
    return _suppliers


def fetch_order_pool() -> Orders:
    return _orders


def fetch_client_pool() -> Clients:
    return _clients


def fetch_shipment_pool() -> Shipments:
    return _shipments
