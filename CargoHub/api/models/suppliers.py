import json

from models.base import Base

SUPPLIERS = []


class Suppliers(Base):
    def __init__(self, root_path, is_debug=False) -> None:
        self.data_path = root_path + "suppliers.json"
        self.load(is_debug)

    def get_suppliers(self) -> list:
        # returns all suppliers available
        return self.data

    def get_supplier(self, supplier_id) -> dict or None:  # type: ignore
        # returns a supplier by id or None if not found
        for x in self.data:
            if x["id"] == supplier_id:
                return x
        return None

    def add_supplier(self, supplier) -> None:
        # adds a new supplier
        supplier["created_at"] = self.get_timestamp()
        supplier["updated_at"] = self.get_timestamp()
        self.data.append(supplier)

    def update_supplier(self, supplier_id, supplier) -> None:
        # updates a supplier
        supplier["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == supplier_id:
                self.data[i] = supplier
                break

    def remove_supplier(self, supplier_id) -> None:
        # removes a supplier
        for x in self.data:
            if x["id"] == supplier_id:
                self.data.remove(x)

    def load(self, is_debug) -> None:
        # sets self.data to SUPPLIERS if debug is true or loads from file if debug is false
        if is_debug:
            self.data = SUPPLIERS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self) -> None:
        # saves self.data to file
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()
