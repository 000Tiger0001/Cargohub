import socketserver
import http.server
import json

from providers import auth_provider
from providers import data_provider

from processors import notification_processor


class ApiRequestHandler(http.server.BaseHTTPRequestHandler):
    def handle_get_version_1(self, path, user) -> None:
        # checks if the user has access to the requested endpoint, so here it checks if the user has access to the get endpoint
        if not auth_provider.has_access(user, path, "get"):
            # if the user does not have access, it returns a 403 (Forbidden)
            self.send_response(403)
            self.end_headers()
            return
        if path[0] == "warehouses":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all warehouses and returns it to the user
                    warehouses = data_provider.fetch_warehouse_pool().get_warehouses()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(warehouses).encode("utf-8"))
                case 2:
                    # fetches a specific warehouse with a specific id and returns it to the user
                    warehouse_id = int(path[1])
                    warehouse = data_provider.fetch_warehouse_pool().get_warehouse(warehouse_id)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(warehouse).encode("utf-8"))
                case 3:
                    # fetches all locations in a specific warehouse and returns it to the user
                    if path[2] == "locations":
                        warehouse_id = int(path[1])
                        locations = data_provider.fetch_location_pool(
                        ).get_locations_in_warehouse(warehouse_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(locations).encode("utf-8"))
                    else:
                        # if the last part of the path is not locations or it can not find the warehouse, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "locations":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all locations and returns it to the user
                    locations = data_provider.fetch_location_pool().get_locations()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(locations).encode("utf-8"))
                case 2:
                    # fetches a specific location with a specific id and returns it to the user
                    location_id = int(path[1])
                    location = data_provider.fetch_location_pool().get_location(location_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(location).encode("utf-8"))
                case _:
                    # if the path length is longer than 2 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "transfers":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all transfers and returns it to the user
                    transfers = data_provider.fetch_transfer_pool().get_transfers()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(transfers).encode("utf-8"))
                case 2:
                    # fetches a specific transfer with a specific id and returns it to the user
                    transfer_id = int(path[1])
                    transfer = data_provider.fetch_transfer_pool().get_transfer(transfer_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(transfer).encode("utf-8"))
                case 3:
                    if path[2] == "items":
                        # fetches all items in a specific transfer and returns it to the user
                        transfer_id = int(path[1])
                        items = data_provider.fetch_transfer_pool().get_items_in_transfer(transfer_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the last part of the path is not items or it can not find the warehouse, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "items":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all items and returns it to the user
                    items = data_provider.fetch_item_pool().get_items()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(items).encode("utf-8"))
                case 2:
                    # fetches a specific item with a specific id (in items cas the "uid") and returns it to the user
                    item_id = path[1]
                    item = data_provider.fetch_item_pool().get_item(item_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item).encode("utf-8"))
                case 3:
                    if path[2] == "inventory":
                        # fetches all inventories with a specific item and returns it to the user
                        item_id = path[1]
                        inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(item_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(
                            inventories).encode("utf-8"))
                    else:
                        # if the third part of the path is not inventory, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case 4:
                    if path[2] == "inventory" and path[3] == "totals":
                        # fetches all inventory totals with a specific item and returns it to the user
                        item_id = path[1]
                        totals = data_provider.fetch_inventory_pool().get_inventory_totals_for_item(item_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(totals).encode("utf-8"))
                    else:
                        # if the third part of the path is not inventory and the fourth part is not totals, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 4 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "item_lines":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all item lines and returns it to the user
                    item_lines = data_provider.fetch_item_line_pool().get_item_lines()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item_lines).encode("utf-8"))
                case 2:
                    # fetches a specific item line with a specific id and returns it to the user
                    item_line_id = int(path[1])
                    item_line = data_provider.fetch_item_line_pool().get_item_line(item_line_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item_line).encode("utf-8"))
                case 3:
                    if path[2] == "items":
                        # fetches all items in a specific item line and returns it to the user
                        item_line_id = int(path[1])
                        items = data_provider.fetch_item_pool().get_items_for_item_line(item_line_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the third part of the path is not items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "item_groups":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all item groups and returns it to the user
                    item_groups = data_provider.fetch_item_group_pool().get_item_groups()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item_groups).encode("utf-8"))
                case 2:
                    # fetches a specific item group with a specific id and returns it to the user
                    item_group_id = int(path[1])
                    item_group = data_provider.fetch_item_group_pool().get_item_group(item_group_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item_group).encode("utf-8"))
                case 3:
                    if path[2] == "items":
                        # fetches all items in a specific item group and returns it to the user
                        item_group_id = int(path[1])
                        items = data_provider.fetch_item_pool().get_items_for_item_group(item_group_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the third part of the path is not items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "item_types":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all item types and returns it to the user
                    item_types = data_provider.fetch_item_type_pool().get_item_types()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item_types).encode("utf-8"))
                case 2:
                    # fetches a specific item type with a specific id and returns it to the user
                    item_type_id = int(path[1])
                    item_type = data_provider.fetch_item_type_pool().get_item_type(item_type_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(item_type).encode("utf-8"))
                case 3:
                    if path[2] == "items":
                        # fetches all items in a specific item type and returns it to the user
                        item_type_id = int(path[1])
                        items = data_provider.fetch_item_pool().get_items_for_item_type(item_type_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the third part of the path is not items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "inventories":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all inventories and returns it to the user
                    inventories = data_provider.fetch_inventory_pool().get_inventories()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(inventories).encode("utf-8"))
                case 2:
                    # fetches a specific inventory with a specific id and returns it to the user
                    inventory_id = int(path[1])
                    inventory = data_provider.fetch_inventory_pool().get_inventory(inventory_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(inventory).encode("utf-8"))
                case _:
                    # if the path length is longer than 2 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "suppliers":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all suppliers and returns it to the user
                    suppliers = data_provider.fetch_supplier_pool().get_suppliers()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(suppliers).encode("utf-8"))
                case 2:
                    # fetches a specific supplier with a specific id and returns it to the user
                    supplier_id = int(path[1])
                    supplier = data_provider.fetch_supplier_pool().get_supplier(supplier_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(supplier).encode("utf-8"))
                case 3:
                    if path[2] == "items":
                        # fetches all items from a specific supplier and returns it to the user
                        supplier_id = int(path[1])
                        items = data_provider.fetch_item_pool().get_items_for_supplier(supplier_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the third part of the path is not items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "orders":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all orders and returns it to the user
                    orders = data_provider.fetch_order_pool().get_orders()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    # writes the orders to json file
                    self.wfile.write(json.dumps(orders).encode("utf-8"))
                case 2:
                    # fetches a specific order with a specific id and returns it to the user
                    order_id = int(path[1])
                    order = data_provider.fetch_order_pool().get_order(order_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    # writes the specific order of the id to the json file
                    self.wfile.write(json.dumps(order).encode("utf-8"))
                case 3:
                    if path[2] == "items":
                        # fetches all items in a specific order and returns it to the user
                        order_id = int(path[1])
                        items = data_provider.fetch_order_pool().get_items_in_order(order_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        # writes items from the specific order to the json file
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the third part of the path is not items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "clients":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all clients and returns it to the user
                    clients = data_provider.fetch_client_pool().get_clients()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(clients).encode("utf-8"))
                case 2:
                    # fetches a specific client with a specific id and returns it to the user
                    client_id = int(path[1])
                    client = data_provider.fetch_client_pool().get_client(client_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(client).encode("utf-8"))
                case 3:
                    if path[2] == "orders":
                        # fetches all orders from a specific client and returns it to the user
                        client_id = int(path[1])
                        orders = data_provider.fetch_order_pool().get_orders_for_client(client_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(orders).encode("utf-8"))
                    else:
                        # if the third part of the path is not orders, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "shipments":
            paths = len(path)
            match paths:
                case 1:
                    # fetches all shipments and returns it to the user
                    shipments = data_provider.fetch_shipment_pool().get_shipments()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(shipments).encode("utf-8"))
                case 2:
                    # fetches a specific shipment with a specific id and returns it to the user
                    shipment_id = int(path[1])
                    shipment = data_provider.fetch_shipment_pool().get_shipment(shipment_id)
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.send_header("Content-type", "application/json")
                    self.end_headers()
                    self.wfile.write(json.dumps(shipment).encode("utf-8"))
                case 3:
                    if path[2] == "orders":
                        # fetches all orders in a specific shipment and returns it to the user
                        shipment_id = int(path[1])
                        orders = data_provider.fetch_order_pool().get_orders_in_shipment(shipment_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(orders).encode("utf-8"))
                    elif path[2] == "items":
                        # fetches all items in a specific shipment and returns it to the user
                        shipment_id = int(path[1])
                        items = data_provider.fetch_shipment_pool().get_items_in_shipment(shipment_id)
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.send_header("Content-type", "application/json")
                        self.end_headers()
                        self.wfile.write(json.dumps(items).encode("utf-8"))
                    else:
                        # if the third part of the path is not orders or items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the path length is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        else:
            # if the first part of the path is not warehouses, locations, transfers, items, item_lines, item_groups, item_types, inventories, suppliers, orders, clients, or shipments, it returns a 404 (Page Not Found)
            self.send_response(404)
            self.end_headers()

    def do_GET(self) -> None:
        # gets the api_key from the headers
        api_key = self.headers.get("API_KEY")
        # gets the user from the api_key
        user = auth_provider.get_user(api_key)
        if user == None:
            # if the user is None (user doesn't exist), it returns a 401 (unauthorized)
            self.send_response(401)
            self.end_headers()
        else:
            try:
                # splits the path by /
                path = self.path.split("/")
                # checks if path is longer than 3 and the first part of the path is api and the second part of the path is v1
                if len(path) > 3 and path[1] == "api" and path[2] == "v1":
                    # calls the handle_get_version_1 function with the path and the user
                    # the part of the path passed to the function does not include 'api'and 'v1'
                    self.handle_get_version_1(path[3:], user)
            except Exception:
                # if an exception occurs, it returns a 500 (Internal Server Error)
                self.send_response(500)
                self.end_headers()

    def handle_post_version_1(self, path, user) -> None:
        # checks if the user has access to the requested endpoint, so here it checks if the user has access to the post endpoint
        if not auth_provider.has_access(user, path, "post"):
            # if the user does not have access, it returns a 403 (Forbidden)
            self.send_response(403)
            self.end_headers()
            return
        if path[0] == "warehouses":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_warehouse = json.loads(post_data.decode())
            # adds the new warehouse to the list of all warehouses
            data_provider.fetch_warehouse_pool().add_warehouse(new_warehouse)
            # saves the warehouses to the json file
            data_provider.fetch_warehouse_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "locations":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_location = json.loads(post_data.decode())
            # adds the new location to the list of all locations
            data_provider.fetch_location_pool().add_location(new_location)
            # saves the locations to the json file
            data_provider.fetch_location_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "transfers":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_transfer = json.loads(post_data.decode())
            # adds the new transfer to the list of all transfers
            data_provider.fetch_transfer_pool().add_transfer(new_transfer)
            # saves the transfers to the json file
            data_provider.fetch_transfer_pool().save()
            # sends a notification to the notification processor and eventualy to the user
            notification_processor.push(
                f"Scheduled batch transfer {new_transfer['id']}")
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "items":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_item = json.loads(post_data.decode())
            # adds the new item to the list of all items
            data_provider.fetch_item_pool().add_item(new_item)
            # saves the items to the json file
            data_provider.fetch_item_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "inventories":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_inventory = json.loads(post_data.decode())
            # adds the new inventory to the list of all inventories
            data_provider.fetch_inventory_pool().add_inventory(new_inventory)
            # saves the inventories to the json file
            data_provider.fetch_inventory_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "suppliers":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_supplier = json.loads(post_data.decode())
            # adds the new supplier to the list of all suppliers
            data_provider.fetch_supplier_pool().add_supplier(new_supplier)
            # saves the suppliers to the json file
            data_provider.fetch_supplier_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "orders":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_order = json.loads(post_data.decode())
            # adds the new order to the list of all orders
            data_provider.fetch_order_pool().add_order(new_order)
            # saves the orders to the json file
            data_provider.fetch_order_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "clients":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_client = json.loads(post_data.decode())
            # adds the new order to the list of all clients
            data_provider.fetch_client_pool().add_client(new_client)
            # saves the clients to the json file
            data_provider.fetch_client_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        elif path[0] == "shipments":
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            new_shipment = json.loads(post_data.decode())
            # adds the new order to the list of all shipments
            data_provider.fetch_shipment_pool().add_shipment(new_shipment)
            # saves the shipments to the json file
            data_provider.fetch_shipment_pool().save()
            # if the request is successful, it returns a 201 (Created)
            self.send_response(201)
            self.end_headers()
        else:
            self.send_response(404)
            self.end_headers()

    def do_POST(self) -> None:
        # gets the api_key from the headers
        api_key = self.headers.get("API_KEY")
        # gets the user from the api_key
        user = auth_provider.get_user(api_key)
        if user == None:
            # if the user is None (user doesn't exist), it returns a 401 (unauthorized)
            self.send_response(401)
            self.end_headers()
        else:
            try:
                path = self.path.split("/")
                if len(path) > 3 and path[1] == "api" and path[2] == "v1":
                    self.handle_post_version_1(path[3:], user)
            except Exception:
                # if an exception occurs, it returns a 500 (Internal Server Error)
                self.send_response(500)
                self.end_headers()

    def handle_put_version_1(self, path, user) -> None:
        # checks if the user has access to the requested endpoint, so here it checks if the user has access to the put endpoint
        if not auth_provider.has_access(user, path, "put"):
            # if the user does not have access, it returns a 403 (Forbidden)
            self.send_response(403)
            self.end_headers()
            return
        if path[0] == "warehouses":
            # gets the warehouse id from the path
            warehouse_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_warehouse = json.loads(post_data.decode())
            # updates the warehouse with the new data
            data_provider.fetch_warehouse_pool().update_warehouse(
                warehouse_id, updated_warehouse)
            # saves the warehouses to the json file
            data_provider.fetch_warehouse_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "locations":
            # gets the location id from the path
            location_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_location = json.loads(post_data.decode())
            # updates the location with the new data
            data_provider.fetch_location_pool().update_location(location_id, updated_location)
            # saves the locations to the json file
            data_provider.fetch_location_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "transfers":
            # gets the length of the path
            paths = len(path)
            match paths:
                # if the length of the path is 2, it reads the transfer id from the path and updates the transfer with the new data
                case 2:
                    # gets the transfer id from the path
                    transfer_id = int(path[1])
                    # gets the content length from the headers
                    content_length = int(self.headers["Content-Length"])
                    # reads the post data
                    post_data = self.rfile.read(content_length)
                    # loads json data to python dictionary
                    updated_transfer = json.loads(post_data.decode())
                    # updates the transfer with the new data
                    data_provider.fetch_transfer_pool().update_transfer(transfer_id, updated_transfer)
                    # saves the transfers to the json file
                    data_provider.fetch_transfer_pool().save()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.end_headers()
                # if the length of the path is 3, it reads the transfer id from the path and updates the items in the transfer with the new data
                case 3:
                    # if the third part of the path is commit, it processes the transfer, by transferring the items from one location to another
                    if path[2] == "commit":
                        # gets the transfer id from the path
                        transfer_id = int(path[1])
                        # gets the transfer from the transfer pool with the transfer id
                        transfer = data_provider.fetch_transfer_pool().get_transfer(transfer_id)
                        # loops through the items in the transfer
                        for x in transfer["items"]:
                            # gets the inventories from the item
                            inventories = data_provider.fetch_inventory_pool(
                            ).get_inventories_for_item(x["item_id"])
                            # loops through the inventories
                            for y in inventories:
                                # checks if the location id is the same as the beginning location id from the transfer
                                if y["location_id"] == transfer["transfer_from"]:
                                    # subtracts the transfer amount from the total on hand
                                    y["total_on_hand"] -= x["amount"]
                                    # updates the total expected
                                    y["total_expected"] = y["total_on_hand"] + \
                                        y["total_ordered"]
                                    # updates the total available
                                    y["total_available"] = y["total_on_hand"] - \
                                        y["total_allocated"]
                                    # updates the inventory
                                    data_provider.fetch_inventory_pool(
                                    ).update_inventory(y["id"], y)
                                    # checks if the location id is the same as the end location id from the transfer
                                elif y["location_id"] == transfer["transfer_to"]:
                                    # adds the transfer amount to the total on hand
                                    y["total_on_hand"] += x["amount"]
                                    # updates the total expected
                                    y["total_expected"] = y["total_on_hand"] + \
                                        y["total_ordered"]
                                    # updates the total available
                                    y["total_available"] = y["total_on_hand"] - \
                                        y["total_allocated"]
                                    # updates the inventory
                                    data_provider.fetch_inventory_pool(
                                    ).update_inventory(y["id"], y)
                        # updates the transfer status to processed
                        transfer["transfer_status"] = "Processed"
                        # updates the transfer with the new data
                        data_provider.fetch_transfer_pool().update_transfer(transfer_id, transfer)
                        # sends a notification to the notification processor and eventualy to the user
                        notification_processor.push(
                            f"Processed batch transfer with id:{transfer['id']}")
                        # saves the transfers to the json file
                        data_provider.fetch_transfer_pool().save()
                        # saves the inventories to the json file
                        data_provider.fetch_inventory_pool().save()
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.end_headers()
                    else:
                        # if the third part of the path is not commit, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the length of the path is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "items":
            # gets the item id from the path
            item_id = path[1]
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_item = json.loads(post_data.decode())
            # updates the item with the new data
            data_provider.fetch_item_pool().update_item(item_id, updated_item)
            # saves the items to the json file
            data_provider.fetch_item_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "item_lines":
            # gets the item line id from the path
            item_line_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_item_line = json.loads(post_data.decode())
            # updates the item line with the new data
            data_provider.fetch_item_line_pool().update_item_line(
                item_line_id, updated_item_line)
            # saves the item lines to the json file
            data_provider.fetch_item_line_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "item_groups":
            # gets the item group id from the path
            item_group_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_item_group = json.loads(post_data.decode())
            # updates the item group with the new data
            data_provider.fetch_item_group_pool().update_item_group(
                item_group_id, updated_item_group)
            # saves the item groups to the json file
            data_provider.fetch_item_group_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "item_types":
            # gets the item type id from the path
            item_type_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_item_type = json.loads(post_data.decode())
            # updates the item type with the new data
            data_provider.fetch_item_type_pool().update_item_type(
                item_type_id, updated_item_type)
            # saves the item types to the json file
            data_provider.fetch_item_type_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "inventories":
            # gets the inventory id from the path
            inventory_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_inventory = json.loads(post_data.decode())
            # updates the inventory with the new data
            data_provider.fetch_inventory_pool().update_inventory(
                inventory_id, updated_inventory)
            # saves the inventories to the json file
            data_provider.fetch_inventory_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "suppliers":
            # gets the supplier id from the path
            supplier_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_supplier = json.loads(post_data.decode())
            # updates the supplier with the new data
            data_provider.fetch_supplier_pool().update_supplier(supplier_id, updated_supplier)
            # saves the suppliers to the json file
            data_provider.fetch_supplier_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "orders":
            # gets the length of the path
            paths = len(path)
            match paths:
                # if the length of the path is 2, it reads the order id from the path and updates the order with the new data
                case 2:
                    # gets the order id from the path
                    order_id = int(path[1])
                    # gets the content length from the headers
                    content_length = int(self.headers["Content-Length"])
                    # reads the post data
                    post_data = self.rfile.read(content_length)
                    # loads json data to python dictionary
                    updated_order = json.loads(post_data.decode())
                    # updates the order with the new data
                    data_provider.fetch_order_pool().update_order(order_id, updated_order)
                    # saves the orders to the json file
                    data_provider.fetch_order_pool().save()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.end_headers()
                # if the length of the path is 3, it reads the order id from the path and updates the items in the order with the new data
                case 3:
                    # if the third part of the path is items, it reads the order id from the path and updates the items in the order with the new data
                    if path[2] == "items":
                        # gets the order id from the path
                        order_id = int(path[1])
                        # gets the content length from the headers
                        content_length = int(self.headers["Content-Length"])
                        # reads the post data
                        post_data = self.rfile.read(content_length)
                        # loads json data to python dictionary
                        updated_items = json.loads(post_data.decode())
                        # updates the items in the order with the new data
                        data_provider.fetch_order_pool().update_items_in_order(order_id, updated_items)
                        # saves the orders to the json file
                        data_provider.fetch_order_pool().save()
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.end_headers()
                    else:
                        # if the third part of the path is not items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the length of the path is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        elif path[0] == "clients":
            # gets the length of the path
            client_id = int(path[1])
            # gets the content length from the headers
            content_length = int(self.headers["Content-Length"])
            # reads the post data
            post_data = self.rfile.read(content_length)
            # loads json data to python dictionary
            updated_client = json.loads(post_data.decode())
            # updates the client with the new data
            data_provider.fetch_client_pool().update_client(client_id, updated_client)
            # saves the clients to the json file
            data_provider.fetch_client_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "shipments":
            # gets the length of the path
            paths = len(path)
            match paths:
                # if the length of the path is 2, it reads the shipment id from the path and updates the shipment with the new data
                case 2:
                    # gets the shipment id from the path
                    shipment_id = int(path[1])
                    # gets the content length from the headers
                    content_length = int(self.headers["Content-Length"])
                    # reads the post data
                    post_data = self.rfile.read(content_length)
                    # loads json data to python dictionary
                    updated_shipment = json.loads(post_data.decode())
                    # updates the shipment with the new data
                    data_provider.fetch_shipment_pool().update_shipment(shipment_id, updated_shipment)
                    # saves the shipments to the json file
                    data_provider.fetch_shipment_pool().save()
                    # if the request is successful, it returns a 200 (OK)
                    self.send_response(200)
                    self.end_headers()
                # if the length of the path is 3, it reads the shipment id from the path and updates the items in the shipment with the new data
                case 3:
                    # if the third part of the path is orders, it reads the shipment id from the path and updates the orders in the shipment with the new data
                    if path[2] == "orders":
                        # gets the shipment id from the path
                        shipment_id = int(path[1])
                        # gets the content length from the headers
                        content_length = int(self.headers["Content-Length"])
                        # reads the post data
                        post_data = self.rfile.read(content_length)
                        # loads json data to python dictionary
                        updated_orders = json.loads(post_data.decode())
                        # updates the orders in the shipment with the new data
                        data_provider.fetch_order_pool().update_orders_in_shipment(
                            shipment_id, updated_orders)
                        # saves the orders to the json file
                        data_provider.fetch_order_pool().save()
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.end_headers()
                        # if the third part of the path is items, it reads the shipment id from the path and updates the items in the shipment with the new data
                    elif path[2] == "items":
                        # gets the shipment id from the path
                        shipment_id = int(path[1])
                        # gets the content length from the headers
                        content_length = int(self.headers["Content-Length"])
                        # reads the post data
                        post_data = self.rfile.read(content_length)
                        # loads json data to python dictionary
                        updated_items = json.loads(post_data.decode())
                        # updates the items in the shipment with the new data
                        data_provider.fetch_shipment_pool().update_items_in_shipment(
                            shipment_id, updated_items)
                        # saves the shipments to the json file
                        data_provider.fetch_shipment_pool().save()
                        # if the request is successful, it returns a 200 (OK)
                        self.send_response(200)
                        self.end_headers()
                    elif path[2] == "commit":
                        # requirement 1: the user can commit a shipment
                        pass
                    else:
                        # if the third part of the path is not orders or items, it returns a 404 (Page Not Found)
                        self.send_response(404)
                        self.end_headers()
                case _:
                    # if the length of the path is longer than 3 or shorter than 1, it returns a 404 (Page Not Found)
                    self.send_response(404)
                    self.end_headers()
        else:
            # if the first part of the path is not warehouses, locations, transfers, items, item_lines, item_groups, item_types, inventories, suppliers, orders, clients, or shipments, it returns a 404 (Page Not Found)
            self.send_response(404)
            self.end_headers()

    def do_PUT(self) -> None:
        # gets the api_key from the headers
        api_key = self.headers.get("API_KEY")
        # gets the user from the api_key
        user = auth_provider.get_user(api_key)
        # checks if the user is None (user doesn't exist)
        if user == None:
            # if the user is None (user doesn't exist), it returns a 401 (unauthorized)
            self.send_response(401)
            self.end_headers()
        else:
            try:
                # splits the path by /
                path = self.path.split("/")
                # checks if path is longer than 3 and the first part of the path is api and the second part of the path is v1
                if len(path) > 3 and path[1] == "api" and path[2] == "v1":
                    # calls the handle_put_version_1 function with the path and the user
                    # the part of the path passed to the function does not include 'api'and 'v1'
                    self.handle_put_version_1(path[3:], user)
            except Exception:
                # if an exception occurs, it returns a 500 (Internal Server Error)
                self.send_response(500)
                self.end_headers()

    def handle_delete_version_1(self, path, user) -> None:
        # checks if the user has access to the requested endpoint, so here it checks if the user has access to the delete endpoint
        if not auth_provider.has_access(user, path, "delete"):
            # if the user does not have access, it returns a 403 (Forbidden)
            self.send_response(403)
            self.end_headers()
            return
        if path[0] == "warehouses":
            # gets the warehouse id from the path
            warehouse_id = int(path[1])
            # removes the warehouse with the warehouse id
            data_provider.fetch_warehouse_pool().remove_warehouse(warehouse_id)
            # saves the warehouses to the json file
            data_provider.fetch_warehouse_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "locations":
            # gets the location id from the path
            location_id = int(path[1])
            # removes the location with the location id
            data_provider.fetch_location_pool().remove_location(location_id)
            # saves the locations to the json file
            data_provider.fetch_location_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "transfers":
            # gets the length of the path
            transfer_id = int(path[1])
            # removes the transfer with the transfer id
            data_provider.fetch_transfer_pool().remove_transfer(transfer_id)
            # saves the transfers to the json file
            data_provider.fetch_transfer_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "items":
            # gets the item id from the path
            item_id = path[1]
            # removes the item with the item id
            data_provider.fetch_item_pool().remove_item(item_id)
            # saves the items to the json file
            data_provider.fetch_item_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "item_lines":
            # gets the item line id from the path
            item_line_id = int(path[1])
            # removes the item line with the item line id
            data_provider.fetch_item_line_pool().remove_item_line(item_line_id)
            # saves the item lines to the json file
            data_provider.fetch_item_line_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "item_groups":
            # gets the item group id from the path
            item_group_id = int(path[1])
            # removes the item group with the item group id
            data_provider.fetch_item_group_pool().remove_item_group(item_group_id)
            # saves the item groups to the json file
            data_provider.fetch_item_group_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "item_types":
            # gets the item type id from the path
            item_type_id = int(path[1])
            # removes the item type with the item type id
            data_provider.fetch_item_type_pool().remove_item_type(item_type_id)
            # saves the item types to the json file
            data_provider.fetch_item_type_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "inventories":
            # gets the inventory id from the path
            inventory_id = int(path[1])
            # removes the inventory with the inventory id
            data_provider.fetch_inventory_pool().remove_inventory(inventory_id)
            # saves the inventories to the json file
            data_provider.fetch_inventory_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "suppliers":
            # gets the supplier id from the path
            supplier_id = int(path[1])
            # removes the supplier with the supplier id
            data_provider.fetch_supplier_pool().remove_supplier(supplier_id)
            # saves the suppliers to the json file
            data_provider.fetch_supplier_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "orders":
            # gets the length of the path
            order_id = int(path[1])
            # removes the order with the order id
            data_provider.fetch_order_pool().remove_order(order_id)
            # saves the orders to the json file
            data_provider.fetch_order_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "clients":
            # gets the length of the path
            client_id = int(path[1])
            # removes the client with the client id
            data_provider.fetch_client_pool().remove_client(client_id)
            # saves the clients to the json file
            data_provider.fetch_client_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        elif path[0] == "shipments":
            # gets the length of the path
            shipment_id = int(path[1])
            # removes the shipment with the shipment id
            data_provider.fetch_shipment_pool().remove_shipment(shipment_id)
            # saves the shipments to the json file
            data_provider.fetch_shipment_pool().save()
            # if the request is successful, it returns a 200 (OK)
            self.send_response(200)
            self.end_headers()
        else:
            # if the first part of the path is not warehouses, locations, transfers, items, item_lines, item_groups, item_types, inventories, suppliers, orders, clients, or shipments, it returns a 404 (Page Not Found)
            self.send_response(404)
            self.end_headers()

    def do_DELETE(self) -> None:
        # gets the api_key from the headers
        api_key = self.headers.get("API_KEY")
        # gets the user from the api_key
        user = auth_provider.get_user(api_key)
        # checks if the user is None (user doesn't exist)
        if user == None:
            # if the user is None (user doesn't exist), it returns a 401 (unauthorized)
            self.send_response(401)
            self.end_headers()
        else:
            try:
                # splits the path by /
                path = self.path.split("/")
                # checks if path is longer than 3 and the first part of the path is api and the second part of the path is v1
                if len(path) > 3 and path[1] == "api" and path[2] == "v1":
                    # calls the handle_delete_version_1 function with the path and the user
                    # the part of the path passed to the function does not include 'api'and 'v1'
                    self.handle_delete_version_1(path[3:], user)
            except Exception:
                # if an exception occurs, it returns a 500 (Internal Server Error)
                self.send_response(500)
                self.end_headers()


if __name__ == "__main__":
    PORT = 3000
    with socketserver.TCPServer(("", PORT), ApiRequestHandler) as httpd:
        auth_provider.init()
        data_provider.init()
        notification_processor.start()
        print(f"Serving on port {PORT}...")
        httpd.serve_forever()
