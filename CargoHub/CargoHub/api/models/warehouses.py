import json

from models.base import Base

WAREHOUSES = []


class Warehouses(Base):
    def __init__(self, root_path, is_debug=False) -> None:
        self.data_path = root_path + "warehouses.json"
        self.load(is_debug)

    def get_warehouses(self) -> list:
        # returns all warehouses available
        return self.data

    def get_warehouse(self, warehouse_id) -> dict or None:  # type: ignore
        # returns a warehouse by id or None if not found
        for x in self.data:
            if x["id"] == warehouse_id:
                return x
        return None

    def add_warehouse(self, warehouse) -> None:
        # adds a new warehouse
        warehouse["created_at"] = self.get_timestamp()
        warehouse["updated_at"] = self.get_timestamp()
        self.data.append(warehouse)

    def update_warehouse(self, warehouse_id, warehouse) -> None:
        # updates a warehouse
        warehouse["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == warehouse_id:
                self.data[i] = warehouse
                break

    def remove_warehouse(self, warehouse_id) -> None:
        # removes a warehouse
        for x in self.data:
            if x["id"] == warehouse_id:
                self.data.remove(x)

    def load(self, is_debug) -> None:
        # sets self.data to WARHOUSES if debug is true or loads from file if debug is false
        if is_debug:
            self.data = WAREHOUSES
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self) -> None:
        # saves self.data to file
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()
